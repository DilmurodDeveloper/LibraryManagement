//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someBookId = Guid.NewGuid();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedBookException =
                new LockedBookException(dbUpdateConcurrencyException);

            var expectedBookDependencyValidationException =
                new BookDependencyValidationException(lockedBookException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Book> removeBookById =
                this.bookService.RemoveBookByIdAsync(someBookId);

            BookDependencyValidationException actualBookDependencyValidationException =
                await Assert.ThrowsAsync<BookDependencyValidationException>(() =>
                    removeBookById.AsTask());

            // then
            actualBookDependencyValidationException.Should()
                .BeEquivalentTo(expectedBookDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteBookAsync(It.IsAny<Book>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
