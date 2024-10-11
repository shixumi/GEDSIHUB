document.addEventListener("DOMContentLoaded", () => {
    // Event delegation for publish/unpublish toggle
    document.addEventListener('click', (event) => {
        // Check if the clicked element is a publish or unpublish div
        const publishDiv = event.target.closest('#publishLesson');
        const unpublishDiv = event.target.closest('#unpublishLesson');

        if (publishDiv) {
            const toggleContainer = publishDiv.closest('#toggleButtonPublishUnpublish');
            toggleContainer.querySelector('#publishLesson').style.display = 'none'; // Hide publish div
            toggleContainer.querySelector('#unpublishLesson').style.display = 'flex'; // Show unpublish div
            console.log('Lesson published');
        }

        if (unpublishDiv) {
            const toggleContainer = unpublishDiv.closest('#toggleButtonPublishUnpublish');
            toggleContainer.querySelector('#unpublishLesson').style.display = 'none'; // Hide unpublish div
            toggleContainer.querySelector('#publishLesson').style.display = 'flex'; // Show publish div
            console.log('Lesson unpublished');
        }
    });

    // On initial load or when new lessons are added, only show the publish button
    document.querySelectorAll('#toggleButtonPublishUnpublish').forEach(toggleContainer => {
        toggleContainer.querySelector('#unpublishLesson').style.display = 'none'; // Hide unpublish button
        toggleContainer.querySelector('#publishLesson').style.display = 'flex'; // Show publish button
    });
});
