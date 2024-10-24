// wwwroot/js/global.js

document.addEventListener("DOMContentLoaded", function () {
    // Check if the user is authenticated before establishing a SignalR connection
    const isAuthenticated = document.body.getAttribute('data-authenticated') === 'true';

    if (isAuthenticated) {
        // Initialize SignalR connection
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/analyticsHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        // Handle UpdateActiveUsers event
        connection.on("UpdateActiveUsers", function (count) {
            const activeUsersElement = document.getElementById("activeUsersCount");
            if (activeUsersElement) {
                activeUsersElement.innerText = count;
            } else {
                console.warn("activeUsersCount element not found in DOM.");
            }
        });

        // Start the connection
        connection.start()
            .then(() => {
                console.log("SignalR Connected");

                // Optional: Implement Heartbeat Mechanism
                setInterval(() => {
                    connection.invoke("Heartbeat")
                        .catch(err => console.error("Heartbeat Error: ", err));
                }, 2 * 60 * 1000); // Every 2 minutes
            })
            .catch(err => {
                console.error("SignalR Connection Error: ", err);
                // Optionally, implement retry logic
            });

        // Optional: Handle connection close and retry
        connection.onclose(async () => {
            console.warn("SignalR connection closed. Attempting to reconnect...");
            try {
                await connection.start();
                console.log("SignalR Reconnected");
            } catch (err) {
                console.error("SignalR Reconnection Error: ", err);
                setTimeout(() => connection.start(), 5000); // Retry after 5 seconds
            }
        });
    }
});
