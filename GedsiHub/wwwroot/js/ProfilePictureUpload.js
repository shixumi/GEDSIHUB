document.addEventListener('DOMContentLoaded', function () {
    const editProfilePictureInput = document.getElementById('editProfilePictureInput');
    const editProfilePicture = document.getElementById('editProfilePicture');
    const editCropModal = document.getElementById('editCropModal');
    const editCropButton = document.getElementById('editCropButton');
    const editCroppedImage = document.getElementById('editCroppedImage');
    let cropper;
    let croppedBlob;

    // Show cropping modal when a file is selected
    editProfilePictureInput.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                editProfilePicture.src = e.target.result;
                editCropModal.style.display = 'flex';
                cropper = new Cropper(editProfilePicture, {
                    aspectRatio: 1,
                    viewMode: 1,
                });
            };
            reader.readAsDataURL(file);
        }
    });

    // Crop the image and update the preview
    editCropButton.addEventListener('click', function () {
        if (cropper) {
            const canvas = cropper.getCroppedCanvas({
                width: 150,
                height: 150,
            });
            canvas.toBlob(function (blob) {
                const url = URL.createObjectURL(blob);
                editCroppedImage.src = url;
                croppedBlob = blob; // Store the blob for form submission
                editCropModal.style.display = 'none'; // Hide the modal
                cropper.destroy(); // Destroy the cropper instance
            }, 'image/jpeg'); // Explicitly set to 'image/jpeg'
        }
    });

    // Intercept form submission and attach the cropped image blob
    const form = document.querySelector('.edit-profile-form');
    form.addEventListener('submit', function (event) {
        if (croppedBlob) {
            event.preventDefault();

            const formData = new FormData(form);
            formData.set('ProfilePicture', croppedBlob, 'cropped_image.png');

            fetch(form.action, {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = response.url; // Redirect on success
                    } else {
                        alert('Failed to update profile. Please try again.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while updating your profile.');
                });
        }
    });
});
