using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore.TenantBilling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;
using Stripe;
using Stripe.Checkout;

namespace LefeWareLearning.StripePayment
{
    [Feature(StripePaymentConstants.Features.StripeSubscription)]
    public class StripeSubscriptionPaymentController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ShellSettings _shellSettings;


        public StripeSubscriptionPaymentController(IAuthorizationService authorizationService, ShellSettings shellSettings)
        {
            _authorizationService = authorizationService;
            _shellSettings = shellSettings;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageTenantBilling))
            {
                return Unauthorized();
            }

            var options = new SessionCreateOptions
            {
                CustomerEmail = User.Identity.Name,
                PaymentMethodTypes = new List<string> { "card" },
                SubscriptionData = new SessionSubscriptionDataOptions
                {
                    Items = new List<SessionSubscriptionDataItemOptions>
                    {
                        new SessionSubscriptionDataItemOptions {Plan = "StripePlanKey"},
                    },
                    Metadata = new Dictionary<string, string>()
                    {
                        { "TenantId", _shellSettings["Secret"]},
                        { "TenantName", _shellSettings.Name }
                    }
                },
                SuccessUrl = $"https://{HttpContext.Request.Host.Value}/{_shellSettings.Name}/admin/LefeWareLearning.StripePayment/paymentsuccess?sessionid={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"https://{HttpContext.Request.Host.Value}/{_shellSettings.Name}/admin",
            };

            var service = new SessionService();

            var session = service.Create(options);

            ViewBag.SessionId = session.Id;
            ViewBag.StripePublicKey = "StripePublicKey";
            return View();
        }

        public IActionResult PaymentSuccess(string sessionId)
        {
            return View();
        }


    }
}
