//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using EFxceptions.Models.Exceptions;
using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;

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

        public async ValueTask<Book> AddBookAsync(Book book)
        {
            try
            {
                ValidateBookOnAdd(book);

                return await this.storageBroker.InsertBookAsync(book);
            }
            catch (NullBookException nullBookException)
            {
                var bookValidationException =
                    new BookValidationException(nullBookException);

                this.loggingBroker.LogError(bookValidationException);

                throw bookValidationException;
            }
            catch (InvalidBookException invalidBookException)
            {
                var bookValidationException =
                    new BookValidationException(invalidBookException);

                this.loggingBroker.LogError(bookValidationException);

                throw bookValidationException;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsBookException =
                    new AlreadyExistsBookException(duplicateKeyException);

                var bookDependencyValidationException =
                    new BookDependencyValidationException(alreadyExistsBookException);

                this.loggingBroker.LogError(bookDependencyValidationException);

                throw bookDependencyValidationException;
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
