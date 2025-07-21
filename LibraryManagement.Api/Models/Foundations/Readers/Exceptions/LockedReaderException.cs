//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class LockedReaderException : Xeption
    {
        public LockedReaderException(Exception innerException)
            : base(message: "Reader is locked, please try again later.",
                  innerException)
        { }
    }
}
