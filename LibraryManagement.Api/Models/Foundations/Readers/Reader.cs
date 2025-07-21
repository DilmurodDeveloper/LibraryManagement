//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using System.Text.Json.Serialization;
using LibraryManagement.Api.Models.Foundations.Books;

namespace LibraryManagement.Api.Models.Foundations.Readers
{
    public class Reader
    {
        public Guid ReaderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

        [JsonIgnore]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public List<Book> Books { get; set; } = new();
    }
}
