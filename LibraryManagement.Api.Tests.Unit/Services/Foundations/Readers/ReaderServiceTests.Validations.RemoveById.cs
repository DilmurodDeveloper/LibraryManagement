//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidReaderId = Guid.Empty;

            var invalidReaderException = new InvalidReaderException();

            invalidReaderException.AddData(
                key: nameof(Reader.ReaderId),
                values: "Id is required");

            var expectedReaderValidationException =
                new ReaderValidationException(invalidReaderException);

            // when
            ValueTask<Reader> removeReaderById =
                this.readerService.RemoveReaderByIdAsync(invalidReaderId);

            ReaderValidationException actualReaderValidationException =
                await Assert.ThrowsAsync<ReaderValidationException>(() =>
                    removeReaderById.AsTask());

            // then
            actualReaderValidationException.Should()
                .BeEquivalentTo(expectedReaderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteReaderAsync(It.IsAny<Reader>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
