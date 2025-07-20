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
        public async Task ShouldModifyBookAsync()
        {
            // given
            Book randomBook = CreateRandomBook();
            Book inputBook = randomBook;
            Book persistedBook = inputBook;
            Book updatedBook = inputBook;
            Book expectedBook = updatedBook;
            Guid InputBookId = inputBook.BookId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(InputBookId))
                    .ReturnsAsync(persistedBook);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateBookAsync(inputBook))
                    .ReturnsAsync(updatedBook);

            // when
            Book actualBook =
                await this.bookService
                    .ModifyBookAsync(inputBook);

            // then
            actualBook.Should().BeEquivalentTo(expectedBook);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(InputBookId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateBookAsync(inputBook),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
