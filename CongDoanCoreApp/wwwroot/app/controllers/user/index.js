var userController = function () {
    this.initilize = function () {
        loadData();
        registerEvents();
    };

    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: {
                    required: true
                },
                txtFullName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 7
                }, txtRePassword: {
                    equalTo: "#txtPassword"
                }, txtEmail: {
                    required: true,
                    email: true
                }
            }
        });
        $('body').on('click', '.btn-edit', function (e) {
            
            e.preventDefault();
            var id = this.id;
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: '/admin/user/GetByIdAsync',
                data: {
                    id: id
                },
                success: function (result) {
                    var data = result;
                    $('#hidIdM').val(data.Id);
                    $('#txtFullName').val(data.FullName);
                    $('#txtNameM').val(data.UserName);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);

                    initRoleList(data.Roles);

                    disableFieldEdit(true);
                    $('#modalAddEdit').modal('show');
                },
                error: function (error) {
                    congdoan.notify('Loading failed', 'error');
                }

            })
        })
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = this.id;
            congdoan.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    data: {
                        id: id
                    },
                    dataType: 'json',
                    url:"/admin/user/DeleteAsync",
                    success: function (result) {
                        congdoan.notify('Deleted success', 'success');
                        loadData();
                    },
                    error: function (error) {
                        congdoan.notify('Has an error in deleting progress', 'error');
                    }
                })
            })
        })
        $('body').on('click', '#btnSave', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var fullname = $('#txtFullName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true) {
                        roles.push($(item).prop('value'));
                    }
                });
                $.ajax({
                    type: 'POST',
                    data: {
                        Id: id,
                        FullName: fullname,
                        UserName: name,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: 'json',
                    url: '/admin/user/SaveEntityAsync',
                    success: function (result) {
                        congdoan.notify('Save user succesful', 'success');
                        $('#modalAddEdit').modal('hide');
                        resetFormMaintainance();
                        loadData(true);
                    }, error: function (error) {
                        congdoan.notify('Has an error', 'error');
                        console.log(error);
                    }
                });
            }
        });
        $('#btnCreate').on('click', function () {
            resetFormMaintainance();
            initRoleList();
            $('#modalAddEdit').modal('show');
        });
        $('#btnSearch').on('click', function (e) {
            e.preventDefault();
            loadData();
        });
        $('body').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
    }
    function disableFieldEdit(disabled) {
        $('#txtNameM').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtRePassword').prop('disabled', disabled);
    }
    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidIdM').val('');
        initRoleList();
        $('#txtNameM').val('');
        $('#txtFullName').val('');
        $('#txtPassword').val('');
        $('#txtRePassword').val('');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);
        $('input[name="ckRoles"]').removeAttr('checked');
    }
    function initRoleList(selectedRoles) {
        $.ajax({
            type: 'GET',
            url: '/admin/role/getall',
            dataType: 'json',

            success: function (result) {
                var template = $('#role-template').html();
                var render = '';
                $.each(result, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1) {
                        checked = 'checked';
                    }
                    render += Mustache.render(template, {
                        Name: item.Name,
                        Checked: checked,
                        Description: item.Description
                    });
                });
                $('#list-roles').html(render);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/User/GetAllPaging",
            data: {
                keyword: $('#txtSearchKeyword').val(),
                page: congdoan.configs.pageIndex,
                pageSize: congdoan.configs.pageSize
            },
            dataType: 'json',
            success: function (result) {
                var template = $('#table-template').html();
                var render = '';
                if (result.RowCount > 0) {
                    $.each(result.Results, function (i, item) {
                        render += Mustache.render(template, {
                            FullName: item.FullName,
                            Id: item.Id,
                            UserName: item.UserName,
                            Avatar: item.Avatar === null ? '<img src="/admin-side/images/user.png" width =25 />' : '<img src="' + item.Avatar + '" width=25 /> ',
                            DateCreated: item.DateCreated,
                            Status: congdoan.getStatus(item.Status)
                        });
                    });
                    $('#lblTotalRecords').text(result.RowCount);
                    if (render !== undefined) {
                        $('#tblContent').html(render);
                    }
                    wrapPaging(result.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                } else {
                    $('#tblContent').html('');
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function wrapPaging(recordCount, callback, changePageSize) {
        var totalSize = Math.ceil(recordCount / congdoan.configs.pageSize);
        //unbind pagiantion if it existed or click change pageSize
        if ($('#paginationUL').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //bind pagination event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                congdoan.configs.pageIndex = p;
                setTimeout(callback(), 200);
            }
        });
    }
};