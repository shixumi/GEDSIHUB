document.addEventListener("DOMContentLoaded", function () {
    // Function to handle dropdown and input behavior
    function handleSelectChange(selectElement, inputElement) {
        if (selectElement && inputElement) {
            selectElement.addEventListener("change", function () {
                if (selectElement.value === "Others") {
                    // Show the input field when 'Others' is selected
                    inputElement.style.display = "inline-block";
                    inputElement.focus(); // Focus on the input field

                    // Make the select element value effectively "empty"
                    selectElement.value = ""; // Treat "Others" as an invalid value
                } else {
                    // Hide the input field and reset its value
                    inputElement.style.display = "none";
                    inputElement.value = "";
                }
            });

            // Listen for input in the text field and add a new custom option on 'Enter'
            inputElement.addEventListener("keypress", function (event) {
                if (event.key === "Enter") {
                    event.preventDefault(); // Prevent form submission or other default actions
                    const customText = inputElement.value;

                    if (customText) {
                        // Create a new option with the custom text
                        const newOption = document.createElement("option");
                        newOption.value = customText;
                        newOption.text = customText;

                        // Insert the new option before the "Others" option (which is always the last option)
                        const othersOption = selectElement.querySelector('option[value="Others"]');
                        selectElement.insertBefore(newOption, othersOption);

                        // Select the newly added option
                        selectElement.value = customText;

                        // Hide the input field after entering the text
                        inputElement.style.display = "none";
                    }
                }
            });
        }
    }

    // Check for suggestion elements and apply the function
    const suggestionSelectElement = document.getElementById("typeOfSuggestionSelect");
    const suggestionInputElement = document.getElementById("otherSuggestionInput");

    if (suggestionSelectElement && suggestionInputElement) {
        handleSelectChange(suggestionSelectElement, suggestionInputElement);
    }

    // Check for complaint elements and apply the function
    const issueSelectElement = document.getElementById("typeOfIssueSelect");
    const issueInputElement = document.getElementById("otherComplaintInput");

    if (issueSelectElement && issueInputElement) {
        handleSelectChange(issueSelectElement, issueInputElement);
    }
});
