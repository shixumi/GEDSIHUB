// Apply color shadow dynamically when the page is loaded or color is changed
function applyColorShadowAndPicker(moduleId, color) {
    const shadowColor = hexToRGBA(color, 0.3);  // Set shadow with opacity 0.3
    const colorPickerElement = document.getElementById(`colorPicker-${moduleId}`);
    const moduleIconContainer = colorPickerElement.closest('.moduleIcon_container'); // Get the parent container

    // Apply the selected color as the background of the parent container
    moduleIconContainer.style.backgroundColor = color;

    // Ensure no black lines by setting border and background properly
    moduleIconContainer.style.border = 'none';  // Ensure no border
    colorPickerElement.style.border = 'none';   // Ensure no border on the input

    // Apply the box shadow to the color picker itself
    colorPickerElement.style.boxShadow = `0px 6px 10px ${shadowColor}`;
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

// Initialize color pickers and apply shadows
function initializeColorPickers() {
    const colorPickers = document.querySelectorAll('.ColorInput');

    colorPickers.forEach(picker => {
        const moduleId = picker.id.split('-')[1];  // Get moduleId from input id
        const color = picker.value;  // The initial color value

        applyColorShadowAndPicker(moduleId, color);  // Apply the shadow on page load

        // Listen for color changes from the picker
        picker.addEventListener('input', function (event) {
            const newColor = event.target.value;
            applyColorShadowAndPicker(moduleId, newColor);

            // Here you can add logic to update the color on the server-side using AJAX if needed
        });
    });
}

// Call this function when the page is loaded to initialize all color pickers
window.onload = function () {
    initializeColorPickers(); // Initialize color pickers on page load
};
