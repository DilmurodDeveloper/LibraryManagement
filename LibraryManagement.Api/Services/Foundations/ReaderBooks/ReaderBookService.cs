//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.ReaderBooks;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Services.Foundations.Books;
using LibraryManagement.Api.Services.Foundations.Readers;

namespace LibraryManagement.Api.Services.Foundations.ReaderBooks
{
    public class ReaderBookService : IReaderBookService
    {
        private readonly IReaderService readerService;
        private readonly IBookService bookService;

        public ReaderBookService(
            IReaderService readerService,
            IBookService bookService)
        {
            this.readerService = readerService;
            this.bookService = bookService;
        }

        public async ValueTask<ReaderBook> RetrieveReaderBookByIdAsync(Guid readerId)
        {
            Reader reader =
                await this.readerService.RetrieveReaderByIdAsync(readerId);

            IQueryable<Book> allBooks =
                this.bookService.RetrieveAllBooks();

            List<Book> booksForReader = allBooks
                .Where(book => book.ReaderId == readerId)
                .ToList();

            return new ReaderBook
            {
                Reader = reader,
                Books = booksForReader
            };
        }
    }
}
