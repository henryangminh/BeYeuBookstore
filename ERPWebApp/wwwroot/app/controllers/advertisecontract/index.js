var gDisabledDates = [];
function DisableSpecificDates(date) {


    var currentdate = moment(date).format("DD/MM/YYYY");

    // We will now check if the date belongs to disableddates array 
    for (var i = 0; i < gDisabledDates.length; i++) {

        // Now check if the current date is in disabled dates array. 
        if ($.inArray(currentdate, gDisabledDates) != -1) {
            return false;
        }
    }
    general.stopLoad();
}


var advertiseContractController = function () {
    this.initialize = function () {
        
        loadData();
        registerEvents();
        autoUpdateContractStatus();
    }
    function registerEvents() {
        
        $('#txtFromdate').on('keydown', function (e) {
            e.preventDefault();
            if (e.keyCode >= 37 && e.keyCode <= 40 || e.keyCode == 13) {
                e.stopImmediatePropagation();
                return;
            }
         
        });

        $('#txtTodate').on('keydown', function (e) {
            e.preventDefault();
            if (e.keyCode >= 37 && e.keyCode <= 40 || e.keyCode == 13) {
                e.stopImmediatePropagation();
                return;
            }
        });
   
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selStatus').on('change', function () {
            loadData();
        });

        $('#selAdContent').on('change', function () {
            if ($('#selAdContent option:selected').val() != "") {
                loadAdContentById($('#selAdContent option:selected').val());
            }
            else {
                $('#txtAdPosition').val("");
                $('#txtAdPositionPrice').val("");
                $('#ImgAdPosition').empty();
            }
        });

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });
        
        $('#txtFromdate').on('change', function () {
            $('#txtNodate').val(countDate(moment($('#txtFromdate').val(),"DD/MM/YYYY"), moment($('#txtTodate').val(),"DD/MM/YYYY")));
            $('#txtTotalPrice').val(general.toMoney(general.toInt($('#txtAdPositionPrice').val()) * parseInt($('#txtNodate').val())));
            $('#txtMustPay').val(general.toMoney(general.toInt($('#txtTotalPrice').val()) - general.toInt($('#txtAdPositionPrice').val())));
        });
        
        $('#txtTodate').on('change', function () {
            $('#txtNodate').val(countDate(moment($('#txtFromdate').val(),"DD/MM/YYYY"), moment($('#txtTodate').val(),"DD/MM/YYYY")));
            $('#txtTotalPrice').val(general.toMoney(general.toInt($('#txtAdPositionPrice').val()) * parseInt($('#txtNodate').val())));
            $('#txtMustPay').val(general.toMoney(general.toInt($('#txtTotalPrice').val()) - general.toInt($('#txtAdPositionPrice').val())));
        });

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#addview').removeClass('hidden');
            $('#formCreate').removeClass('hidden');
            $('#formAccountingCensor').addClass('hidden');
            $('#formCensor').addClass('hidden');
            $('#TermsOfUse').removeClass('hidden');
            $('#btnSave').attr('disabled', 'disabled');
            $('#watchview').addClass('hidden');
            $('#txtCensorStatus').html('<span class="badge bg-black" style="font-size:15px;">Chưa kiểm duyệt</span>');
            $.ajax({
                type: 'GET',
                url: '/AdvertiseContract/GetAdvertiserInfo',
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    $('#txtAdvertiser').val(response.BrandName);
                    $('#txtAdvertiserId').val(response.KeyId);
                    $('#txtAdvertiserStatus').val(response.Status);
                    loadAllAdContent();
                    $('#modal-add-edit').modal('show');
                    
                    general.stopLoad();

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoad();

                },
            });
        });

        $('#btnFail').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            var noteAdContract = $('#txtNote').val();
            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.contractStatus.Unqualified,
                    note: noteAdContract,
                },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Kiểm duyệt thành công!', 'success');
                    resetForm();
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');

                },
            });
        });

        $('#btnSuccess').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            var noteAdContract = $('#txtNote').val();

            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.contractStatus.Success,
                    note: noteAdContract,

                },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Kiểm duyệt thành công!', 'success');
                    resetForm();
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoad();

                },
            });
        });

        $('#btnAccountingCensored').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            var noteAdContract = $('#txtNote').val();

            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.contractStatus.AccountingCensored,
                    note: noteAdContract,

                },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Kiểm duyệt thành công!', 'success');
                    resetForm();
                    loadData();
                    general.stopLoad();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoad();

                },
            });
        });

        $('#txtKeyword').on('keyup', function (e) {
            if (e.keyCode === 13) {
                loadData();
            }
        });

        $('#btnCancel').on('click', function () {

            $('#frmMaintainance').trigger('reset');
        });
        //Reset Form

        //Edit

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#addview').addClass('hidden');
            $('#watchview').removeClass('hidden');
            $('#formCreate').addClass('hidden');
            $('#UploadFile').addClass('hidden');
            $('#AdImg').removeClass('hidden');
            var that = $(this).data('id');
            loadDetail(that);

            //document.getElementById("btnSave").style.display = "block";
        });


        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

                selAdContent:
                {
                    required: true
                },
                txtFromdate:
                {
                    required: true
                },
                txtTodate:
                {
                    required: true
                },


            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#txtAdvertiserStatus').val() == general.status.InActive) {
                general.notify('Bạn đã bị Khóa, vui lòng liên hệ Webmaster để biết thêm chi tiết!', 'error');
                return false;
            }
            else {
                if ($('#frmMaintainance').valid()) {
                    e.preventDefault();
                    var keyId;
                    if ($('#txtId').val() == "") {
                        keyId = 0;
                    }
                    else {
                        keyId = parseInt($('#txtId').val());
                    }
                    var adContentId = $('#selAdContent option:selected').val();
                    var dateStart = moment($('#txtFromdate').val(),"DD/MM/YYYY").format("YYYY-MM-DD");
                    var dateFinish = moment($('#txtTodate').val()+'23:59:59', "DD/MM/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss");
                    var noDate = $('#txtNodate').val();
                    var contractValue = general.toFloat($('#txtTotalPrice').val());
                    var note = $('#txtNote').val();
                    var contractStatus = general.contractStatus.Requesting;

                    if (moment(dateStart, "YYYY-MM-DD").diff(moment()) < 3) {
                        general.notify('Ngày bắt đầu phải lớn hơn ngày hôm nay ít nhất hai ngày !', 'error');
                        return false;
                    }

                    if (parseInt(noDate)<1) {
                        general.notify('Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu !', 'error');
                        return false;
                    }
                    var flag;
                    var _curday = moment(dateStart, "YYYY-MM-DD").format("DD/MM/YYYY");
                    for (var i = 0; i < parseInt(noDate); i++)
                    {
                        if ($.inArray(_curday, gDisabledDates) != -1) {
                            flag = false;
                        }
                        _curday = (moment(_curday,"DD/MM/YYYY").add(1, 'days')).format("DD/MM/YYYY");
                    }

                    if (flag==false) {
                        general.notify('Chuỗi ngày bạn chọn có những ngày không còn trống !', 'error');
                        return false;
                    }

                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/SaveEntity',
                        
                        data: {
                            KeyId: keyId,
                            AdvertisementContentFK: adContentId,
                            DateStart: dateStart,
                            DateFinish: dateFinish,
                            ContractValue: contractValue,
                            Status: contractStatus,
                            Note: note,

                        },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoad();
                        },
                       
                        success: function (response) {

                            $('#modal-add-edit').modal('hide');
                            general.notify('Ghi thành công!', 'success');
                            resetForm();
                            general.stopLoad();
                            
                            loadData();
                            
                        },
                        error: function (err) {
                            general.notify('Có lỗi trong khi ghi !', 'error');
                            general.stopLoading();
                            general.stopLoad();

                        },
                    });

                    return false;
                    
                }
            }
        });
   }

   

    function loadDetail(that) {
        $('#selAdContent').empty();
        $.ajax({
            type: "GET",
            url: "/AdvertiseContract/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
               general.startLoad();
            },
            
            success: function (response) {
                $('#ImgAdPosition').empty(); 
                console.log("loaddetailAdContract", response);
                var data = response;
                //$('#formCensor').removeClass('hidden');
                $('#TermsOfUse').addClass('hidden');
                $('#txtId').val(data.KeyId);
                $('#txtAdvertiser').val(data.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName);
                $('#txtAdvertiserId').val(data.AdvertisementContentFKNavigation.AdvertiserFKNavigation.KeyId);
                $('#txtAdvertiserStatus').val(data.AdvertisementContentFKNavigation.AdvertiserFKNavigation.Status);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#selAdContent').append("<option value='" + data.AdvertisementContentFKNavigation.KeyId + "'>" + data.AdvertisementContentFKNavigation.Title + "</option>");
                $('#txtAdPosition').val(data.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation.Title);
                $('#txtAdPositionPrice').val(general.toMoney(data.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation.AdvertisePrice));
                $('#ImgAdPosition').append('<img src="' + data.AdvertisementContentFKNavigation.ImageLink + '" style="width:100%"> ');
                $('#txtFromdate').val(moment(data.DateStart).format("DD/MM/YYYY"));
                $('#txtTodate').val(moment(data.DateFinish).format("DD/MM/YYYY"));
                var _dateFinish = data.DateFinish.split("T");
                $('#txtNodate').val(countDate(data.DateStart, _dateFinish[0]));
                $('#txtTotalPrice').val(general.toMoney(data.ContractValue));
                $('#txtMustPay').val(general.toMoney(data.ContractValue-data.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation.AdvertisePrice));
                $('#txtNote').val(data.Note);
                var _color = '';
                var _status = '';
                switch (data.Status) {
                    case general.contractStatus.Requesting:
                        _color = 'black';
                        _status = 'Chưa kiểm duyệt';
                        $('#formAccountingCensor').removeClass('hidden');
                        $('#formCensor').addClass('hidden');
                        break;
                    case general.contractStatus.Success:
                        _color = 'green';
                        _status = 'Thành công';
                        $('#formCensor').addClass('hidden');
                        $('#formAccountingCensor').addClass('hidden');
                        break;
                    case general.contractStatus.Unqualified:
                        _color = 'red';
                        _status = 'Không thành công';
                        $('#formCensor').addClass('hidden');
                        $('#formAccountingCensor').addClass('hidden');
                        break;
                    case general.contractStatus.AccountingCensored:
                        _color = 'orange';
                        _status = 'Kê toán đã kiểm duyệt';
                        $('#formCensor').removeClass('hidden');
                        $('#formAccountingCensor').addClass('hidden');
                        break;
                }
                $('#txtCensorStatus').html('<span class="badge bg-' + _color + '" style="font-size:15px;">' + _status + '</span>');
                $('#modal-add-edit').modal('show');

                general.stopLoad();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoad();
            }
        });


    }
}
function loadData(isPageChanged) {
    resetForm();
    var template = $('#table-template').html();
    var render = "";
   
    $.ajax({
        type: 'GET',
        data: {

            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            status: $('#selStatus').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/AdvertiseContract/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);

            $.each(response.Results, function (i, item) {
                var _color = "";
                var _statusName = '';
                switch (item.Status) {
                    case general.contractStatus.Requesting:
                        _color = 'black';
                        _statusName = 'Chưa kiểm duyệt';
                        break;

                    case general.contractStatus.Success:
                        _color = 'green';
                        _statusName = 'Thành công';
                        break;

                    case general.contractStatus.Unqualified:
                        _color = 'red';
                        _statusName = 'Không thành công';
                        break;

                    case general.contractStatus.AccountingCensored:
                        _color = 'orange';
                        _statusName = 'Đã thanh toán';
                        break;
                }
                var _fromdate = moment(item.DateStart).format("DD/MM/YYYY HH:mm:ss");
                var _todate = moment(item.DateFinish).format("DD/MM/YYYY HH:mm:ss");
                var _contract = general.toMoney(item.ContractValue);
                var _dateCreated = moment(item.DateCreated).format("DD/MM/YYYY");
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    BrandName: item.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName,
                    AdTitle: item.AdvertisementContentFKNavigation.Title,
                    Fromdate: _fromdate,
                    Todate: _todate,
                    ContractValue: _contract,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName   + '</span>',
                    DateCreated: _dateCreated,
                });

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            general.stopLoad();
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoad():
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

function loadAdContentById(that) {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAdContentById',
        data: {id:that},
        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("AdContent", response);
            loadAllFutureSuccessContract(response.AdvertisementPositionFK);
            $('#ImgAdPosition').empty();
            $('#txtAdPosition').val(response.AdvertisementPositionFKNavigation.Title);
            $('#txtAdPositionPrice').val(general.toMoney(response.AdvertisementPositionFKNavigation.AdvertisePrice));
            $('#txtFromdate').val('');
            $('#txtTodate').val('');
            $('#txtTotalPrice').val('');
            $('#txtNodate').val('');
            $('#ImgAdPosition').append('<img src="' + response.ImageLink + '" style="width:100%"> ');
            general.stopLoad();
        },
        error: function (err) {
            general.stopLoad();
            general.notify('Có lỗi trong khi load nội dung quảng cáo !', 'error');

        },
    });

}
function loadAllFutureSuccessContract(that) {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAllFutureSuccessContract',
        dataType: "json",
        data: { id: that },
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {
            console.log("Contract", response);
            $.each(response, function (i, item) {
                var _fromdate = moment(item.DateFinish);
                var temp = moment(item.DateStart);
                while (temp <= _fromdate) {
                    gDisabledDates.push(temp.format("DD/MM/YYYY"));
                    temp = temp.add(1, 'days');
                }
            });
            $('#txtFromdate').datepicker("destroy");
            $('#txtTodate').datepicker("destroy");
            $('#txtFromdate').datepicker({
                format: "dd/mm/yyyy",
                language: "vi",
                clearBtn: true,
                todayHighlight: true,
                beforeShowDay: DisableSpecificDates
            });
            $('#txtTodate').datepicker({
                format: "dd/mm/yyyy",
                language: "vi",
                clearBtn: true,
                todayHighlight: true,
                beforeShowDay: DisableSpecificDates
            });
            general.stopLoad():
          
            
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nội dung quảng cáo !', 'error');
            general.stopLoad();
        },
    });

}
function loadAllAdContent() {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAllAdContentByAdvertiserId',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {
            $('#selAdContent').empty();
            $('#selAdContent').append("<option value=''>Chọn nội dung quảng cáo</option>");
            $.each(response, function (i, item) {
                $('#selAdContent').append("<option value='" + item.KeyId + "'>" + item.Title + "</option>");
            });
            general.stopLoad();
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nội dung quảng cáo !', 'error');
            general.stopLoad();

        },
    });

}

