// Removed from the integration since this is no longer needed


document.addEventListener('DOMContentLoaded', function () {
    // Initialize the inline date range picker (keep it hidden by default)
    const datePicker = flatpickr("#inlineDatePicker", {
        mode: "range",          // Enable range selection
        inline: true,           // Display the calendar inline
        dateFormat: "m/d/Y",    // Format the date as MM/DD/YYYY
        onChange: function (selectedDates) {
            const startDate = selectedDates[0];
            const endDate = selectedDates[1];
            const selectElement = document.getElementById('dateRangeSelect');
            const customDateContainer = document.getElementById('customDateContainer');

            // If both start and end dates are selected, display the selected range in the select input
            if (startDate && endDate) {
                const formattedRange = `${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`;

                // Check if the range option already exists, otherwise create it
                let rangeOption = selectElement.querySelector('option[data-type="range"]');
                if (!rangeOption) {
                    rangeOption = document.createElement('option');
                    rangeOption.setAttribute('data-type', 'range');
                    selectElement.appendChild(rangeOption);
                }

                // Set the text and value of the newly created range option
                rangeOption.textContent = formattedRange;
                rangeOption.value = formattedRange;
                rangeOption.selected = true;  // Set the range option as selected

                // Hide the calendar after selecting the date range
                customDateContainer.style.display = 'none';
            }
        }
    });

    // Show or hide the calendar container based on the dropdown selection
    document.getElementById('dateRangeSelect').addEventListener('change', function () {
        const selectedValue = this.value;
        const customDateContainer = document.getElementById('customDateContainer');

        // If "Custom" is selected, show the container (and the calendar inside)
        if (selectedValue === 'custom') {
            customDateContainer.style.display = 'block';  // Show the calendar container
        } else {
            customDateContainer.style.display = 'none';   // Hide the calendar container
        }
    });

    // Ensure the calendar reappears if the "Custom" option is clicked again
    document.getElementById('dateRangeSelect').addEventListener('click', function () {
        const selectedValue = this.value;
        const customDateContainer = document.getElementById('customDateContainer');

        // Re-show the calendar when "Custom" is clicked again
        if (selectedValue === 'custom') {
            customDateContainer.style.display = 'block';
        }
    });
});
