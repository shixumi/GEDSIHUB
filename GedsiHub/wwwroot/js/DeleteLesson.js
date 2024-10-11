document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener('click', (event) => {
        const deleteDiv = event.target.closest('#DeleteLesson');
        if (deleteDiv) {
            const lessonContainer = deleteDiv.closest('.edit_new_lesson_container');
            const lessonNumber = lessonContainer.querySelector('#LessonNumberDisplay').textContent.trim();

            // Confirmation before deletion
            if (confirm('Are you sure you want to delete this lesson?')) {
                const moduleId = sessionStorage.getItem('currentModuleId');

                // Remove lesson from local storage
                removeLessonFromLocalStorage(moduleId, lessonNumber);
                
                // Remove the lesson from the DOM
                lessonContainer.remove();
                console.log(`Lesson ${lessonNumber} deleted.`);
            }
        }
    });
});

// Function to remove a lesson from localStorage
function removeLessonFromLocalStorage(moduleId, lessonNumber) {
    // Retrieve the existing lessons from localStorage
    const lessons = JSON.parse(localStorage.getItem(`lessons_${moduleId}`)) || [];

    // Filter out the lesson that matches the lessonNumber
    const updatedLessons = lessons.filter(lesson => lesson.number !== lessonNumber);

    // Save the updated lessons back to localStorage
    localStorage.setItem(`lessons_${moduleId}`, JSON.stringify(updatedLessons));
}
