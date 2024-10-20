document.addEventListener("DOMContentLoaded", () => {
    // Event delegation for publish/unpublish toggle
    document.addEventListener('click', (event) => {
        // Check if the clicked element is a publish or unpublish div
        const publishDiv = event.target.closest('#publishAssessment');
        const unpublishDiv = event.target.closest('#unpublishAssessment');

        if (publishDiv) {
            const toggleContainer = publishDiv.closest('#toggleButtonPublishUnpublishAssessment');
            toggleContainer.querySelector('#publishAssessment').style.display = 'none'; // Hide publish div
            toggleContainer.querySelector('#unpublishAssessment').style.display = 'flex'; // Show unpublish div
            console.log('Assessment published');
        }

        if (unpublishDiv) {
            const toggleContainer = unpublishDiv.closest('#toggleButtonPublishUnpublishAssessment');
            toggleContainer.querySelector('#unpublishAssessment').style.display = 'none'; // Hide unpublish div
            toggleContainer.querySelector('#publishAssessment').style.display = 'flex'; // Show publish div
            console.log('Assessment unpublished');
        }
    });

    // On initial load or when new lessons are added, only show the publish button
    document.querySelectorAll('#toggleButtonPublishUnpublishAssessment').forEach(toggleContainer => {
        toggleContainer.querySelector('#unpublishAssessment').style.display = 'none'; // Hide unpublish button
        toggleContainer.querySelector('#publishAssessment').style.display = 'flex'; // Show publish button
    });
});
