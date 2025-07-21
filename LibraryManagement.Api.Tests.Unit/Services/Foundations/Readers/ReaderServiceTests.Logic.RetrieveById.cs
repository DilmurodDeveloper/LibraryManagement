//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Readers;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveReaderByIdAsync()
        {
            // given
            Guid randomReaderId = Guid.NewGuid();
            Guid inputReaderId = randomReaderId;
            Reader randomReader = CreateRandomReader();
            Reader storageReader = randomReader;
            Reader expectedReader = storageReader;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(inputReaderId))
                    .ReturnsAsync(storageReader);

            // when
            Reader actualReader = await this
                .readerService.RetrieveReaderByIdAsync(inputReaderId);

            // then
            actualReader.Should().BeEquivalentTo(expectedReader);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(inputReaderId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
