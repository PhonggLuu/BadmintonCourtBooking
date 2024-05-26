// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#createSchedule').on('click', function (e) {
    $(`${$(this).attr('data-target')}`).modal('toggle')
})