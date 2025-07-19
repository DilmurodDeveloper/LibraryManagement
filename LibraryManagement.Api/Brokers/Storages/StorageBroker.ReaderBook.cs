//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.ReaderBooks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ReaderBook> ReaderBooks { get; set; }
    }
}
