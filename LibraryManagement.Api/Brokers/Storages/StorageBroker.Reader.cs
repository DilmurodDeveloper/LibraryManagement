//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Reader> Readers { get; set; }

        public async ValueTask<Reader> InsertReaderAsync(Reader reader) =>
            await InsertAsync(reader);

        public IQueryable<Reader> SelectAllReaders() =>
            SelectAll<Reader>().Include(readers => readers.Books);

        public async ValueTask<Reader> SelectReaderByIdAsync(Guid readerId)
        {
            var readerWithBooks = await SelectAll<Reader>()
                .Include(reader => reader.Books)
                .FirstOrDefaultAsync(c => c.ReaderId == readerId);

            return readerWithBooks;
        }
    }
}
