/// <reference path="../../../lib/jquery-validation/dist/jquery.validate.js" />
/// <reference path="../../../lib/jquery/dist/jquery.js" />

var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };
    var registerEvents = function () {
        $("#frmLogin").validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($("#frmLogin").valid()) {
                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
        });
        $('body').keypress(function (e) {
            e.preventDefault();
            if (e.which === 13) {
                $('#btnLogin').click();
            }
        })
    };

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dataType: 'json',
            url: '/admin/login/authen',
            success: function (result) {
                if (result.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    congdoan.notify('Tài khoản hoặc mật khẩu không chính xác!', 'error');
                }
            }
        });
    };
};