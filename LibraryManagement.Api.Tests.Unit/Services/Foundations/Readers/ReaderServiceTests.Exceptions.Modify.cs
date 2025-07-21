//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Readers
{
    public partial class ReaderServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader someReader = randomReader;
            Guid readerId = someReader.ReaderId;
            SqlException sqlException = GetSqlError();

            var failedReaderStorageException =
                new FailedReaderStorageException(sqlException);

            var expectedReaderDependencyException =
                new ReaderDependencyException(failedReaderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(readerId))
                    .Throws(sqlException);

            // when
            ValueTask<Reader> modifyReaderTask =
                this.readerService.ModifyReaderAsync(someReader);

            ReaderDependencyException actualReaderDependencyException =
                await Assert.ThrowsAsync<ReaderDependencyException>(() =>
                    modifyReaderTask.AsTask());

            // then
            actualReaderDependencyException.Should()
                .BeEquivalentTo(expectedReaderDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedReaderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(readerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReaderAsync(someReader),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader someReader = randomReader;
            Guid readerId = someReader.ReaderId;
            var databaseUpdateException = new DbUpdateException();

            var failedReaderStorageException =
                new FailedReaderStorageException(databaseUpdateException);

            var expectedReaderDependencyException =
                new ReaderDependencyException(failedReaderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(readerId))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Reader> modifyReaderTask =
                this.readerService.ModifyReaderAsync(someReader);

            ReaderDependencyException actualReaderDependencyException =
                await Assert.ThrowsAsync<ReaderDependencyException>(() =>
                    modifyReaderTask.AsTask());

            // then
            actualReaderDependencyException.Should()
                .BeEquivalentTo(expectedReaderDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(readerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReaderAsync(someReader),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader someReader = randomReader;
            Guid readerId = someReader.ReaderId;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedReaderException =
                new LockedReaderException(dbUpdateConcurrencyException);

            var expectedReaderDependencyValidationException =
                new ReaderDependencyValidationException(lockedReaderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(readerId))
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Reader> modifyReaderTask =
                this.readerService.ModifyReaderAsync(someReader);

            ReaderDependencyValidationException actualReaderDependencyValidationException =
                await Assert.ThrowsAsync<ReaderDependencyValidationException>(() =>
                    modifyReaderTask.AsTask());

            // then
            actualReaderDependencyValidationException.Should()
                .BeEquivalentTo(expectedReaderDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(readerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReaderAsync(someReader),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Reader randomReader = CreateRandomReader();
            Reader someReader = randomReader;
            Guid readerId = someReader.ReaderId;
            var serviceException = new Exception();

            var failedReaderServiceException =
                new FailedReaderServiceException(serviceException);

            var expectedReaderServiceException =
                new ReaderServiceException(failedReaderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReaderByIdAsync(readerId))
                    .Throws(serviceException);

            // when
            ValueTask<Reader> modifyReaderTask =
                this.readerService.ModifyReaderAsync(someReader);

            ReaderServiceException actualReaderServiceException =
                await Assert.ThrowsAsync<ReaderServiceException>(() =>
                    modifyReaderTask.AsTask());

            // then
            actualReaderServiceException.Should()
                .BeEquivalentTo(expectedReaderServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReaderServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReaderByIdAsync(readerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReaderAsync(someReader),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
