using System.Collections.Generic;

namespace Demo.Domain.DTOs
{
    public class ProductSites
    {
        public string Product { get; set; }

        public IEnumerable<string> Sites{get;set;}
    }
}