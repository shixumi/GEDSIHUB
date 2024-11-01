// Set the selected view and save to localStorage
function setView(view) {
    localStorage.setItem('selectedView', view);
    applyView(); // Apply the view immediately
}

/*
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
*/

function applyView() {
    const selectedView = localStorage.getItem('selectedView') || 'grid';

    const gridView = document.getElementById('grid-view');
    const listView = document.getElementById('list-view');
    const gridTab = document.getElementById('grid-tab');
    const listTab = document.getElementById('list-tab');

    if (selectedView === 'grid') {
        if (gridView) gridView.classList.add('show', 'active');
        if (listView) listView.classList.remove('show', 'active');
        if (gridTab) gridTab.classList.add('active');
        if (listTab) listTab.classList.remove('active');
    } else {
        if (listView) listView.classList.add('show', 'active');
        if (gridView) gridView.classList.remove('show', 'active');
        if (listTab) listTab.classList.add('active');
        if (gridTab) gridTab.classList.remove('active');
    }
}

document.addEventListener('DOMContentLoaded', function () {
    applyView();
});
