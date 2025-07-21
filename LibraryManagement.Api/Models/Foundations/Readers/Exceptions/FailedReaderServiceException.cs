//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class FailedReaderServiceException : Xeption
    {
        public FailedReaderServiceException(Exception innerException)
            : base(message: "Failed reader service error occurred, contact support.",
                  innerException)
        { }
    }
}
