//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Book randomBook = CreateRandomBook();
            Book someBook = randomBook;
            Guid bookId = someBook.BookId;
            SqlException sqlException = GetSqlError();

            var failedBookStorageException =
                new FailedBookStorageException(sqlException);

            var expectedBookDependencyException =
                new BookDependencyException(failedBookStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(bookId))
                    .Throws(sqlException);

            // when
            ValueTask<Book> modifyBookTask =
                this.bookService.ModifyBookAsync(someBook);

            BookDependencyException actualBookDependencyException =
                await Assert.ThrowsAsync<BookDependencyException>(() =>
                    modifyBookTask.AsTask());

            // then
            actualBookDependencyException.Should()
                .BeEquivalentTo(expectedBookDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedBookDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(bookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateBookAsync(someBook),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Book randomBook = CreateRandomBook();
            Book someBook = randomBook;
            Guid bookId = someBook.BookId;
            var databaseUpdateException = new DbUpdateException();

            var failedBookStorageException =
                new FailedBookStorageException(databaseUpdateException);

            var expectedBookDependencyException =
                new BookDependencyException(failedBookStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(bookId))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Book> modifyBookTask =
                this.bookService.ModifyBookAsync(someBook);

            BookDependencyException actualBookDependencyException =
                await Assert.ThrowsAsync<BookDependencyException>(() =>
                    modifyBookTask.AsTask());

            // then
            actualBookDependencyException.Should()
                .BeEquivalentTo(expectedBookDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(bookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateBookAsync(someBook),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Book randomBook = CreateRandomBook();
            Book someBook = randomBook;
            Guid bookId = someBook.BookId;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedBookException =
                new LockedBookException(dbUpdateConcurrencyException);

            var expectedBookDependencyValidationException =
                new BookDependencyValidationException(lockedBookException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(bookId))
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Book> modifyBookTask =
                this.bookService.ModifyBookAsync(someBook);

            BookDependencyValidationException actualBookDependencyValidationException =
                await Assert.ThrowsAsync<BookDependencyValidationException>(
                    modifyBookTask.AsTask);

            // then
            actualBookDependencyValidationException.Should()
                .BeEquivalentTo(expectedBookDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(bookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateBookAsync(someBook),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Book randomBook = CreateRandomBook();
            Book someBook = randomBook;
            Guid bookId = someBook.BookId;
            var serviceException = new Exception();

            var failedBookServiceException =
                new FailedBookServiceException(serviceException);

            var expectedBookServiceException =
                new BookServiceException(failedBookServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(bookId))
                    .Throws(serviceException);

            // when
            ValueTask<Book> modifyBookTask =
                this.bookService.ModifyBookAsync(someBook);

            BookServiceException actualBookServiceException =
                await Assert.ThrowsAsync<BookServiceException>(() =>
                    modifyBookTask.AsTask());

            // then
            actualBookServiceException.Should()
                .BeEquivalentTo(expectedBookServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(bookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateBookAsync(someBook),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
