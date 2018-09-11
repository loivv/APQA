function showModel(id) {

    $('#' + id).modal('show');
}
function showModelFix(id) {
    $('#' + id).modal({
        backdrop: 'static',
        keyboard: false
    })
    $('#' + id).modal('show');
}
function hideModel(id) {
    $('#' + id).modal('hide');
}


function showLoader(isShow) {

    if (isShow) {
        $("#loader").css("display", "block");
    } else {
        $("#loader").css("display", "none");
    }
}

// show notify
function showNotify(title, mesenger) {

    $.notify({
        title: title,
        message: mesenger
    }, {
        type: 'success'
    });

}

function showNotify( mesenger) {

    $.notify(mesenger, {
        type: 'success'
    });

}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});