var WareHouseController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $("#chkStatus").prop('checked', true);

        $("#txtId").attr('maxlength', '15');
        $("#txtWarehouseName").attr('maxlength', '50');
        $("#txtWarehouseAddress").attr('maxlength', '100');

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtId: {
                    required: true
                },
                txtWarehouseName: {
                    required: true
                },
                txtWarehouseAddress: {
                    required: true
                }
            }
        });
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
            gId = 0;
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');

            $.ajax({
                type: "GET",
                url: "/Warehouse/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = data.KeyId;
                    $('#txtId').val(data.Id);
                    $('#txtWarehouseName').val(data.WarehouseName);
                    $('#txtWarehouseAddress').val(data.WarehouseAddress);
                    if (data.Status == 1)
                        $('#chkStatus').prop('checked', true);
                    else
                        $('#chkStatus').prop('checked', false);
                    $('#modal-add-edit').modal('show');
                    general.stopLoading();

                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                save();
            }

        });
    }
    function save() {
        var status;
        var Id = $('#txtId').val();
        var WarehouseName = $('#txtWarehouseName').val();
        var WarehouseAddress = $('#txtWarehouseAddress').val();
        var a = $("#chkStatus").is(':checked');
        if (a == true)
            status = 1;
        else
            status = 0;
        var data = {
            KeyId: gId,
            Id: Id,
            WarehouseName: WarehouseName,
            WarehouseAddress: WarehouseAddress,
            Status: status
        };

        $.ajax({
            type: "POST",
            url: "/Warehouse/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                general.notify('Ghi thành công!', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();

                general.stopLoading();
                loadData(true);
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi danh bạ!', 'error');
                general.stopLoading();
            }
        });
    }
    function resetFormMaintainance() {
        $('#txtId').val('');
        $('#txtWarehouseName').val('');
        $('#txtWarehouseAddress').val('');
        $("#chkStatus").prop('checked', true);
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
        url: '/Warehouse/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    stt:i+1,
                    Id: item.Id,
                    WarehouseName: item.WarehouseName,
                    WarehouseAddress: item.WarehouseAddress,
                    Status: general.getStatus(item.Status)
                });
                
                
            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
            general.stopLoading();
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
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
