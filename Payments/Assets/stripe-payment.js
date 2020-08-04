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
var form = document.getElementsByClassName('form-camp-registration-form')[0];
form.addEventListener('submit', function (ev) {
    ev.preventDefault();
    changeLoadingState(true);
    const billingName = document.getElementById('firstName').value;
    const billingLastName = document.getElementById('lastName').value;
    stripe.confirmCardPayment(intentClientSecret, {
        payment_method: {
            card: card,
            billing_details: {
                name: `${billingName} ${billingLastName}`
            }
        }
    }).then(function (result) {
        if (result.error) {
            showError(result.error.message);
        } else {
            // The payment has been processed!
            if (result.paymentIntent.status === 'succeeded') {
                // Show a success message to your customer
                // There's a risk of the customer closing the window before callback
                // execution. Set up a webhook or plugin to listen for the
                // payment_intent.succeeded event that handles any business critical
                // post-payment actions.
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