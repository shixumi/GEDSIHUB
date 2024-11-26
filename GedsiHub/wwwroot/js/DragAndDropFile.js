(function () {
    let uploadContainer = document.getElementById('Lesson_Content_Upload_Container');
    const fileInput = document.getElementById('uploadFile');
    const placeholder = document.createElement('div'); // Placeholder to maintain position
    placeholder.id = 'uploadContainerPlaceholder';

    if (!uploadContainer || !fileInput) {
        console.error("Upload container or file input not found.");
        return;
    }

    // Prevent default drag-and-drop behavior
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        uploadContainer.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    // Highlight container on drag over
    ['dragenter', 'dragover'].forEach(eventName => {
        uploadContainer.addEventListener(eventName, () => {
            uploadContainer.classList.add('highlight');
            uploadContainer.style.backgroundColor = '#f0f8ff'; // Change background color
            uploadContainer.style.borderColor = '#3b82f6'; // Highlight border color
        });
    });

    // Remove highlight on drag leave or drop
    ['dragleave', 'drop'].forEach(eventName => {
        uploadContainer.addEventListener(eventName, () => {
            uploadContainer.classList.remove('highlight');
            uploadContainer.style.backgroundColor = ''; // Reset background color
            uploadContainer.style.borderColor = ''; // Reset border color
        });
    });

    // Handle file drop
    uploadContainer.addEventListener('drop', (e) => {
        const files = e.dataTransfer.files;
        if (files.length) {
            removeUploadContainer();
            handleFiles(files);
        }
    });

    // Handle file input selection
    fileInput.addEventListener('change', (e) => {
        const files = e.target.files;
        if (files.length) {
            removeUploadContainer();
            handleFiles(files);
        }
    });

    // Remove the upload container
    function removeUploadContainer() {
        uploadContainer.parentNode.insertBefore(placeholder, uploadContainer); // Insert placeholder
        uploadContainer.remove(); // Remove the container from the DOM
    }

    // Handle file processing and previews
    function handleFiles(files) {
        [...files].forEach(file => previewFile(file));
    }

    // Preview files
    function previewFile(file) {
        const previewWrapper = document.createElement('div');
        previewWrapper.classList.add('file-preview');
        previewWrapper.style.marginTop = '10px';
        previewWrapper.style.textAlign = 'center';
        previewWrapper.style.border = '2px solid #eee';
        previewWrapper.style.borderRadius = '10px';
        previewWrapper.style.padding = '10px';
        previewWrapper.style.backgroundColor = '#f9f9f9';
        previewWrapper.style.display = 'flex';
        previewWrapper.style.flexDirection = 'column'; 
        previewWrapper.style.alignItems = 'center';
        previewWrapper.style.justifyContent = 'center';
        
        let filePreviewElement;

        if (file.type.startsWith("image/")) {
            filePreviewElement = document.createElement('img');
            filePreviewElement.src = URL.createObjectURL(file);
            filePreviewElement.style.maxWidth = '100%';
            filePreviewElement.style.height = 'auto';
            filePreviewElement.style.display = 'block';
        } else if (file.type.startsWith("video/")) {
            filePreviewElement = document.createElement('video');
            filePreviewElement.src = URL.createObjectURL(file);
            filePreviewElement.controls = true;
            filePreviewElement.style.maxWidth = '100%';
            filePreviewElement.style.height = 'auto';
        } else if (file.type === 'application/pdf') {
            const pdfUrl = `${URL.createObjectURL(file)}#zoom=100`;
            filePreviewElement = document.createElement('iframe');
            filePreviewElement.src = pdfUrl;
            filePreviewElement.style.width = '100%';
            filePreviewElement.style.height = '100vh';
            filePreviewElement.style.border = 'none';
            previewWrapper.style.width = '100%';
        } else {
            filePreviewElement = document.createElement('a');
            filePreviewElement.href = URL.createObjectURL(file);
            filePreviewElement.textContent = `Download ${file.name}`;
            filePreviewElement.style.textDecoration = 'underline';
            filePreviewElement.style.color = '#007bff';
            filePreviewElement.target = '_blank';
        }

        previewWrapper.appendChild(filePreviewElement);

        const removeButton = document.createElement('button');
        removeButton.textContent = 'Remove File';
        removeButton.classList.add('remove-button');
   

        removeButton.addEventListener('click', () => {
            previewWrapper.remove();
            restoreUploadContainer();
        });

        previewWrapper.appendChild(removeButton);

        // Append the preview wrapper to the parent container
        placeholder.parentNode.insertBefore(previewWrapper, placeholder);
    }

    // Restore the upload container
    function restoreUploadContainer() {
        const restoredContainer = document.createElement('div');
        restoredContainer.id = 'Lesson_Content_Upload_Container';
        restoredContainer.className = uploadContainer.className;
        restoredContainer.innerHTML = `
            <img class="upload_icon" src="/images/Upload.png" />
            <span class="upload_text">Drag and Drop Files or Click to Upload Text or Images</span>
            <input type="file" name="uploadFile" id="uploadFile" class="uploadButton" accept=".mp4,.mov,.avi,.jpg,.png,.jpeg,.pdf,.doc,.docx" />
            <label for="uploadFile" class="uploadButtonLabel"> Upload</label>
        `;

        // Replace the placeholder with the restored container
        placeholder.parentNode.replaceChild(restoredContainer, placeholder);

        // Update the global reference
        uploadContainer = restoredContainer;

        // Reattach the change event listener to the new file input
        const newFileInput = restoredContainer.querySelector('#uploadFile');
        newFileInput.addEventListener('change', (e) => {
            const files = e.target.files;
            if (files.length) {
                removeUploadContainer();
                handleFiles(files);
            }
        });

        // Reapply drag-and-drop listeners
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            restoredContainer.addEventListener(eventName, preventDefaults, false);
        });
    }
})();
