using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        //GetBasket
        //DeletBasket
        //CreateOrUpdateBasket

        Task <CustomerBasket?> GetBasketAsync (string id);
        Task <bool> DeleteBasketAsync(string id);
        Task <CustomerBasket?> UpdateBasket(CustomerBasket basket, TimeSpan? timeToLive=null);
    }
}
