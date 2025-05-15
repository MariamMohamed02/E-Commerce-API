using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;

namespace Presentation
{
    [Authorize]
    public class BasketController(IServiceManager _serviceManager): ApiController
    {

        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.UpdateBAsketAsync(basketDto);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _serviceManager.BasketService.DeleteBAsketAsync(id);
            return NoContent();
        }
    }
}
