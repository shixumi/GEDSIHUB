// wwwroot/js/analytics-dashboard.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/analyticsHub")
    .build();

connection.on("ReceiveAnalyticsUpdate", function (data) {
    // Update the dashboard with new data
    console.log("Analytics Update Received:", data);
    // Implement UI update logic here
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
