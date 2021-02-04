using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace OrchardCore.StripePayment.Services
{
    public interface IStripePaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(string stripeSecretKey, long cost, string currency, Dictionary<string,string> metadata);
    }
}
