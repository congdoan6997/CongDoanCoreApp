var functionController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            lang: 'en',
            ignore: [],
            rules: {
                txtIdM: { required: true },
                txtNameM: { required: true },
                txtOrderM: { required: true },
                txtUrlM: { required: true }
            }
        });
        $('#btnCreate').on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            disableFieldEdit(false);
            $('#modalAddEdit').modal('show');
        })
        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#txtIdM').val();
            $.ajax({
                url: '/admin/function/getbyid',
                data: {
                    id: that
                },
                type: 'GET',
                dataType: 'json',
                success: function (result) {
                    disableFieldEdit(true);
                    $('#txtNameM').val(result.Name);
                    $('#txtUrlM').val(result.Url);
                    $('#txtIconM').val(result.IconCss);
                    $('#txtOrderM').val(result.SortOrder);
                    $('#ckStatusM').prop('checked', result.Status === 1);
                    initTreeDropDownFunction(result.ParentId);
                    $('#modalAddEdit').modal('show');
                }, error: function (error) {
                    congdoan.notify('Has an error in loading function', 'error');
                }
            })
        });
        $('body').on('click', '#btnSave', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#txtIdM').val().toUpperCase();
                var name = $('#txtNameM').val();
                var url = $('#txtUrlM').val();
                var icon = $('#txtIconM').val();
                var order = parseInt($('#txtOrderM').val());
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var parentId = $('#ddlFunctionIdM').combotree('getValue');
                $.ajax({
                    url: '/admin/function/saveentity',
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        Id: id,
                        Name: name,
                        Url: url,
                        ParentId: parentId,
                        IconCss: icon,
                        SortOrder: order,
                        Status: status
                    },
                    success: function (result) {
                        congdoan.notify('Update success', 'success');
                        $('#modalAddEdit').modal('hide');
                        loadData();
                    },
                    error: function (error) {
                        console.log(error);
                        congdoan.notify('Has an error in update progress', 'error');
                    }
                })
            }
        });
        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#txtIdM').val().toUpperCase();
            congdoan.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        id: that
                    },
                    url: '/admin/function/delete',
                    success: function (result) {
                        congdoan.notify('Delete success', 'success');
                        loadData();
                    }, error: function (error) {
                        congdoan.notify('Has an error in deleting progress', 'error');
                    }
                });
            });
        })
    }
    function disableFieldEdit(disabled) {
        $('#txtIdM').prop('disabled', disabled);
    }
    function resetFormMaintainance() {
        $('#txtIdM').val('');
        $('#txtNameM').val('');
        $('#txtUrlM').val('');
        $('#txtIconM').val('');
        $('#txtOrderM').val('');
        $('#ckStatusM').prop('checked', true);
        initTreeDropDownFunction('');
    }
    function initTreeDropDownFunction(selectedId) {
        $.ajax({
            url: '/admin/function/getall',
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                var data = [];
                $.each(result, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });

                var treeArr = congdoan.unflattern(data);

                $('#ddlFunctionIdM').combotree({
                    data: treeArr
                });
                if (selectedId !== undefined) {
                    $('#ddlFunctionIdM').combotree('setValue', selectedId);
                }
            }, error: function (error) {
                congdoan.notify('Cannot loading data', 'error');
            }
        })
    }
    function loadData() {
        $.ajax({
            type: 'GET',
            url: '/admin/function/getall',
            dataType: 'json',
            success: function (result) {
                var data = [];
                $.each(result, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var treeArr = congdoan.unflattern(data);

                var $tree = $('#treeFunction').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        $('#treeFunction').tree('select', node.target);
                        $('#txtIdM').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    animate: true,
                    lines: true,
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });
                            //update to database

                            $.ajax({
                                url: '/admin/function/updateparentId',
                                type: 'POST',
                                datatype: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (result) {
                                    loadData();
                                },
                                error: function (error) {
                                    console.log(error);
                                    congdoan.notify('Cannot update database', 'error');
                                }
                            });
                        }
                        //update
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/admin/function/reorder',
                                type: 'POST',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (result) {
                                    loadData();
                                },
                                error: function (error) {
                                    console.log(error);
                                    congdoan.notify('Cannot update point', 'error');
                                }
                            });
                        }
                    }
                });
            },
            error: function (error) {
                congdoan.notify('Cannot loading data', 'error');
            }
        })
    }
}