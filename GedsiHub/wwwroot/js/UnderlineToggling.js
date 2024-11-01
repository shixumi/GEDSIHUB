// Function to handle the underline effect when a sidebar item is clicked
document.querySelectorAll('.leaderboard-sidebar').forEach(sidebarItem => {
    sidebarItem.addEventListener('click', function (event) {
        // Prevent the default action if needed, or let it proceed to navigate
        event.preventDefault();

        // Remove the active class from all sidebar items
        document.querySelectorAll('.leaderboard-sidebar').forEach(item => item.classList.remove('active'));

        // Add the active class to the clicked sidebar item
        this.classList.add('active');

        // Navigate to the link inside the sidebar item
        const link = this.querySelector('a');
        if (link) {
            window.location.href = link.href; // Redirect to the link's URL
        }
    });
});
