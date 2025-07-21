//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using LibraryManagement.Api.Models.Foundations.Readers;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldRemoveReaderByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputReaderId = randomId;
            Reader randomReader = CreateRandomReader();
            Reader storageReader = randomReader;
            Reader expectedInputReader = storageReader;
            Reader deletedReader = expectedInputReader;
            Reader expectedReader = deletedReader.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(inputReaderId))
                    .ReturnsAsync(storageReader);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteReaderAsync(expectedInputReader))
                    .ReturnsAsync(deletedReader);

            // when
            Reader actualReader =
                await this.readerService.RemoveReaderByIdAsync(randomId);

            // then
            actualReader.Should().BeEquivalentTo(expectedReader);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(inputReaderId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteReaderAsync(expectedInputReader),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
