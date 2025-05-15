using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


//StripeConfiguration.ApiKey = "sk_test_51RMu4SPIMw402cCUBrmIJBPYCwKUrpLO71p7i2pG4nhm7xqkiVpLDaqGnVf2b8VDLdrCQ5bOJS5wYHXD2Qhv4CQg00wqhyTdVF";

namespace Presentation

{
    public class PaymentsController(IServiceManager serviceManager ): ApiController
    {

        // If you are testing your webhook locally with the Stripe CLI you
        // can find the endpoint's secret by running `stripe listen`
        // Otherwise, find your endpoint's secret in your webhook settings
        // in the Developer Dashboard
        //const string endpointSecret = "whsec_...";

        [HttpPost("webhook")]
        public async Task<ActionResult> webHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeHeaders = Request.Headers["Stripe-Signature"];
            await serviceManager.PaymentService.UpdatePaymentStatus(json, stripeHeaders);
            return new EmptyResult();
          
        }





        // ----------------------------------------------------------------------------------------
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }



    }
}
