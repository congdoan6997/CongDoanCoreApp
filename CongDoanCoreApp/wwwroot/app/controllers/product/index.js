/// <reference path="../../../lib/mustache/mustache.js" />
/// <reference path="../../../lib/twbs-pagination/jquery.twbspagination.js" />

var productController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    };

    function registerEvents() {
        $("#ddlShowPage").on('change', function () {
            congdoan.configs.pageSize = $(this).val();
            congdoan.configs.pageIndex = 1;
            loadData(true);
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: "GET",
            data: {
                categoryId: null,
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
                        Price: congdoan.formatNumber( item.Price, 0),
                        Image: item.Image === null ? '<img src="/admin-side/images/user.png" width=25/>' : '<img src="' + item.Image + '" width="25" />',
                        CreatedDate: congdoan.dateTimeFormatJson(item.DateCreated),
                        Status: congdoan.getStatus( item.Status)
                    });

                });
                $("#lblTotalRecords").text(result.PageCount);
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