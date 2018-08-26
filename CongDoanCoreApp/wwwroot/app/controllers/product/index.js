/// <reference path="../../../lib/mustache/mustache.js" />
/// <reference path="../../../lib/twbs-pagination/jquery.twbspagination.js" />
/// <reference path="../../../lib/ckeditor/ckeditor.js" />

var productController = function () {
    this.initialize = function () {
        loadCategories();
        loadData();
        registerEvents();
        registerControls();
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
                txtPriceM: {
                    number: true,
                    required: true
                },
                ddlCategoryIdM: { required: true }
            }
        });

        $("#ddlShowPage").on('change', function () {
            congdoan.configs.pageSize = $(this).val();
            congdoan.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSearch').on('click', function () {
            loadData();
        });

        $('#txtSearchKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });

        $('#btnCreate').on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            $('#modalAddEdit').modal('show');
        });

        $('body').on('click', '.btnEdit', function (e) {
            e.preventDefault();
            var that = this.id;
            $.ajax({
                type: 'GET',
                dataType: 'json',
                data: {
                    id: that
                },
                url: '/admin/product/getbyid',
                success: function (result) {
                    $('#hidIdM').val(result.Id);
                    $('#txtNameM').val(result.Name);
                    //drop
                    initTreeDropDownCategory(result.CategoryId);
                    $('#txtDescM').val(result.Description);
                    $('#txtUnitM').val(result.Unit);

                    $('#txtPriceM').val(result.Price);
                    $('#txtOriginalPriceM').val(result.OriginalPrice);
                    $('#txtPromotionPriceM').val(result.PromotionPrice);
                    //$('#txtImageM').val(result.Image);

                    CKEDITOR.instances.txtContentM.setData(result.Content);

                    $('#txtTagM').val(result.Tags);
                    $('#txtSeoPageTitleM').val(result.SeoPageTitle);
                    $('#txtSeoAliasM').val(result.SeoAlias);
                    $('#txtSeoKeywordM').val(result.SeoKeywords);
                    $('#txtSeoDescriptionM').val(result.SeoDescription);

                    $('#ckStatusM').prop('checked', result.Status === 1);
                    $('#ckHotM').prop('checked', result.HotFlag);
                    $('#ckShowHomeM').prop('checked', result.HomeFlag);

                    $('#modalAddEdit').modal('show');
                },
                error: function (error) {
                    congdoan.notify('Has an error', 'error');
                }
            });
        });

        $('body').on('click', '.btnDelete', function (e) {
            e.preventDefault();
            var that = this.id;
            congdoan.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/admin/product/delete',
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

                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var categoryId = $('#ddlCategoryIdM').combotree('getValue');

                var description = $('#txtDescM').val();
                var unit = $('#txtUnitM').val();

                var price = $('#txtPriceM').val();
                var originalPrice = $('#txtOriginalPriceM').val();
                var promotionPrice = $('#txtPromotionPriceM').val();

                //var image = $('#txtImageM').val();

                var tags = $('#txtTagM').val();
                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();

                var content = CKEDITOR.instances.txtContentM.getData();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var hot = $('#ckHotM').prop('checked');
                var showHome = $('#ckShowHomeM').prop('checked');

                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        CategoryId: categoryId,
                        Image: '',
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,

                        HomeFlag: showHome,
                        HotFlag: hot,
                        Content: content,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "json",
                    beforeSend: function () {
                        congdoan.startLoading();
                    },
                    success: function (response) {
                        congdoan.notify('Update product successful', 'success');
                        $('#modalAddEdit').modal('hide');
                        resetFormMaintainance();

                        congdoan.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        congdoan.notify('Has an error in save product progress', 'error');
                        congdoan.stopLoading();
                    }
                });
                return false;
            }
        });

        $('body').on('click', 'btnCancel', function (e) {
            e.preventDefault();
            resetFormMaintainance();
        });

        $('#btnSelectImg').on('click', function (e) {
            e.preventDefault();
            $('#fileInputImage').click();
        });

        $('#fileInputImage').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: 'POST',
                url: '/admin/upload/uploadimage',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $('#txtImageM').val(result);
                    congdoan.notify('Upload image succesful', 'success');
                },
                error: function (error) {
                    congdoan.notify('There was error uploading files', 'error');
                    console.log(error);
                }
            });
        });

        $('#btnImport').on('click', function (e) {
            e.preventDefault();
            initTreeDropDownCategory();
            $('#modalImportExcel').modal('show');
        });
        $('#btnImportExcel').on('click', function () {
            var fileUpload = $('#fileInputExcel').get(0);

            var files = fileUpload.files;

            //create formData object
            var fileData = new FormData();
            //looping over all files and add iy yo FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('files', files[i]);
            }

            //adding one more key to formdata object
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));

            $.ajax({
                url: '/admin/product/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,//tell jquery not to process the data
                contentType: false,//tell jquery not to set contenttype
                success: function (result) {
                    congdoan.notify('Add product successful', 'success');
                    $('#modalImportExcel').modal('hide');
                    loadData();
                }, error: function (error) {
                    congdoan.notify('Has an error in save product progress', 'error');
                    console.log(error);
                }
            });
        });
    }
    function registerControls() {
        CKEDITOR.replace('txtContentM', {
            extraPlugins: 'colorbutton'
        });
        // bootstrap-ckeditor-fix.js
        // hack to fix ckeditor/bootstrap compatiability bug when ckeditor appears in a bootstrap modal dialog
        //
        // Include this file AFTER both jQuery and bootstrap are loaded.
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            modal_this = this;
            $(document).on('focusin.modal', function (e) {
                if (modal_this.$element[0] !== e.target && !modal_this.$element.has(e.target).length
                    && !$(e.target.parentNode).hasClass('cke_dialog_ui_input_select')
                    && !$(e.target.parentNode).hasClass('cke_dialog_ui_input_text')) {
                    modal_this.$element.focus();
                }
            });
        };
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: '/admin/productCategory/getall',
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
                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtUnitM').val('');

        $('#txtPriceM').val('');
        $('#txtOriginalPriceM').val('');
        $('#txtPromotionPriceM').val('');
        CKEDITOR.instances.txtContentM.setData('');
        $('#txtTagM').val('');
        $('#txtSeoKeywordM').val('');
        $('#txtSeoDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
    }

    function loadCategories() {
        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/admin/product/GetAllCategories',
            success: function (result) {
                var render = '<option value="">--Select category--</option>';
                $.each(result, function (i, item) {
                    render += "<option value= '" + item.Id + "'>" + item.Name + " </option>";
                });
                $('#ddlCategorySearch').html(render);
            },
            error: function (error) {
                console.log(error);
                congdoan.notify('Connot loading product category data', 'error');
            }
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: "GET",
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $("#txtSearchKeyword").val(),
                page: congdoan.configs.pageIndex,
                pageSize: congdoan.configs.pageSize
            },
            url: "/admin/product/getallpaging",
            dataType: "json",
            success: function (result) {
                console.log(result);
                $.each(result.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        ProductCategoryName: item.ProductCategory.Name,
                        Price: congdoan.formatNumber(item.Price, 0),
                        Image: item.Image === null ? '<img src="/admin-side/images/user.png" width=25/>' : '<img src="' + item.Image + '" width="25" />',
                        CreatedDate: item.DateCreated,
                        Status: congdoan.getStatus(item.Status)
                    });
                });
                //hiển thị số lượng trang
                $("#lblTotalRecords").text(result.RowCount);
                //xuất html danh sách sản phẩm
                if (render !== '') {
                    $("#tbl-content").html(render);
                }
                wrapPaging(result.RowCount, function () {
                    loadData();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                congdoan.notify("Cannot loading data", "error");
            }
        });
    }

    function wrapPaging(recordCount, callBack, changPageSize) {
        var totalSize = Math.ceil(recordCount / congdoan.configs.pageSize);
        //unbind pagination
        if ($("#paginationUL a").length === 0 || changPageSize === true) {
            $("#paginationUL").empty();
            $("#paginationUL").removeData("twbs-pagination");
            $("#paginationUL").unbind("page");
        }
        //bind pgination
        $("#paginationUL").twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, page) {
                congdoan.configs.pageIndex = page;
                setTimeout(callBack(), 200);
            }
        });
    }
};