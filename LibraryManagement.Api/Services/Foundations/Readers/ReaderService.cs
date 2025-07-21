//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Readers;

namespace LibraryManagement.Api.Services.Foundations.Readers
{
    public partial class ReaderService : IReaderService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public ReaderService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Reader> AddReaderAsync(Reader reader) =>
        TryCatch(async () =>
        {
            ValidateReaderOnAdd(reader);

            return await this.storageBroker.InsertReaderAsync(reader);
        });

        public IQueryable<Reader> RetrieveAllReaders() =>
            TryCatch(() => this.storageBroker.SelectAllReaders());

        public ValueTask<Reader> RetrieveReaderByIdAsync(Guid readerId) =>
        TryCatch(async () =>
        {
            ValidateReaderId(readerId);

            Reader maybeReader =
                await this.storageBroker.SelectReaderByIdAsync(readerId);

            ValidateStorageReader(maybeReader, readerId);

            return maybeReader;
        });

        public ValueTask<Reader> ModifyReaderAsync(Reader reader) =>
        TryCatch(async () =>
        {
            ValidateReaderOnModify(reader);

            Reader maybeReader =
                await this.storageBroker.SelectReaderByIdAsync(reader.ReaderId);

            ValidateAgainstStorageReaderOnModify(reader, maybeReader);

            return await this.storageBroker.UpdateReaderAsync(reader);
        });
    }
}
