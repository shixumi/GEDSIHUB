document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener('click', (event) => {
        const editDiv = event.target.closest('#EditLesson');
        if (editDiv) {
            const lessonContainer = editDiv.closest('.edit_new_lesson_container');

            // Retrieve the lesson number and remove the period
            const lessonNumber = lessonContainer.querySelector('#LessonNumberDisplay').textContent.trim().replace('.', '').trim();
            const lessonTitle = lessonContainer.querySelector('#LessonTitleDisplay').textContent.trim();
            const lessonOverview = lessonContainer.querySelector('#LessonOverviewDisplay').textContent.trim();

            // Redirect to createNewLesson.html with query parameters for editing
            const moduleId = sessionStorage.getItem('currentModuleId');
            const editUrl = `createNewLesson.html?moduleId=${moduleId}&lessonNumber=${lessonNumber}&lessonTitle=${encodeURIComponent(lessonTitle)}&lessonOverview=${encodeURIComponent(lessonOverview)}`;
            window.location.href = editUrl;
        }
    });
});
