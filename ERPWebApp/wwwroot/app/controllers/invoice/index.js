var invoiceController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $("#txtId").prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#txtNumberOfConstruction").prop('disabled', true);
        $("#chkStatus").prop('checked', true);
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

                selFullName: {
                    required: true
                }
            }
        });
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selFullName').on('change', function () {
            var id = $('#selFullName option:selected').val();
            var temp = general.getCode(general.addressBookType.CustomerC, id, general.addressBookType.max_length);
            $("#txtId").val(temp);
        })

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
            loadSelCustomer(false, general.addressBookType.CustomerC);
            $('#modal-add-edit').modal('show');

        });
        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            $('#selFullName').prop('disabled', true);
            var that = $(this).data('id');
            loadSelCustomer(true, general.addressBookType.CustomerC, function () {
                loadDetail(that);
            });
        });
        var status;
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var addressBookFk = $('#selFullName option:selected').val();
                var id = $('#txtId').val();

                var customerType = $('#selCustomerType option:selected').val();

                var lastRevisedCreditLimitDate = $('#dpLastRevisedCreditLimitDate').val();
                var creditlimit = $('#txtCreditLimit').val();

                var evaluatedCustomer = $('#txtEvaluatedCustomer').val();
                var note = $('#txtNotes').val();
                //var lastUpdatedBy = $('#user').data('userid');
                //var lastUpdatedDate = $('#dtDateModified').val();

                var a = $("#chkStatus").is(':checked');
                if (a == true)
                    status = 1;
                else
                    status = 0;
                var data = {
                    KeyId: gId,
                    Id: id,
                    AddressBook_FK: addressBookFk,
                    CreditLimit: creditlimit,
                    LastRevisedCreditLimitDate: lastRevisedCreditLimitDate,
                    CustomerType: customerType,
                    EvaluatedCustomer: evaluatedCustomer,
                    Notes: note,
                    //LastUpdatedByFk: lastUpdatedBy,
                    //DateModified: lastUpdatedDate,
                    Status: status
                };

                $.ajax({
                    type: "POST",
                    url: "/customer/SaveEntity",
                    data: data,
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {
                        updateIsCustomer(response.AddressBook_FK);
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
                return false;
            }

        });
    }
    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/invoice/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                var data = response;
                gId = data.KeyId;
                $('#txtId').val(data.KeyId);
                $('#txtFullName').val(data.CustomerFkNavigation.UserBy.FullName);
                $('#dtDateCreated').val(general.dateFormatJson(data.DateCreated));
                $('#txtCreatedBy').val(data.SalespersonNoNavigation.FullName);
                $('#txtSalesPerson').val(data.SalespersonNoNavigation.FullName);
                $('#txtNotes').val(data.Description);
                $('#Total').text(general.commaSeparateNumber(data.Amount));
                $('#DiscountAmt').text(general.commaSeparateNumber(data.Discount));
                $('#TotalInvoice').text(general.commaSeparateNumber(data.TotalInvoice));
                $('#PaidAmount').text(general.commaSeparateNumber(data.PaidAmount));
                loadInvoiceDetail(that);
                $('#modal-add-edit').modal('show');
                general.stopLoading();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }
    function loadInvoiceDetail(that) {
        var template = $('#detail-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/invoice/GetAllInvoiceDetail',
            data: { invoiceId: that },
            success: function (response) {
                console.log(response);
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        ProductCode: item.InventoryFkNavigation.ProductFkNavigation.ProductCode,
                        ProductName: item.InventoryFkNavigation.ProductFkNavigation.ProductName,
                        Qty: item.QtyOrderedNow,
                        UnitPrice: general.commaSeparateNumber(item.UnitPrice),
                        Discount: general.commaSeparateNumber(item.Discount),
                        PriceSale: general.commaSeparateNumber(item.UnitPrice - item.Discount),
                        SubTotal: item.Subtotal
                    });
                    if (render != '') {
                        $('#tbl-detail').html(render);
                    }
                });
            },
            error: function () {

            }
        });
    }
    function updateIsCustomer(id) {
        $.ajax({
            type: 'POST',
            url: '/addressbook/UpdateIsCustomer',
            data: { id },
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        });
    }
    function resetFormMaintainance() {
        $('#selFullName').prop('disabled', false);
        $('#txtId').val(0);
        $('#selFullName').val('');

        $('#selCustomerType').val('');

        $('#txtCreditLimit').val('');
        $('#txtNumberOfConstruction').val('');
        $('#txtEvaluatedCustomer').val('');

        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        $('#dpLastRevisedCreditLimitDate').val('');
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);
    }
}

function loadSelCustomer(status, type, callback) {
    $('#selFullName option').remove();
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
            $('#selFullName').append("<option value=''>Chọn tên</option>");
            $.each(response, function (i, item) {
                $('#selFullName').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
            });
            callback();
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
        url: '/invoice/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    DateCreated: general.dateFormatJson(item.DateCreated,true),
                    FullName: item.CustomerFkNavigation.UserBy.FullName,
                    TotalInvoice: general.commaSeparateNumber(item.TotalInvoice),
                    Discount: general.commaSeparateNumber(item.Discount),
                    PaidAmount: general.commaSeparateNumber(item.PaidAmount)
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
