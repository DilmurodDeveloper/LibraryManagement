//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReaderIsNullAndLogItAsync()
        {
            // given
            Reader nullReader = null;
            var nullReaderException = new NullReaderException();

            var expectedReaderValidationException =
                new ReaderValidationException(nullReaderException);

            // when
            ValueTask<Reader> addReaderTask =
                this.readerService.AddReaderAsync(nullReader);

            // then
            await Assert.ThrowsAsync<ReaderValidationException>(() =>
                addReaderTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReaderAsync(It.IsAny<Reader>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfReaderIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidReader = new Reader
            {
                FirstName = invalidText
            };

            var invalidReaderException = new InvalidReaderException();

            invalidReaderException.AddData(
                key: nameof(Reader.ReaderId),
                values: "Id is required");

            invalidReaderException.AddData(
                key: nameof(Reader.FirstName),
                values: "Text is required");

            invalidReaderException.AddData(
                key: nameof(Reader.LastName),
                values: "Text is required");

            invalidReaderException.AddData(
                key: nameof(Reader.DateOfBirth),
                values: "Date is required");

            var expectedReaderValidationException =
                new ReaderValidationException(invalidReaderException);

            // when
            ValueTask<Reader> addReaderTask =
                this.readerService.AddReaderAsync(invalidReader);

            // then
            await Assert.ThrowsAsync<ReaderValidationException>(() =>
                addReaderTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReaderAsync(It.IsAny<Reader>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
