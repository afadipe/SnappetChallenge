$(document).ready(function () {
    
    var ErrMsg = $('#hndErrMsg').val();
    if (ErrMsg !== "" && ErrMsg != undefined) {
        toastr.error(ErrMsg,{ timeOut: 5e3 });
    }

    var Msg = $('#hndMsg').val();
    if (Msg !== "" && Msg != undefined) {
        toastr.success(Msg, { showMethod: "fadeIn", hideMethod: "fadeOut", timeOut: 2e3 })
    }


   // $("#slide-toast").on("click", function () {
   //     toastr.success("I do not think that word means what you think it means.", "Slide Down / Slide Up!", { showMethod: "slideDown", hideMethod: "slideUp", timeOut: 2e3 })
   // }),
    //$("#fade-toast").on("click", function () {
    //    toastr.success("I do not think that word means what you think it means.", { showMethod: "fadeIn", hideMethod: "fadeOut", timeOut: 2e3 })
    //})
});