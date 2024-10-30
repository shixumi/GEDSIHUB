document.getElementById('viewMoreButton').addEventListener('click', function () {
    const additionalDetails = document.querySelector('.additional-details');
    additionalDetails.classList.toggle('show'); // Toggle visibility

    // Change the button text based on visibility
    const spanElement = this.querySelector('span');
    spanElement.innerHTML = additionalDetails.classList.contains('show') ? '< View Less' : 'View More >';
});