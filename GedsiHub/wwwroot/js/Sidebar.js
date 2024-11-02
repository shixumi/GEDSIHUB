// Function to handle the redirection and active state of all sidebar buttons
function setActiveSidebar() {
    const currentPath = window.location.pathname;

    // Map each page to its corresponding sidebar button ID
    const pageMapping = {
        '/Dashboard': 'SidebarButton_Dashboard',
        '/Module': 'SidebarButton_Modules',
        'addNewModule.html': 'SidebarButton_Modules',
        '/Leaderboard': 'SidebarButton_Leaderboard', // Fixed path
        '/ForumPost': 'SidebarButton_Forum',
        '/Reports/Demographic': 'SidebarButton_Reports',
        '/Feedback': 'SidebarButton_Feedback'
    };

    // Add the 'active' class to the correct button based on the current page
    for (const page in pageMapping) {
        if (currentPath.includes(page)) {
            document.getElementById(pageMapping[page]).classList.add('active');
            break; // Stop after finding the correct active page
        }
    }
}

// Function to handle redirection for sidebar buttons
function setupSidebarButtonActions() {
    const buttonActions = {
        'SidebarButton_Dashboard': '/Dashboard',
        'SidebarButton_Modules': '/Module',
        'SidebarButton_Leaderboard': '/Leaderboard',
        'SidebarButton_Forum': '/ForumPost',
        'SidebarButton_Reports': '/Reports/Demographic',
        'SidebarButton_Feedback': '/Feedback'
    };

    // Add click event listeners to the buttons
    for (const buttonId in buttonActions) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.addEventListener('click', function () {
                window.location.href = buttonActions[buttonId];
            });
        }
    }
}

// Initialize everything when the DOM is fully loaded
document.addEventListener('DOMContentLoaded', function () {
    setActiveSidebar();
    setupSidebarButtonActions(); // Set up the click actions for sidebar buttons
});
