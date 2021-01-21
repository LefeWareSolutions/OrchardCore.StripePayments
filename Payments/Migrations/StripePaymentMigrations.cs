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
                .WithDescription("Provides the template needed to display a stripe payment form.")
                .WithField("StripePublishableKey", f => f
                    .OfType("TextField")
                    .WithSettings(new TextFieldSettings() { Required = true, Hint = "Enter your stripe publishable API key (https://stripe.com/docs/keys)" })
                    .WithDisplayName("Stripe Publishable Key")
                )
                .WithField("StripeSecretKey", f => f
                    .OfType("TextField")
                    .WithSettings(new TextFieldSettings() { Required = true, Hint = "Enter your stripe secret API key (https://stripe.com/docs/keys)" })
                    .WithDisplayName("Stripe Secret Key")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("StripePaymentForm", builder => builder
                .Creatable()
                .Listable()
                .WithPart("TitlePart", part => part.WithPosition("1"))
                .WithPart("PaymentPart", part => part.WithPosition("2"))
                .WithPart("StripePaymentFormPart", part => part.WithPosition("3"))
            );

            return 1;
        }
    }
}
