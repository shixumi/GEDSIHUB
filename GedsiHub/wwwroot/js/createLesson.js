document.addEventListener("DOMContentLoaded", () => {
    const currentPage = window.location.pathname;

    // When a new module is created or edited
    if (currentPage.includes('editNewCreatedModule.html')) {
        const moduleId = getModuleId();
        clearLessonContainer(); // Clear the lesson container
        loadLessonData(moduleId); // Load lessons specific to the current module
    } else if (currentPage.includes('createNewLesson.html')) {
        const moduleId = getModuleId();
        handleCreateLessonPage(moduleId);
    } else if (currentPage.includes('addNewModule.html')) {
        resetSessionForNewModule(); // Reset data for a fresh start when creating a new module
    }
});

// Function to get or create the module ID, using the input field or sessionStorage
function getModuleId() {
    const moduleInput = document.getElementById('moduleInput'); // Assuming an input field exists with this ID for module title or name
    const moduleName = moduleInput ? moduleInput.value.trim() : '';

    // Check if a module with the same name exists in localStorage
    if (moduleName && isModuleDuplicate(moduleName)) {
        alert('A module with the same name already exists. Please use a different name.');
        return null; // Stop execution, preventing redirection
    }

    let moduleId = sessionStorage.getItem('currentModuleId');

    // If moduleId is not found in sessionStorage, create a new one
    if (!moduleId) {
        moduleId = `module_${Date.now()}`; // Create a new unique ID
        sessionStorage.setItem('currentModuleId', moduleId); // Store it in sessionStorage
    }

    return moduleId;
}

// Function to check if a module with the same name already exists in localStorage
function isModuleDuplicate(moduleName) {
    for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        if (key.startsWith('module_')) {
            const moduleData = JSON.parse(localStorage.getItem(key));
            if (moduleData && moduleData.name === moduleName) {
                return true; // A duplicate exists
            }
        }
    }
    return false;
}

// Function to reset the session and prepare for a new module creation
function resetSessionForNewModule() {
    sessionStorage.removeItem('currentModuleId'); // Remove any previously stored module ID
    sessionStorage.removeItem('currentLessonData'); // Clear any previous lesson data
}

// Function to clear the lesson container and display the "No lessons added" message
function clearLessonContainer() {
    const lessonContainer = document.getElementById('all_lesson_container');
    if (lessonContainer) {
        lessonContainer.innerHTML = `
            <div id="create_new_lesson_container" class="container d-xxl-flex AddlLessonContainer">
                <div id="add_new_lesson_container" class="d-xxl-flex justify-content-xxl-center align-items-xxl-center add_new_lesson_container">
                    <span class="no_lessons_added_text">No lessons have been added yet</span>
                    <button id="createNewLessonButton" class="create_lesson_button" type="button">
                        <img id="add_icon_create_lesson" src="Add_White.png" />Create Lesson</button>
                </div>
            </div>
        `;

        // Add event listener to redirect to createNewLesson.html
        document.getElementById('createNewLessonButton').addEventListener('click', () => {
            const moduleId = getModuleId();
            window.location.href = `createNewLesson.html?moduleId=${moduleId}`;
        });
    }
}

