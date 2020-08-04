/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./assets/stripe-payment.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./assets/stripe-payment.js":
/*!**********************************!*\
  !*** ./assets/stripe-payment.js ***!
  \**********************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("//Create an instance of an Element and mount it to the Element container\r\nconst publishableKey = document.getElementById(\"publishableKey\").value;\r\nconst intentClientSecret = document.getElementById(\"intentClientSecret\").value;\r\nvar stripe = Stripe(publishableKey);\r\nvar elements = stripe.elements();\r\nvar style = {\r\n    base: {\r\n        color: \"#32325d\",\r\n    }\r\n};\r\nvar card = elements.create(\"card\", { style: style });\r\ncard.mount(\"#card-element\");\r\n\r\n\r\n//Validates user input as it is typed\r\n//cardElement.on('change', function (event) {\r\n//    var displayError = document.getElementById('card-errors');\r\n//    if (event.error) {\r\n//        displayError.textContent = event.error.message;\r\n//    } else {\r\n//        displayError.textContent = '';\r\n//    }\r\n//});\r\n\r\n//Submit the payment to stripe\r\nvar form = document.getElementsByClassName('form-camp-registration-form')[0];\r\nform.addEventListener('submit', function (ev) {\r\n    ev.preventDefault();\r\n    changeLoadingState(true);\r\n    const billingName = document.getElementById('firstName').value;\r\n    const billingLastName = document.getElementById('lastName').value;\r\n    stripe.confirmCardPayment(intentClientSecret, {\r\n        payment_method: {\r\n            card: card,\r\n            billing_details: {\r\n                name: `${billingName} ${billingLastName}`\r\n            }\r\n        }\r\n    }).then(function (result) {\r\n        if (result.error) {\r\n            showError(result.error.message);\r\n        } else {\r\n            // The payment has been processed!\r\n            if (result.paymentIntent.status === 'succeeded') {\r\n                // Show a success message to your customer\r\n                // There's a risk of the customer closing the window before callback\r\n                // execution. Set up a webhook or plugin to listen for the\r\n                // payment_intent.succeeded event that handles any business critical\r\n                // post-payment actions.\r\n            }\r\n        }\r\n    });\r\n});\r\n\r\nvar showError = function (errorMsgText) {\r\n    changeLoadingState(false);\r\n    var errorMsg = document.querySelector(\".sr-field-error\");\r\n    errorMsg.textContent = errorMsgText;\r\n    setTimeout(function () {\r\n        errorMsg.textContent = \"\";\r\n    }, 4000);\r\n};\r\n\r\nvar changeLoadingState = function (isLoading) {\r\n    if (isLoading) {\r\n        document.querySelector(\"button\").disabled = true;\r\n        document.querySelector(\"#spinner\").classList.remove(\"hidden\");\r\n    } else {\r\n        document.querySelector(\"button\").disabled = false;\r\n        document.querySelector(\"#spinner\").classList.add(\"hidden\");\r\n    }\r\n};\n\n//# sourceURL=webpack:///./assets/stripe-payment.js?");

/***/ })

/******/ });