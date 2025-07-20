//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using LibraryManagement.Api.Models.Foundations.Books;
using Moq;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllBooks()
        {
            // given
            IQueryable<Book> randomBook = CreateRandomBooks();
            IQueryable<Book> storageBook = randomBook;
            IQueryable<Book> expectedBook = storageBook.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllBooks())
                    .Returns(storageBook);

            // when
            IQueryable<Book> actualBook =
                this.bookService.RetrieveAllBooks();

            // then
            actualBook.Should().BeEquivalentTo(expectedBook);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllBooks(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
