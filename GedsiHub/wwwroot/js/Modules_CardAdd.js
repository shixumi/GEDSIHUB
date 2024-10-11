// Common logic to save or update module data for both Add and Edit 
function saveModuleData() {
    const moduleTitle = document.getElementById('AddModule_TitleInput').value;
    const moduleNumber = document.getElementById('AddModule_NumberInput').value; // Get the value from the input

    if (!moduleTitle) {
        alert('Please enter the module title');
        return;
    }

    if (!moduleNumber) {
        alert('Please enter the module number');
        return;
    }

    let modules = JSON.parse(localStorage.getItem('modules')) || [];
    
    /*
    // Check for duplicate module number
    const isDuplicate = modules.some(module => module.number === moduleNumber);
    if (isDuplicate) {
        alert('Module number must be unique. Please enter a different module number.');
        return; // Do not save if duplicate
    }
    */

    const editModuleIndex = localStorage.getItem('editModuleIndex');

    if (editModuleIndex !== null && editModuleIndex !== '') {
        const index = parseInt(editModuleIndex, 10); // Ensure we are using an integer index
        if (!isNaN(index)) {
            // Edit existing module
            modules[index].title = moduleTitle;
            modules[index].number = moduleNumber; // Store module number
            localStorage.removeItem('editModuleIndex'); // Clear after editing to avoid duplication
        }
    } else {
        // Add new module
        modules.push({ title: moduleTitle, number: moduleNumber, color: '#000000', isPublished: false });

        // Set the last added module index to be edited on redirect
        const newModuleIndex = modules.length - 1;
        localStorage.setItem('editModuleIndex', newModuleIndex); // Store the new module's index for editing
    }

    // Update localStorage but don't trigger rendering immediately
    localStorage.setItem('modules', JSON.stringify(modules));
    
    // Save the newly added module title and number to localStorage for displaying in editNewCreatedModule.html
    localStorage.setItem('newModuleTitle', moduleTitle);
    localStorage.setItem('module_number', moduleNumber); // Store the module number

    console.log('Saved Module Number:', moduleNumber); // Log the number to verify it's being saved

    // Redirect to editNewCreatedModule.html
    window.location.href = 'editNewCreatedModule.html';
}


// Logic for deleting a module
function deleteModule(index) {
  const confirmDelete = confirm("Are you sure you want to delete this module?");
  if (confirmDelete) {
    let modules = JSON.parse(localStorage.getItem('modules')) || [];
    modules.splice(index, 1);
    localStorage.setItem('modules', JSON.stringify(modules));
    location.reload(); // Reload the page to update the displayed modules
  }
}

// Logic to update module color in localStorage
function updateModuleColor(index, color) {
  let modules = JSON.parse(localStorage.getItem('modules')) || [];
  modules[index].color = color;
  localStorage.setItem('modules', JSON.stringify(modules));

  document.getElementById(`colorPicker-${index}`).style.backgroundColor = color;
  const shadowColor = hexToRGBA(color, 0.3);
  document.getElementById(`colorPicker-${index}`).style.boxShadow = `0px 6px 10px ${shadowColor}`;
}

// Helper to convert hex to rgba color with opacity
function hexToRGBA(hex, opacity) {
  let r = 0, g = 0, b = 0;
  if (hex.length === 4) {
    r = "0x" + hex[1] + hex[1];
    g = "0x" + hex[2] + hex[2];
    b = "0x" + hex[3] + hex[3];
  } else if (hex.length === 7) {
    r = "0x" + hex[1] + hex[2];
    g = "0x" + hex[3] + hex[4];
    b = "0x" + hex[5] + hex[6];
  }
  return `rgba(${+r}, ${+g}, ${+b}, ${opacity})`;
}

