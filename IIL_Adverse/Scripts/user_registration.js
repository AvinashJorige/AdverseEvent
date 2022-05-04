jQuery.validator.setDefaults({
    debug: true,
    success: "valid"
});

$(document).ready(function () {
  
    $('.Year').hide();
    $('.country-list').select2();
    $('.doc_reg_no_form').hide();
    $('#doc_reg_no').removeAttr('required');

    $('.reset').click(function () {
        $('.country-list').val('').trigger('change');
        $(".age").text(0);
        $('.Year').hide();
    });

    $('#Policy').change(function () {
        if ($(this).is(":checked")) {
            $('.Register').removeAttr("disabled");
        }
        else {
            $('.Register').attr("disabled","disabled");
        }
    });
    
    $('.Register').click(function () {
        if(!$('#Policy').is(":checked")) {
            return;
        }

        var Form = $("#CustomerRegistrationForm");

        var obj = {
            country     : $('.country-list option:selected').val(),
            name        : $('#name').val(),
            gender      : $('.gender option:selected').val(),
            occupation  : $('.occupation option:selected').val(),
            dob         : $('#dob').val(),
            age         : $('.age').html(),
            Phone       : $('#phone').val(),
            email       : $('#email').val(),
            stAddress   : $('#stAddress').val(),
            city        : $('#city').val(),
            state       : $('#state').val(),
            postalCode  : $('#postalcode').val(),
            docRegNo    : $('#doc_reg_no').val(),
            language    : $('.language').val()
        }

        var isFilled = true;
        if (!$('.country-list option:selected').val() == "none") {
            alert("Please select your Country.");
        }
        else if (!$('#name').val()) {
            alert("please enter your Name."); isFilled = false;
        } else if (!$('.gender option:selected').val()) {
            alert("please select Gender."); isFilled = false;
        } else if (!$('#dob').val()) {
            alert("Please select your Date of Birth"); isFilled = false;
        } else if (!$('#phone').val()) {
            alert("Please enter your Phone Number"); isFilled = false;
        } else if ($('#phone').val().length > 10) {
            alert("Please enter valid Phone Number"); isFilled = false;
        } else if (!$('#email').val()) {
            alert("Please enter Email Address"); isFilled = false;
        } else if (!ValidateEmail($('#email').val())) {
            alert("Please enter valid Email Id"); isFilled = false;
        } else if (!$('#stAddress').val()) {
            alert("Please enter Street Address"); isFilled = false;
        } else if (!$('#city').val()) {
            alert("Please enter City"); isFilled = false;
        } else if (!$('#state').val()) {
            alert("Please enter State"); isFilled = false;
        } else if (!$('#postalcode').val()) {
            alert("Please enter Postal Code."); isFilled = false;
        } else if (!$('#doc_reg_no').attr('required')) {
            if ($('#doc_reg_no').val()) {
                alert("Please enter Doctor Registration Number."); isFilled = false;
            }
        } else if (!$('.language').val()) {
            alert("Please select Language"); isFilled = false;
        } else if ($(".error-mail-msg").html() != "") {
            isFilled = false;
        }

        if (isFilled) {
            //localStorage.setItem('basicinfo', JSON.stringify(obj));
            saveData(obj);
            $('.registiondone').click();
            $('.reset').click();
            $(".age").text(0);
            $('.Year').hide();
        }
    });

    $('.occupation').change(function () {
        var type = $('.occupation option:selected').val();
        if (type != "phy") {
            $('.doc_reg_no_form').hide();
            $('#doc_reg_no').removeAttr('required');
        }
        else {
            $('.doc_reg_no_form').show();
            $('#doc_reg_no').attr('required');
        }
        //localStorage.setItem('occupation', type);
    });

    $('.language').change(function () {
        if ($('.language option:selected').val() == 'hi') {
            localStorage.setItem('language', 'hi');
            for (i = 0; i < hi_in.length; i++) {
                $('.' + hi_in[i]['Key']).html(hi_in[i]['Value']);
            }
        } else {
            localStorage.setItem('language', 'en');
            for (i = 0; i < en_us.length; i++) {
                $('.' + en_us[i]['Key']).html(en_us[i]['Value']);
            }
        }
    });
});

function Emailverification(input) {
    try {

        if (!ValidateEmail(input)) {
            $(".error-mail-msg").html("Invalid Email address. Please enter valid Email Address");
        }
        else {
            $(".error-mail-msg").html("");
        }

        $.ajax({
            type: "POST",
            url: '/CustomerRegistration/VerifyEmail',
            data: "{Email: '" + input + "'}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                debugger;
                if (res && res.Status == 200) {
                    $(".error-mail-msg").html(res.Message);
                }
                else {
                    $(".error-mail-msg").html("");
                }
            },
            error: function (e, f) {
                alert("Error while inserting data");
            }
        });
    } catch (e) {

    }
}

function ValidateEmail(inputText) {
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (inputText.match(mailformat)) {
        return true;
    }
    else {
        return false;
    }
}

var today = new Date()
//Calculates age from given Birth Date in the form//
function _calcAge(birthDate, givenDate) {
    givenDate = new Date(today);
    var dt1 = document.getElementById('dob').value;
    var birthDate = new Date(dt1);
    var years = (givenDate.getFullYear() - birthDate.getFullYear());

    if (givenDate.getMonth() < birthDate.getMonth() ||
        givenDate.getMonth() == birthDate.getMonth() && givenDate.getDate() < birthDate.getDate()) {
        years--;
    }

    return years;
}

function _setAge() {
    var age = _calcAge();
    $('.Year').show();
    $(".age").text(age);
}

function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function saveData(obj) {
    try {      
        $.ajax({
            type: "POST",
            url: '/CustomerRegistration/SaveInfo',
            data: '{entity: ' + JSON.stringify(obj) + '}',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                debugger;
                if (res && res.Status == 200) {
                    swal({
                        title: "Customer Registration",
                        text: res.Message,
                        icon: "success",
                    });
                }
                else {
                    swal({
                        title: "Customer Registration",
                        text: "Error in saving details. Please try again.",
                        icon: "error",
                    });
                }                
            },
            error: function (e,f) {
                alert("Error while inserting data");
            }
        });  
    } catch (e) {
        console.log(e);
    }
}