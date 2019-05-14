var notificationController = function () {
    this.initialize = function () {
        registerEvents();
        loadData();
    }
    function registerEvents() {
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });
    }

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
        url: '/Notification/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    stt: i + 1,
                    KeyId: item.Id,
                    NotificationContent: item.NotificationContent,
                    NotificationTime: general.dateTimeFormatJson(item.NotificationTime),
                    Status: item.Status
                });
                $('#lblTotalRecords').text(response.RowCount);
                if (render != '') {
                    $('#tbl-content').html(render);
                }
                general.stopLoad();
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
            });
            $('#tbl-content tr').each(function () {
                var status = general.toInt($(this).children('#stt').data('status'));
                if (status == 0)
                    $(this).css({ 'background-color': ' #ccf5ff' });
            });
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
