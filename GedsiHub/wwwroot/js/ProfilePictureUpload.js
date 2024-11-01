document.addEventListener('DOMContentLoaded', function () {
    // Your JavaScript code goes here
    const editProfilePictureInput = document.getElementById('editProfilePictureInput');
    const editProfilePicture = document.getElementById('editProfilePicture');
    const editCropModal = document.getElementById('editCropModal');
    const editCropButton = document.getElementById('editCropButton');
    const editCroppedImage = document.getElementById('editCroppedImage');
    let cropper;

    // Handle file input change
    editProfilePictureInput.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                editProfilePicture.src = e.target.result;
                editCropModal.style.display = 'flex'; // Show the modal
                cropper = new Cropper(editProfilePicture, {
                    aspectRatio: 1, // Square crop
                    viewMode: 1, // Restrict the crop box to the image bounds
                });
            };
            reader.readAsDataURL(file);
        }
    });

    // Handle crop button click
    editCropButton.addEventListener('click', function () {
        if (cropper) {
            const canvas = cropper.getCroppedCanvas({
                width: 150,
                height: 150,
            });
            // Convert the canvas to a blob and update the profile image
            canvas.toBlob(function (blob) {
                const url = URL.createObjectURL(blob);
                editCroppedImage.src = url;
                editCropModal.style.display = 'none'; // Hide the modal
                cropper.destroy(); // Destroy the cropper instance
            });
        }
    });

    // Close the modal when clicking outside of it
    window.addEventListener('click', function (event) {
        if (event.target === editCropModal) {
            editCropModal.style.display = 'none';
            if (cropper) {
                cropper.destroy();
            }
        }
    });
});
