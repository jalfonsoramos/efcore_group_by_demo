using System.Collections.Generic;
using System;
using System.Linq;
using Demo.Domain.DTOs;

namespace Demo.Data
{
    public class ProductService
    {
        private readonly DemoContext context;

        public ProductService(DemoContext context)
        {
            this.context = context;
        }

        public IEnumerable<ProductSites> GetProductSites(List<int> productIds)
        {
            var query = from p in context.Products
                        join ps in context.ProductSites on p.Id equals ps.ProductId
                        join s in context.Sites on ps.SiteId equals s.Id
                        where productIds.Contains(p.Id)
                        select new { ProductName = p.Name, SiteName = s.Name } into result
                        group result by result.ProductName into g
                        select new { ProductName = g.Key, Sites = g.Select(x => x.SiteName) };

            if (query.Any())
            {
                return query.ToList().Select(x => new ProductSites { Product = x.ProductName, Sites = x.Sites });
            }

            return Enumerable.Empty<ProductSites>();
        }
    }
}