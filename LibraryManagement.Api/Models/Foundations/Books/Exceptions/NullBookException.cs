//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class NullBookException : Xeption
    {
        public NullBookException()
            : base(message: "Book is null.")
        { }
    }
}
