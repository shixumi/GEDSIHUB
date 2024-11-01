// Select both toggle buttons
const demoReportButton = document.getElementById('generateDemoReportButton');
const modReportButton = document.getElementById('generateModReportButton');

// Function to toggle active class and update background colors
function toggleActive(buttonToActivate, buttonToDeactivate) {
    buttonToActivate.classList.add('active');
    buttonToDeactivate.classList.remove('active');
}

// Add click event listeners to each button
demoReportButton.addEventListener('click', () => {
    toggleActive(demoReportButton, modReportButton);
});

modReportButton.addEventListener('click', () => {
    toggleActive(modReportButton, demoReportButton);
});
