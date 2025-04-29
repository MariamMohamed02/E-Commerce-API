using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Services.Abstraction
{
    public interface IBasketService
    {
        // Get, DEelete, Update

        Task<BasketDto?> GetBasketAsync(string id);
        Task<bool> DeleteBAsketAsync(string id);
        Task<BasketDto?> UpdateBAsketAsync(BasketDto basket);
    }
}
