using System.Collections.Generic;
using System.Linq;
using Demo.Data;
using Demo.Domain.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.DataTests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void When_get_products_with_related_sites()
        {
            // In-memory database only exists while the connection is open
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
                    var service = new ProductService(context);
                    var result = service.GetProductSites(new List<int> { 1, 2, 3 });

                    Assert.AreEqual(3, result.Count());

                    //Product 1
                    Assert.AreEqual("Product A", result.ToArray()[0].Product);
                    Assert.AreEqual(3,result.ToArray()[0].Sites.Count());
                    Assert.AreEqual("Site A", result.ToArray()[0].Sites.ToArray()[0]);
                    Assert.AreEqual("Site B", result.ToArray()[0].Sites.ToArray()[1]);
                    Assert.AreEqual("Site C", result.ToArray()[0].Sites.ToArray()[2]);

                    //Product 2
                    Assert.AreEqual("Product B", result.ToArray()[1].Product);
                    Assert.AreEqual(2,result.ToArray()[1].Sites.Count());
                    Assert.AreEqual("Site A", result.ToArray()[1].Sites.ToArray()[0]);
                    Assert.AreEqual("Site D", result.ToArray()[1].Sites.ToArray()[1]);

                    //Product 3
                    Assert.AreEqual("Product C", result.ToArray()[2].Product);
                    Assert.AreEqual("Site C", result.ToArray()[2].Sites.ToArray()[0]);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
