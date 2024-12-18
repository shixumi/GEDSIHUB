// reportsButtonToggle.js

// Select both toggle buttons
const demoReportButton = document.getElementById('generateDemoReportButton');
const modReportButton = document.getElementById('generateModReportButton');

// Function to toggle active class and update background colors
function toggleActive(buttonToActivate, buttonToDeactivate) {
    buttonToActivate.classList.add('active');
    buttonToDeactivate.classList.remove('active');
}

// Add click event listeners to each button only if they exist
if (demoReportButton && modReportButton) {
    demoReportButton.addEventListener('click', () => {
        toggleActive(demoReportButton, modReportButton);
        // Show Demographic Report Form and hide Module Report Form
        document.getElementById('demographicReportForm').style.display = 'block';
        document.getElementById('moduleReportForm').style.display = 'none';
    });

    modReportButton.addEventListener('click', () => {
        toggleActive(modReportButton, demoReportButton);
        // Show Module Report Form and hide Demographic Report Form
        document.getElementById('moduleReportForm').style.display = 'block';
        document.getElementById('demographicReportForm').style.display = 'none';
    });
} else {
    console.warn('Toggle buttons not found in the DOM.');
}
