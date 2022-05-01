// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function clearValidation(input) {
    input.classList.remove("is-valid");
    input.classList.remove("is-invalid");
    input.closest(".form-group").querySelector(".invalid-feedback").innerHTML = "";
}

function setVailidation(data, idInputs) {
    idInputs.forEach(item => {
        let temp = data.ValidationMessage[item];
        let element = document.getElementById(item);

        if (temp != undefined) {

            setValidationInElement(element, "is-invalid", data.ValidationMessage[item]);
        }
        else {
            setValidationInElement(element, "is-valid", "");
        }
    });
}

function setValidationInElement(element, cssClass, text) {
    element.classList.remove("is-valid");
    element.classList.remove("is-invalid");
    element.classList.add(cssClass);
    element.closest(".form-group").querySelector(".invalid-feedback").innerHTML = text;
}