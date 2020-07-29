using System.Threading.Tasks;
using GraphQL;
using LefeWareLearning.StripePayment;
using LefeWareSolutions.Payments.Models;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.StripePayment.Services;
using OrchardCore.StripePayment.ViewModels;

namespace OrchardCore.StripePayment
{
    public class StripePaymentFormPartDisplay : ContentPartDisplayDriver<StripePaymentFormPart>
    {
        private readonly IStripePaymentService _stripePaymentService;
        private readonly StripeConfigurationOptions _stripeConfig;

        public StripePaymentFormPartDisplay(IStripePaymentService stripePaymentService, IOptions<StripeConfigurationOptions> stripeConfig)
        {
            _stripeConfig = stripeConfig.Value;
            _stripePaymentService = stripePaymentService;
        }

        public override async Task<IDisplayResult> DisplayAsync(StripePaymentFormPart stripePaymentFormPart, BuildPartDisplayContext context)
        {
            var paymentPart = stripePaymentFormPart.ContentItem.Get<PaymentPart>("PaymentPart");
            var cost = (long)paymentPart.Cost;
            var currency = paymentPart.Currency.Text;
            var paymentIntent = await _stripePaymentService.CreatePaymentIntent(cost, currency);

            return Initialize<StripePaymentFormPartViewModel>("StripePaymentFormPart", m =>
            {
                m.PublishableKey = _stripeConfig.StripePublicKey;
                m.IntentClientSecret = paymentIntent.ClientSecret;
            })
            .Location("Detail", "Content:5")
            .Location("Summary", "Content:5");
        }
    }
}
