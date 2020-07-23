using System.Threading.Tasks;
using Stripe;

namespace OrchardCore.StripePayment.Services
{
    public interface IStripePaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(long cost, string currency);
    }
}
