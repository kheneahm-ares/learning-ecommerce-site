using Basket.Entities;

namespace Basket.Features.Basket.DTOs
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }

        // fine to use entity isntead of dto since data is in cache and nothing needs to be hidden
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
