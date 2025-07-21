//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;

namespace LibraryManagement.Api.Services.Foundations.Readers
{
    public interface IReaderService
    {
        ValueTask<Reader> AddReaderAsync(Reader reader);
        IQueryable<Reader> RetrieveAllReaders();
        ValueTask<Reader> RetrieveReaderByIdAsync(Guid readerId);
        ValueTask<Reader> ModifyReaderAsync(Reader reader);
        ValueTask<Reader> RemoveReaderByIdAsync(Guid readerId);
    }
}
