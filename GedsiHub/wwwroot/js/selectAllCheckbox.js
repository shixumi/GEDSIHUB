document.addEventListener('DOMContentLoaded', function () {
    // Functionality for the first group (original group)
    const selectAllCheckboxGroup1 = document.getElementById('selectAllGroup1');
    const checkboxesGroup1 = document.querySelectorAll('.checkbox-item-group1');

    // Add event listener to "Select All" for Group 1
    selectAllCheckboxGroup1.addEventListener('change', function () {
        checkboxesGroup1.forEach(checkbox => {
            checkbox.checked = this.checked;
        });
    });

    // Uncheck "Select All" for Group 1 if any individual checkbox is manually unchecked
    checkboxesGroup1.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            if (!this.checked) {
                selectAllCheckboxGroup1.checked = false;
            }
        });
    });

    // Add functionality for the previous set (if it exists, replace "group1" with your previous group selectors)
    const selectAllPreviousGroup = document.getElementById('selectAll'); // ID from previous group
    const checkboxesPreviousGroup = document.querySelectorAll('.checkbox-item'); // Class from previous group

    // Add event listener to "Select All" for the previous group
    selectAllPreviousGroup.addEventListener('change', function () {
        checkboxesPreviousGroup.forEach(checkbox => {
            checkbox.checked = this.checked;
        });
    });

    // Uncheck "Select All" for the previous group if any individual checkbox is manually unchecked
    checkboxesPreviousGroup.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            if (!this.checked) {
                selectAllPreviousGroup.checked = false;
            }
        });
    });
});
