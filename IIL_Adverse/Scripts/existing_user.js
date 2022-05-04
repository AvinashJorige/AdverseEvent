$(document).ready(function () {

    $('.endstop').hide();
    var type = localStorage.getItem("occupation");
    var language = localStorage.getItem("language");
    $('.other-relation').hide();
    $('.Doctor_Registration_Number_star').hide();

    LoadbasicDetails();

    // for loading the different language.
    if (language == 'hi') {
        for (i = 0; i < hi_in.length; i++) {
            $('.' + hi_in[i]['Key']).html(hi_in[i]['Value']);
        }
    } else {
        for (i = 0; i < en_us.length; i++) {
            $('.' + en_us[i]['Key']).html(en_us[i]['Value']);
        }
    }

    if (type == 'phy') {
        $('.Doctor_Registration_Number_star').show();
    }

    $('#vaccname, .country-list').select2();

    $("#vaccname").append('<option value="">-- Select -- </option>');
    for (var i in products) {
        $("#vaccname").append('<option value="' + products[i]["ProductCode"] + '">' + products[i]["ProductDesc"] + '</option>');
    }

    $('.country_admin').change(function () {
        var selectedCountry = $('.country_admin option:selected').val();
        var dial_code = Phone.filter(function (e) {
            if (e.code == selectedCountry) {
                return e.dial_code;
            }
        })

        if (dial_code) {
            $('.ph_admin').val(dial_code[0].dial_code);
        }
    })
    $('.country-list-2').change(function () {
        var selectedCountry = $('.country-list-2 option:selected').val();
        var dial_code = Phone.filter(function (e) {
            if (e.code == selectedCountry) {
                return e.dial_code;
            }
        })

        if (dial_code) {
            $('#phCode_other').val(dial_code[0].dial_code);
        }
    })

    $('.eCountry').change(function () {
        var selectedCountry = $('.eCountry option:selected').val();
        var dial_code = Phone.filter(function (e) {
            if (e.code == selectedCountry) {
                return e.dial_code;
            }
        })

        if (dial_code) {
            $('#ephCode_other').val(dial_code[0].dial_code);
        }
    })

    $('.logout').click(function () {
        localStorage.clear();
    });

    $('#treatmentupload').change(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#treatmentupload").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object  
            fileData.append('username', 'Mars');

            $.ajax({
                url: '/AdverseDetail/UploadTreatmentFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    alert(result);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#resultMedicalupload').change(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#resultMedicalupload").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object  
            fileData.append('username', 'Mars');

            $.ajax({
                url: '/AdverseDetail/UploadMedicalReportFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    //alert(result);
                },
                error: function (err) {
                    //alert(err.statusText);
                }
            });
        } else {
            //alert("FormData is not supported.");
        }
    });

    $('.btn-submit').click(function () {
        try {
            var OtherDetails = "";
            var radio = document.getElementsByName('optradio');
            for (i = 0; i < radio.length; i++) {
                if (radio[i].checked)
                    OtherDetails = radio[i].value;
            }

            var AdverseEventData = {};

            var PatientDetails = {};
            PatientDetails.pName = $('#pName').val();
            PatientDetails.pEmail = $('#pemail').val();
            PatientDetails.pPhone = $('#pphone').val();
            PatientDetails.pStreetAddress = $('#paddress').val();
            PatientDetails.pCity = $('#pcity').val();
            PatientDetails.pState = $('#pState').val();
            PatientDetails.pPostalCode = $('#ppostal').val();
            PatientDetails.pCountry = $('#pCountry option:selected').val();
            PatientDetails.pGender = $('#pGender option:selected').val();
            PatientDetails.pAge = $('#page').val();
            PatientDetails.pDOB = $('#pdob').val();

            var Vaccine = {};
            Vaccine.DateTimeVaccine = $("#datetime_vacc").val();
            Vaccine.VaccineName = $("#vaccname option:selected").val();
            Vaccine.Manufacturer = $("#manufac").val();
            Vaccine.BatchNo = $("#batchno").val();
            Vaccine.ManufactureDateMonth = $("#manufac_datemonth").val();
            Vaccine.ExpireDateMonth = $("#expiry_datemonth").val();

            var Vaccination = {};
            Vaccination.Route = $("#route option:selected").val();
            Vaccination.DoseNumber = $("#dose_number").val();
            Vaccination.SiteAdmin = $("#site_admin").val();

            var Facility = {};
            Facility.FacilityName = $("#ffacility").val();
            Facility.FStreetAddress = $("#fstrAddress").val();
            Facility.FState = $("#fstate").val();
            Facility.FEmail = $("#femail").val();
            Facility.FCity = $("#fcity").val();
            Facility.FCountry = $("#fCountry option:selected").val();
            Facility.FPhoneCode = $("#fphCode").val();
            Facility.FPhone = $("#fphone").val();
            Facility.FFax = $("#ffax").val();

            if (window.FormData !== undefined) {
                var fileUpload1 = $("#treatmentupload").get(0);
                var files1 = fileUpload1.files;
                var fileData1 = new FormData();
                for (var i = 0; i < files1.length; i++) {
                    fileData1.append(files1[i].name, files1[i]);
                }
                fileData1.append('username', 'Faisal');

                var fileUpload2 = $("#resultMedicalupload").get(0);
                var files2 = fileUpload2.files;
                var fileData2 = new FormData();
                for (var i = 0; i < files2.length; i++) {
                    fileData2.append(files2[i].name, files2[i]);
                }
                fileData2.append('username', 'Faisal');
            }

            var AdverseEvent = {};
            AdverseEvent.Event = $("#esymptom").val();
            AdverseEvent.EventContinue = $('input[name="eoptradio"]:checked').val();
            AdverseEvent.EStopDate = $("#estopdate").val();
            AdverseEvent.EDCT = $('input[name="eoptradioDCT"]:checked').val();
            AdverseEvent.EStopDate = $("#estopdate").val();
            AdverseEvent.ETreatment = $("#etreatment").val();
            AdverseEvent.EResultMedical = $("#eresultMedical").val();
            AdverseEvent.EStartDate = $("#estartdate").val();
            AdverseEvent.EClinic = $("#eclinic").val();
            AdverseEvent.EDocName = $("#edocName").val();
            AdverseEvent.ECity = $("#ecity").val();
            AdverseEvent.EState = $("#estate").val();
            AdverseEvent.ECountry = $("#eCountry option:selected").val();
            AdverseEvent.EPhoneCodeOther = $("#ephCode_other").val();
            AdverseEvent.EPhone = $("#ephone").val();
            AdverseEvent.EEmail = $("#eemails").val();
            AdverseEvent.EStreetAddress = $("#estrAddress").val();
            AdverseEvent.treatmentupload = ""; //fileData1;
            AdverseEvent.resultMedicalupload = ""; //fileData2;

            var History = {};
            History.PregnantVaccination = $("#pregnantVacc option:selected").val();
            History.Medications = $("#medications").val();
            History.Allergies = $("#Allergies").val();
            History.Illnesses = $("#illnesses").val();
            History.Chronic = $("#Chronic").val();

            AdverseEventData.PatientDetail = PatientDetails;
            AdverseEventData.Vaccine = (Vaccine);
            AdverseEventData.Vaccination = (Vaccination);
            AdverseEventData.Facility = (Facility);
            AdverseEventData.AdverseEvent = (AdverseEvent);
            AdverseEventData.History = (History);
            AdverseEventData.OtherDetails = (OtherDetails);

            $.ajax({
                type: "POST",
                url: '/AdverseDetail/AddCustomerDetail',
                data: JSON.stringify(AdverseEventData),
                async: false,
                dataType: "json",
                contentType: "application/json",
                success: function (res) {
                    if (res && res.Status == 200) {
                        swal("Success!", res.Message, "success");
                    }
                    else if (res && res.Status == 300) {
                        swal("Error!", "Error while saving details. Please retry again.", "error");
                        console.log(res.Message);
                    }
                    else {
                        swal("Error!", "Error while saving details. Please retry again.", "error");
                        console.log(res.Message);
                    }

                    //$(".ResetForm").click();
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
                    console.log(msg);
                }
            });


        } catch (e) {
            console.log(e);
        }
    });

    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    }
});

