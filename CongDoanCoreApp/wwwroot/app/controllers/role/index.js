var roleController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    };
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtName: { required: true },
                txtDescription: { required: true }
            }
        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var id = this.id;
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: '/admin/role/GetByIdAsync',
                data: {
                    id: id
                },
                success: function (result) {
                    $('#hidIdM').val(result.Id);
                    $('#txtName').val(result.Name);
                    $('#txtDescription').val(result.Description);

                    $('#modalAddEdit').modal('show');
                }, error: function (error) {
                    congdoan.notify('Loading failded ', 'error');
                }
            })
        });
        $('#btnCreate').on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            $('#modalAddEdit').modal('show');
        });
        $('body').on('click', '#btnSave', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtName').val();
                var description = $('#txtDescription').val();
                $.ajax({
                    url: '/admin/role/SaveEntityAsync',
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: id,
                        Name: name,
                        Description: description
                    },
                    success: function (result) {
                        congdoan.notify('Save role succesful', 'success');
                        $('#modalAddEdit').modal('hide');
                        loadData();
                    },
                    error: function (error) {
                        congdoan.notify('Has an error', 'error');
                    }
                });
            }
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = this.id;
            congdoan.confirm('Are you sure to delete', function () {
                $.ajax({
                    type: 'POST',
                    data: {
                        id: id
                    },
                    dataType: 'json',
                    url: '/admin/role/DeleteAsync',
                    success: function (result) {
                        congdoan.notify('Deleted success', 'success');
                        loadData();
                    }, error: function (error) {
                        congdoan.notify('Has an error in deleting progress', 'error');
                    }
                });
            });
        });
        $('#txtSearchKeyword').on('click', function () {
            loadData();
        });
        $('body').keypress(function (e) {
            if (e.which === 13) {
                $('#txtSearchKeyword').click();
            }
        });
        $('body').on('click', '.btn-grant', function (e) {
            e.preventDefault();
            $('#hidRoleId').val(this.id);
            $.when(loadFunctionList()).done(fillPermission(this.id));
            //loadFunctionList();
            //fillPermission(this.id);
            $('#modal-grantpermission').modal('show');
        });
        $('#btnSavePermission').off('click').on('click', function (e) {
            e.preventDefault();
            var listPermission = [];
            $.each($('#tblFunction tbody tr'), function (i, item) {
                listPermission.push({
                    RoleId: $('#hidRoleId').val(),
                    FunctionId: $(item).data('id'),
                    CanRead: $(item).find('.ckView').first().prop('checked'),
                    CanCreate: $(item).find('.ckAdd').first().prop('checked'),
                    CanUpdate: $(item).find('.ckEdit').first().prop('checked'),
                    CanDelete: $(item).find('.ckDelete').first().prop('checked')
                });
            });
            $.ajax({
                url: '/admin/role/SavePermission',
                type: 'POST',
                dataType: 'json',
                data: {
                    permissionViewModels: listPermission,
                    roleId: $('#hidRoleId').val()
                },
                success: function (result) {
                    congdoan.notify('Save permission successful', 'success');
                    $('#modal-grantpermission').modal('hide');
                }, error: function (error) {
                    congdoan.notify('Has an error in save permission progress', 'error');
                }
            })
        })
    }

    function loadFunctionList() {
        $.ajax({
            url: '/admin/function/getall',
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                var template = $('#resultDataFunction').html();
                var render = '';
                $.each(result, function (i, item) {
                    render += Mustache.render(template, {
                        Name: item.Name,
                        Id: item.Id,
                        treegridparent: item.ParentId !== null ? "treegrid-parent-" + item.ParentId : ""
                    });
                });

                $('#lstDataFunction').html(render);
                $('#tblFunction').treegrid();

                $('#ckCheckAllView').on('click', function () {
                    $('.ckView').prop('checked', this.checked);
                });
                $('#ckCheckAllCreate').on('click', function () {
                    $('.ckAdd').prop('checked', this.checked);
                });
                $('#ckCheckAllEdit').on('click', function () {
                    $('.ckEdit').prop('checked', this.checked);
                });
                $('#ckCheckAllDelete').on('click', function () {
                    $('.ckDelete').prop('checked', this.checked);
                });

                $('.ckView').on('click', function () {
                    if (this.checked && $('.ckView:checked').length === result.length) {
                        $('#ckCheckAllView').prop('checked', true);
                    } else {
                        $('#ckCheckAllView').prop('checked', false);
                    }
                });

                $('.ckAdd').on('click', function () {
                    if (this.checked && $('.ckAdd:checked').length === result.length) {
                        $('#ckCheckAllCreate').prop('checked', true);
                    } else {
                        $('#ckCheckAllCreate').prop('checked', false);
                    }
                });

                $('.ckEdit').on('click', function () {
                    if (this.checked && $('.ckEdit:checked').length === result.length) {
                        $('#ckCheckAllEdit').prop('checked', true);
                    } else {
                        $('#ckCheckAllEdit').prop('checked', false);
                    }
                });

                $('.ckDelete').on('click', function () {
                    if (this.checked && $('.ckDelete:checked').length === result.length) {
                        $('#ckCheckAllDelete').prop('checked', true);
                    } else {
                        $('#ckCheckAllDelete').prop('checked', false);
                    }
                });
            }, error: function (error) {
                congdoan.notify('Has an error to load function list', 'error');
                console.log(error);
            }
        });
    }
    function fillPermission(roleId) {
        $.ajax({
            url: '/admin/role/listallfunction',
            type: 'POST',
            dataType: 'json',
            data: {
                roleId: roleId
            },
            success: function (result) {
                $.each($('#tblFunction tbody tr'), function (i, item) {
                    $.each(result, function (j, jitem) {
                        if (jitem.FunctionId === $(item).data('id')) {
                            $(item).find('.ckView').first().prop('checked', jitem.CanRead);
                            $(item).find('.ckAdd').first().prop('checked', jitem.CanCreate);
                            $(item).find('.ckEdit').first().prop('checked', jitem.CanUpdate);
                            $(item).find('.ckDelete').first().prop('checked', jitem.CanDelete);
                        }
                    });
                });

                if ($('.ckView:checked').length === $('#tblFunction tbody tr .ckView').length) {
                    $('#ckCheckAllView').prop('checked', true);
                } else {
                    $('#ckCheckAllView').prop('checked', false);
                }
                if ($('.ckAdd:checked').length === $('#tblFunction tbody tr .ckAdd').length) {
                    $('#ckCheckAllCreate').prop('checked', true);
                } else {
                    $('#ckCheckAllCreate').prop('checked', false);
                }
                if ($('.ckEdit:checked').length === $('#tblFunction tbody tr .ckEdit').length) {
                    $('#ckCheckAllEdit').prop('checked', true);
                } else {
                    $('#ckCheckAllEdit').prop('checked', false);
                }
                if ($('.ckDelete:checked').length === $('#tblFunction tbody tr .ckDelete').length) {
                    $('#ckCheckAllDelete').prop('checked', true);
                } else {
                    $('#ckCheckAllDelete').prop('checked', false);
                }
            }, error: function (error) {
                congdoan.notify('Has an error', 'error');
                console.log(error);
            }
        })
    }
    function resetFormMaintainance() {
        $('#hidIdM').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
    }
    function loadData(isChangePaged) {
        $.ajax({
            url: '/admin/role/GetAllPaging',
            type: 'GET',
            dataType: 'json',
            data: {
                keyword: $('#txtSearchKeyword').val(),
                page: congdoan.configs.pageIndex,
                pageSize: congdoan.configs.pageSize
            },
            success: function (result) {
                var template = $('#table-template').html();
                var render = '';
                if (result.RowCount > 0) {
                    $.each(result.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Description: item.Description,
                            Id: item.Id
                        });
                    });
                    $('#lblTotalRecords').text(result.RowCount);
                    if (render !== '') {
                        $('#tblContent').html(render);
                    }
                    wrapPaging(result.RowCount, function () {
                        loadData();
                    }, isChangePaged);
                } else {
                    congdoan.notify('Dont have anything', 'error');
                }
            },
            error: function (error) {
                congdoan.notify('Load data failed', 'error');
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