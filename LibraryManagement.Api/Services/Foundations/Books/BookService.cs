//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Services.Foundations.Books
{
    public partial class BookService : IBookService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public BookService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Book> AddBookAsync(Book book) =>
        TryCatch(async () =>
        {
            ValidateBookOnAdd(book);

            return await this.storageBroker.InsertBookAsync(book);
        });

        public IQueryable<Book> RetrieveAllBooks() =>
            TryCatch(() => this.storageBroker.SelectAllBooks());

        public ValueTask<Book> RetrieveBookByIdAsync(Guid bookId) =>
        TryCatch(async () =>
        {
            ValidateBookId(bookId);

            Book maybeBook =
                await this.storageBroker.SelectBookByIdAsync(bookId);

            ValidateStorageBook(maybeBook, bookId);

            return maybeBook;
        });

        public ValueTask<Book> ModifyBookAsync(Book book) =>
        TryCatch(async () =>
        {
            ValidateBookOnModify(book);

            Book maybeBook =
                await this.storageBroker.SelectBookByIdAsync(book.BookId);

            ValidateAgainstStorageBookOnModify(book, maybeBook);

            return await this.storageBroker.UpdateBookAsync(book);
        });

        public async ValueTask<Book> RemoveBookByIdAsync(Guid bookId)
        {
            try
            {
                ValidateBookId(bookId);

                Book maybeBook =
                    await this.storageBroker.SelectBookByIdAsync(bookId);

                ValidateStorageBook(maybeBook, bookId);

                return await this.storageBroker.DeleteBookAsync(maybeBook);
            }
            catch (InvalidBookException invalidBookException)
            {
                var bookValidationException =
                    new BookValidationException(invalidBookException);

                this.loggingBroker.LogError(bookValidationException);

                throw bookValidationException;
            }
            catch (NotFoundBookException notFoundBookException)
            {
                var bookValidationException =
                    new BookValidationException(notFoundBookException);

                this.loggingBroker.LogError(bookValidationException);

                throw bookValidationException;
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedBookException =
                    new LockedBookException(dbUpdateConcurrencyException);

                var bookDependencyValidationException =
                    new BookDependencyValidationException(lockedBookException);

                this.loggingBroker.LogError(bookDependencyValidationException);

                throw bookDependencyValidationException;
            }
            catch (SqlException sqlException)
            {
                var failedBookStorageException =
                    new FailedBookStorageException(sqlException);

                var bookDependencyException =
                    new BookDependencyException(failedBookStorageException);

                this.loggingBroker.LogCritical(bookDependencyException);

                throw bookDependencyException;
            }
            catch (Exception exception)
            {
                var failedBookServiceException =
                    new FailedBookServiceException(exception);

                var bookServiceException =
                    new BookServiceException(failedBookServiceException);

                this.loggingBroker.LogError(bookServiceException);

                throw bookServiceException;
            }
        }
    }
}
