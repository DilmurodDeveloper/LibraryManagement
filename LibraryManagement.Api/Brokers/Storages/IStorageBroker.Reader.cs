//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Reader> InsertReaderAsync(Reader reader);
    }
}
