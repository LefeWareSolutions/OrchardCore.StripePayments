using OrchardCore.DisplayManagement;

namespace OrchardCore.StripePayment.StripePayments.ViewModels
{
    public class StripePaymentFormViewModel
    {
        public string PaymentIntentClientSecret { get; set; }
        public string StripePublishableAPIKey { get;  set; }
        public IShape Shape { get; set; }
    }
}
