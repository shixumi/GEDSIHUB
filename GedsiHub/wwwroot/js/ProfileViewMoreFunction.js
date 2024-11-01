/*
document.getElementById('viewMoreButton').addEventListener('click', function () {
    const additionalDetails = document.querySelector('.additional-details');
    const userProfileTable = document.querySelector('.user-profile-table-container');

    // Toggle visibility of additional details
    additionalDetails.classList.toggle('show');

    // Hide or show the user profile table based on the visibility of additional details
    if (additionalDetails.classList.contains('show')) {
        userProfileTable.style.display = 'none'; // Hide the table
    } else {
        userProfileTable.style.display = ''; // Reset to default display
    }

    // Change the button text based on visibility
    const spanElement = this.querySelector('span');
    spanElement.innerHTML = additionalDetails.classList.contains('show') ? '< View Less' : 'View More >';
});
comment out for the debugging
*/

document.addEventListener('DOMContentLoaded', function () {
    const viewMoreButton = document.getElementById('viewMoreButton');
    if (viewMoreButton) {
        viewMoreButton.addEventListener('click', function () {
            const additionalDetails = document.querySelector('.additional-details');
            const userProfileTable = document.querySelector('.user-profile-table-container');

            // Toggle visibility of additional details
            additionalDetails.classList.toggle('show');

            // Hide or show the user profile table based on the visibility of additional details
            if (additionalDetails.classList.contains('show')) {
                userProfileTable.style.display = 'none'; // Hide the table
            } else {
                userProfileTable.style.display = ''; // Reset to default display
            }

            // Change the button text based on visibility
            const spanElement = this.querySelector('span');
            spanElement.innerHTML = additionalDetails.classList.contains('show') ? '< View Less' : 'View More >';
        });
    }
});

