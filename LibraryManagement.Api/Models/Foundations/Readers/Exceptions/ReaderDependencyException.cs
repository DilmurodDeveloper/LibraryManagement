//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class ReaderDependencyException : Xeption
    {
        public ReaderDependencyException(Xeption innerException)
            : base(message: "Reader dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
