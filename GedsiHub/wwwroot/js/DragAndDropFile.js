// Get references to DOM elements for both containers
const uploadContainer = document.getElementById('Media_Upload_Container');
const lessonUploadContainer = document.getElementById('Lesson_Content_Upload_Container');
const fileInput = document.getElementById('uploadMedia');
const textInput = document.getElementById('uploadText');

// Save the initial HTML content of each container
const initialUploadContainerHTML = uploadContainer.innerHTML;
const initialLessonUploadContainerHTML = lessonUploadContainer.innerHTML;

// Prevent default behavior for drag and drop
['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, preventDefaults, false);
    lessonUploadContainer.addEventListener(eventName, preventDefaults, false);
});

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

// Highlight containers when files are dragged over them
['dragenter', 'dragover'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, () => {
        uploadContainer.classList.add('highlight');
        uploadContainer.style.backgroundColor = '#f0f8ff'; // Change background color on hover
        uploadContainer.style.borderColor = '#3b82f6'; // Change border color on hover
    });

    lessonUploadContainer.addEventListener(eventName, () => {
        lessonUploadContainer.classList.add('highlight');
        lessonUploadContainer.style.backgroundColor = '#f0f8ff'; // Change background color on hover
        lessonUploadContainer.style.borderColor = '#3b82f6'; // Change border color on hover
    });
});

// Remove highlight when dragging stops
['dragleave', 'drop'].forEach(eventName => {
    uploadContainer.addEventListener(eventName, () => {
        uploadContainer.classList.remove('highlight');
        uploadContainer.style.backgroundColor = ''; // Reset background color
        uploadContainer.style.borderColor = '#ccc'; // Reset border color
    });

    lessonUploadContainer.addEventListener(eventName, () => {
        lessonUploadContainer.classList.remove('highlight');
        lessonUploadContainer.style.backgroundColor = ''; // Reset background color
        lessonUploadContainer.style.borderColor = '#ccc'; // Reset border color
    });
});

// Handle file drop for both containers
uploadContainer.addEventListener('drop', handleDropMedia, false);
lessonUploadContainer.addEventListener('drop', handleDropLessonContent, false);

// Handle file input click (for when users click the "Upload" button)
fileInput.addEventListener('change', (event) => {
    let files = event.target.files;
    console.log("File selected:", files); // Log file selection
    if (files.length > 0) {
        handleFiles(files, uploadContainer, initialUploadContainerHTML, fileInput); // Process the files for Media
    } else {
        console.log("No file selected");
    }
});

textInput.addEventListener('change', (event) => {
    let files = event.target.files;
    console.log("File selected:", files); // Log file selection
    if (files.length > 0) {
        handleFiles(files, lessonUploadContainer, initialLessonUploadContainerHTML, textInput); // Process the files for Lesson Content
    } else {
        console.log("No file selected");
    }
});

// Handle file drop for media
function handleDropMedia(e) {
    let dt = e.dataTransfer;
    let files = dt.files;
    console.log("File dropped for Media:", files);

    // Update the input element with the dropped file
    updateFileInput(fileInput, files);

    // Process the files for Media
    handleFiles(files, uploadContainer, initialUploadContainerHTML, fileInput);
}

// Handle file drop for lesson content
function handleDropLessonContent(e) {
    let dt = e.dataTransfer;
    let files = dt.files;
    console.log("File dropped for Lesson Content:", files);

    // Update the input element with the dropped file
    updateFileInput(textInput, files);

    // Process the files for Lesson Content
    handleFiles(files, lessonUploadContainer, initialLessonUploadContainerHTML, textInput);
}

// Function to update the input file element with the dropped files
function updateFileInput(inputElement, files) {
    const dataTransfer = new DataTransfer(); // Create a new DataTransfer object
    [...files].forEach(file => dataTransfer.items.add(file)); // Add the dropped files to the DataTransfer object
    inputElement.files = dataTransfer.files; // Update the input element's files with the DataTransfer object
}

// Function to process files and display preview in the appropriate container
function handleFiles(files, container, initialHTML, inputElement) {
    console.log("Handling files:", files);
    [...files].forEach(file => previewFile(file, container, initialHTML, inputElement)); // Preview each file

    // Clear the input value after processing files to allow for re-uploading
    setTimeout(() => {
        inputElement.value = '';
    }, 100);  // Small delay to ensure file processing is complete before clearing input
}

