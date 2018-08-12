var productCategoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    };
    function registerEvents() {
        $("#frmMaintainance").validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: {
                    required: true
                },
                txtOrderM: {
                    number: true
                },
                txtHomeOrderM: { number: true }
            }
        });
        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownCategory();
            $('#modalAddEdit').modal('show');
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            $.ajax({
                type: "GET",
                url: "/admin/productcategory/getbyid",
                data: {
                    id: that
                },
                dataType: "json",
                beforeSend: function () {
                    congdoan.startLoading();
                },
                success: function (result) {
                    $('#hidIdM').val(result.Id);
                    $('#txtNameM').val(result.Name);
                    //drop
                    initTreeDropDownCategory(result.ParentId);
                    $('#txtDescM').val(result.Description);
                    $('#txtOrderM').val(result.SortOrder);

                    $('#txtHomeOrderM').val(result.HomeOrder);
                    $('#txtImageM').val(result.Image);
                    $('#txtSeoPageTitleM').val(result.SeoPageTitle);
                    $('#txtSeoAliasM').val(result.SeoAlias);
                    $('#txtSeoKeywordM').val(result.SeoKeywords);
                    $('#txtSeoDescriptionM').val(result.SeoDescription);
                    $('#ckStatusM').prop('checked', result.Status === 1);
                    $('#ckShowHomeM').prop('checked', result.HomeFlag);

                    $('#modalAddEdit').modal('show');
                    congdoan.stopLoading();
                },
                error: function (error) {
                    console.log(error);
                    congdoan.notify('Update product category if fail', 'error');
                    congdoan.stopLoading();
                }
            });
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            congdoan.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/admin/productcategory/delete',
                    data: {
                        id: that
                    },
                    dataType: 'json',
                    beforeSend: function () {
                        congdoan.startLoading();
                    },
                    success: function (result) {
                        congdoan.notify('Deleted success', 'success');
                        congdoan.stopLoading();
                        loadData();
                    },
                    error: function (error) {
                        congdoan.notify('Has an error in deleting progress', 'error');
                        congdoan.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();

                var image = $('#txtImageM').val();
                var order = parseInt($('#txtOrderM').val());
                var homeOrder = $('#txtHomeOrderM').val();

                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var showHome = $('#ckShowHomeM').prop('checked');
                $.ajax({
                    type: 'POST',
                    url: '/admin/productcategory/saveentity',
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    }, dataType: 'json',
                    beforeSend: function () {
                        congdoan.startLoading();
                    },
                    success: function (result) {
                        congdoan.notify('Update success', 'success');
                        $('#modalAddEdit').modal('hide');

                        resetFormMaintainance();

                        congdoan.stopLoading();

                        loadData(true);
                    },
                    error: function (error) {
                        console.log(error);
                        congdoan.notify('Has an error in update progress', 'error');
                        congdoan.stopLoading();
                    }
                });
            }
            return false;
        });
        $('#btnCancel').on('click', function (e) {
            resetFormMaintainance();
        });
    }
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
    }
    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: '/admin/productcategory/getall',
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
                var arr = congdoan.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
    function loadData() {
        $.ajax({
            url: '/admin/productcategory/getall',
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
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                var $tree = $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        $('#hidIdM').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
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
                                url: '/admin/productcategory/updateparentId',
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
                                url: '/admin/productcategory/reorder',
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
                console.log('Cannot loading data', 'error');
            }
        });
    }
};