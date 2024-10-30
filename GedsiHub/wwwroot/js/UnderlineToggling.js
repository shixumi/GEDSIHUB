// Function to handle the underline effect when a button is clicked
document.querySelectorAll('.leaderboard-sidebar').forEach(button => {
    button.addEventListener('click', function () {
        // Remove the active class from all buttons
        document.querySelectorAll('.leaderboard-sidebar').forEach(btn => btn.classList.remove('active'));

        // Add the active class to the clicked button
        this.classList.add('active');
    });
});


// Function to handle the underline effect when a tab is clicked
document.querySelectorAll('.tab-link').forEach(tab => {
    tab.addEventListener('click', function () {
        // Remove the active class from all tabs
        document.querySelectorAll('.tab-link').forEach(tab => tab.classList.remove('active'));

        // Add the active class to the clicked tab
        this.classList.add('active');
    });
});