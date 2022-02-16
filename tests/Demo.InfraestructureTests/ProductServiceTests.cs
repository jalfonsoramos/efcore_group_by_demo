using System.Collections.Generic;
using System.Linq;
using Demo.Core.Aggregation;
using Demo.Core.Entities;
using Demo.Core.Interfaces;
using Demo.Infraestructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Demo.DataTests;

public class ProductServiceTests
{
    [Fact]
    public void When_get_products_with_related_sites()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        try
        {
            var options = new DbContextOptionsBuilder<DemoContext>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database and seed data
            using (var context = new DemoContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Products.Add(new Product { Name = "Product A" });//1
                context.Products.Add(new Product { Name = "Product B" });//2
                context.Products.Add(new Product { Name = "Product C" });//3
                context.SaveChanges();

                context.Sites.Add(new Site { Name = "Site A" });//1
                context.Sites.Add(new Site { Name = "Site B" });//2
                context.Sites.Add(new Site { Name = "Site C" });//3
                context.Sites.Add(new Site { Name = "Site D" });//4
                context.SaveChanges();

                context.ProductSites.Add(new ProductSite { ProductId = 1, SiteId = 1 });
                context.ProductSites.Add(new ProductSite { ProductId = 1, SiteId = 2 });
                context.ProductSites.Add(new ProductSite { ProductId = 1, SiteId = 3 });
                context.ProductSites.Add(new ProductSite { ProductId = 2, SiteId = 1 });
                context.ProductSites.Add(new ProductSite { ProductId = 2, SiteId = 4 });
                context.ProductSites.Add(new ProductSite { ProductId = 3, SiteId = 3 });
                context.SaveChanges();
            }

            //  Run the test against one instance of the context
            using (var context = new DemoContext(options))
            {
                IProductRepository service = new ProductRepository(context);
                IEnumerable<ProductSites> result = service.GetProductSites(new List<int> { 1, 2, 3 });

                Assert.Equal(3, result.Count());

                // Product 1
                Assert.Equal("Product A", result.ToArray()[0].Product);
                Assert.Equal(3, result.ToArray()[0].Sites.Count());
                Assert.Equal("Site A", result.ToArray()[0].Sites.ToArray()[0]);
                Assert.Equal("Site B", result.ToArray()[0].Sites.ToArray()[1]);
                Assert.Equal("Site C", result.ToArray()[0].Sites.ToArray()[2]);

                // Product 2
                Assert.Equal("Product B", result.ToArray()[1].Product);
                Assert.Equal(2, result.ToArray()[1].Sites.Count());
                Assert.Equal("Site A", result.ToArray()[1].Sites.ToArray()[0]);
                Assert.Equal("Site D", result.ToArray()[1].Sites.ToArray()[1]);

                // Product 3
                Assert.Equal("Product C", result.ToArray()[2].Product);
                Assert.Equal("Site C", result.ToArray()[2].Sites.ToArray()[0]);
            }
        }
        finally
        {
            connection.Close();
        }
    }
}
