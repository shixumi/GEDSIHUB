document.addEventListener("DOMContentLoaded", function () {
    const notificationsDropdown = document.getElementById("notificationsDropdown");
    const bellIcon = notificationsDropdown.querySelector(".fa-bell");

    notificationsDropdown.addEventListener("show.bs.dropdown", function () {
        bellIcon.style.color = "#2c2c2c"; // Set darker color
    });

    notificationsDropdown.addEventListener("hide.bs.dropdown", function () {
        bellIcon.style.color = "#464040"; // Restore original color
    });
});