function LoadbasicDetails() {
    try {        
        $.ajax({
            type: "POST",
            url: '/AdverseDetail/GetCustomerInfo',
            data: null,
            async: false,
            dataType: "json",
            contentType: "application/json",
            success: function (res) {
                if (res && res.Status == 200) {
                    FillDetails(JSON.parse(res.Message)[0]);
                }
                else {                    
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

    }
}

function FillDetails(obj) {
    $('#pName').val(obj.name);
    // $('.occupval').val(occupation[0].val);
    $('#pphone').val(obj.Phone.split(' ')[1]);
    $('#paddress').val(obj.stAddress);
    $('#pState').val(obj.state);
    $('#ppostal').val(obj.postalCode);
    // $('#pGender').attr("value", gender[0].val);
    $('select[name^="pGender"] option[value=' + obj.gender + ']').attr("selected", "selected");
    // $('#pGender').val(gender[0].val);
    $('#pemail').val(obj.email);
    $('#pcity').val(obj.city);
    $('#page').val(obj.age);
    $('#pdob').val(dateformat(new Date(obj.dob)));
    $("#pCountry").val(obj.country.split('@')[1]);
    $("#fCountry").val(obj.country.split('@')[1]);


    $('.nameval').html(obj.name);
    $('.occupval').html(obj.occupation);
    $('.phoneval').html(obj.Phone);
    $('.addressval').html(obj.stAddress);
    $('.stateval').html(obj.state);
    $('.postalval').html(obj.postalCode);
    $('.genderval').html(obj.gender);
    $('.ageval').html(obj.age + ' Years / ' + dateformat(obj.dob));
    $('.emailval').html(obj.email);
    $('.cityval').html(obj.city);
    $('.countryval').html(obj.country.split('@')[0]);
    localStorage.removeItem("basicinfo");
    localStorage.setItem("basicinfo", JSON.stringify(obj));
}

function dateformat(dob) {
    var today = new Date(dob);
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = dd + '-' + mm + '-' + yyyy;
    return today;
}

function checkRadio(name) {
    if (name == 'yes') {
        $('.endstop').hide();
    } else {
        $('.endstop').show();
    }
}

function checkRelation(name) {
    if (name == 'other') {
        $('.other-relation').show();
        $('#pName').val("");
        $('#pphone').val("");
        $('#paddress').val("");
        $('#pState').val("");
        $('#ppostal').val("");
        // $('#pGender').attr("value", gender[0].val);
        //$('select[name^="pGender"] option[value=' + gender[0].val + ']').attr("selected", "selected");
        // $('#pGender').val(gender[0].val);
        $('#pemail').val("");
        $('#pcity').val("");
        $('#page').val("");
        $('#pdob').val("");
    }
    else if (name == 'self') {
        var gender = [
            { key: 'male', val: 'Male' },
            { key: 'female', val: 'Female' }
        ]
        var obj = localStorage.getItem('basicinfo');
        obj = JSON.parse(obj);

        $('#pName').val(obj.name);
        // $('.occupval').val(occupation[0].val);
        $('#pphone').val(obj.Phone);
        $('#paddress').val(obj.stAddress);
        $('#pState').val(obj.state);
        $('#ppostal').val(obj.postalCode);
        // $('#pGender').attr("value", gender[0].val);
        $('select[name^="pGender"] option[value=' + obj.gender + ']').attr("selected", "selected");
        // $('#pGender').val(gender[0].val);
        $('#pemail').val(obj.email);
        $('#pcity').val(obj.city);
        $('#page').val(obj.age);
        $('#pdob').val(obj.dob);

        $('.other-relation').hide();

    }
    else if (name == '') { }
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