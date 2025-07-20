//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Book> Books { get; set; }

        public async ValueTask<Book> InsertBookAsync(Book book) =>
            await InsertAsync(book);

        public IQueryable<Book> SelectAllBooks()
        {
            var books = SelectAll<Book>().Include(a => a.Reader);

            return books;
        }

        public async ValueTask<Book> SelectBookByIdAsync(Guid bookId)
        {
            var readerWithBooks = Books
            .Include(a => a.Reader)
                .FirstOrDefault(a => a.BookId == bookId);

            return await ValueTask.FromResult(readerWithBooks);
        }

        public async ValueTask<Book> UpdateBookAsync(Book book) =>
            await UpdateAsync(book);

        public async ValueTask<Book> DeleteBookAsync(Book book) =>
            await DeleteAsync(book);
    }
}
