//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class NotFoundBookException : Xeption
    {
        public NotFoundBookException(Guid bookId)
            : base(message: $"Book is not found with id: {bookId}")
        { }
    }
}
