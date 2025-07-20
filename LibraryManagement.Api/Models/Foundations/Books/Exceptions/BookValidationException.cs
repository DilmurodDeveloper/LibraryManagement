//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class BookValidationException : Xeption
    {
        public BookValidationException(Xeption innerException)
            : base(message: "Book validation errors occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
