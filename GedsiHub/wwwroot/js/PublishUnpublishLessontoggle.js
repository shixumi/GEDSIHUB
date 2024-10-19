document.addEventListener("DOMContentLoaded", () => {
    // Event delegation for publish/unpublish toggle
    document.addEventListener('click', (event) => {
        // Check if the clicked element is a publish or unpublish div
        const publishDiv = event.target.closest('#publishModule');
        const unpublishDiv = event.target.closest('#unpublishModule');

        if (publishDiv) {
            const toggleContainer = publishDiv.closest('#toggleButtonPublishUnpublishModule');
            toggleContainer.querySelector('#publishModule').style.display = 'none'; // Hide publish div
            toggleContainer.querySelector('#unpublishModule').style.display = 'flex'; // Show unpublish div
            console.log('Module published');
        }

        if (unpublishDiv) {
            const toggleContainer = unpublishDiv.closest('#toggleButtonPublishUnpublishModule');
            toggleContainer.querySelector('#unpublishModule').style.display = 'none'; // Hide unpublish div
            toggleContainer.querySelector('#publishModule').style.display = 'flex'; // Show publish div
            console.log('Module unpublished');
        }
    });

    // On initial load or when new modules are added, only show the publish button
    document.querySelectorAll('#toggleButtonPublishUnpublishModule').forEach(toggleContainer => {
        toggleContainer.querySelector('#unpublishModule').style.display = 'none'; // Hide unpublish button
        toggleContainer.querySelector('#publishModule').style.display = 'flex'; // Show publish button
    });
});