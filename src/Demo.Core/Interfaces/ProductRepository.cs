using Demo.Core.Aggregation;

namespace Demo.Core.Interfaces;
public interface IProductRepository
{
    IEnumerable<ProductSites> GetProductSites(List<int> productIds);
}
