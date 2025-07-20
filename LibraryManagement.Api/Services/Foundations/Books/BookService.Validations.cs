//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;

namespace LibraryManagement.Api.Services.Foundations.Books
{
    public partial class BookService
    {
        private void ValidateBookNotNull(Book book)
        {
            if (book is null)
            {
                throw new NullBookException();
            }
        }
    }
}
