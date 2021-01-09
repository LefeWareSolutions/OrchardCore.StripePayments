using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.StripePayment
{
    public class StripePaymentMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public StripePaymentMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("StripePaymentFormPart", builder => builder
                .Attachable()
                .WithDescription("Provides the template needed to dispaly a stripe payment form.")
                .WithField("StripeAPIKey", f => f
                    .OfType("TextField")
                    .WithSettings(new TextFieldSettings() { Required = true, Hint = "Enter your stripe API key"})
                    .WithDisplayName("Stripe API Key")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("PaymentForm", builder => builder
                .WithPart("StripePaymentFormPart", part => part.WithPosition("3"))
            );

            return 1;
        }
    }
}
