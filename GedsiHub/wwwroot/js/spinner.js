// spinner.js
window.addEventListener('load', function () {
    // Select all forms on the page
    const forms = document.querySelectorAll('form');

    // Add the onsubmit event to each form
    forms.forEach(form => {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity()) {
                showSpinner();
            }
        });
    });
});

function showSpinner() {
    document.getElementById("spinner").style.display = "flex";
}

function hideSpinner() {
    document.getElementById("spinner").style.display = "none";
}


