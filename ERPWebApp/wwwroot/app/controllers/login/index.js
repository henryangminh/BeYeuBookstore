var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }
    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                UserName: { //name cua obj
                    required: true
                },
                Password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                // use DOM
                var user = $('#txtUserName').val();
                var pass = $('#txtPassword').val();
                login(user, pass);
            }
        });
        $('form').on('keypress', function (e) {
            if ($('#frmLogin').valid()) {
                if (e.keyCode == 13) {
                    var user = $('#txtUserName').val();
                    var pass = $('#txtPassword').val();
                    login(user, pass);
                }
            }
        });
    }
    
    var login = function (user, pass) {
        $.ajax({
            type:'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dataType: 'json',
            url: '/admin/login/authen',
            beforeSend: function () {
                general.startLoad();
            },
            success: function (res) {
                if (res.Success) {
                    window.location.href = 'admin/Home/Index';
                    
                }
                else {
                    general.notify('Đăng nhập không đúng', 'error');
                 
                }
                general.stopLoad();
            }
        })
    }
}