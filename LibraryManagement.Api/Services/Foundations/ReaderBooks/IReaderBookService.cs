//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.ReaderBooks;

namespace LibraryManagement.Api.Services.Foundations.ReaderBooks
{
    public interface IReaderBookService
    {
        ValueTask<ReaderBook> RetrieveReaderBookByIdAsync(Guid readerId);
    }
}