function countDate(dateStart, dateFinish) {
    var start = moment(dateFinish);
    var end = moment(dateStart);
    return moment.duration(start.diff(end)).asDays() + 1;
}

function autoUpdateContractStatus() {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAllRequestingNPaidContract',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },
        

        success: function (response) {
            $.each(response, function (i, item) {
                if ((moment(item.DateFinish) < moment()) && (item.Status == general.contractStatus.AccountingCensored)) {
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/UpdateStatus',
                        data: {
                            id: item.KeyId,
                            status: general.contractStatus.Success,
                            note: 'Hệ thống tự cập nhật trạng thái',

                        },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoad();
                        },

                        success: function (response) {

                            $('#modal-add-edit').modal('hide');
                            general.notify('Tự động cập nhật hợp đồng quảng cáo mã: '+item.KeyId+' thành công!', 'success');
                           
                            loadData();
                        },
                        error: function (err) {
                            general.notify('Có lỗi trong khi tự động cập nhật trạng thái hợp đồng '+item.KeyId+'!', 'error');

                        },
                    });
                }
                if ((moment(item.DateFinish) < moment()) && (item.Status == general.contractStatus.Requesting)) {
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/UpdateStatus',
                        data: {
                            id: item.KeyId,
                            status: general.contractStatus.Unqualified,
                            note: 'Hệ thống tự cập nhật trạng thái không thành công do không thanh toán đúng hạn!',

                        },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoad();
                        },

                        success: function (response) {

                            $('#modal-add-edit').modal('hide');
                            general.notify('Tự động cập nhật hợp đồng quảng cáo mã: ' + item.KeyId + 'thành công!', 'success');
                      
                            loadData();
                            general.stopLoad();
                        },
                        error: function (err) {
                            general.notify('Có lỗi trong khi tự động cập nhật trạng thái hợp đồng ' + item.KeyId + '!', 'error');
                            general.stopLoad();

                        },
                    });
                }
            });
            
        },
        error: function (err) {
            general.notify('Có lỗi trong khi tự động cập nhật trạng thái hợp đồng !', 'error');
            general.stopLoad();

        },
    });

}
function resetForm() {
    $('#frmMaintainance').trigger('reset');
    $('#ImgAdPosition').empty();
    $('#txtNodate').val('');
    $('#txtMustPay').val('');
    $('#txtNote').val('');
    gDisabledDates.length = 0;

    $('#txtFromdate').datepicker("destroy");
    $('#txtTodate').datepicker("destroy");
    $('#txtFromdate').datepicker({
        format: "dd/mm/yyyy",
        language: "vi",
        clearBtn: true,
        todayHighlight: true,
        beforeShowDay: DisableSpecificDates
    });
    $('#txtTodate').datepicker({
        format: "dd/mm/yyyy",
        language: "vi",
        clearBtn: true,
        todayHighlight: true,
        beforeShowDay: DisableSpecificDates
    });
}
