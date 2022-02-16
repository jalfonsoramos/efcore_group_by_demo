namespace Demo.Core.Aggregation;

public class ProductSites
{
    public string Product { get; set; }

    public IEnumerable<string> Sites { get; set; }
}