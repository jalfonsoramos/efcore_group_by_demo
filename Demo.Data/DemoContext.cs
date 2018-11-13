using System;
using Demo.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class DemoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<ProductSite> ProductSites { get; set; }

        public DemoContext()
        { }

        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
