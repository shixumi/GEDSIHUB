$(document).ready(function () {
    // SignalR setup
    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

    connection.on("UpdateNotifications", function () {
        getUnreadCount();
        // Optionally refresh the notifications list
        refreshNotifications();
    });

    connection.start().then(function () {
        console.log('SignalR connected');
    }).catch(function (err) {
        return console.error(err.toString());
    });

    // Function to get unread notifications count
    function getUnreadCount() {
        $.getJSON('/Notifications/GetUnreadCount', function (count) {
            $('#notificationsCount').text(count);
        });
    }

    // Function to refresh notifications on the page
    function refreshNotifications() {
        $.get('/Notifications/Index', function (data) {
            $('#notification-container').html($(data).find('#notification-container').html());
        });
    }

    // Initial load
    getUnreadCount();

    // Toggle between All and Unread notifications
    $(document).on('click', '.toggle-option', function () {
        $('.toggle-option').removeClass('selected');
        $(this).addClass('selected');
        var filter = $(this).data('filter');
        if (filter === 'unread') {
            $('.notification-item.read').hide();
            $('.notification-item.unread').show();
        } else {
            $('.notification-item').show();
        }
    });

    // Mark notification as read when clicked
    $(document).on('click', '.notification-item', function () {
        var notificationId = $(this).data('id');
        var row = $(this);
        $.post('/Notifications/MarkAsRead', { id: notificationId }, function () {
            row.removeClass('unread').addClass('read');
            getUnreadCount();
        });
    });
});
