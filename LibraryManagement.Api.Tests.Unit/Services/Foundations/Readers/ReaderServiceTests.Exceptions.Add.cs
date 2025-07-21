//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using EFxceptions.Models.Exceptions;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Reader someReader = CreateRandomReader();
            SqlException sqlException = GetSqlError();

            var failedReaderStorageException =
                new FailedReaderStorageException(sqlException);

            var expectedReaderDependencyException =
                new ReaderDependencyException(failedReaderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReaderAsync(someReader))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Reader> addReaderTask =
                this.readerService.AddReaderAsync(someReader);

            // then
            await Assert.ThrowsAsync<ReaderDependencyException>(() =>
                addReaderTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReaderAsync(someReader),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedReaderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Reader someReader = CreateRandomReader();
            string someMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(someMessage);

            var alreadyExistsReaderException =
                new AlreadyExistsReaderException(duplicateKeyException);

            var expectedReaderDependencyValidationException =
                new ReaderDependencyValidationException(alreadyExistsReaderException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReaderAsync(someReader))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Reader> addReaderTask =
                this.readerService.AddReaderAsync(someReader);

            // then
            await Assert.ThrowsAsync<ReaderDependencyValidationException>(() =>
                addReaderTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReaderAsync(someReader),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
