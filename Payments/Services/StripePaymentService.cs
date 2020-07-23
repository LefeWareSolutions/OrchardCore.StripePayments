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
        private readonly StripeConfigurationOptions _stripeConfig;

        public StripePaymentService(IOptions<StripeConfigurationOptions> stripeConfig)
        {
            _stripeConfig = stripeConfig.Value;
        }

        public async Task<PaymentIntent> CreatePaymentIntent(long cost, string currency)
        {
            StripeConfiguration.ApiKey = _stripeConfig.StripeAPIKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = cost,
                Currency = currency,
                Metadata = new Dictionary<string, string>
                {
                  { "test", "test123" },
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return paymentIntent;
        }
    }
}
