document.addEventListener('DOMContentLoaded', function() {
    const currentPage = window.location.pathname;

    // Check if we are on the CMSEditNewCreatedLesson.html page
    if (currentPage.includes('CMSEditNewCreatedLesson.html')) {
        const moduleTitleElement = document.getElementById('added_module_title');
        const moduleNumberElement = document.getElementById('module_number');

        // Retrieve module title and module number from sessionStorage or localStorage
        const moduleTitle = sessionStorage.getItem('moduleTitle') || 'Default Module Title'; // Replace with the actual key if needed
        const moduleNumber = sessionStorage.getItem('moduleNumber') || '01'; // Replace with the actual key if needed

        // Update the title and number on the page
        if (moduleTitleElement) {
            moduleTitleElement.textContent = moduleTitle;
        }
        if (moduleNumberElement) {
            moduleNumberElement.textContent = moduleNumber;
        }
    }

    // Breadcrumb navigation logic as per your previous code
    const modulesBreadcrumb = document.getElementById('modules_breadcrumb');
    if (modulesBreadcrumb) {
        modulesBreadcrumb.addEventListener('click', function() {
            window.location.href = 'modules.html';
        });
    }

    const modulesLessonsBreadcrumb = document.getElementById('modules_lessons_breadcrumb');
    if (modulesLessonsBreadcrumb) {
        modulesLessonsBreadcrumb.addEventListener('click', function() {
            window.location.href = 'editNewCreatedModule.html';
        });
    }

    const cmsLessonBreadcrumb = document.getElementById('cms_lesson_breadcrumb');
    if (cmsLessonBreadcrumb) {
        cmsLessonBreadcrumb.addEventListener('click', function() {
            window.location.href = 'CMSEditNewCreatedLesson.html';
        });
    }
});
