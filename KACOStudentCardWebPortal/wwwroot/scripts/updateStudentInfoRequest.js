jQuery(document).ready(function () {
    //debugger;
    jQuery('#updatieStudentInfoRequestForm')[0].reset();

    //----Save Success or Fail---//
    //if ($("#success_id").val() == "true") {
    //    bootbox.alert("تم تسجيل طلب الانضمام للغرفة بنجاح", function () {
    //        window.location.href = '/Home/Index';
    //    });
    //}
    //else if ($("#success_id").val() == "false") {
    //    bootbox.alert("عفوا,البريد الالكترونى الذى تم ادخاله مسجل بالفعل !!")
    //}
    //else if ($("#success_id").val() == "SaveSendError") {
    //    bootbox.alert("عفوا لم نتمكن من ارسال طلبك , من فضلك تأكد من صحة البيانات المدخلة !!")
    //}
});


function sendStudentUpdateInfoRequest() {
    //debugger;
    //if (!$("#Confirmation").is(':checked')) {
    //    bootbox.alert("يجب الموافقة أولا على شروط و أحكام الإنضمام للغرفة التجارية !!");
    //    return;
    //} else if ($('#PasswordConfirm').val() != $('#Password').val()) {
    //    bootbox.alert("كلمة السر يجب أن تطابق تأكيد كلمة السر");
    //    $("#Password").val("");
    //    $("#PasswordConfirm").val("");
    //    document.RequestForm_Name.Password.focus();
    //    return;
    //} else if (CheckPassword($("#Password").val()) === false) {
    //    bootbox.alert("  يجب أن تتراوح كلمة المرور بين 6 و 15 حرفًا و تحتوي على الأقل على حرف صغير وحرف كبير ورقم وحرف مميز (@,$)");
    //    $("#Password").val("");
    //    $("#PasswordConfirm").val("");
    //    document.RequestForm_Name.Password.focus();

    //if (!jQuery("#FullName").val() == null || (!jQuery("#Email").val() == null)) {
    //    bootbox.alert("يجب تسجيل كافة البيانات !!");
    //    return;
    //}

    //init: function() {
    //    document.getElementsByName('submit').onclick = obj.validate;
    //};

    //validate: function() {
    //    var check = document.getElementsByTagName('input');
    //    var len = check.length;
    //    for (var i = 0; i < len; i++) {
    //        if (check[i].value === '') {
    //            alert('required');
    //            return false;
    //        };

    //    };

    //};


    //var formData = new FormData();
    //var check = document.getElementsByTagName('input');
    //var len = check.length;
    //for (var i = 0; i < len; i++) {
    //    if (check[i].value === '') {
    //        bootbox.alert({
    //            message: "من فضلك أدخل كافة البيانات لتسجيل الطلب. شكرا",
    //            locale: 'ar'
    //        });
    //        return false;
    //    }
    //    else {
    //        bootbox.alert("ok");
    //        return true;
    //    };
    //};

    //debugger;
    if (document.getElementById('ImageId').value === "") {
        bootbox.alert("من فضلك أدخل الصورة الشخصية");
        return false;
    }
    else if (document.getElementById('StudentInfoFullName').value === "") {
        bootbox.alert("من فضلك أدخل الإسم الرباعى");
        return false;
    }
    else if (document.getElementById('StudentInfoNationalIdNo').value === "") {
        bootbox.alert("من فضلك أدخل الرقم المدنى");
        return false;
    }
    else if (document.getElementById('StudentPassportNo').value === "") {
        bootbox.alert("من فضلك أدخل رقم جواز السفر صحيحا");
        return false;
    }
    else if (document.getElementById('StudentInfoNationality').value === "") {
        bootbox.alert("من فضلك إختر الجنسية");
        return false;
    }
    else if (document.getElementById('StudentInfoEgCellNo').value === "") {
        bootbox.alert("من فضلك رقم الموبايل المصرى صحيحا");
        return false;
    }
    else if (document.getElementById('StudentInfoKwCellNo').value === "") {
        bootbox.alert("من فضلك أدخل رقم الموبايل الكويتى صحيحا");
        return false;
    }
    else if (document.getElementById('StudentInfoUrgentEgCellNo').value === "") {
        bootbox.alert("من فضلك أدخل الرقم الخاص بالطوارئ");
        return false;
    }
    else if (document.getElementById('StudentInfoEmail').value === "") {
        bootbox.alert("من فضلك أدخل البريد الإلكترونى صحيحا");
        return false;
    }
    else if (document.getElementById('StudentInfoEgAddress').value === "") {
        bootbox.alert("من فضلك أدخل العنوان بمصر");
        return false;
    }
    else if (document.getElementById('UniversityId').value === "") {
        bootbox.alert("من فضلك أدخل الجامعة");
        return false;
    }
    else if (document.getElementById('CollegeId').value === "") {
        bootbox.alert("من فضلك أدخل الكلية");
        return false;
    }
    else if (document.getElementById('EducationLevel').value === "") {
        bootbox.alert("من فضلك إختر الفرقة");
        return false;
    }
    else if (document.getElementById('EducationCategory').value === "") {
        bootbox.alert("من فضلك إختر نوع الدراسة");
        return false;
    }
    else if (document.getElementById('termsOfUse').checked == false) {
        bootbox.alert("يجب الموافقة على الشروط");
        return false;
    }
    //    if (document.getElementById('success_id').value = "false") {
    //        bootbox.alert("عفوا,الرقم المدنى الذى تم ادخاله مسجل بالفعل !!");
    //        document.getElementById('NationalIDNo').value = '';
    //        return false;
    //}
    //return true;
    

    //else if (document.getElementById('success_id').value == true) {
    //    return true;
    //}
    var formData = new FormData();
    formData.append('StudentInfoFullName', jQuery("#StudentInfoFullName").val());
    formData.append('StudentInfoNationalIdNo', jQuery("#StudentInfoNationalIdNo").val());
    formData.append('StudentPassportNo', jQuery("#StudentPassportNo").val());
    formData.append('StudentInfoNationality', jQuery("#StudentInfoNationality :selected").val());
    formData.append('ImageId', jQuery('#ImageId').get(0).files[0]);
    formData.append('StudentInfoEgCellNo', jQuery("#StudentInfoEgCellNo").val());
    formData.append('StudentInfoKwCellNo', jQuery("#StudentInfoKwCellNo").val());
    formData.append('StudentInfoUrgentEgCellNo', jQuery("#StudentInfoUrgentEgCellNo").val());
    formData.append('StudentInfoEmail', jQuery("#StudentInfoEmail").val());
    formData.append('StudentInfoEgAddress', jQuery("#StudentInfoEgAddress").val());
    formData.append('UniversityId', jQuery("#UniversityId :selected").val());
    formData.append('CollegeId', jQuery("#CollegeId :selected").val());
    formData.append('EducationLevel', jQuery("#EducationLevel :selected").val());
    formData.append('EducationCategory', jQuery("#EducationCategory :selected").val());
    //formData.append('SeasonId', jQuery("#SeasonId").val());

    var dialog = bootbox.dialog({
        message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> جارى إرسال البيانات. من فضلك إنتظر قليلا </p>',
        closeButton: false
    });
    //var regNo = document.getElementById('RegistrationNo').value;
    jQuery.ajax({
        url: '/Student/UpdateStudentInfo',
        type: 'POST',
        enctype: 'multipart/form-data',
        contentType: false,
        processData: false,
        dataType: 'json',
        data: formData,
        //beforeSend: function () {
        //   dialog.show();
        //},
        //, function() {
        //    window.location.href = '/Student/Success/';
        //}
        success: function (data) {
            if (data.err === "") {
                dialog.hide();
                bootbox.alert({
                    title: "تم إرسال الطلب",
                    message: "تم إرسال البيانات و سيتم مراجعتها. شكرا" ,
                    callback: function () { window.location.href = '/Home/Index/'; }
                })
                //bootbox.alert("تم الإرسال. برجاء الإحتفاظ برقم القيد الخاص بك حتى يتم تسليم البطاقة فى أسرع وقت" regNo + );
            }
            else {
                bootbox.alert(data.err);
            }
        },
        error: function (xhr, status, err) {
            bootbox.alert(err);
        }
    });
    return false;
}