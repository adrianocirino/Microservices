using System;
using System.Text.Json;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        public async Task<ShoppingCart> Getbasket(string userName)
        {
            // tá retornando um array de bytes, então precisa desserializar no formato json
            var basket = await _redisCache.GetStringAsync(userName);

            if (basket is null) return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            // serializa a cesta para armazenar no redis, pq lá trabalha com array de bytes.
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
            
            return await Getbasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
