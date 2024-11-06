// Array of colors for the circles (rainbow colors)
const circleColors = ['#FF6663', '#FEB144', '#f7e788', '#91d694', '#9ECBCF', '#8183a9', '#c594c3'];

// Function to set colors for existing notifications
function updateNotificationColors(tableClass) {
    const tableBody = document.querySelector(`.${tableClass} tbody`);
    const notifications = tableBody.querySelectorAll('tr'); // Select all notification rows

    notifications.forEach((notificationRow, index) => {
        const notifCircle = notificationRow.querySelector('.notif-circle');
        if (notifCircle) {
            notifCircle.style.backgroundColor = circleColors[index % circleColors.length]; // Assign color
        }
    });
}

// Function to add a new row with a colored circle to each table independently
function addNotificationRow(content, timeAgo, title, isImportant) {
    const tableClass = isImportant ? 'important-notif-table' : 'less-important-notif-table';
    const tableBody = document.querySelector(`.${tableClass} tbody`);

    // Create new row
    const newRow = document.createElement('tr');
    newRow.classList.add('notification-item');

    // Create the circle cell
    const circleCell = document.createElement('td');
    const notifCircle = document.createElement('div');
    notifCircle.classList.add('notif-circle');

    // Set the color based on the current number of rows in the specific table
    const rowIndex = tableBody.children.length; // Only count rows in this table
    notifCircle.style.backgroundColor = circleColors[rowIndex % circleColors.length]; // Rainbow color assignment

    // Append the circle to the cell
    circleCell.appendChild(notifCircle);
    newRow.appendChild(circleCell);

    // Create content cell
    const contentCell = document.createElement('td');
    contentCell.innerHTML = `<strong>${title}</strong><br /><span>${content.length > 100 ? content.substring(0, 100) + "..." : content}</span>`;
    newRow.appendChild(contentCell);

    // Create time cell
    const timeCell = document.createElement('td');
    timeCell.classList.add('timestamp');
    timeCell.innerHTML = timeAgo;
    newRow.appendChild(timeCell);

    // Append the new row to the table body
    tableBody.appendChild(newRow);

    // Update colors for all notifications after adding a new one
    updateNotificationColors(tableClass);
}


// Call to update existing notifications (you may call this once during initialization if needed)
updateNotificationColors('important-notif-table'); // Update existing important notifications
updateNotificationColors('less-important-notif-table'); // Update existing less important notifications
