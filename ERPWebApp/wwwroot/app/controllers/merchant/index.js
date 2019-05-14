var merchantController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        //loadScale();

        //loadStatus();
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selStatus').on('change', function () {
            loadData();
        });

        $('#selScale').on('change', function () {
            loadData();
        });

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });

        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#frmMaintainance').trigger('reset');
            $('#modal-add-edit').modal('show');
   
        });

        $('#txtKeyword').on('keyup change', function (e) {
           
            loadData();
        
        });

        $('#btnCancel').on('click', function () {

            $('#frmMaintainance').trigger('reset');
        });
        //Reset Form

        //Edit

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetail(that);

            
        });



        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtAddress:
                {
                    required: true
                },
                txtBussinessRegisterId:
                {
                    required: true,
                    number: true,
                },
                txtTaxId: {

                    required: true,
                    number: true,
                },
                txtWebsite: {
                    required: true
                },
                selStatus: {
                    required: true
                },
                selScale: {
                    required: true
                },
                txtDirectContactName: {
                    required: true
                },
                txtContactAddress: {
                    required: true
                },
                txtHotline: {

                    required: true,
                    number: true,
                },
            }
        });
        //Save 
    
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var keyId;
                if ($('#txtId').val() == "") {
                    keyId = 0;
                }
                else {
                    keyId = parseInt($('#txtId').val());
                }
                
                var directContactName = $('#txtDirectContactName').val();
                var hotline = $('#txtHotline').val();
                var contactAddress = $('#txtContactAddress').val();
                //var merchantCompanyName =;
                //var address = ;
                //var bussinessRegisterId = ;
                //var taxId = ;
                //var website = ;
                //var legalRepresentative = ;
                //var merchantBankingName = ;
                //var bank = ;
                //var bankBranch = ;
                //var status = ;
                //var scale = ;
                //var establishDate = ;
                $.ajax({
                    type: 'POST',
                    url: '/Merchant/SaveEntity',
                    data: {
                        KeyId: keyId,
                        TimeKeepingTypeFK: timeKeepingTypeFK,
                        FromDate: fromDate,
                        ToDate: toDate,
                        NODate: nODate,
                        PercentSalary: percentSalary,
                        Reason: reason,
                        Note: note,
                        StatusFK: statusFK,
                        HandoverToFK: handOverTo,
                        CommitBackToWork: commitBackToWork,
                        RequestToFK: requestTo,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        general.stopLoad();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoad();

                    },
                });
                return false;
            }
        });




    }

    function resetForm() {
        $('#txtId').val('');
        $('#txtEmployeeKeyId').val('')
        $('#txtFromdate').val('');
        $('#txtTodate').val('');
        $('#txtNOdate').val('');
        $('#txtPercentSalary').val('');
        $('#txtReason').val('');
        $('#txtNote').val('');
        $('#dtDateModified').val('');
        $('#chkCommitBackToWork').prop('checked', false);
        $('#txtStatus').empty();
        

    }

    function loadDetail(that) {

    
        $.ajax({
            type: "GET",
            url: "/Merchant/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log("loaddetail", response);
                $('#txtId').val(response.KeyId);
                $('#txtMerchantCompanyName').val(response.MerchantCompanyName);
                $('#dtDateModified').val(response.DateModified);
                $('#dtDateCreated').val(response.DateModified);
                $('#txtAddress').val(response.Address);
                $('#txtBussinessRegisterId').val(response.BussinessRegisterId);
                $('#txtTaxId').val(response.TaxId);
                $('#txtWebsite').val(response.Website);
                $('#selDetailStatus').val(response.Status+'');
                $('#selDetailScale').val(response.Scales+'');
                $('#txtDirectContactName').val(response.DirectContactName);
                $('#txtHotline').val(response.Hotline);
                $('#txtContactAddress').val(response.ContactAddress);
            
                $('#modal-add-edit').modal('show');
                general.stopLoad();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra khi load chi tiết', 'error');
                general.stopLoad();
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
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            status: $('#selStatus').val(),
            scale: $('#selScale').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Merchant/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);
            var order = 1;
            $.each(response.Results, function (i, item) {
                var _color = '';
                var _statusName = '';
                switch (item.Status) {
                    case general.status.Active:
                        _color = 'green';
                        _statusName = 'Kích hoạt';
                        break;
                    case general.status.InActive:
                        _color = 'red'
                        _statusName = 'Khóa';
                        break;
                }
                var _scale = '';
                switch (item.Scales) {
                    case general.merchantScale.Large:
                        _scale = 'Lớn';
                        break;
                    case general.merchantScale.Medium:
                        _scale = 'Vừa';
                        break;
                    case general.merchantScale.Small:
                        _scale = 'Nhỏ';
                        break;
                }
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    MerchantCompanyName: item.MerchantCompanyName,
                    Scale: _scale,
                    Hotline: item.Hotline,
                    Website: item.Website,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
                    DirectContactName: item.DirectContactName,
             
                });
                order++;

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
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

function loadStatus() {
    $.ajax({
        type: 'GET',
        url: '/Merchant/GetAllStatus',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {
            console.log("Status", response);
            $.each(response, function (i, item) {
                $('#selStatus').append("<option value='" + item.KeyId + "'>" + item.Status + "</option>");


            });
            general.stopLoad();
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load trạng thái !', 'error');
            general.stopLoad();

        },
    });

}

function loadScale() {
    $.ajax({
        type: 'GET',
        url: '/Merchant/GetAllScale',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {

            console.log("Scale", response);
            $.each(response, function (i, item) {
                $('#selStatus').append("<option value='" + item.KeyId + "'>" + item.Status + "</option>");


            });
            general.stopLoad();
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load quy mô !', 'error');
            general.stopLoad();

        },
    });

}

