using Basket.Entities;
using Basket.Features.Basket.DTOs;
using Basket.Features.Shared.Mappers;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Features.Basket
{
    public class BasketService
    {
        private readonly IDistributedCache _redishCache;

        public BasketService(IDistributedCache redishCache)
        {
            _redishCache = redishCache;
        }

        // delete basket
        public async Task DeleteBasket(string userName)
        {
            await _redishCache.RemoveAsync(userName);
        }

        // get basket by username 
        public async Task<ShoppingCartResponse> GetBasket(string userName)
        {
            var basket = await _redishCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
            {
                // if there is no basket for the user, return an empty basket
                return new ShoppingCartResponse
                {
                    UserName = userName,
                    Items = new List<ShoppingCartItem>()
                };
            }

            return JsonSerializer.Deserialize<ShoppingCartResponse>(basket);
        }

        // upsert basket (create and update)
        public async Task<ShoppingCartResponse> UpsertBasket(CreateShoppingCartRequest request)
        {
            var entity = request.ToEntity();
            var jsonCart = JsonSerializer.Serialize(entity);

            await _redishCache.SetStringAsync(request.Username, jsonCart);

            var basket = await GetBasket(request.Username);

            return basket;
        }
    }
}
