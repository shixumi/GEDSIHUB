


// JavaScript to update the file name in the label
document.getElementById('imageUpload').addEventListener('change', function () {
    var fileName = this.files[0] ? this.files[0].name : "No file chosen";
    document.getElementById('fileLabel').textContent = fileName;
});




// Doesn't Work Properly Please Check
let files = [];

const input = document.getElementById('ImageFile');
const previewContainer = document.getElementById('previewContainer');

// Listen for file input changes
input.addEventListener('change', () => {
    const file = input.files[0]; // Only accept one file at a time for this example

    if (file) {
        files = [file]; // Store the file
        showImage();    // Display the image preview
    }
});

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




document.addEventListener("DOMContentLoaded", function () {
    const img = document.querySelector('.post-img');
    const container = document.querySelector('.post-img-container');

    img.onload = function () {
        const imgWidth = img.naturalWidth;
        const imgHeight = img.naturalHeight;
        const containerWidth = container.clientWidth;
        const containerHeight = container.clientHeight;

        // Check if the image fits inside the container
        if (imgWidth <= containerWidth && imgHeight <= containerHeight) {
            container.classList.add('rounded-corners');
        } else {
            container.classList.remove('rounded-corners');
        }
    };
});
