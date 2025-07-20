//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using LibraryManagement.Api.Models.Foundations.Books;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public async Task ShouldRemoveBookByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputBookId = randomId;
            Book randomBook = CreateRandomBook();
            Book storageBook = randomBook;
            Book expectedInputBook = storageBook;
            Book deletedBook = expectedInputBook;
            Book expectedBook = deletedBook;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(inputBookId))
                    .ReturnsAsync(storageBook);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteBookAsync(expectedInputBook))
                    .ReturnsAsync(deletedBook);

            // when
            Book actualBook =
                await this.bookService.RemoveBookByIdAsync(randomId);

            // then
            actualBook.Should().BeEquivalentTo(expectedBook);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(inputBookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteBookAsync(expectedInputBook),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
