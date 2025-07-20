//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedBookStorageException =
                new FailedBookStorageException(sqlException);

            var expectedBookDependencyException =
                new BookDependencyException(failedBookStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Book> retrieveBookById =
                this.bookService.RetrieveBookByIdAsync(someId);

            BookDependencyException actualBookDependencyException =
                await Assert.ThrowsAsync<BookDependencyException>(() =>
                    retrieveBookById.AsTask());

            // then
            actualBookDependencyException.Should()
                .BeEquivalentTo(expectedBookDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(someId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedBookDependencyException))),
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

            var failedBookServiceException =
                new FailedBookServiceException(serverException);

            var expectedBookServiceException =
                new BookServiceException(failedBookServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ValueTask<Book> retrieveBookById =
                this.bookService.RetrieveBookByIdAsync(someId);

            BookServiceException actualBookServiceException =
                await Assert.ThrowsAsync<BookServiceException>(() =>
                    retrieveBookById.AsTask());

            // then
            actualBookServiceException.Should()
                .BeEquivalentTo(expectedBookServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
