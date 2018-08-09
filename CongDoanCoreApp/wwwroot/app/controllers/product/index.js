/// <reference path="../../../lib/mustache/mustache.js" />

var productController = function () {
    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        loadData();
    }

    function loadData() {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: "GET",
            url: "/admin/product/getall",
            dataType: "json",
            success: function (result) {
                $.each(result, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        ProductCategoryName: item.ProductCategory.Name,
                        Price: congdoan.formatNumber( item.Price, 0),
                        Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=25/>' : '<img src="' + item.Image + '" width="25" />',
                        CreatedDate: congdoan.dateTimeFormatJson(item.DateCreated),
                        Status: congdoan.getStatus( item.Status)
                    });
                    if (render !== '') {
                        $("#tbl-content").html(render);
                    }
                });
            },
            error: function (status) {
                console.log(status);
                congdoan.notify("Cannot loading data", "error");
            }
        });
    }
};