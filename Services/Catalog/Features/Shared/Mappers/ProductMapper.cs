using Catalog.Entities;
using Catalog.Features.Products.DTOs;

namespace Catalog.Features.Shared.Mappers
{
    public static class ProductMapper
    {
        // map from ProductResponse to Product
        public static Product ToEntity(this ProductResponse response)
        {
            return new Product
            {
                Name = response.Name,
                Summary = response.Summary,
                Description = response.Description,
                ImageFile = response.ImageFile,
                Price = response.Price,
                CreatedDate = response.CreatedDate,
                Brand = response.Brand.ToEntity(),
                Type = response.Type.ToEntity()
            };
        }

        // map from Product to ProductResponse
        public static ProductResponse ToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Summary = product.Summary,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Price = product.Price,
                CreatedDate = product.CreatedDate,
                Brand = product.Brand.ToResponse(),
                Type = product.Type.ToResponse()
            };
        }

    }
}
