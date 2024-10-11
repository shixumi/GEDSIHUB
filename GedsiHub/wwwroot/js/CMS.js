document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener('click', (event) => {
        const goToLessonButton = event.target.closest('#go_to_lesson_button');
        if (goToLessonButton) {
            const lessonContainer = goToLessonButton.closest('.edit_new_lesson_container');
            const lessonNumber = lessonContainer.querySelector('#LessonNumberDisplay').textContent.trim();
            const lessonTitle = lessonContainer.querySelector('#LessonTitleDisplay').textContent.trim();
            const lessonOverview = lessonContainer.querySelector('#LessonOverviewDisplay').textContent.trim();

            // Retrieve the moduleId from sessionStorage or relevant storage
            const moduleId = sessionStorage.getItem('currentModuleId');

            // Redirect to CMSEditNewCreatedLesson.html with the lesson details as query parameters
            const editUrl = `CMSEditNewCreatedLesson.html?moduleId=${moduleId}&lessonNumber=${encodeURIComponent(lessonNumber)}&lessonTitle=${encodeURIComponent(lessonTitle)}&lessonOverview=${encodeURIComponent(lessonOverview)}`;
            window.location.href = editUrl;
        }
    });
});
