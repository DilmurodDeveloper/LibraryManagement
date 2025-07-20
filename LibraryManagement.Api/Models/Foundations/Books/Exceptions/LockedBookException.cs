//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class LockedBookException : Xeption
    {
        public LockedBookException(Exception innerException)
            : base(message: "Book is locked, please try again later.",
                  innerException)
        { }
    }
}
