jQuery(document).ready(function () {
    //debugger;
    jQuery('#contactRequestForm')[0].reset();
});


function sendContactRequest() {
    //debugger;
    if (document.getElementById('ContactFullName').value === "") {
        bootbox.alert("من فضلك أدخل الإسم الرباعى");
        return false;
    }
    else if (document.getElementById('ContactNationalIDNo').value === "") {
        bootbox.alert("من فضلك أدخل الرقم المدنى صحيحا");
        return false;
    }
    else if (document.getElementById('ContactPhoneNo').value === "") {
        bootbox.alert("من فضلك أدخل رقم الموبايل صحيحا");
        return false;
    }
    else if (document.getElementById('ContactEmail').value === "") {
        bootbox.alert("من فضلك أدخل البريد الإلكترونى صحيحا");
        return false;
    }
    else if (document.getElementById('ContactMessageHeader').value === "") {
        bootbox.alert("من فضلك أدخل موضوع الرسالة");
        return false;
    }
    else if (document.getElementById('ContactMessageBody').value === "") {
        bootbox.alert("من فضلك أدخل محتوى الرسالة");
        return false;
    }
 
    var formData = new FormData();
    formData.append('ContactFullName', jQuery("#ContactFullName").val());
    formData.append('ContactNationalIDNo', jQuery("#ContactNationalIDNo").val());
    formData.append('ContactPhoneNo', jQuery("#ContactPhoneNo").val());
    formData.append('ContactEmail', jQuery("#ContactEmail").val());
    formData.append('ContactMessageHeader', jQuery("#ContactMessageHeader").val());
    formData.append('ContactMessageBody', jQuery("#ContactMessageBody").val());
    var dialog = bootbox.dialog({
        message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> من فضلك إنتظر قليلا </p>',
        closeButton: false
    });
    jQuery.ajax({
        url: '/Home/PostContactRequest',
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
                    title: "تم الإرسال",
                    message: "تم إرسال رسالتك. سنعاود التواصل معك فى أقرب وقت. شكرا" ,
                    callback: function () { window.location.href = '/Home/Index/'; }
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