var customerController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $('.form_date').datetimepicker({
            language: 'vi',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });
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
                txtCustomer: {
                    required: true
                },
                txtCreditLimit: {
                    required: true,
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
            loadSelCustomer(false, general.addressBookType.CustomerC, function (arr) {
                autoAddressbook(document.getElementById("txtCustomer"), arr);
                $('#modal-add-edit').modal('show');
            });       

        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#txtCustomer').prop('disabled', true);
            var that = $(this).data('id');
            loadDetail(that);
        });
        var status;
        $('#txtCreditLimit').keyup(function (event) {
            
            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var userFk = document.getElementById('txtCustomer').getAttribute('data-addressbookid');
                if (userFk == '') {
                    general.notify('Vui lòng chọn khách hàng trước !', 'error')
                    return false;
                }
                var id = $('#txtId').val();

                var customerType = $('#selCustomerType option:selected').val();
                var lastRevisedCreditLimitDate = '';
                var dtRevisedCL = $('#dpLastRevisedCreditLimitDate');
                if (dtRevisedCL.data('date')!='') // co chon tg
                    lastRevisedCreditLimitDate = general.dateFormatJson(dtRevisedCL.data("datetimepicker").getDate(), false);
                var creditlimit = $('#txtCreditLimit').val();

                var evaluatedCustomer = $('#txtEvaluatedCustomer').val();
                var note = $('#txtNotes').val();
               
                //var lastUpdatedDate = $('#dtDateModified').val();
                
                var a = $("#chkStatus").is(':checked');
                if (a == true)
                    status = 1;
                else
                    status = 0;
                var data = {
                    KeyId: gId,
                    Id: id,
                    UserFk:userFk,
                    CreditLimit: creditlimit,
                    LastRevisedCreditLimitDate: lastRevisedCreditLimitDate,
                    CustomerType: customerType,
                    EvaluatedCustomer: evaluatedCustomer,
                    Notes: note,
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
                        if (gId==0)
                            updateIsCustomer(userFk);
                        general.notify('Ghi thành công!', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        general.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoading();
                    }
                });
                return false;
            }

        });
    }
    function loadRight() {
        document.getElementById("txtCreditLimit").style.textAlign = "right";
    }
    function loadDetail(that) { 
        $.ajax({
            type: "GET",
            url: "/customer/GetById",
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
                document.getElementById('txtCustomer').setAttribute('data-addressbookid',data.UserFk);
                $('#txtCustomer').val(data.UserBy.FullName);
                $('#selCustomerType').val(data.CustomerType);

                $('#txtCreditLimit').val(general.toMoney(data.CreditLimit));
                loadRight();
                if (data.LastRevisedCreditLimitDate != null) {
                    $('#dpLastRevisedCreditLimitDate').data('datetimepicker').setValue(data.LastRevisedCreditLimitDate);
                }
                $('#txtNumberOfConstruction').val(data.NumberOfConstruction);
                $('#txtEvaluatedCustomer').val(data.EvaluatedCustomer);
                $('#txtNotes').val(data.Notes);
                if (data.LastUpdatedByFk != null)
                    $('#txtLastUpdatedByFK').val(data.LastUpdatedByFkNavigation.FullName);

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
        $('#txtCustomer').prop('disabled', false);
        $('#txtCustomer').val('');
        document.getElementById('txtCustomer').setAttribute('data-addressbookid', '');
        
        $('#txtId').val('');
       
        $('#selCustomerType').val('');

        $('#txtCreditLimit').val('');
        $('#txtNumberOfConstruction').val('');
        $('#txtEvaluatedCustomer').val('');

        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        $('#dpLastRevisedCreditLimitDate').data('datetimepicker').reset();
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);
    }
}

function loadSelCustomer(status,type,callback) {
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
            if (response != null && response.length>0)
                callback(response);
            else {
                general.notify('Hết danh sách khách hàng cần tạo mới. Vui lòng thêm ở danh bạ trước!', 'error');
                return false;
            }
            return true;
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            return false;
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
        url: '/customer/GetAllPaging',
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
                    FullName: item.UserBy!=null? item.UserBy.FullName:"",
                    IDNumber: item.UserBy != null ? item.UserBy.IdNumber:'',
                    MobilePhone: item.UserBy != null ? item.UserBy.PhoneNumber:'',
                    TaxIDNumber: item.UserBy.TaxIdnumber != null ? item.UserBy.TaxIdnumber:'',
                    Email: item.UserBy != null ? item.UserBy.Email:''
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
