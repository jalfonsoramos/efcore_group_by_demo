using Demo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infraestructure;

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

    }
}
