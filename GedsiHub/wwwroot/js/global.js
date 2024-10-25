// wwwroot/js/global.js

(function () {
    if (window.signalRConnection) {
        console.log("SignalR connection already exists.");
        return;
    }

    window.signalRConnection = null;

    document.addEventListener("DOMContentLoaded", function () {
        const isAuthenticated = document.body.getAttribute('data-authenticated') === 'true';

        if (isAuthenticated) {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("/analyticsHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            connection.on("UpdateActiveUsers", function (count) {
                const activeUsersElements = document.querySelectorAll(".activeUsersCount");
                if (activeUsersElements.length > 0) {
                    activeUsersElements.forEach(function (element) {
                        element.innerText = count;
                    });
                } else {

                }
            });

            connection.start()
                .then(() => {
                    console.log("SignalR Connected");

                    setInterval(() => {
                        connection.invoke("Heartbeat")
                            .catch(err => console.error("Heartbeat Error: ", err));
                    }, 2 * 60 * 1000); // Every 2 minutes
                })
                .catch(err => {
                    console.error("SignalR Connection Error: ", err);
                });

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

            window.signalRConnection = connection;
        } else {
            console.warn("User is not authenticated. SignalR connection not established.");
        }
    });
})();
