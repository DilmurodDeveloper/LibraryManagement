//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class BookDependencyException : Xeption
    {
        public BookDependencyException(Xeption innerException)
            : base(message: "Book dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
