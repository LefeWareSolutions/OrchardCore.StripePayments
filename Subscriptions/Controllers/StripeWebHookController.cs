using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OrchardCore.TenantBilling;
using OrchardCore.TenantBilling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.TenantBilling.EventHandlers;
using Stripe;

namespace LefeWareLearning.StripePayment.Controllers
{
    [Route("api/stripewebhook")]
    [ApiController]
    [IgnoreAntiforgeryToken, AllowAnonymous]
    public class StripeWebHookController : Controller
    {
        private const string WebhookSecret = "";
        private readonly ILogger<StripeWebHookController> _logger;
        private readonly IServiceProvider _serviceProvider;
        public StripeWebHookController(IServiceProvider serviceProvider, ILogger<StripeWebHookController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync()
        {
            //if (!IsDefaultShell())
            //{
            //    return Unauthorized();
            //}

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WebhookSecret, throwOnApiVersionMismatch: false);

                switch (stripeEvent.Type)
                {
                    case Events.InvoicePaymentSucceeded:
                    {
                        var invoice = stripeEvent.Data.Object as Invoice;

                        //Line Items
                        var lineItem = invoice.Lines.Data[0];//Only interested in the first item 
                        var tenantId = lineItem.Metadata["TenantId"];
                        var tenantName = lineItem.Metadata["TenantName"];
                        var planName = lineItem.Description;
                        var amount = lineItem.Plan.AmountDecimal.Value;
                        var period = new BillingPeriod(lineItem.Period.Start.Value, lineItem.Period.End.Value);

                        _logger.LogInformation($"Incoming Invoice PaymentSucceededEvent for tenant {tenantName} for {planName} plan of amount ${amount} for period of {period.Start} to {period.End}");

                        //Payment Method
                        var paymentMethod = await GetPaymentInformation(invoice);

                        var paymentSuccessEventHandlers = _serviceProvider.GetRequiredService<IEnumerable<ISubscriptionPaymentSuccessEventHandler>>();
                        await paymentSuccessEventHandlers.InvokeAsync(x => x.SubscriptionPaymentSuccess(tenantId, tenantName, period, amount, paymentMethod, planName), _logger);
                        break;
                    }
                    case Events.InvoicePaymentFailed:
                    {
                        var invoice = stripeEvent.Data.Object as Invoice;

                        //Line Items
                        var lineItem = invoice.Lines.Data[0];//Only interested in the first item 
                        var tenantId = lineItem.Metadata["TenantId"];
                        var tenantName = lineItem.Metadata["TenantName"];
                        var planName = lineItem.Description;
                        var period = new BillingPeriod(lineItem.Period.Start.Value, lineItem.Period.End.Value);

                        //Payment Method
                        var paymentMethod = await GetPaymentInformation(invoice);

                        _logger.LogInformation($"Incoming Invoice PaymentFailedEvent for tenant {tenantName} for {planName} plan for period of {period.Start} to {period.End}");

                        var paymentFailedEventHandlers = _serviceProvider.GetRequiredService<IEnumerable<ISubscriptionPaymentFailedEventHandler>>();
                        await paymentFailedEventHandlers.InvokeAsync(x => x.SubscriptionPaymentFailed(tenantId, tenantName, period, paymentMethod, planName), _logger);
                        break;
                    }
                }
            }
            catch (StripeException e)
            {
                _logger.LogError($"Stripe Web Hook failed: {e.Message}");
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError($"Stripe Web Hook failed: {e.Message}");
                return BadRequest(e.Message);
            }

            return Ok();
        }

        private async Task<OrchardCore.TenantBilling.Models.PaymentMethod> GetPaymentInformation(Invoice invoice)
        {
            //Get Customer
            var stripeCustomerId = invoice.CustomerId;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);

            //User Customer to extract subscription payment
            var paymentMethodId = customer.Subscriptions.Data[0].DefaultPaymentMethodId;
            var paymentService = new PaymentMethodService();
            var paymentMethod = await paymentService.GetAsync(paymentMethodId);

            //Get card info
            var card = paymentMethod.Card;
            var cardType = (CardType)Enum.Parse(typeof(CardType), card.Brand.ToLower());
            var creditCardInformation = new CreditCardInformation(cardType, Int32.Parse(card.Last4), Convert.ToInt32(card.ExpMonth), Convert.ToInt32(card.ExpYear));
            var paymentInformation = new OrchardCore.TenantBilling.Models.PaymentMethod(true,  creditCardInformation);

            return paymentInformation;
        }
    }
}

