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
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                var $tree = $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
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