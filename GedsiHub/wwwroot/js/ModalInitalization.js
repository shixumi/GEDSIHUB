﻿$(document).ready(function () {
    // Reinitialize all Bootstrap modals when the page loads
    $('.modal').each(function () {
        $(this).on('show.bs.modal', function (e) {
            $(this).modal('show');
        });
    });
});