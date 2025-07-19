//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

namespace LibraryManagement.Api.Models.Foundations.Books
{
    public class Book
    {
        public Guid BookId { get; set; }
        public Guid ReaderId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
