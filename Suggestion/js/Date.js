
$(document).ready(function () {
    $('#reservation').daterangepicker(null, function (start, end) {
        console.log(start.toISOString(), end.toISOString());
    });
});
