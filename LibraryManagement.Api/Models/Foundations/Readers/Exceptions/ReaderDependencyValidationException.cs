//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class ReaderDependencyValidationException : Xeption
    {
        public ReaderDependencyValidationException(Xeption innerException)
            : base(message: "Reader dependency validation errors occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
