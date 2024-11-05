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

function submitFormWithSpinner() {
    // Hide other modal content
    $('.modal-body').children().not('#spinner-modal').hide();
    // Show the spinner
    $('#spinner-modal').show();
}

// Show the spinner when an AJAX request starts
$(document).ajaxStart(function () {
    $("#globalSpinner").show();
});

// Hide the spinner when an AJAX request completes
$(document).ajaxStop(function () {
    $("#globalSpinner").hide();
});

// Optionally, for full page transitions (non-AJAX)
$(document).ready(function () {
    // Show the spinner before unloading the page
    $(window).on('beforeunload', function () {
        $("#globalSpinner").show();
    });
});


// Hide the spinner after 10 seconds (as a fallback)
setTimeout(function () {
    $("#globalSpinner").hide();
}, 10000); // 10,000 milliseconds = 10 seconds