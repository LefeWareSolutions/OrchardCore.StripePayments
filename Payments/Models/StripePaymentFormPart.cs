using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.StripePayment
{
    public class StripePaymentFormPart : ContentPart
    {
        public TextField StripePublishableKey { get; set; }
        public TextField StripeSecretKey { get; set; }
    }
}
