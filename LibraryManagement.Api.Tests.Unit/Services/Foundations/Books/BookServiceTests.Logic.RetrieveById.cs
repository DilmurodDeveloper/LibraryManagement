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
        public async Task ShouldRetrieveBookByIdAsync()
        {
            // given
            Guid randomBookId = Guid.NewGuid();
            Guid inputBookId = randomBookId;
            Book randomBook = CreateRandomBook();
            Book storgaeBook = randomBook;
            Book expectedBook = storgaeBook;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectBookByIdAsync(inputBookId))
                    .ReturnsAsync(storgaeBook);

            // when
            Book actualBook =
                await this.bookService
                    .RetrieveBookByIdAsync(inputBookId);

            // then
            actualBook.Should().BeEquivalentTo(expectedBook);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectBookByIdAsync(inputBookId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
