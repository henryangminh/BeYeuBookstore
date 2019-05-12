var advertiseContractStatisticController = function () {
    this.initialize = function () {
        loadAllAdvertiser();
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });

        $('#selAdvertiser').on('change', function () {
            loadData();
        });

        $('.monthpicker').datepicker({
            format: "mm/yyyy",
            //todayBtn: "linked",
            clearBtn: true,
            language: "vi",
            todayHighlight: true,
            startView: 1,
            minViewMode: 1
        });

    }
}

function loadData(isPageChanged) {

    var template = $('#table-template').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            advertiserId: $('#selAdvertiser option:selected').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/AdvertiseContract/GetAllStatisticPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
            general.startLoading();
        },
        success: function (response) {
            console.log("Statisticdata", response);

            $.each(response.Results, function (i, item) {
                var _totalContractValuePeriod = general.toMoney(item.TotalContractValuePeriod);
                var _totalContractValue = general.toMoney(item.TotalContractValue);
                render += Mustache.render(template, {
                    Advertiser: item.Advertiser,
                    NoContract: item.NoOfContract,
                    NoSuccessContract: item.NoOfSuccessContract,
                    TotalContractValuePeriod: _totalContractValuePeriod,
                    TotalContractValue: _totalContractValue,

                });

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
            general.stopLoad();
            general.stopLoading();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');

            general.stopLoad();
            general.stopLoading();
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
    if (totalsize > 0)
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
function loadAllAdvertiser() {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAdvertiserByStatistic',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selAdvertiser').append("<option value='" + item.KeyId + "'>" + item.BrandName + "</option>");
            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });
}