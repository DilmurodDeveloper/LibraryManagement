//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Microsoft.Data.SqlClient;

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

        public IQueryable<Reader> RetrieveAllReaders()
        {
            try
            {
                return this.storageBroker.SelectAllReaders();
            }
            catch (SqlException sqlException)
            {
                var failedReaderStorageException =
                    new FailedReaderStorageException(sqlException);

                var readerDependencyException =
                    new ReaderDependencyException(failedReaderStorageException);

                this.loggingBroker.LogCritical(readerDependencyException);

                throw readerDependencyException;
            }
            catch (Exception exception)
            {
                var failedReaderServiceException =
                    new FailedReaderServiceException(exception);

                var readerServiceException =
                    new ReaderServiceException(failedReaderServiceException);

                this.loggingBroker.LogError(readerServiceException);

                throw readerServiceException;
            }
        }
    }
}
