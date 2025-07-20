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

        public async ValueTask<Book> AddBookAsync(Book book)
        {
            try
            {
                ValidateBookNotNull(book);

                return await this.storageBroker.InsertBookAsync(book);
            }
            catch (NullBookException nullBookException)
            {
                var bookValidationException =
                    new BookValidationException(nullBookException);

                this.loggingBroker.LogError(bookValidationException);

                throw bookValidationException;
            }
        }
    }
}
