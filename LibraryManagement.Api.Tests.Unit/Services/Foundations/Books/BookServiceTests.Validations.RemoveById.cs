//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidBookId = Guid.Empty;

            var invalidBookException = new InvalidBookException();

            invalidBookException.AddData(
                key: nameof(Book.BookId),
                values: "Id is required");

            var expectedBookValidationException =
                new BookValidationException(invalidBookException);

            // when
            ValueTask<Book> removeBookById =
                this.bookService.RemoveBookByIdAsync(invalidBookId);

            BookValidationException actualBookValidationException =
                await Assert.ThrowsAsync<BookValidationException>(() =>
                    removeBookById.AsTask());

            // then
            actualBookValidationException.Should()
                .BeEquivalentTo(expectedBookValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteBookAsync(It.IsAny<Book>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveBookByIdIsNotFoundAndLogItAsync()
        {
            // given
            Guid inputBookId = Guid.NewGuid();
            Book noBook = null;

            var notFoundBookException =
                new NotFoundBookException(inputBookId);

            var expectedBookValidationException =
                new BookValidationException(notFoundBookException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noBook);

            // when
            ValueTask<Book> removeBookByIdTask =
                this.bookService.RemoveBookByIdAsync(inputBookId);

            var actualBookValidationException =
                await Assert.ThrowsAsync<BookValidationException>(() =>
                    removeBookByIdTask.AsTask());

            // then
            actualBookValidationException.Should()
                .BeEquivalentTo(expectedBookValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedBookValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteBookAsync(It.IsAny<Book>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