// Function to preview file and handle different file types (videos, images, PDFs, Word docs, etc.)
function previewFile(file, container, initialHTML, inputElement) {
    console.log(`Previewing file: ${file.name}, type: ${file.type}`); // Log file preview

    // Clear the container content immediately
    container.innerHTML = ''; // Clear previous content

    // Clear the style of the container (removing borders, centering, etc.)
    container.style.border = 'none'; // Remove border
    container.style.display = 'block'; // Remove flex centering
    container.style.width = '100%'; // Set container width
    container.style.height = 'auto'; // Allow the file to define height
    container.style.cursor = 'default'; // Change cursor back to default

    let filePreviewElement;

    if (file.type.startsWith('video/')) {
        // Handle video files
        filePreviewElement = document.createElement('video');
        filePreviewElement.src = URL.createObjectURL(file);
        filePreviewElement.controls = true; // Add controls to make the video playable
        filePreviewElement.autoplay = false; // Prevent auto-play
        filePreviewElement.style.maxWidth = '100%';
        filePreviewElement.style.maxHeight = '500px'; // Adjust video height
        container.appendChild(filePreviewElement);

        filePreviewElement.onloadeddata = () => {
            URL.revokeObjectURL(filePreviewElement.src); // Revoke URL after video is loaded
        };
    } else if (file.type.startsWith('image/')) {
        // Handle image files
        filePreviewElement = document.createElement('img');
        filePreviewElement.src = URL.createObjectURL(file);
        filePreviewElement.style.maxWidth = '100%';
        filePreviewElement.style.maxHeight = '500px';
        container.appendChild(filePreviewElement);

        filePreviewElement.onload = () => {
            URL.revokeObjectURL(filePreviewElement.src); // Revoke URL after image is loaded
        };
    } else if (file.type === 'application/pdf') {
        // Handle PDF files
        filePreviewElement = document.createElement('iframe');
        filePreviewElement.src = URL.createObjectURL(file);
        filePreviewElement.style.width = '100%';
        filePreviewElement.style.height = '500px'; // Adjust the height as needed
        container.appendChild(filePreviewElement);

        filePreviewElement.onload = () => {
            URL.revokeObjectURL(filePreviewElement.src); // Revoke URL after the PDF is displayed
        };
    } else if (file.type === 'application/msword' || file.type === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
        // Handle Word documents (display clickable link)
        const fileReader = new FileReader();
        fileReader.onload = function (e) {
            const wordUrl = e.target.result;

            // Create a clickable link for the Word document
            filePreviewElement = document.createElement('a');
            filePreviewElement.href = wordUrl;
            filePreviewElement.textContent = `Click to preview ${file.name}`;
            filePreviewElement.target = '_blank'; // Open in a new tab
            filePreviewElement.style.display = 'block';
            filePreviewElement.style.textDecoration = 'underline';
            filePreviewElement.style.color = '#007bff'; // Bootstrap link color

            container.appendChild(filePreviewElement);
        };
        fileReader.readAsDataURL(file); // Read file as DataURL to create clickable link
    } else if (file.name.endsWith('.h5p')) {
        // Handle .h5p files (display as a clickable link)
        filePreviewElement = document.createElement('a');
        filePreviewElement.href = URL.createObjectURL(file);
        filePreviewElement.textContent = `Click to preview ${file.name} (H5P content)`;
        filePreviewElement.target = '_blank'; // Open in a new tab for H5P content
        filePreviewElement.style.display = 'block';
        filePreviewElement.style.textDecoration = 'underline';
        filePreviewElement.style.color = '#007bff'; // Bootstrap link color
        container.appendChild(filePreviewElement);
    } else {
        // Handle unsupported file types or provide a download link for any other file types
        filePreviewElement = document.createElement('a');
        filePreviewElement.href = URL.createObjectURL(file);
        filePreviewElement.download = file.name; // Allow downloading of the file
        filePreviewElement.textContent = `Click to download ${file.name}`;
        filePreviewElement.style.display = 'block';
        filePreviewElement.style.textDecoration = 'underline';
        filePreviewElement.style.color = '#007bff'; // Bootstrap link color
        container.appendChild(filePreviewElement);
    }

    // Create and append the remove button BELOW the file preview element
    const removeButton = document.createElement('button');
    removeButton.textContent = 'Remove File';
    removeButton.classList.add('remove-button');

    // Add event listener for the remove button to clear the container and restore original content
    removeButton.addEventListener('click', () => {
        container.innerHTML = initialHTML; // Restore the original HTML content
        container.style.border = ''; // Reset styles if needed
        container.style.backgroundColor = ''; // Reset background color
        container.style.width = ''; // Reset width
        container.style.height = ''; // Reset height
        inputElement.value = ''; // Clear the input for new uploads
    });

    // Append remove button BELOW the file preview element
    container.appendChild(removeButton);
}
