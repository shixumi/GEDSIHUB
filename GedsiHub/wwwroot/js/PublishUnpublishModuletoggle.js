document.addEventListener("DOMContentLoaded", function () {
    // Find all publish/unpublish containers
    document.querySelectorAll('[id^="toggleButtonPublishUnpublishModule-"]').forEach(function (toggleContainer) {
        const moduleId = toggleContainer.id.split('-')[1]; // Extract the module ID from the element ID
        const publishButton = document.getElementById("publishModule-" + moduleId);
        const unpublishButton = document.getElementById("unpublishModule-" + moduleId);

        // Show the publish button initially and hide the unpublish button
        publishButton.style.display = "flex";
        unpublishButton.style.display = "none";

        // Add event listener for the publish button
        publishButton.addEventListener("click", function () {
            publishButton.style.display = "none"; // Hide publish button
            unpublishButton.style.display = "flex"; // Show unpublish button
        });

        // Add event listener for the unpublish button
        unpublishButton.addEventListener("click", function () {
            unpublishButton.style.display = "none"; // Hide unpublish button
            publishButton.style.display = "flex"; // Show publish button
        });
    });
});
