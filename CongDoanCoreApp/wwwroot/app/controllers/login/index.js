/// <reference path="../../../lib/jquery/dist/jquery.js" />

var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };
    var registerEvents = function () {
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            var user = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            login(user, password);
        });
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