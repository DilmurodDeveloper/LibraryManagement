//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

        public async ValueTask<Reader> ModifyReaderAsync(Reader reader)
        {
            try
            {
                ValidateReaderOnModify(reader);

                Reader maybeReader =
                    await this.storageBroker.SelectReaderByIdAsync(reader.ReaderId);

                ValidateAgainstStorageReaderOnModify(reader, maybeReader);

                return await this.storageBroker.UpdateReaderAsync(reader);
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
            catch (NotFoundReaderException notFoundReaderException)
            {
                var readerValidationException =
                    new ReaderValidationException(notFoundReaderException);

                this.loggingBroker.LogError(readerValidationException);

                throw readerValidationException;
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedReaderException =
                    new LockedReaderException(dbUpdateConcurrencyException);

                var readerDependencyValidationException =
                    new ReaderDependencyValidationException(lockedReaderException);

                this.loggingBroker.LogError(readerDependencyValidationException);

                throw readerDependencyValidationException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedReaderStorageException =
                    new FailedReaderStorageException(dbUpdateException);

                var readerDependencyException =
                    new ReaderDependencyException(failedReaderStorageException);

                this.loggingBroker.LogError(readerDependencyException);

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
