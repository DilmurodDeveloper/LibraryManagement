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
        public async Task ShouldAddReaderAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader inputReader = randomReader;
            Reader storageReader = inputReader;
            Reader expectedReader = storageReader.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReaderAsync(inputReader))
                    .ReturnsAsync(storageReader);

            // when
            Reader actualReader =
                await this.readerService.AddReaderAsync(inputReader);

            // then
            actualReader.Should().BeEquivalentTo(expectedReader);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReaderAsync(inputReader),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
