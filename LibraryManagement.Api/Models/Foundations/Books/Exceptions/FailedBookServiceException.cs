//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class FailedBookServiceException : Xeption
    {
        public FailedBookServiceException(Exception innerException)
            : base(message: "Failed book service error occurred, contact support.",
                  innerException)
        { }
    }
}
