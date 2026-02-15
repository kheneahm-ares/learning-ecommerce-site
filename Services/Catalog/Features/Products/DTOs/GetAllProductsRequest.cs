namespace Catalog.Features.Products.DTOs
{
    public class GetAllProductsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // filters
        public string? SortBy { get; set; }
        public string? Search { get; set; }
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
    }
}
