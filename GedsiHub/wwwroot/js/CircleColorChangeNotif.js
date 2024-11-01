// Array of colors for the circles (rainbow colors)
const circleColors = ['#FF6663', '#FEB144', '#f7e788', '#91d694', '#9ECBCF', '#8183a9', '#c594c3'];

// Function to add a new row with a colored circle to each table independently
function addNotificationRow(content, timeAgo, tableClass) {
    const tableBody = document.querySelector(`.${tableClass} tbody`);

    //if (!tableBody) {
    //    console.error(`Table with class ${tableClass} not found.`);
    //    return;
    //}

    // Create new row
    const newRow = document.createElement('tr');

    // Create the circle cell
    const circleCell = document.createElement('td');
    const notifCircle = document.createElement('div');
    notifCircle.classList.add('notif-circle');
    notifCircle.innerHTML = '<img src="/images/GADO_Logo_white.png" alt="GADO Icon" />';

    // Set the color based on the current number of rows in the specific table
    const rowIndex = tableBody.children.length; // Only count rows in this table
    notifCircle.style.backgroundColor = circleColors[rowIndex % circleColors.length];

    // Append the circle to the cell
    circleCell.appendChild(notifCircle);
    newRow.appendChild(circleCell);

    // Create content cell
    const contentCell = document.createElement('td');
    contentCell.innerHTML = content;
    newRow.appendChild(contentCell);

    // Create time cell
    const timeCell = document.createElement('td');
    timeCell.innerHTML = timeAgo;
    newRow.appendChild(timeCell);

    // Append the new row to the table body
    tableBody.appendChild(newRow);
}

// ERROR: Table with class less-important-notif-table not found.
// Example usage for both tables, each starting with red, orange, yellow, etc.
/*
addNotificationRow('The new module "Understanding Workplace Inclusivity" has been successfully published.', '1h ago', 'important-notif-table');
addNotificationRow('Another important update.', '2h ago', 'important-notif-table');

addNotificationRow('The less important module has been published.', '1h ago', 'less-important-notif-table');
addNotificationRow('Another less important update.', '2h ago', 'less-important-notif-table');
*/
