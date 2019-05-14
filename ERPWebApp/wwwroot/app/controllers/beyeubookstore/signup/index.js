var signUpController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

function registerEvents() {
    $('body').on('click', '#btnSignUp', function () {
        if (validateSignUp()) {
            $.ajax({
                type: 'POST',
                url: '/BeyeuBookstore/Register',
                data: {
                    Email: $('#txtEmailSignUp').val(),
                    Password: $('#txtPasswordSignUp').val(),
                    ConfirmPassword: $('#txtRetypePasswordSignUp').val(),
                    FullName: $('#txtNameSignUp').val(),
                    Address: $('#txtAddressSignUp').val(),
                    PhoneNumber: $('#txtPhoneNumberSignUp').val(),
                },
                success: function () {
                    window.location.href = "/BeyeuBookstore/WaitingConfirmation";
                },
                error: function () {
                }
            })
        }
    })
}

function validateSignUp() {
    var validated = true;
    var emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9-]+.+.[A-Z]{2,4}$/igm;
    var phoneRegex = /^0+[0-9]{9}$/;

    $('#txtEmailMessage').text("");
    $('#txtNameMessage').text("");
    $('#txtPasswordMessage').text("");
    $('#txtRetypePasswordMessage').text("");
    $('#txtAddressMessage').text("");
    $('#txtPhoneNoMessage').text("");

    if ($('#txtEmailSignUp').val() == "" || $('#txtEmailSignUp').val() == null) {
        $('#txtEmailMessage').text("Không được để trống");
        validated = false;
    }
    else if ($('#txtEmailSignUp').val().match(emailRegex) == null) {
        $('#txtEmailMessage').text("Email sai");
        validated = false;
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/BeyeuBookstore/CheckEmailExist',
            data: { email: decodeURI($('#txtEmailSignUp').val()) },
            beforeSend: function () {
                general.startLoading();
            },
            success: function (respond) {
                if (!respond) {
                    $('#txtEmailMessage').text("Tài khoản đã có");
                    validated = false;
                }
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
    if ($('#txtNameSignUp').val() == "" || $('#txtNameSignUp').val() == null) {
        $('#txtNameMessage').text("Không được để trống");
        validated = false;
    }
    if ($('#txtPasswordSignUp').val() == "" || $('#txtPasswordSignUp').val() == null) {
        $('#txtPasswordMessage').text("Không được để trống");
        validated = false;
    }
    else if ($('#txtPasswordSignUp').val().length < 7) {
        $('#txtPasswordMessage').text("Mật khẩu phải có ít nhất 7 ký tự");
        validated = false;
    }
    if ($('#txtRetypePasswordSignUp').val() == "" || $('#txtRetypePasswordSignUp').val() == null) {
        $('#txtRetypePasswordMessage').text("Không được để trống");
        validated = false;
    }
    if ($('#txtAddressSignUp').val() == "" || $('#txtAddressSignUp').val() == null) {
        $('#txtAddressMessage').text("Không được để trống");
        validated = false;
    }
    if ($('#txtPhoneNumberSignUp').val() == "" || $('#txtPhoneNumberSignUp').val() == null) {
        $('#txtPhoneNoMessage').text("Không được để trống");
        validated = false;
    }
    else if ($('#txtPhoneNumberSignUp').val().match(phoneRegex) == null) {
        $('#txtPhoneNoMessage').text("Số điện thoại sai");
        validated = false;
    }
    if ($('#txtPasswordSignUp').val() != $('#txtRetypePasswordSignUp').val()) {
        $('#txtRetypePasswordMessage').text("Nhập lại mật khẩu không đúng với mật khẩu");
        validated = false;
    }
    return validated;
}