var signInController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

function registerEvents() {
    $('body').on('click', '#btnRegister', function () {
        var email = $('#txtEmailSignUp').val();
        var password = $('#txtPasswordSignUp').val();
        var rememberme = $('rememberme').is(':checked');
        var message = "";

        $.ajax({
            type: 'POST',
            url: '/BeyeuBookstore/Login',
            data: {
                Email: email,
                Password: password,
                RememberMe: rememberme,
            },
            success: function (response) {
                switch (response) {
                    case "locked": message += "Tài khoản của bạn đã bị khóa";
                        break;
                    case "invalid": message += "Thông tin đăng nhập không đúng";
                        break;
                    case "failed": message += "Đăng nhập thất bại, kiểm tra lại email và mật khẩu";
                        break;
                    case "permission": message += "Bạn không có quyền đăng nhập trên trang này";
                        break;
                    case "emailconfirm": message += "Bạn chưa xác nhận email";
                        break;
                    default: window.location.href = response;
                }

                $('#rsSignIn').text(message);
            },
            error: function (e) {
                console.log(e);
            }
        })
    })
}