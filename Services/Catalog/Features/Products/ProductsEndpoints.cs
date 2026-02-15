using Catalog.Features.Products.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.Products
{
    public static class ProductsEndpoints
    {

        public static RouteGroupBuilder MapProductsEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([AsParameters] GetAllProductsRequest request, ProductsService service) => await service.GetProducts(request));
            group.MapGet("/{id}", async (string id, ProductsService service) => await service.GetProductById(id));

            group.MapPost("/", async (CreateProductRequest request, ProductsService service) => await service.CreateProduct(request));
            group.MapPut("/", async (UpdateProductRequest request, ProductsService service) => await service.UpdateProduct(request));
            group.MapDelete("/{id}", async (string id, ProductsService service) => await service.DeleteProduct(id));

            return group;
        }

    }
}
