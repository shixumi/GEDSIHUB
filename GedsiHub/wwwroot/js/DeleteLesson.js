// Toggle custom dropdown visibility when filter button is clicked
document.getElementById('userManagementFilter').addEventListener('click', function () {
    var dropdown = document.getElementById('customDropdown');
    dropdown.style.display = dropdown.style.display === 'none' ? 'block' : 'none';
});

// Event listener for selecting an option
document.querySelectorAll('.dropdown-option').forEach(function (option) {
    option.addEventListener('click', function () {
        var selectedValue = this.getAttribute('data-value');
        document.getElementById('isActiveSelect').value = selectedValue; // Set the hidden select's value
        document.getElementById('customDropdown').style.display = 'none'; // Hide dropdown
        // Optionally, submit the form or trigger an action
    });
});

// Hide dropdown if clicked outside
document.addEventListener('click', function (event) {
    var dropdown = document.getElementById('customDropdown');
    var filterButton = document.getElementById('userManagementFilter');
    if (!dropdown.contains(event.target) && event.target !== filterButton) {
        dropdown.style.display = 'none';
    }
});
