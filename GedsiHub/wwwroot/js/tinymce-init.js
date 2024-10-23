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
    if (typeof tinymce !== 'undefined') {
        tinymce.init({
            selector: '#tinymce_textarea',  // Applies TinyMCE to any <textarea> element
            plugins: [
                'autolink', 'link', 'lists', 'table', 'anchor', 'charmap', 'codesample', 
                'image', 'media', 'visualblocks', 'wordcount', 'fullscreen', 
                'autoresize', 'code'
            ],
            toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table fullscreen | code | removeformat',
            tinycomments_mode: 'embedded',
            tinycomments_author: 'Author name',
            mergetags_list: [
                { value: 'First.Name', title: 'First Name' },
                { value: 'Email', title: 'Email' },
            ],
            min_height: 700,  // Set the minimum height of the editor
            max_height: 1600,  // Set the maximum height the editor can grow to
            autoresize_bottom_margin: 20,  // Optional: Adds margin when resizing

            // This allows <iframe> and <script> tags with certain attributes
            extended_valid_elements: 'iframe[src|width|height|frameborder|allowfullscreen|allow|aria-label],script[src|charset]',
            
            setup: (editor) => {
                editor.on('init', () => {
                    console.log('TinyMCE initialized successfully.');
                });
                editor.on('error', (error) => {
                    console.error('TinyMCE encountered an error:', error);
                    displayEditorError('Error loading editor. Please try again later.');
                });
            }
        });
    } else {
        console.error("TinyMCE script wasn't loaded correctly.");
    }
}

// Function to display error if TinyMCE fails
function displayEditorError(message) {
    const editorContainer = document.getElementById('tinymce_textarea');
    editorContainer.innerHTML = `<div class="editor-error">${message}</div>`;
    editorContainer.style.color = 'red';
}

// Automatically load TinyMCE when the page loads
window.addEventListener('DOMContentLoaded', () => {
    const apiKey = 'sogt2rj9sryftyoyv1rqi93jqzwli2cnzhdo1hgt5b3hyct4';  // Your API key here

    // Check if TinyMCE is already loaded
    if (typeof tinymce === 'undefined') {
        // Load the TinyMCE script and initialize
        loadTinyMCE(apiKey).then(() => {
            initializeTinyMCE();
        }).catch(error => {
            console.error('TinyMCE failed to load:', error);
            displayEditorError('Failed to load editor. Please check your network connection.');
        });
    } else {
        // If TinyMCE is already loaded, just initialize it
        initializeTinyMCE();
    }
});
