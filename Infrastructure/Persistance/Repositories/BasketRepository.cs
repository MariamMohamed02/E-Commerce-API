using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using StackExchange.Redis;

namespace Persistance.Repositories
{
    public class BasketRepository (IConnectionMultiplexer connectionMultiplexer): IBasketRepository
    {

        private readonly IDatabase _database= connectionMultiplexer.GetDatabase();
        public async Task<bool> DeleteBasketAsync(string id)
         =>await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var value= await _database.StringGetAsync(id);
            if (value.IsNullOrEmpty) return null;
            // convert the json format from the inmemory db into a c# object
            return JsonSerializer.Deserialize<CustomerBasket?>(value);

        }

        public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);  
            var isCreatedrUpdated = await _database.StringSetAsync(basket.Id,jsonBasket, timeToLive?? TimeSpan.FromDays(30));  // if timespan is null, make it 30 days
            return isCreatedrUpdated? await GetBasketAsync(basket.Id) : null;
        }
    }

}
