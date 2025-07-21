//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class ReaderServiceException : Xeption
    {
        public ReaderServiceException(Xeption innerException)
            : base(message: "Reader service error occurred, contact support.",
                  innerException)
        { }
    }
}
