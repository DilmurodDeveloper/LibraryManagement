//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using EFxceptions.Models.Exceptions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LibraryManagement.Api.Services.Foundations.Books
{
    public partial class BookService
    {
        private delegate ValueTask<Book> ReturningBookFunction();
        private delegate IQueryable<Book> ReturningBooksFunction();

        private async ValueTask<Book> TryCatch(ReturningBookFunction returningBookFunction)
        {
            try
            {
                return await returningBookFunction();
            }
            catch (NullBookException nullBookException)
            {
                throw CreateAndLogValidationException(nullBookException);
            }
            catch (InvalidBookException invalidBookException)
            {
                throw CreateAndLogValidationException(invalidBookException);
            }
            catch (NotFoundBookException notFoundBookException)
            {
                throw CreateAndLogValidationException(notFoundBookException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedBookException =
                    new LockedBookException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedBookException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedBookStorageException =
                    new FailedBookStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedBookStorageException);
            }
            catch (SqlException sqlException)
            {
                var failedBookStorageException =
                    new FailedBookStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedBookStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsBookException =
                    new AlreadyExistsBookException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsBookException);
            }
            catch (Exception exception)
            {
                var failedBookServiceException =
                    new FailedBookServiceException(exception);

                throw CreateAndLogServiceException(failedBookServiceException);
            }
        }

        private IQueryable<Book> TryCatch(ReturningBooksFunction returningBooksFunction)
        {
            try
            {
                return returningBooksFunction();
            }
            catch (SqlException sqlException)
            {
                var failedBookStorageException =
                    new FailedBookStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedBookStorageException);
            }
            catch (Exception exception)
            {
                var failedBookServiceException =
                    new FailedBookServiceException(exception);

                throw CreateAndLogServiceException(failedBookServiceException);
            }
        }

        private BookValidationException CreateAndLogValidationException(Xeption exception)
        {
            var bookValidationException =
                new BookValidationException(exception);

            this.loggingBroker.LogError(bookValidationException);

            return bookValidationException;
        }

        private BookDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var bookDependencyException =
                new BookDependencyException(exception);

            this.loggingBroker.LogCritical(bookDependencyException);

            return bookDependencyException;
        }

        private BookDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var bookDependencyValidationException =
                new BookDependencyValidationException(exception);

            this.loggingBroker.LogError(bookDependencyValidationException);

            return bookDependencyValidationException;
        }

        private BookServiceException CreateAndLogServiceException(Xeption exception)
        {
            var bookServiceException =
                new BookServiceException(exception);

            this.loggingBroker.LogError(bookServiceException);

            return bookServiceException;
        }

        private BookDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var bookDependencyException =
                new BookDependencyException(exception);

            this.loggingBroker.LogError(bookDependencyException);

            return bookDependencyException;
        }
    }
}
