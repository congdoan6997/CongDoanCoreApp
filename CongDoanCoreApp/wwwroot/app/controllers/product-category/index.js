var productCategoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    };
    function registerEvents() {

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

                var $tree = $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd:true
                });
            },
            error: function (error) {
                console.log('Cannot loading data', 'error');
            }
        });
    }
};