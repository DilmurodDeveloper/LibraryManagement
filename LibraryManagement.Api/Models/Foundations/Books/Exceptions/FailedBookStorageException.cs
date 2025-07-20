//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Books.Exceptions
{
    public class FailedBookStorageException : Xeption
    {
        public FailedBookStorageException(Exception innerException)
            : base(message: "Failed book storage error occurred, contact support.",
                  innerException)
        { }
    }
}
