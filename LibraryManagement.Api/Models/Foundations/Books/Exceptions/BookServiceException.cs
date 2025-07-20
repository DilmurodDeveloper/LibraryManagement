//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class BookServiceException : Xeption
    {
        public BookServiceException(Xeption innerException)
            : base(message: "Book service error occurred, contact support.",
                  innerException)
        { }
    }
}
