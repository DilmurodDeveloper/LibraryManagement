//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;

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

        public async ValueTask<Reader> AddReaderAsync(Reader reader)
        {
            try
            {
                ValidateReaderOnAdd(reader);

                return await this.storageBroker.InsertReaderAsync(reader);
            }
            catch (NullReaderException nullReaderException)
            {
                var readerValidationException =
                    new ReaderValidationException(nullReaderException);

                this.loggingBroker.LogError(readerValidationException);

                throw readerValidationException;
            }
            catch (InvalidReaderException invalidReaderException)
            {
                var readerValidationException =
                    new ReaderValidationException(invalidReaderException);

                this.loggingBroker.LogError(readerValidationException);

                throw readerValidationException;
            }
        }
    }
}
