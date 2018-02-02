$(function () {
    $("a[data-modal]").on("click", function () {
   

        $("#bootsrapModalContent").load(this.href, function () {
            $("#bootsrapModal").modal({ keyboard: true }, "show");

         

            $("#actionForm").submit(function () {
              
                if ($("#actionForm").valid()) {

                   
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),

                        success: function (result) {
                           
                            if (result.success) {
                              
                                $("#bootsrapModal").modal("hide");
                                location.reload();
                            }
                            else {
                                $("#messageToClient").html(result.message);
                            }

                        },

                        error: function () {
                            $("#messageToClient").text("The web server had an error");
                        }



                    });

                    return false;

                }

            })

        });

        return false;
    });

});