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

        public StripePaymentFormPartDisplay(IStripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
    }

        public override async Task<IDisplayResult> DisplayAsync(StripePaymentFormPart stripePaymentFormPart, BuildPartDisplayContext context)
        {
            var paymentPart = stripePaymentFormPart.ContentItem.Get<PaymentPart>("PaymentPart");
            var cost = (long)(paymentPart.Cost * 100);
            var currency = paymentPart.Currency.Text;
            var paymentIntent = await _stripePaymentService.CreatePaymentIntent(cost, currency, stripePaymentFormPart.StripeSecretKey.Text);

            return Initialize<StripePaymentFormPartViewModel>("StripePaymentFormPart", m =>
            {
                m.PublishableKey = stripePaymentFormPart.StripePublishableKey.Text;
                m.IntentClientSecret = paymentIntent.ClientSecret;
            })
            .Location("Detail", "Content:5")
            .Location("Summary", "Content:5");
        }
    }
}
