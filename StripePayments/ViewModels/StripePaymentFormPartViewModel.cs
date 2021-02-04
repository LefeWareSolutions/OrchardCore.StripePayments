using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.StripePayment.ViewModels
{
    public class StripePaymentFormPartViewModel
    {
        public string IntentClientSecret { get; set; }
        public string PublishableKey { get;  set; }
    }
}
