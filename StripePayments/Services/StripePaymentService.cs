using System.Collections.Generic;
using System.Threading.Tasks;
using LefeWareLearning.StripePayment;
using LefeWareSolutions.Payments.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace OrchardCore.StripePayment.Services
{
    public class StripePaymentService : IStripePaymentService
    {


        public Task<PaymentIntent> CreatePaymentIntent(string stripeSecretKey, long cost, string currency, Dictionary<string, string> metadata)
        {
            StripeConfiguration.ApiKey = stripeSecretKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = cost,
                Currency = currency,
                PaymentMethodTypes = new List<string>(){"card"},
                Metadata = metadata
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Task.FromResult<PaymentIntent>(paymentIntent);
        }
    }
}
