using LefeWareLearning.StripePayment;
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StripePayment",
    Author = "LefeWare Solutions",
    Website = "https://LefeWareSolutions.com",
    Version = "1.0.0",
    Category = "Payment"
)]

[assembly: Feature(
    Id = StripePaymentConstants.Features.StripePayment,
    Name = "Stripe Payment",
    Category = "Payment",
    Description = "Allows users to use stripe as their payment method",
    Dependencies = new[]
    {
        "OrchardCore.PaymentForm",
    }
)]

[assembly: Feature(
    Id = StripePaymentConstants.Features.StripeSubscription,
    Name = "Stripe Subscriptions",
    Category = "Payment",
    Description = "Allows users to use stripe for reocurring payment subscriptions",
    Dependencies = new[]
    {
        "OrchardCore.TenantBilling",
    }
)]
