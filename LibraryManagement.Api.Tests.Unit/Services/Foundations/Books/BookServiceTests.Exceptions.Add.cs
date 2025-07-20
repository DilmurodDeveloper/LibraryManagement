//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using EFxceptions.Models.Exceptions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Book someBook = CreateRandomBook();
            SqlException sqlException = GetSqlError();

            var failedBookStorageException =
                new FailedBookStorageException(sqlException);

            var expectedBookDependencyException =
                new BookDependencyException(failedBookStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertBookAsync(someBook))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Book> addBookTask =
                this.bookService.AddBookAsync(someBook);

            // then
            await Assert.ThrowsAsync<BookDependencyException>(() =>
                addBookTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertBookAsync(someBook),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedBookDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Book someBook = CreateRandomBook();
            string someMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(someMessage);

            var alreadyExistsBookException =
                new AlreadyExistsBookException(duplicateKeyException);

            var expectedBookDependencyValidationException =
                new BookDependencyValidationException(alreadyExistsBookException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertBookAsync(someBook))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Book> addBookTask =
                this.bookService.AddBookAsync(someBook);

            // then
            await Assert.ThrowsAsync<BookDependencyValidationException>(() =>
                addBookTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertBookAsync(someBook),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Book someBook = CreateRandomBook();
            var serviceException = new Exception();

            var failedBookServiceException =
                new FailedBookServiceException(serviceException);

            var expectedBookServiceException =
                new BookServiceException(failedBookServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertBookAsync(someBook))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Book> addBookTask =
                this.bookService.AddBookAsync(someBook);

            // then
            await Assert.ThrowsAsync<BookServiceException>(() =>
                addBookTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertBookAsync(someBook),
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
