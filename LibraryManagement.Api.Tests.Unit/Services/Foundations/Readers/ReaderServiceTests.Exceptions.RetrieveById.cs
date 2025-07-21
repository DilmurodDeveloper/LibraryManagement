//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedReaderStorageException =
                new FailedReaderStorageException(sqlException);

            var expectedReaderDependencyException =
                new ReaderDependencyException(failedReaderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Reader> retrieveReaderById =
                this.readerService.RetrieveReaderByIdAsync(someId);

            ReaderDependencyException actualReaderDependencyException =
                await Assert.ThrowsAsync<ReaderDependencyException>(() =>
                    retrieveReaderById.AsTask());

            // then
            actualReaderDependencyException.Should()
                .BeEquivalentTo(expectedReaderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(someId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedReaderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serverException = new Exception();

            var failedReaderServiceException =
                new FailedReaderServiceException(serverException);

            var expectedReaderServiceException =
                new ReaderServiceException(failedReaderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ValueTask<Reader> retrieveReaderById =
                this.readerService.RetrieveReaderByIdAsync(someId);

            ReaderServiceException actualReaderServiceException =
                await Assert.ThrowsAsync<ReaderServiceException>(() =>
                    retrieveReaderById.AsTask());

            // then
            actualReaderServiceException.Should()
                .BeEquivalentTo(expectedReaderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
