$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {
        //e.preventDefault();
        var urlPath = this.href;
        var CntrlType = $("#MyModal").attr('type');

        $("#MyModalContent").load(urlPath, function () {

            $("#MyModal").modal({
                //backdrop: 'static',
                keyboard: false
            }, 'show');
            bindForm2(this);
        });
        return false;
    });
});

function bindForm2(dialog) {

    $('form', dialog).submit(function () {
        //$.validator.unobtrusive.parse($('form'));
        alert(10);
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $("#MyModal").modal('hide');
                    toastr.success(result.message);
                    window.location.reload(forceGet = true);
                }
                else {
                    $("#MyModal").modal('show');
                    //$("#MyModalContent").html(result);
                    toastr.error(result.ErrorMessage);
                    bindForm2(dialog);
                }
            }
            ,
            error: function (xml, message, text) {
                toastr.error("Msg: " + message + ", Text: " + text);
            }
        });
        return false;
    });
}