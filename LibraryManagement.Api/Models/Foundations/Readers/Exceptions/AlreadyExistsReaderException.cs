//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class AlreadyExistsReaderException : Xeption
    {
        public AlreadyExistsReaderException(Exception innerException)
            : base(message: "Reader already exists.", innerException)
        { }
    }
}
