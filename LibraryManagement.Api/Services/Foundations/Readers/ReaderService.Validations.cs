//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;

namespace LibraryManagement.Api.Services.Foundations.Readers
{
    public partial class ReaderService
    {
        private void ValidateReaderNotNull(Reader reader)
        {
            if (reader is null)
            {
                throw new NullReaderException();
            }
        }
    }
}
