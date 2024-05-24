
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CacheBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userId, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userId, cancellationToken);
            await cache.RemoveAsync(userId, cancellationToken);
            return true;

        }

        public async Task<ShoppingCart> GetBasket(string userId, CancellationToken cancellationToken = default)
        {
            var cacheBasket = await cache.GetStringAsync(userId, cancellationToken);

            if (!string.IsNullOrEmpty(cacheBasket)) return JsonSerializer.Deserialize<ShoppingCart>(cacheBasket)!;

            var basket = await repository.GetBasket(userId, cancellationToken) ?? new ShoppingCart();
            
            await cache.SetStringAsync(userId, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.CustomerId, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }
    }
}
