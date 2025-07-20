//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class AlreadyExistsBookException : Xeption
    {
        public AlreadyExistsBookException(Exception innerException)
            : base(message: "Book already exists.", innerException)
        { }
    }
}
