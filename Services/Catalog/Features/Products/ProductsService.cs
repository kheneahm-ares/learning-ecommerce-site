using Catalog.Entities;
using Catalog.Features.ProductBrands;
using Catalog.Features.Products.DTOs;
using Catalog.Features.ProductTypes;
using Catalog.Features.Shared;
using Catalog.Features.Shared.Mappers;
using Catalog.Specifications;
using MongoDB.Driver;
using static System.Net.WebRequestMethods;

namespace Catalog.Features.Products
{
    public class ProductsService
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly ProductBrandsService _brandsService;
        private readonly ProductTypesServices _typesService;

        public ProductsService(IMongoDatabase mongoDB,
                               IConfiguration config,
                               ProductBrandsService brandsService,
                               ProductTypesServices typesService)
        {
            _collection = mongoDB.GetCollection<Product>(config["DatabaseSettings:ProductCollectionName"]);
            _brandsService = brandsService;
            _typesService = typesService;
        }

        // get all products
        public async Task<ServiceResult<IEnumerable<ProductResponse>>> GetAllProducts()
        {
            var products = await _collection.Find(_ => true).ToListAsync();
            var response = products.Select(p => p.ToResponse());
            return new ServiceResult<IEnumerable<ProductResponse>>
            {
                Data = response,
                IsSuccessful = true
            };
        }

        // paginated get all products
        public async Task<ServiceResult<Pagination<ProductResponse>>> GetProducts(GetAllProductsRequest request)
        {
            var totalItems = await _collection.CountDocumentsAsync(_ => true);

            var filterDef = BuildFilter(request);
            var sortDef = BuildSort(request);

            var products = await _collection.Find(filterDef)
                                            .Sort(sortDef)
                                            .Skip((request.PageNumber - 1) * request.PageSize)
                                            .Limit(request.PageSize)
                                            .ToListAsync();

            var response = products.Select(p => p.ToResponse());

            var pagination = new Pagination<ProductResponse>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                Count = (int)totalItems,
                Data = response
            };

            return new ServiceResult<Pagination<ProductResponse>>
            {
                Data = pagination,
                IsSuccessful = true
            };
        }

        // get product by id
        public async Task<ServiceResult<ProductResponse>> GetProductById(string id)
        {
            var product = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return new ServiceResult<ProductResponse>
                {
                    IsSuccessful = false,
                    ErrorMessage = $"Product with id {id} not found",
                    ErrorType = ErrorType.NotFound
                };
            }
            var response = product.ToResponse();
            return new ServiceResult<ProductResponse>
            {
                Data = response,
                IsSuccessful = true
            };
        }


        public async Task<ServiceResult<ProductResponse>> CreateProduct(CreateProductRequest request)
        {
            var serviceResult = new ServiceResult<ProductResponse>();

            var tplBrandType = await IsValidBrandAndType(request);

            if (tplBrandType == null)
            {
                serviceResult.Failure($"Invalid Brand or Type", ErrorType.BadRequest);
                return serviceResult;
            }

            var productBrand = tplBrandType.Item1;
            var productType = tplBrandType.Item2;

            var product = new Product
            {
                Name = request.Name,
                Summary = request.Summary,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                Brand = productBrand,
                Type = productType
            };

            await _collection.InsertOneAsync(product);

            var response = product.ToResponse();

            serviceResult.Success(response);

            return serviceResult;
        }

        // update product 
        public async Task<ServiceResult<bool>> UpdateProduct(UpdateProductRequest request)
        {
            var serviceResult = new ServiceResult<bool>();

            var tplBrandType = await IsValidBrandAndType(request);

            if (tplBrandType == null)
            {
                serviceResult.Failure($"Invalid Brand or Type", ErrorType.BadRequest);
                return serviceResult;
            }

            var productBrand = tplBrandType.Item1;
            var productType = tplBrandType.Item2;

            var updateDef = Builders<Product>.Update
                .Set(p => p.Name, request.Name)
                .Set(p => p.Summary, request.Summary)
                .Set(p => p.Description, request.Description)
                .Set(p => p.ImageFile, request.ImageFile)
                .Set(p => p.Price, request.Price)
                .Set(p => p.Brand, productBrand)
                .Set(p => p.Type, productType);
            var result = await _collection.UpdateOneAsync(p => p.Id == request.Id, updateDef);
            if (result.MatchedCount == 0)
            {
                serviceResult.Failure($"Product with id {request.Id} not found", ErrorType.NotFound);
                return serviceResult;
            }
            serviceResult.Success(true);
            return serviceResult;
        }

        // delete product by id
        public async Task<ServiceResult<bool>> DeleteProduct(string id)
        {
            var serviceResult = new ServiceResult<bool>();
            var result = await _collection.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0)
            {
                serviceResult.Failure($"Product with id {id} not found", ErrorType.NotFound);
                return serviceResult;
            }
            serviceResult.Success(true);
            return serviceResult;
        }


        // get products by brand name
        public async Task<ServiceResult<IEnumerable<ProductResponse>>> GetProductsByBrand(string brandName)
        {
            var filterDef = Builders<Product>.Filter.Where(p => p.Brand.Name.ToLower() == brandName.ToLower());
            var products = await _collection.Find(filterDef).ToListAsync();
            var response = products.Select(p => p.ToResponse());
            return new ServiceResult<IEnumerable<ProductResponse>>
            {
                Data = response,
                IsSuccessful = true
            };
        }

        // get products by type name
        public async Task<ServiceResult<IEnumerable<ProductResponse>>> GetProductsByType(string typeName)
        {
            var filterDef = Builders<Product>.Filter.Where(p => p.Type.Name.ToLower() == typeName.ToLower());
            var products = await _collection.Find(filterDef).ToListAsync();
            var response = products.Select(p => p.ToResponse());
            return new ServiceResult<IEnumerable<ProductResponse>>
            {
                Data = response,
                IsSuccessful = true
            };
        }

        private SortDefinition<Product> BuildSort(GetAllProductsRequest request)
        {
            var sortBuilder = Builders<Product>.Sort;
            return request.SortBy switch
            {
                "priceAsc" => sortBuilder.Ascending(p => p.Price),
                "priceDesc" => sortBuilder.Descending(p => p.Price),
                _ => sortBuilder.Ascending(p => p.Name)
            };
        }


        private FilterDefinition<Product> BuildFilter(GetAllProductsRequest request)
        {
            // build filter using builder obj from MongoDB.Driver
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty; // filter definition
            if (!string.IsNullOrEmpty(request.Search))
            {
                filter &= filterBuilder.Where(p => p.Name.ToLower().Contains(request.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.BrandId))
            {
                filter &= filterBuilder.Where(p => p.Brand.Id == request.BrandId);
            }
            if (!string.IsNullOrEmpty(request.TypeId))
            {
                filter &= filterBuilder.Where(p => p.Type.Id == request.TypeId);
            }
            return filter;
        }

        private async Task<Tuple<ProductBrand, ProductType>> IsValidBrandAndType(CreateProductRequest request)
        {
            var brandResult = await _brandsService.GetById(request.BrandId);
            var typeResult = await _typesService.GetById(request.TypeId);
            if (brandResult.Data == null || typeResult.Data == null)
            {
                return null;
            }
            return new Tuple<ProductBrand, ProductType>(brandResult.Data.ToEntity(), typeResult.Data.ToEntity());

        }

    }
}
