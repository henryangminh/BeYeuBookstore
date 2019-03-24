var vendorController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $("#txtId").prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#txtOutstand_Inv_Amt").prop('disabled', true);
        $("#txtOutstand_Credit").prop('disabled', true);
        $("#dtpLast_Purchase_Date").prop('disabled', true);
        $("#dtpLast_Payment_Date").prop('disabled', true);
        $("#txtVender").prop('disabled', true);
        $("#chkStatus").prop('checked', true);
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtVender: {
                    required: true
                },
                txtAP_Balance:{
                    number: true
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
            loadSelVendor(false, general.addressBookType.Vendor, function (arr) {
                autoAddressbook(document.getElementById('txtVender'), arr);
                $('#modal-add-edit').modal('show');
            })
            

        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#txtVender').prop('disabled', true);
            var that = $(this).data('id');
            loadDetail(that);
           ;
            
        });
        var status;
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var addressBookFk = document.getElementById('txtVender').getAttribute('data-addressbookid');;
                var id = $('#txtId').val();

                var performanceRating = $('#selPerformanceRating option:selected').val();

                var ap_Balance =general.toInt($('#txtAP_Balance').val());
                var outstand_Inv_Amt =general.toInt( $('#txtOutstand_Inv_Amt').val());
                var outstand_Credit =general.toInt($('#txtOutstand_Credit').val());

                var last_Purchase_Date =general.dateFormatJson($('#dtpLast_Purchase_Date').val(),false);
                var last_Payment_Date = general.dateFormatJson($('#dtpLast_Payment_Date').val(),false);
                var note = $('#txtNotes').val();
               
               // var lastUpdatedDate = $('#dtDateModified').val();

                var a = $("#chkStatus").is(':checked');
                if (a == true)
                    status = 1;
                else
                    status = 0;
                var data = {
                    KeyId: gId,
                    Id: id,
                    UserFk: addressBookFk,
                    PerformanceRating: performanceRating,
                    ApBalance: ap_Balance,
                    OutstandInvAmt: outstand_Inv_Amt,
                    OutstandCredit: outstand_Credit,
                    LastPurchaseDate: last_Purchase_Date,
                    LastPaymentDate: last_Payment_Date,
                    Notes: note,
                    Status: status
                };
                if (addressBookFk != '')
                    $.ajax({
                        type: "POST",
                        url: "/vendor/SaveEntity",
                        data: data,
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoading();
                        },
                        success: function (response) {
                            if (gId == 0)
                                updateIsVendor(addressBookFk);
                            general.notify('Ghi thành công!', 'success');
                            $('#modal-add-edit').modal('hide');
                            resetFormMaintainance();

                            general.stopLoading();
                            loadData(true);
                        },
                        error: function () {
                            general.notify('Có lỗi trong khi ghi!', 'error');
                            general.stopLoading();
                        }
                    });
                else {
                    general.notify('Vui lòng chọn nhà cung cấp trước', 'error');
                }
                return false;
            }

        });
    }
    function loadDetail(that) {
        $.ajax({
            type: "GET",
            url: "/vendor/GetById",
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
                if (data.UserBy != null) {
                    $('#txtVender').val(data.UserBy.FullName);
                }
                document.getElementById('txtVender').setAttribute('data-addressbookid', data.UserFk);
                    $('#selPerformanceRating').val(data.PerformanceRating);

                $('#txtAP_Balance').val(data.ApBalance);

                $('#txtOutstand_Inv_Amt').val(general.dateFormatJson(data.OutstandInvAmt));
                $('#txtOutstand_Credit').val(data.OutstandCredit);
                $('#dtpLast_Purchase_Date').val(general.dateFormatJson(data.Last_Purchase_Date, true));
                $('#dtpLast_Payment_Date').val(general.dateFormatJson(data.Last_Payment_Date, true));
                $('#txtNotes').val(data.Notes);
                if (data.LastUpdatedFk != null)
                    $('#txtLastUpdatedByFK').val(data.LastUpdatedByNavigation.FullName);

                $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
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
    }
    function updateIsVendor(id) {
        $.ajax({
            type: 'POST',
            url: '/addressbook/UpdateIsVendor',
            data: { id },
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        });
    }
    function resetFormMaintainance() {
        $('#txtVender').prop('disabled', false);
        $('#txtId').val(0);
        $('#txtVender').val('');

        $('#selPerformanceRating').val('');

        $('#txtAP_Balance').val('');
        $('#txtOutstand_Inv_Amt').val('');
        $('#txtOutstand_Credit').val('');

        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        
        $('#dtpLast_Payment_Date').val('');
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);

    }



}

function loadSelVendor(status, type,callback) {
    $.ajax({
        type: 'GET',
        data: {
            status: status,
            Type: type
        },
        url: '/addressbook/GetListFullName',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            if (response != null && response.length > 0)
                callback(response);
            else {
                general.notify('Hết danh sách nhà cung cấp cần tạo mới. Vui lòng thêm ở danh bạ trước!', 'error');
                return false;
            }
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
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
        url: '/vendor/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    Id: item.Id,
                    FullName: item.UserBy==null?'': item.UserBy.FullName,
                    MobilePhone: item.UserBy == null ? '' :item.UserBy.PhoneNumber,
                    TaxIDNumber: item.UserBy == null ? '' : item.UserBy.TaxIdnumber,
                    Email: item.UserBy == null ? '' : item.UserBy.Email
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
