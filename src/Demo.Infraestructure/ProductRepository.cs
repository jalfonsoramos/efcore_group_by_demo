using Demo.Core.Aggregation;
using Demo.Core.Interfaces;

namespace Demo.Infraestructure;

public class ProductRepository : IProductRepository
{
    private readonly DemoContext _context;

    public ProductRepository(DemoContext context)
    {
        _context = context;
    }

    public IEnumerable<ProductSites> GetProductSites(List<int> productIds)
    {
        var query = from p in _context.Products
                    join ps in _context.ProductSites on p.Id equals ps.ProductId
                    join s in _context.Sites on ps.SiteId equals s.Id
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