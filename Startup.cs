using System;
using OrchardCore.TenantBilling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using Stripe;
using LefeWareLearning.StripePayment;
using OrchardCore.Data.Migration;
using OrchardCore.StripePayment.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;

namespace OrchardCore.StripePayment
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, Permissions>();
            services.Configure<StripeConfigurationOptions>(_configuration.GetSection("Stripe"));
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:StripeAPIKey"];
        }
    }

    [Feature(StripePaymentConstants.Features.StripePayment)]
    public class StripePaymentStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, StripePaymentMigrations>();

            services.AddTransient<IStripePaymentService, StripePaymentService>();

            services.AddContentPart<StripePaymentFormPart>()
                .UseDisplayDriver<StripePaymentFormPartDisplay>();
        }


    }
}