// Display all modules on modules.html
function addAllModules() {
  const modules = JSON.parse(localStorage.getItem('modules')) || [];
  if (modules.length === 0) return;

  const moduleContainer = document.getElementById('moduleCardsContainer');
  
  // Clear all module cards but NOT the "Add New Module" button
  const existingCards = moduleContainer.querySelectorAll('.module_card');
  existingCards.forEach(card => card.remove()); // Ensure we remove old cards to prevent duplicates

modules.forEach((moduleData, index) => {
    const newCard = document.createElement('div');
    newCard.classList.add('module_card');
    newCard.id = `moduleCard-${index}`;

    newCard.innerHTML = `
      <div class="module_card" style="padding-right: 27px;padding-left: 27px;">
        <span class="moduleIcon_container" style="color: rgb(33, 37, 41);margin-left: 0px;">
          <img class="ModuleTempIcon" src="GADO_Logo_white.png" />
          <input id="colorPicker-${index}" class="ColorInput" type="color" value="${moduleData.color}"/></span>
          <span id="ModuleTitle-${index}" class="fw-bolder ModuleTitle">${moduleData.title}</span>
        <div id="NumberOfLessonsContainer" class="NumberOfLessonsContainer">
          <img class="LessonIcon" src="lesson.png" /><span id="NumberOfLessons" class="NumberOfLessons">0</span><span class="fw-normal LessonsText" style="margin-left: 0px;position: static;margin-top: 0px;">lessons</span></div>
        <div class="LikeViewContainer">
          <div class="LikeOnlyContainer"><img class="LikeIcon" src="Like.png"  /><span id="LikeCount">0</span></div>
          <div class="ViewOnlyContainer"><img class="ViewIcon" src="View.png"  /><span id="ViewCount">0</span></div>
        </div>
        <div class="ModuleCard_LineBreak"></div>
        <button id="publishButton-${index}" class="d-xxl-flex justify-content-xxl-start publishButton" type="button">
          <img id="publishIcon-${index}" class="PublishIcon" src="${moduleData.isPublished ? 'Unpublish.png' : 'Publish.png'}" /> ${moduleData.isPublished ? 'Unpublish' : 'Publish'}
        </button>
        <div class="d-xxl-flex justify-content-xxl-end" style="margin-top: 19px;">
          <img id="EditModule-${index}" class="Edit" src="Edit.png" />
          <img id="DeleteModule" class="Delete" src="Delete.png" onclick="deleteModule(${index})"/></div>
      </div>
    `;

    moduleContainer.insertBefore(newCard, moduleContainer.querySelector('.add-new-module'));

    applyColorShadowAndPicker(index, moduleData.color);

    document.getElementById(`colorPicker-${index}`).addEventListener('input', function (event) {
      updateModuleColor(index, event.target.value);
    });

    document.getElementById(`publishButton-${index}`).addEventListener('click', function() {
      togglePublish(index);
    });

    // Handle edit button click
    document.getElementById(`EditModule-${index}`).addEventListener('click', function () {
      localStorage.setItem('editModuleIndex', index);  // Store index of module to edit
      window.location.href = 'addNewModule.html';      // Redirect to add/edit page
    });

    // Add event listener to the ModuleTitle span for navigation
    document.getElementById(`ModuleTitle-${index}`).addEventListener('click', function () {
      // Store the selected module data in localStorage
      localStorage.setItem('newModuleTitle', moduleData.title);
      localStorage.setItem('module_number', moduleData.number);
      // Redirect to editNewCreatedModule.html
      window.location.href = 'editNewCreatedModule.html';
    });
});

}

// Apply color shadow dynamically when the page is loaded or color is changed
function applyColorShadowAndPicker(index, color) {
  const shadowColor = hexToRGBA(color, 0.3);
  const colorPickerElement = document.getElementById(`colorPicker-${index}`);
  colorPickerElement.style.boxShadow = `0px 6px 10px ${shadowColor}`;
  colorPickerElement.style.backgroundColor = color;
}

