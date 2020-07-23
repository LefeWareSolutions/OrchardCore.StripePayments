# OrchardCore.StripePayment
An Orchard Core module for making one time and reoccuring payments using Stripe.

##OrchardCore.StripePayment Feature
The Stripe Payment feature is used for one time payments. It has a strong dependency on the following module:
- [Orchard.Payments](https://github.com/LefeWareSolutions/OrchardCore.Payments)

##OrchardCore.StripeSubscription Feature
The Stripe Subscription feature is used for monthly reoccuring payments. It uses Stripe Webhooks to accomplish this
- This feature is not ready for consumption



## Setting up your dev environment
1. **Prerequisites:** Make sure you have an up-to-date clone of [the Orchard Core repository](https://github.com/OrchardCMS/OrchardCore) on the `dev` branch. Please consult [the Orchard Core documentation](https://orchardcore.readthedocs.io/en/latest/) and make sure you have a working Orchard before you proceed. You'll also, of course, need all of Orchard Core's prerequisites for development (.NET Core, a code editor, etc.). The following steps assume some basic understanding of Orchard Core.
2. Clone the module under `[your Orchard Core clone's root]/src/OrchardCore.Modules`.
3. Add the existing project to the solution under `src/OrchardCore.Modules` in the solution explorer if you're using Visual Studio.
4. Add a reference to the module from the `OrchardCore.Cms.Web` project.
5. OrchardCore.StripePayment has a strong dependency on the OrchardCore.Tenant billing project found here: https://github.com/JoshLefebvre/OrchardCore.TenantBilling. You will need to repeat steps 2,3 and 4 for this project. 
5. Build, run.
6. From the admin, enable the module's only feature.