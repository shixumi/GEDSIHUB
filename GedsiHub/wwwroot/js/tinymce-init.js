// Function to dynamically load the TinyMCE script asynchronously
function loadTinyMCE(apiKey) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = `https://cdn.tiny.cloud/1/${apiKey}/tinymce/7/tinymce.min.js`;
        script.referrerPolicy = 'origin';
        script.async = true;  // Load asynchronously
        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load TinyMCE'));
        document.head.appendChild(script);
    });
}

// Function to initialize TinyMCE after the script is loaded
function initializeTinyMCE() {
    tinymce.init({
        selector: '#tinymce_textarea',  // Applies TinyMCE to any <textarea> element
        plugins: [
            'autolink', 'link', 'lists', 'table',  
            'anchor', 'charmap', 'codesample', 'image', 'media',
            'visualblocks', 'wordcount'
        ],
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table | removeformat',
        tinycomments_mode: 'embedded',
        tinycomments_author: 'Author name',
        mergetags_list: [
            { value: 'First.Name', title: 'First Name' },
            { value: 'Email', title: 'Email' },
        ],
    });
}

// Automatically load TinyMCE when the page loads
window.addEventListener('DOMContentLoaded', () => {
    // Check if TinyMCE is already loaded
    if (typeof tinymce === 'undefined') {
        // Load the TinyMCE script and initialize
        const apiKey = 'sogt2rj9sryftyoyv1rqi93jqzwli2cnzhdo1hgt5b3hyct4';  // Your API key here
        loadTinyMCE(apiKey).then(() => {
            initializeTinyMCE();
        }).catch(error => {
            console.error('TinyMCE failed to load:', error);
        });
    } else {
        // If TinyMCE is already loaded, just initialize it
        initializeTinyMCE();
    }
});
