using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLibrary2.Entities;

namespace TaskLibrary2
{
    internal class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"host=localhost;port=5432;database=TaskLibrary2;username=postgres;password=meowmurrmeowMEOWsrsly");
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Library> Libraries { get; set; }

        public DbSet<Libraries> Library_Numbers { get; set; }

        public DbSet<BookOrder> BookOrders { get; set; }
    }
}
