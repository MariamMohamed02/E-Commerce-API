using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Services.Abstraction
{
    public interface IPaymentService
    {
        // Create or Update PaymentIntent
        public Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
