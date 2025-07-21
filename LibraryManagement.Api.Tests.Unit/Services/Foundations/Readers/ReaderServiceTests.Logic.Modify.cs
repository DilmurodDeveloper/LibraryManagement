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
        public async Task ShouldModifyReaderAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader inputReader = randomReader;
            Reader persistedReader = inputReader;
            Reader updatedReader = inputReader;
            Reader expectedReader = updatedReader;
            Guid InputReaderId = inputReader.ReaderId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(InputReaderId))
                    .ReturnsAsync(persistedReader);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateReaderAsync(inputReader))
                    .ReturnsAsync(updatedReader);

            // when
            Reader actualReader =
                await this.readerService
                    .ModifyReaderAsync(inputReader);

            // then
            actualReader.Should().BeEquivalentTo(expectedReader);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(InputReaderId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReaderAsync(inputReader),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
