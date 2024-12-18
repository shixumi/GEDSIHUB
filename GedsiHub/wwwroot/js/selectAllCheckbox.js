// selectAllCheckbox.js
document.addEventListener('DOMContentLoaded', function () {
    // **Group 1: Demographic Report**
    const selectAllGroup1 = document.getElementById('selectAllGroup1');
    const checkboxesGroup1 = document.querySelectorAll('.checkbox-item-group1');

    // Add event listener to "Select All" for Group 1
    if (selectAllGroup1 && checkboxesGroup1.length > 0) {
        selectAllGroup1.addEventListener('change', function () {
            console.log('Select All Group 1 changed:', this.checked);
            checkboxesGroup1.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
            toggleGenerateButton('demographicReportForm', 'metricsValidationDemographic');
        });

        // Uncheck "Select All" for Group 1 if any individual checkbox is manually unchecked
        checkboxesGroup1.forEach(checkbox => {
            checkbox.addEventListener('change', function () {
                console.log('Checkbox in Group 1 changed:', this.checked);
                if (!this.checked) {
                    selectAllGroup1.checked = false;
                } else {
                    // If all checkboxes are checked, set "Select All" to checked
                    const allChecked = Array.from(checkboxesGroup1).every(cb => cb.checked);
                    if (allChecked) {
                        selectAllGroup1.checked = true;
                    }
                }
                toggleGenerateButton('demographicReportForm', 'metricsValidationDemographic');
            });
        });
    }

    // **Group 2: Module Report**
    const selectAllGroup2 = document.getElementById('selectAllGroup2');
    const checkboxesGroup2 = document.querySelectorAll('.checkbox-item-group2');

    // Add event listener to "Select All" for Group 2
    if (selectAllGroup2 && checkboxesGroup2.length > 0) {
        selectAllGroup2.addEventListener('change', function () {
            console.log('Select All Group 2 changed:', this.checked);
            checkboxesGroup2.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
            toggleGenerateButton('moduleReportForm', 'metricsValidationModule');
        });

        // Uncheck "Select All" for Group 2 if any individual checkbox is manually unchecked
        checkboxesGroup2.forEach(checkbox => {
            checkbox.addEventListener('change', function () {
                console.log('Checkbox in Group 2 changed:', this.checked);
                if (!this.checked) {
                    selectAllGroup2.checked = false;
                } else {
                    // If all checkboxes are checked, set "Select All" to checked
                    const allChecked = Array.from(checkboxesGroup2).every(cb => cb.checked);
                    if (allChecked) {
                        selectAllGroup2.checked = true;
                    }
                }
                toggleGenerateButton('moduleReportForm', 'metricsValidationModule');
            });
        });
    }

    // **Function to Toggle Generate Button and Validation Summary**
    function toggleGenerateButton(formId, validationSummaryId) {
        const form = document.getElementById(formId);
        if (!form) {
            console.warn(`Form with ID '${formId}' not found.`);
            return;
        }

        let generateButton, metricCheckboxes, validationSummary;
        if (formId === 'demographicReportForm') {
            generateButton = form.querySelector('.generate-report-button');
            metricCheckboxes = form.querySelectorAll('.checkbox-item-group1');
            validationSummary = document.getElementById(validationSummaryId);
        } else if (formId === 'moduleReportForm') {
            generateButton = form.querySelector('.generate-report-button');
            metricCheckboxes = form.querySelectorAll('.checkbox-item-group2');
            validationSummary = document.getElementById(validationSummaryId);
        }

            if (!generateButton) {
                console.warn(`Generate button not found in form '${formId}'.`);
                return;
            }

            if (metricCheckboxes.length === 0) {
                console.warn(`No checkboxes found for form '${formId}'.`);
                return;
            }

        const anyChecked = Array.from(metricCheckboxes).some(cb => cb.checked);
        generateButton.disabled = !anyChecked;

        if (validationSummary) {
            if (!anyChecked) {
                validationSummary.style.display = 'block';
            } else {
                validationSummary.style.display = 'none';
            }
        }
    }

    // **Initial Check for Demographic Report**
    toggleGenerateButton('demographicReportForm', 'metricsValidationDemographic');

    // **Initial Check for Module Report**
    toggleGenerateButton('moduleReportForm', 'metricsValidationModule');
});
