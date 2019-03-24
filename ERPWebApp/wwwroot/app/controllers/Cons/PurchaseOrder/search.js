var POSearchController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    this.initData = function () {
        loadData();
    }
    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSearch').on('click', function () {
            loadData(true);
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
        });

        $("#btnCreate").on('click', function () {
            $('#Page-SearchPurchaseOrder').hide();
            $('#Page-mainPO').show();
            purchaseOrder.loadById(0, false);
        });
        $('#btnClose').on('click', function () {
            $('#Page-SearchPurchaseOrder').show();
            $('#Page-mainPO').hide();
        })
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadProductBom(that, function (data) {
                gId = data.KeyId;
                loadBom(data.KeyId);
            });
            $('#modal-add-edit').modal('show');
        });
    }
    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            url: '/PurchaseOrderCons/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        stt: i + 1,
                        KeyId: item.KeyId,
                        ProductCode: item.ProductCode,
                        ProductName: item.ProductName,
                        ProductUnit: item.ProductUnitNavigation.UnitName,
                        Status: general.getStatus(item.Status)
                    });

                });
                $('#lblTotalRecords').text(response.RowCount);
                $('#tbl-content').html(render);
                wrapPagingUnit(response.RowCount, function () {
                    loadData();
                }, isPageChanged);

                //if (render == '')
                //   $('#datatable-checkbox_paginate').hide();
                //else
                //   $('#datatable-checkbox_paginate').show();
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function wrapPagingUnit(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / general.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        if (totalsize > 0) {
            $('#paginationUL').twbsPagination({
                totalPages: totalsize,
                visiblePages: 7,
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (event, p) {
                    general.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }

    }
}