//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class InvalidBookException : Xeption
    {
        public InvalidBookException()
            : base(message: "Book is invalid.")
        { }
    }
}
