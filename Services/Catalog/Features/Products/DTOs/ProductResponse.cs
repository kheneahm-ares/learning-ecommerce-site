using Catalog.Entities;
using Catalog.Features.ProductBrands.DTOs;
using Catalog.Features.ProductTypes.DTOs;

namespace Catalog.Features.Products.DTOs
{
    public record ProductResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public string ImageFile { get; init; }
        public ProductBrandResponse Brand { get; init; }
        public ProductTypeResponse Type { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
