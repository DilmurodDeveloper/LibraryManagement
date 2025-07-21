//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using Xeptions;

namespace LibraryManagement.Api.Models.Foundations.Readers.Exceptions
{
    public class NotFoundReaderException : Xeption
    {
        public NotFoundReaderException(Guid readerId)
            : base(message: $"Reader is not found with id: {readerId}")
        { }
    }
}
