using Catalog.Entities;
using Catalog.Features.ProductBrands.DTOs;

namespace Catalog.Features.Shared.Mappers
{
    public static class ProductBrandMapper
    {
        // map from ProductBrandResponse to ProductBrand
        public static ProductBrand ToEntity(this ProductBrandResponse response)
        {
            return new ProductBrand
            {
                Name = response.Name
            };
        }

        // map from ProductBrand to ProductBrandResponse
        public static ProductBrandResponse ToResponse(this ProductBrand brand)
        {
            return new ProductBrandResponse
            {
                Name = brand.Name
            };
        }
    }
}