// Toggle Publish/Unpublish state of a module
function togglePublish(index) {
  let modules = JSON.parse(localStorage.getItem('modules')) || [];
  let module = modules[index];

  module.isPublished = !module.isPublished;
  localStorage.setItem('modules', JSON.stringify(modules));

  const button = document.getElementById(`publishButton-${index}`);
  const icon = document.getElementById(`publishIcon-${index}`);

  if (module.isPublished) {
    button.innerHTML = '<img class="PublishIcon" src="Unpublish.png" /> Unpublish';
  } else {
    button.innerHTML = '<img class="PublishIcon" src="Publish.png" /> Publish';
  }
}

// Redirect to addNewModule.html when clicking the Add Module button
function initializeAddModuleButtons() {
  const addModuleTopButton = document.getElementById('addModuleTopButton');
  const addNewModuleButtons = document.querySelectorAll('.add-new-module');

  // Redirect to 'addNewModule.html' for all add-related buttons
  if (addModuleTopButton) {
    addModuleTopButton.addEventListener('click', function () {
      localStorage.removeItem('editModuleIndex'); // Clear edit mode
      window.location.href = 'addNewModule.html';  // Go to add module mode
    });
  }

  addNewModuleButtons.forEach(button => {
    button.addEventListener('click', function () {
      localStorage.removeItem('editModuleIndex'); // Clear edit mode
      window.location.href = 'addNewModule.html';  // Go to add module mode
    });
  });
}

// Check for edit mode on addNewModule.html and pre-fill the form
function initializeEditModule() {
  const editModuleIndex = localStorage.getItem('editModuleIndex'); // Check if there's a module to edit

  if (editModuleIndex !== null) {
      const index = parseInt(editModuleIndex, 10); // Ensure index is parsed as an integer
      const modules = JSON.parse(localStorage.getItem('modules')) || [];
      const module = modules[index];

      if (module) {
          // Pre-fill the input fields with the existing module data
          document.getElementById('AddModule_TitleInput').value = module.title;
          document.getElementById('AddModule_NumberInput').value = module.number; // Pre-fill module number

          // Change the header text to "Edit Module"
          const headerTitle = document.getElementById('addNewModule_HeaderTitle');
          if (headerTitle) {
              headerTitle.textContent = 'Edit Module';
          }

          // Change the button text to "Save Module"
          const addModuleButton = document.getElementById('AddModuleButton');
          if (addModuleButton) {
              addModuleButton.textContent = 'Save Module';
          }
      } else {
          console.error('Module not found at the provided index in localStorage.');
      }
  } else {
      // We're in add mode, leave the header and button text as they are
      const headerTitle = document.getElementById('addNewModule_HeaderTitle');
      if (headerTitle) {
          headerTitle.textContent = 'Add Module';
      }

      const addModuleButton = document.getElementById('AddModuleButton');
      if (addModuleButton) {
          addModuleButton.textContent = 'Add Module';
      }
  }

  // Add event listener to handle saving or updating the module
  document.getElementById('AddModuleButton').addEventListener('click', saveModuleData);
}


// Automatically display modules on modules.html and handle adding/editing modules
window.onload = function () {
    // Check the current page and perform actions accordingly
    if (window.location.pathname.includes('modules.html')) {
        // Handle module display on the modules page
        addAllModules();
        initializeAddModuleButtons();
    } else if (window.location.pathname.includes('addNewModule.html')) {
        // Handle the add/edit module page
        initializeEditModule();
    } else if (window.location.pathname.includes('editNewCreatedModule.html')) {
        // Handle the editNewCreatedModule page, display newly added module title and number
        const newModuleTitle = localStorage.getItem('newModuleTitle');
        const moduleNumber = localStorage.getItem('module_number');

        // Display the module title if it exists
        if (newModuleTitle) {
            const titleElement = document.getElementById('added_module_title');
            if (titleElement) {
                titleElement.textContent = newModuleTitle;
            } else {
                console.error('Element with id "added_module_title" not found.');
            }
        }

        // Display the module number if it exists
        if (moduleNumber) {
            const numberElement = document.getElementById('added_module_number');
            if (numberElement) {
                numberElement.textContent = moduleNumber;
            } else {
                console.error('Element with id "added_module_number" not found.');
            }
        }
    }
};
