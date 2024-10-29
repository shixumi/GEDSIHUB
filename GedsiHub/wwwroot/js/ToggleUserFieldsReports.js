function toggleUserFields() {
    const userType = document.getElementById("userType").value;
    const studentFields = document.getElementById("studentFields");
    const employeeFields = document.getElementById("employeeFields");
    const studentCheckboxField = document.getElementById("studentCheckboxField");
    const employeeCheckboxField = document.getElementById("employeeCheckboxField");

    // Hide all fields initially
    studentFields.style.display = "none";
    employeeFields.style.display = "none";
    studentCheckboxField.style.display = "none";
    employeeCheckboxField.style.display = "none";

    if (userType === "student") {
        studentFields.style.display = "block"; // Show student fields
        studentCheckboxField.style.display = "block"; // Show student checkboxes
    } else if (userType === "employee") {
        employeeFields.style.display = "block"; // Show employee fields
        employeeCheckboxField.style.display = "block"; // Show employee checkboxes
    }
}
