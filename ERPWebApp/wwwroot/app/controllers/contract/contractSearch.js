var contractSearchController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        //-------------  event button main -------------------
        $('#btnSearch_search').on('click', function () {
            loadData(true);
        });
        $('#txtKeyword_search').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
        });
        
        $('#btnRefresh-search').on('click', function () {
            $('#btnRefresh-search').hide();
            datarefresh(function () {
                $('#btnRefresh-search').show();
            });
        });
        $('#btn-cancel-Contract').on('click', function () {
            contract.reset_ControlForm();
            $('#Page-searchContract').show();
            $('#Page-mainContract').hide();
            // load lại data;
            $('#btnSearch_search').click();

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchContract').hide();
                $('#Page-mainContract').show();
                contract.loadbyid(SalesOrderId, false);
            }

        });
        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchContract').hide();
                $('#Page-mainContract').show();
                contract.loadbyid(SalesOrderId, true);
            }
            //$("#horizontal-form").addClass("disabledbutton");
        });
        //---------------end search--------------------------

        //----------------event button main ---------------------
     
        $('#btnClose').on('click', function () {
            general.confirm('Bạn có chắc chắn không ?', function () {
                contract.reset_ControlForm();
                $('#Page-searchContract').show();
                $('#Page-mainContract').hide();
            });
        });

        //----------------end event button main ---------------------

        function datarefresh(callback) {
            loadData(true);
            callback();
        }
    }

}

function loadData(isPageChanged) {
    var template = $('#table-template-search').html();
    
    var render = "";
    $.ajax({
        type: 'GET',
        data: {
            keyword: $('#txtKeyword_search').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize
        },
        url: '/Contract/GetAllPagingContract',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    stt: i + 1,
                    KeyId: item.KeyId,
                    ConstructionId: item.ConstructionId,
                    ConstructionName: item.ConstructionName,
                    CustomerName: item.CustomerName,
                    CustomerPONumber: item.CustomerPONumber,
                    CustomerPODate: general.dateFormatJson(item.CustomerPODate,true),
                    StatusName: item.Status
                });
                
                
            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content_search').html(render);
            general.stopLoad();
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoad();
        }
    });
}
function wrapPaging(recordCount, callBack, changePageSize) {
    var totalsize = Math.ceil(recordCount / general.configs.pageSize);
    //Unbind pagination if it existed or click change pagesize
    if ($('#paginationUL a').length === 0 || changePageSize === true) {
        $('#paginationUL').empty();
        $('#paginationUL').removeData("twbs-pagination");
        $('#paginationUL').unbind("page");
    }
    //Bind Pagination Event
    if (totalsize>0)
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
