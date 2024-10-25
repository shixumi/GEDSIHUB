// Set the selected view and save to localStorage
function setView(view) {
    localStorage.setItem('selectedView', view);
    applyView(); // Apply the view immediately
}


// Apply the saved view on page load
function applyView() {
    const selectedView = localStorage.getItem('selectedView') || 'grid'; // Default to grid if no choice saved
    if (selectedView === 'grid') {
        document.getElementById('grid-view').classList.add('show', 'active');
        document.getElementById('list-view').classList.remove('show', 'active');
        document.getElementById('grid-tab').classList.add('active');
        document.getElementById('list-tab').classList.remove('active');
    } else {
        document.getElementById('list-view').classList.add('show', 'active');
        document.getElementById('grid-view').classList.remove('show', 'active');
        document.getElementById('list-tab').classList.add('active');
        document.getElementById('grid-tab').classList.remove('active');
    }
}

// Run on page load
document.addEventListener('DOMContentLoaded', applyView);
