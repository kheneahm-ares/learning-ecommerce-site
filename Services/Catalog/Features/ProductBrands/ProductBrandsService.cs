using Catalog.Entities;
using Catalog.Features.ProductBrands.DTOs;
using Catalog.Features.Shared;
using MediatR.NotificationPublishers;
using MongoDB.Driver;

namespace Catalog.Features.ProductBrands
{
    public class ProductBrandsService
    {
        private readonly IMongoCollection<ProductBrand> _collection;

        public ProductBrandsService(IMongoDatabase mongoDB, IConfiguration config)
        {
            _collection = mongoDB.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
        }


        public async Task<ServiceResult<IEnumerable<ProductBrandResponse>>> GetAll()
        {
            var allBrands =
                (await _collection.Find(_ => true).ToListAsync())
                .Select(b => new ProductBrandResponse { Name = b.Name });

            return new ServiceResult<IEnumerable<ProductBrandResponse>>
            {
                Data = allBrands,
                IsSuccessful = true
            };
        }

        public async Task<ServiceResult<ProductBrandResponse>> GetById(string id)
        {
            var brand = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (brand == null)
            {
                return new ServiceResult<ProductBrandResponse>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Product brand not found",
                    ErrorType = ErrorType.NotFound
                };
            }
            return new ServiceResult<ProductBrandResponse>
            {
                Data = new ProductBrandResponse { Name = brand.Name },
                IsSuccessful = true
            };
        }
    }
}
