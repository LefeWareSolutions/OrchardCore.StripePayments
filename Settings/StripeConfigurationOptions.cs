using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LefeWareLearning.StripePayment
{
    public class StripeConfigurationOptions
    {
        public string WebhookSecret { get; set; }
        public string StripeAPIKey { get; set; }
        public string StripePlanKey { get; set; }
        public string StripePublicKey { get; set; }
    }
}
