jQuery(document).ready(function () {
    //debugger;
    jQuery('#feePaymentRequestForm')[0].reset();
});


function sendFeePaymentRequest() {
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
        bootbox.alert("من فضلك إختر المرحلة الدراسية");
        return false;
    }
    else if (document.getElementById('IsHavingSiblingsSameCollege').value === "") {
        bootbox.alert("من فضلك إختر إذ كان لديك أشقاء بنفس الكلية أم لا");
        return false;
    }
    else if (document.getElementById('termsOfUse').checked == false) {
        bootbox.alert("يجب الموافقة على شروط الإرسال");
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
    formData.append('CollegeId', jQuery("#CollegeId").val());
    formData.append('UniversityId', jQuery("#UniversityId").val());
    formData.append('EducationLevel', jQuery("#EducationLevel").val());
    formData.append('IsHavingSiblingsSameCollege', jQuery("#IsHavingSiblingsSameCollege").val());
    var dialog = bootbox.dialog({
        message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> من فضلك إنتظر قليلا </p>',
        closeButton: false
    });
    jQuery.ajax({
        url: '/FeePayment/PostFeePaymentRequest',
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
                    title: "تم إرسال طلب الطلب.",
                    message: "تم إستلام الطلب و سيتم التواصل معكم فى حالة وجود مشكلة ببيانات الطلب. شكرا" ,
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