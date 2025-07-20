//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class BookDependencyValidationException : Xeption
    {
        public BookDependencyValidationException(Xeption innerException)
            : base(message: "Book dependency validation errors occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
