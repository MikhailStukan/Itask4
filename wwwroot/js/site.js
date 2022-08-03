// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#fullCheckBox').click(function () {
    if ($(this).is(':checked')) {
        $('input:checkbox').prop('checked', true);
    } else {
        $('input:checkbox').prop('checked', false);
    }
});

$('input:checkbox').click(function () {
    if ($(this).is(':checked')) { }
    else { $('#fullCheckBox').prop('checked', false); }
});

var id = [];
function CheckedID() {
    var object = $('td input[type ="checkbox"]:checked');

    $.each(object, function (index, value) {
        console.log('Index:' + index + '; Value' + value.id);
        id[index] = value.id;
    });

}

$('#buttonDelete').click(function () {
    CheckedID();
    $.ajax({
        url: '/Home/Delete',
        type: 'POST',
        data: { id: id },
        success: function (result) {
            if (result == 1) {
                window.location = '/';
            }
            else {
                alert("Error delete");
            }
        }
    });
});

$('#buttonBlock').click(function () {
    CheckedID();
    $.ajax({
        url: '/Home/BlockUser',
        type: 'POST',
        data: { id: id, block: "Block" },
        success: function (result) {
            if (result == 1) {
                window.location = '/';
            }
            else {
                alert("Error status");
            }
        }
    });
});

$('#buttonUnblock').click(function () {
    CheckedID();
    $.ajax({
        url: '/Home/BlockUser',
        type: 'POST',
        data: { id: id, block: "Unblock" },
        success: function (result) {
            if (result == 1) {
                window.location = '/';
            }
            else {
                alert("Error status");
            }
        }
    });
});