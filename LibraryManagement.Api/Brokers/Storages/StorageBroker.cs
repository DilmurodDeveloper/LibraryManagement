//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using EFxceptions;
using LibraryManagement.Api.Models.Foundations.Books;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                this.configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Reader)
                .WithMany(r => r.Books)
                .HasForeignKey(b => b.ReaderId);

            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose() { }
    }
}
