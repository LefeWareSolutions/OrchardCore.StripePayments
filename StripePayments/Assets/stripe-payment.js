//Create an instance of an Element and mount it to the Element container
const publishableKey = document.getElementById("publishableKey").value;
const intentClientSecret = document.getElementById("intentClientSecret").value;
var stripe = Stripe(publishableKey);
var elements = stripe.elements();
var style = {
    base: {
        color: "#32325d",
    }
};
var card = elements.create("card", { style: style });
card.mount("#card-element");


//Validates user input as it is typed
//cardElement.on('change', function (event) {
//    var displayError = document.getElementById('card-errors');
//    if (event.error) {
//        displayError.textContent = event.error.message;
//    } else {
//        displayError.textContent = '';
//    }
//});

//Submit the payment to stripe
var form = document.getElementById('payment-form');
form.addEventListener('submit', function (ev) {
    ev.preventDefault();
    changeLoadingState(true);
    const billingName = document.getElementById('firstName').value;
    const billingLastName = document.getElementById('lastName').value;
    stripe.confirmCardPayment(intentClientSecret, {
        payment_method: {
            card: card,
            type: 'card',
            billing_details: {
                name: `${billingName} ${billingLastName}`
            }
        },
        metadata: {
            stripePaymentFormId: "485p3amrvm02p24mzz07y0rddm",
            camperId: "47d7n3n3sbt8yyczf8c00g3hmf"
        },
    }).then(function (result) {
        if (result.error) {
            showError(result.error.message);
        } else {
            // The payment has been processed!
            if (result.paymentIntent.status === 'succeeded') {
                // TODO: There's a risk of the customer closing the window before callback execution. 
                // Set up a webhook to listen for the payment_intent.succeeded event
                const url = window.location.href;
                let baseUrl = `${url.split('StripePayment')[0]}StripePayment/PaymentIntentSuccess`;
                const request = {
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(result.paymentIntent),
                    method: "POST"
                };

                fetch(baseUrl, request).then(x => {
                    window.location.replace(window.location.href);
                }).catch(error=>console.log(error))
            } else {
                showError("Error with payment processing")
            }
        }
    });
});

var showError = function (errorMsgText) {
    changeLoadingState(false);
    var errorMsg = document.querySelector(".sr-field-error");
    errorMsg.textContent = errorMsgText;
    setTimeout(function () {
        errorMsg.textContent = "";
    }, 4000);
};

var changeLoadingState = function (isLoading) {
    if (isLoading) {
        document.querySelector("button").disabled = true;
        document.querySelector("#spinner").classList.remove("hidden");
    } else {
        document.querySelector("button").disabled = false;
        document.querySelector("#spinner").classList.add("hidden");
    }
};