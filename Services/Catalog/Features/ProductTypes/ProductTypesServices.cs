using Catalog.Entities;
using Catalog.Features.ProductTypes.DTOs;
using Catalog.Features.Shared;
using MongoDB.Driver;

namespace Catalog.Features.ProductTypes
{
    public class ProductTypesServices
    {
        private readonly IMongoCollection<ProductBrand> _collection;

        public ProductTypesServices(IMongoDatabase mongoDB, IConfiguration config)
        {
            _collection = mongoDB.GetCollection<ProductBrand>(config["DatabaseSettings:TypeCollectionName"]);
        }


        public async Task<ServiceResult<IEnumerable<ProductTypeResponse>>> GetAll()
        {
            var allBrands =
                (await _collection.Find(_ => true).ToListAsync())
                .Select(b => new ProductTypeResponse { Name = b.Name });

            return new ServiceResult<IEnumerable<ProductTypeResponse>>
            {
                Data = allBrands,
                IsSuccessful = true
            };
        }

        public async Task<ServiceResult<ProductTypeResponse>> GetById(string id)
        {
            var brand = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (brand == null)
            {
                return new ServiceResult<ProductTypeResponse>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Product type not found",
                    ErrorType = ErrorType.NotFound
                };
            }
            return new ServiceResult<ProductTypeResponse>
            {
                Data = new ProductTypeResponse { Name = brand.Name },
                IsSuccessful = true
            };
        }
    }
}
