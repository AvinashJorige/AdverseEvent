$(document).ready(function () {
    $(".btn-login").click(function () {
        try {
            if (!$("#User").val()) {
                alert("Please enter User Id");
            }

            if (!$("#pwd").val()) {
                alert("Please enter password");
            }

            if ($("#pwd").val().length >= 30) {
                alert("Password cannot be more then 30 characters");
            }
            var entity = {
                UserCode: $("#User").val(),
                Password: $("#pwd").val()
            }

            $.ajax({
                type: "POST",
                url: '/Account/Login',
                data: JSON.stringify(entity),
                async: false,
                dataType: "json",
                contentType: "application/json",
                success: function (res) {
                    if (res && res.Status == 100) {
                        alert(res.Message);
                    }
                    else {
                        window.location.href = "/AdverseDetail/Index";
                    }
                },
                error: function (jqXHR, exception) {
                    var msg = '';
                    if (jqXHR.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (jqXHR.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (jqXHR.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else if (exception === 'parsererror') {
                        msg = 'Requested JSON parse failed.';
                    } else if (exception === 'timeout') {
                        msg = 'Time out error.';
                    } else if (exception === 'abort') {
                        msg = 'Ajax request aborted.';
                    } else {
                        msg = 'Uncaught Error.\n' + jqXHR.responseText;
                    }
                    alert(msg);
                }
            }); 

        } catch (e) {
            console.log(e);
        }
    })
});