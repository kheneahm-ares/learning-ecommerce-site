using Catalog.Entities;
using Catalog.Features.ProductTypes.DTOs;

namespace Catalog.Features.Shared.Mappers
{
    public static class ProductTypeMapper
    {
        // map from ProductTypeResponse to ProductType
        public static ProductType ToEntity(this ProductTypeResponse response)
        {
            return new ProductType
            {
                Name = response.Name
            };
        }

        // map from ProductType to ProductTypeResponse
        public static ProductTypeResponse ToResponse(this ProductType type)
        {
            return new ProductTypeResponse
            {
                Name = type.Name
            };
        }
    }
}
