using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LefeWareLearning.StripePayment;
using LefeWareSolutions.Payments.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Modules;
using OrchardCore.PaymentForm.IEventHandlers;
using OrchardCore.StripePayment.Services;
using OrchardCore.StripePayment.StripePayments.ViewModels;
using Stripe;

namespace OrchardCore.StripePayment.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Feature(StripePaymentConstants.Features.StripePayment)]
    public class StripePaymentController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentDisplay;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStripePaymentService _stripePaymentService;
        private readonly ILogger<StripePaymentController> _logger;


        public StripePaymentController(IContentManager contentManager, IContentItemDisplayManager contentDisplay, IUpdateModelAccessor updateModelAccessor,
            IServiceProvider serviceProvider, IStripePaymentService stripePaymentService, ILogger<StripePaymentController> logger)
        {
            _contentManager = contentManager;
            _contentDisplay = contentDisplay;
            _updateModelAccessor = updateModelAccessor;
            _serviceProvider = serviceProvider;
            _stripePaymentService = stripePaymentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string stripePaymentFormId, [FromQuery] Dictionary<string, string> metadata)
        {
            var stripePaymentForm = await _contentManager.GetAsync(stripePaymentFormId);

            //payment part
            var paymentPart = stripePaymentForm.As<PaymentPart>();
            var cost = (long)(paymentPart.Cost * 100);
            var currency = paymentPart.Currency.Text;

            //stripe payment form part
            var stripePaymentFormPart = stripePaymentForm.As<StripePaymentFormPart>();
            var StripeSecretKey = stripePaymentFormPart.StripeSecretKey.Text;
            var stripePublishableKey = stripePaymentFormPart.StripePublishableKey.Text;
            var paymentIntent = await _stripePaymentService.CreatePaymentIntent(StripeSecretKey, cost, currency, metadata);

            //Should be using shapes but need a way to pass in custom metadata to an existing form
            var shape = await _contentDisplay.BuildDisplayAsync(stripePaymentForm, _updateModelAccessor.ModelUpdater);

            var stripePaymentFormViewModel = new StripePaymentFormViewModel()
            {
                PaymentIntentClientSecret = paymentIntent.ClientSecret,
                StripePublishableAPIKey = stripePublishableKey,
                Shape = shape
            };

            return View(stripePaymentFormViewModel);
        }



        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> PaymentIntentSuccess(PaymentIntent paymentIntent)
        {
            //TODO: This should be a webhook, but multi tenancy brings on challenges
            var paymentSuccessEventHandlers = _serviceProvider.GetRequiredService<IEnumerable<IPaymentSuccessEventHandler>>();
            await paymentSuccessEventHandlers.InvokeAsync(x => x.PaymentSuccess(paymentIntent.Id, "Stripe", paymentIntent.Metadata), _logger);

            return Ok();
        }

    }
}
