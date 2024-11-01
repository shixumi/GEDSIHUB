// JavaScript to update the file name in the label
// CONSOLE ERROR: Uncaught TypeError: Cannot read properties of null (reading 'addEventListener')
/*
document.getElementById('imageUpload').addEventListener('change', function () {
    var fileName = this.files[0] ? this.files[0].name : "No file chosen";
    document.getElementById('fileLabel').textContent = fileName;
});
*/



// Doesn't Work Properly Please Check
let files = [];

const input = document.getElementById('ImageFile');
const previewContainer = document.getElementById('previewContainer');

// ERROR: ForumPost.js:19  Uncaught TypeError: Cannot read properties of null (reading 'addEventListener')
// Listen for file input changes

/*
input.addEventListener('change', () => {
    const file = input.files[0]; // Only accept one file at a time for this example

    if (file) {
        files = [file]; // Store the file
        showImage();    // Display the image preview
    }
});
*/

const showImage = () => {
    let imageHTML = '';
    if (files.length > 0) {
        const file = files[0];
        const imageUrl = URL.createObjectURL(file);

        imageHTML = `
            <div class="imagePreview">
                <img src="${imageUrl}" alt="comment-image" />
                <span onclick="removeImage()">&times;</span>
            </div>
        `;

        // Insert the image into the preview container
        previewContainer.innerHTML = imageHTML;
    }
}

// Function to remove the image
const removeImage = () => {
    files = [];  // Clear the file list
    input.value = ''; // Clear the file input
    previewContainer.innerHTML = ''; // Remove the image from the preview
}