function loadLessonData(moduleId) {
    // Retrieve lesson data specific to the module from localStorage
    const lessonData = JSON.parse(localStorage.getItem(`lessons_${moduleId}`));
    
    const lessonContainer = document.getElementById('all_lesson_container');
    const modulesContainer = document.getElementById('modules_container'); // Assuming this is the ID for the modules container

    // Clear existing lessons
    lessonContainer.innerHTML = ''; // Clear the lesson container

    if (lessonData && lessonData.length > 0) {
        // Lessons exist for this module, display them
        lessonData.forEach(lesson => {
            // Create a new lesson container
            const lessonElement = document.createElement('div');
            lessonElement.id = 'edit_new_lesson_container'; // Assuming this is a general ID; consider using a class instead for multiple lessons
            lessonElement.className = 'd-xxl-flex edit_new_lesson_container';

            // Set the inner HTML of the lesson element
            lessonElement.innerHTML = `
                
                
                <div class="lessonNo_title_container">
                    <span>Lesson</span><span id="LessonNumberDisplay">${lesson.number}.</span>
                    <span id="LessonTitleDisplay">${lesson.title}</span>
                    <div class="lesson_con_linebreak"></div>
                </div>
                <span id="LessonOverviewDisplay" class="lesson_brief_overview">${lesson.overview}</span>
                <div class="button_container">
                    <div id="DeleteLesson"><img id="Lesson_Delete" class="Delete" src="Delete.png" /><span>Delete</span></div>
                    <div id="EditLesson"><img class="Edit" src="Edit.png" /><span>Edit</span></div>
                    <div id="toggleButtonPublishUnpublish" class="d-xxl-flex">
                        <div id="publishLesson" class="toggle-content"><img class="PublishIcon" src="Publish.png" /><span>Publish</span></div>
                        <div id="unpublishLesson" class="toggle-content"><img class="PublishIcon" src="Unpublish.png" /><span>Unpublish</span>
                    </div>
                </div><button id="go_to_lesson_button" class="btn btn-primary go_to_lesson_button" type="button">Go To Lesson</button>
                </div>

            `;

            // Append the new lesson element to the lesson container
            lessonContainer.appendChild(lessonElement);
        });

        // Now inject the buttons into the modules_container instead of lesson_container
        modulesContainer.innerHTML += `
            <button class="create_assessment_button" type="button">Create Assessment</button>
            <div id="addNewLesson" class="container d-xxl-flex addNewLesson">
                <div><img class="add_icon_lesson" src="Add.png" /><span>Add New Lesson</span></div>
            </div>
        `;

        // Add event listener for the "Add New Lesson" button
        document.getElementById('addNewLesson').addEventListener('click', () => {
            const moduleId = getModuleId();
            window.location.href = `createNewLesson.html?moduleId=${moduleId}`; // Redirect to create new lesson page
        });

    } else {
        // No lessons have been added, show the "Create Lesson" button
        clearLessonContainer();
    }
}

// Function to handle the create lesson page
function handleCreateLessonPage(moduleId) {
    const createLessonButton = document.getElementById('CreateLessonButton');
    const cancelButton = document.getElementById('cancelButton_createLesson');

    const urlParams = new URLSearchParams(window.location.search);
    const lessonNumber = urlParams.get('lessonNumber');
    const lessonTitle = urlParams.get('lessonTitle');
    const lessonOverview = urlParams.get('lessonOverview');

    // If in edit mode, populate the fields
    if (lessonNumber && lessonTitle && lessonOverview) {
        document.getElementById('CreateLesson_NumberInput').value = lessonNumber;
        document.getElementById('CreateLesson_TitleInput').value = lessonTitle;
        document.getElementById('CreateLesson_InputOverview').value = lessonOverview;

        // Change the button text to 'Save'
        createLessonButton.textContent = 'Save';
    }

    createLessonButton.addEventListener('click', (event) => {
        event.preventDefault();

        const newLessonNumber = document.getElementById('CreateLesson_NumberInput').value;
        const newLessonTitle = document.getElementById('CreateLesson_TitleInput').value;
        const newLessonOverview = document.getElementById('CreateLesson_InputOverview').value;

        if (!newLessonNumber || !newLessonTitle || !newLessonOverview) {
            alert('All fields are required!');
            return;
        }

        // Retrieve existing lessons for this module
        let lessons = JSON.parse(localStorage.getItem(`lessons_${moduleId}`)) || [];

        if (lessonNumber) {
            // If in edit mode, update the existing lesson
            const index = lessons.findIndex(lesson => lesson.number === lessonNumber);
            if (index !== -1) {
                lessons[index] = {
                    number: newLessonNumber,
                    title: newLessonTitle,
                    overview: newLessonOverview
                };
            }
        } else {
            // Add the new lesson to the array
            lessons.push({
                number: newLessonNumber,
                title: newLessonTitle,
                overview: newLessonOverview
            });
        }

        // Save lessons back to localStorage
        localStorage.setItem(`lessons_${moduleId}`, JSON.stringify(lessons));

        // Redirect back to editNewCreatedModule.html with moduleId
        window.location.href = `editNewCreatedModule.html?moduleId=${moduleId}`;
    });

    cancelButton.addEventListener('click', (event) => {
        event.preventDefault();
        window.location.href = `editNewCreatedModule.html?moduleId=${moduleId}`;
    });
}
