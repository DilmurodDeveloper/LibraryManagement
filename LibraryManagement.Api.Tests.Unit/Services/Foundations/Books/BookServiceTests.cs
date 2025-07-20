//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using System.Linq.Expressions;
using LibraryManagement.Api.Brokers.Loggings;
using LibraryManagement.Api.Brokers.Storages;
using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Services.Foundations.Books;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LibraryManagement.Api.Tests.Unit.Services.Foundations.Books
{
    public partial class BookServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IBookService bookService;

        public BookServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.bookService = new BookService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Book CreateRandomBook() =>
            CreateBookFiller(date: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<Book> CreateBookFiller(DateTimeOffset date)
        {
            var filler = new Filler<Book>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
