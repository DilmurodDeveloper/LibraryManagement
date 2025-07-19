//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;

namespace LibraryManagement.Api.Models.Foundations.Readers
{
    public class Reader
    {
        public Guid ReaderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public List<Book> Books { get; set; }
    }
}
