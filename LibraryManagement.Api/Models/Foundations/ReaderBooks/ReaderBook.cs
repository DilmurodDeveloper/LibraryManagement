//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Readers;

namespace LibraryManagement.Api.Models.Foundations.ReaderBooks
{
    public class ReaderBook
    {
        public Reader Reader { get; set; }
        public List<Book> Books { get; set; }
    }
}
