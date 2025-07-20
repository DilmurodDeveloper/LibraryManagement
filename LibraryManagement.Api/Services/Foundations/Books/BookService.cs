//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;

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

        public async ValueTask<Book> RetrieveBookByIdAsync(Guid bookId)
        {
            try
            {
                ValidateBookId(bookId);

                Book maybeBook =
                    await this.storageBroker.SelectBookByIdAsync(bookId);

                ValidateStorageBook(maybeBook, bookId);

                return await this.storageBroker.SelectBookByIdAsync(bookId);
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
        }
    }
}
