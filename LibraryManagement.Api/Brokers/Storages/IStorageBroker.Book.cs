//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Book> InsertBookAsync(Book book);
        IQueryable<Book> SelectAllBooks();
        ValueTask<Book> SelectBookByIdAsync(Guid bookId);
    }
}
