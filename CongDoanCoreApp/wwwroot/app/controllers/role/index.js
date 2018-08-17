var roleController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtName: { required: true },
                txtDescription: { required: true }
            }
        })
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
                })
            }
        })
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
}