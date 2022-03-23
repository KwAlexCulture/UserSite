jQuery(document).ready(function () {
    //debugger;
    jQuery('#ticketExchangeRequestForm')[0].reset();

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


function sendTicketExchangeRequest() {
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
    if (document.getElementById('FullName').value === "") {
        bootbox.alert("من فضلك أدخل الإسم الرباعى");
        return false;
    }

    else if (document.getElementById('NationalIDNo').value === "") {
        bootbox.alert("من فضلك أدخل الرقم المدنى صحيحا");
        return false;
    }
    else if (document.getElementById('CollegeId').value === "") {
        bootbox.alert("من فضلك إختر الكلية");
        return false;
    }
    else if (document.getElementById('UniversityId').value === "") {
        bootbox.alert("من فضلك إختر الجامعة");
        return false;
    }

    else if (document.getElementById('EducationLevel').value === "") {
        bootbox.alert("من فضلك أدخل المرحلة الدراسية");
        return false;
    }

    else if (document.getElementById('ImageId').value === "") {
        bootbox.alert("من فضلك أرفق كشف الدرجات");
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
    formData.append('FullName', jQuery("#FullName").val());
    formData.append('NationalIDNo', jQuery("#NationalIDNo").val());
    formData.append('CollegeId', jQuery("#CollegeId :selected").val());
    formData.append('UniversityId', jQuery("#UniversityId :selected").val());
    formData.append('EducationLevel', jQuery("#EducationLevel :selected").val());
    formData.append('ImageId', jQuery('#ImageId').get(0).files[0]);
    var dialog = bootbox.dialog({
        message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> من فضلك إنتظر قليلا </p>',
        closeButton: false
    });
    jQuery.ajax({
        url: '/Service/PostTicketExchange',
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
                    message: "سيتم مراجعة البيانات المرسلة و إستكمال الإجراءات فى أقرب وقت. شكرا" ,
                    callback: function () { window.location.href = '/home/index/'; }
                })
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