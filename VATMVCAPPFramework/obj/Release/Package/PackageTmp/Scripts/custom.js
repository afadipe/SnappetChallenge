
$(function () {

    $.fn.datepicker.defaults.autoclose = true;
    $.fn.datepicker.defaults.format = "dd-mm-yyyy";
    $(".datepicker").datepicker({
        startDate: "-3d"
    });


});