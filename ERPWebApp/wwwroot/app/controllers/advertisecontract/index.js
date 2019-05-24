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

        loadAdPosition();
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


        $('#selAdPosition').on('change', function () {
            gDisabledDates.length = 0;
            $.ajax({
                type: 'GET',
                url: '/AdvertiseContract/GetAdPositionById',
                data: {
                    id: $('#selAdPosition option:selected').val(),
                },
                dataType: "json",

                success: function (response) {
                    $('#txtAdPositionPrice').val(general.toMoney(response.AdvertisePrice));
                    $('#txtTodate').val('');
                    $('#txtFromdate').val('');
                    $('#txtNodate').val('');
                    $('#txtTotalPrice').val('');
                    $('#txtMustPay').val('');
                    loadAllFutureSuccessContract($('#selAdPosition option:selected').val());
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load giá vị trí quảng cáo !', 'error');

                },
            });
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
            $('#txtMustPay').val(general.toMoney(general.toInt($('#txtTotalPrice').val())*50/100));
        });
        
        $('#txtTodate').on('change', function () {
            $('#txtNodate').val(countDate(moment($('#txtFromdate').val(),"DD/MM/YYYY"), moment($('#txtTodate').val(),"DD/MM/YYYY")));
            $('#txtTotalPrice').val(general.toMoney(general.toInt($('#txtAdPositionPrice').val()) * parseInt($('#txtNodate').val())));
            $('#txtMustPay').val(general.toMoney(general.toInt($('#txtTotalPrice').val()) * 50 / 100));
        });

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#newImg').addClass('hidden');
            $('#btnSaveAdContent').addClass('hidden');
            $('#btnSave').removeClass('hidden');
            $('#selAdPosition').removeAttr('disabled', 'disabled');
            $('#txtFromdate').removeAttr('disabled', 'disabled');
            $('#txtTodate').removeAttr('disabled', 'disabled');
            $('#txtNote').removeAttr('readonly', 'readonly');
            $('.star').removeClass('hidden');
            $('#addview').removeClass('hidden');
            $('#formCreate').removeClass('hidden');
            $('#formAccountingCensor').addClass('hidden');
            $('#formCensor').addClass('hidden');
            $('#TermsOfUse').removeClass('hidden');
            $('#btnSave').attr('disabled', 'disabled');
            $('#UploadFile').removeClass('hidden');
            $('#watchview').addClass('hidden');
            $('#txtCensorStatus').html('<span class="badge bg-black" style="font-size:15px;">Chưa kiểm duyệt</span>');
            $('#txtAdContentStatus').html('<span class="badge bg-black" style="font-size:15px;">Chưa kiểm duyệt</span>');
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
                    $('#modal-add-edit').modal('show');
                    
                    general.stopLoad();

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoad();

                },
            });
        });

        $('#btnAccountingCensoredDeposite').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            var noteAdContract = $('#txtNote').val();
            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.contractStatus.DepositePaid,
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

        $('#btnSaveAdContent').on('click', function (e) {
            e.preventDefault();
            if ($('#frmMaintainance').valid()) {
                var keyId = parseInt($('#txtAdContentKeyId').val());
                var title = $('#txtTitle').val();
                var _link = $('#txtLink').val().substr(0, 4);
                var urlToBrand = "";
                if (_link.toUpperCase == 'HTTP:') {
                    urlToBrand = $('#txtLink').val();
                }
                else {
                    urlToBrand = 'http://' + $('#txtLink').val();
                }
                var description = $('#txtDescription').val();
                var noteAdContract = $('#txtAdContentNote').val();

                var data = new FormData();

                fileUpload = $('#fileAdImg').get(0);
                files = fileUpload.files;
                data.append("files", files[0]);
                if ($('#fileAdImg').val() != "") {
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/ImportFiles',
                        data: data,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                            general.startLoad();
                        },
                        success: function (e) {
                            if ($('#fileAdImg').val() != '') {

                                linkImg = e[0];

                                e.shift();

                            }
                            $.ajax({
                                type: 'POST',
                                url: '/AdvertiseContract/UpdateUnqualifiedAdContent',
                                data: {
                                    KeyId: keyId,
                                    Title: title,
                                    ImageLink: linkImg,
                                    UrlToAdvertisement: urlToBrand,
                                    Description: description,
                                    Note: noteAdContract,

                                },
                                dataType: "json",
                                beforeSend: function () {
                                    general.startLoad();
                                },

                                success: function (response) {

                                    $('#modal-add-edit').modal('hide');
                                    general.notify('Cập nhật nội dung quảng cáo thành công, bạn vui lòng chờ duyệt nhé!', 'success');
                                    resetForm();
                                    loadData();
                                },
                                error: function (err) {
                                    general.notify('Có lỗi trong khi ghi !', 'error');
                                    general.stopLoad();

                                },
                            });
                        },

                        error: function (err) {
                            general.notify('Có lỗi trong khi ghi !', 'error');
                            general.stopLoad();

                        },
                    });

                }
                else {

                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/UpdateUnqualifiedAdContent',
                        data: {
                            KeyId: keyId,
                            Title: title,
                            UrlToAdvertisement: urlToBrand,
                            Description: description,
                            Note: noteAdContract,

                        },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoad();
                        },

                        success: function (response) {

                            $('#modal-add-edit').modal('hide');
                            general.notify('Cập nhật nội dung quảng cáo thành công, bạn vui lòng chờ duyệt nhé!', 'success');
                            resetForm();
                            loadData();
                        },
                        error: function (err) {
                            general.notify('Có lỗi trong khi ghi !', 'error');
                            general.stopLoad();

                        },
                    });
                }
            }
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
            $('.star').addClass('hidden');
            $('#AdImg').removeClass('hidden');

            $('#selAdPosition').attr('disabled', 'disabled');
            $('#txtFromdate').attr('disabled', 'disabled');
            $('#txtTodate').attr('disabled', 'disabled');

            $('#oldImg').removeClass('hidden');
            $('#newImg').addClass('hidden');
            var that = $(this).data('id');
            loadDetail(that);

            //document.getElementById("btnSave").style.display = "block";
        });
        
        $('body').on('click', '.btn-editContent', function (e) {
            e.preventDefault();
            $('#UploadFile').removeClass('hidden');
            $('#newImg').removeClass('hidden');
            $('#oldImg').addClass('hidden');
            $('#selAdPosition').attr('disabled', 'disabled');
            $('#txtFromdate').attr('disabled', 'disabled');
            $('#txtTodate').attr('disabled', 'disabled');
            $('#txtNote').attr('readonly', 'readonly');
            $('#addview').addClass('hidden');
            $('#formConfirm').addClass('hidden');
            $('#formCreate').removeClass('hidden');
            $('#watchview').removeClass('hidden');
            $('#btnSave').addClass('hidden');
            $('#btnSaveAdContent').removeClass('hidden');
        
           
            $('.star').removeClass('hidden');
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
                txtFromdate:
                {
                    required: true
                },
                txtTodate:
                {
                    required: true
                },
                txtTitle:
                {
                    required: true
                },
                txtLink:
                {
                    required: true
                },
                selAdPosition:
                {
                    required: true
                },
              


            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#txtAdvertiserStatus').val() == general.status.InActive) {
                general.notify('Bạn đã bị Khóa hoặc bạn không có quyền thêm mới, vui lòng liên hệ Webmaster để biết thêm chi tiết!', 'error');
                return false;
            }
            else {
                if ($('#frmMaintainance').valid()) {
                    e.preventDefault();
                    var adContract = new Object();
                    
                    if ($('#txtId').val() == "") {
                        adContract.KeyId = 0;
                    }
                    else {
                        adContract.KeyId = parseInt($('#txtId').val());
                    }
                    adContract.DateStart = moment($('#txtFromdate').val(), "DD/MM/YYYY").format("YYYY-MM-DD");
                    adContract.DateFinish = moment($('#txtTodate').val() + '23:59:59', "DD/MM/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss");
                    var noDate = $('#txtNodate').val();
                    adContract.ContractValue = general.toFloat($('#txtTotalPrice').val());
                    adContract.Note = $('#txtNote').val();
                    adContract.ContractStatus = general.contractStatus.Requesting;
                    adContract.Deposite = general.toFloat($('#txtMustPay').val());

                    if (countDate(moment(adContract.DateStart).format("YYYY-MM-DD"), moment().format("YYYY-MM-DD")) > -1) {
                        general.notify('Ngày bắt đầu phải lớn hơn ngày hôm nay ít nhất hai ngày !', 'error');
                        return false;
                    }

                    if (parseInt(noDate) < 1) {
                        general.notify('Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu !', 'error');
                        return false;
                    }
                    
                    var flag;
                    var _curday = moment(adContract.DateStart, "YYYY-MM-DD").format("DD/MM/YYYY");
                    for (var i = 0; i < parseInt(noDate); i++) {
                        if ($.inArray(_curday, gDisabledDates) != -1) {
                            flag = false;
                        }
                        _curday = (moment(_curday, "DD/MM/YYYY").add(1, 'days')).format("DD/MM/YYYY");
                    }

                    if (flag == false) {
                        general.notify('Chuỗi ngày bạn chọn có những ngày không còn trống !', 'error');
                        return false;
                    }
                    var data = new FormData();

                    fileUpload = $('#fileAdImg').get(0);
                    files = fileUpload.files;
                    data.append("files", files[0]);
                    var filename = $('#fileAdImg').val().split('\\').pop();
                    var extension = filename.substr((filename.lastIndexOf('.') + 1));
                    if (extension.toUpperCase() != "JPG" && extension.toUpperCase() != "PNG") {
                        general.notify('File ảnh phải ở định dạng JPG hoặc PNG !', 'error');
                        return false;
                    }
                    if ($('#fileAdImg')[0].files[0].size > general.maxSizeAllowed.AdContentImg) {
                        general.notify('Kích thước ảnh phải nhỏ hơn 3Mb !', 'error');
                        return false;
                    }
                    var adContent = new Object();
                    if ($('#txtAdContentKeyId').val() == "")
                    {
                        adContent.KeyId = 0;
                    }
                    else
                    {
                        parseInt(adContent.KeyId = $('#txtAdContentKeyId').val());
                    }
                    adContent.AdvertisementPositionFK = $('#selAdPosition option:selected').val();
                    adContent.AdvertiserFK = $('#txtAdvertiserId').val();
                    adContent.Title = $('#txtTitle').val();
                    adContent.Deposite = general.toFloat($('#txtMustPay').val());
                    adContent.CensorStatus = general.censorStatus.Uncensored;
                 
                    var _link = $('#txtLink').val().substr(0,4);
                    if (_link.toUpperCase == 'HTTP:') {
                        adContent.UrlToAdvertisement = $('#txtLink').val();
                    }
                    else {
                        adContent.UrlToAdvertisement = 'http://' + $('#txtLink').val();
                    }
                    adContent.Description = $('#txtDescription').val();
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/ImportFiles',
                        data: data,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                            general.startLoad();
                        },
                        success: function (e) {
                            if ($('#fileAdImg').val() != '') {

                                linkImg = e[0];

                                e.shift();

                            }
                            adContent.ImageLink = linkImg;
                            $.ajax({
                                type: 'POST',
                                url: '/AdvertiseContract/SaveEntity',

                                data: {
                                    adContentVm: adContent,
                                    advertiseContractVm: adContract,

                                },
                                dataType: "json",
                                beforeSend: function () {
                                    general.startLoad();
                                },

                                success: function (response) {

                                    $('#modal-add-edit').modal('hide');
                                    general.notify('Ghi thành công!', 'success');

                                    $('#modal-add-success').modal('show');
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
                        },
                        error: function (e) { },
                    });
                    

                    return false;
                    
                }
            }
        });
   }

   

    function loadDetail(that) {
        $('#selAdContent').empty();
        $('#AdImg').empty();
       


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
                $('#txtNodate').val(countDate(moment(data.DateStart).format("YYYY-MM-DD"), moment(data.DateFinish).format("YYYY-MM-DD")));
                $('#txtTotalPrice').val(general.toMoney(data.ContractValue));
                $('#txtMustPay').val(general.toMoney(data.Deposite));
                $('#txtNote').val(data.Note);
               
                
                $.ajax({
                    type: "GET",
                    url: "/AdvertiseContract/GetAdContentById",
                    data: { id: data.AdvertisementContentFK },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },

                    success: function (_response) {
                        var _data = _response;
                        $('#txtTitle').val(_data.Title);
                        $('#txtAdContentKeyId').val(_data.KeyId);
                        $('#txtLink').val(_data.UrlToAdvertisement);
                        $('#txtDescription').val(_data.Description);
                        $('#txtAdContentNote').val(_data.Note);
                        var __color = "";
                        var __status = "";
                        switch (_data.CensorStatus) {
                            case general.censorStatus.Uncensored:
                                __color = 'black';
                                __status = 'Chưa kiểm duyệt';
                                $('#formAccountingCensor').addClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;
                            case general.censorStatus.ContentCensored:
                                __color = 'green';
                                __status = 'Đã kiểm duyệt nội dung';
                                $('#formAccountingCensor').removeClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;
                            case general.censorStatus.Unqualified:
                                __color = 'red';
                                __status = 'Nội dung không phù hợp';

                                $('#formAccountingCensor').addClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;


                        }
                        var _color = '';
                        var _status = '';
                        switch (data.Status) {
                            case general.contractStatus.Requesting:
                                _color = 'black';
                                _status = 'Chưa kiểm duyệt';
                                $('#formAccountingCensorDeposite').removeClass('hidden');
                                $('#formAccountingCensor').addClass('hidden');
                                $('#formCensor').addClass('hidden');
                                break;
                            case general.contractStatus.Success:
                                _color = 'green';
                                _status = 'Thành công';
                                $('#formCensor').addClass('hidden');
                                $('#formAccountingCensor').addClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;
                            case general.contractStatus.DepositePaid:
                                _color = 'purple';
                                _status = 'Đã đóng cọc';
                                $('#formCensor').addClass('hidden');
                                $('#formAccountingCensor').addClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;
                            case general.contractStatus.Unqualified:
                                _color = 'red';
                                _status = 'Không thành công';
                                $('#formCensor').addClass('hidden');
                                $('#formAccountingCensor').addClass('hidden');
                                $('#formAccountingCensorDeposite').addClass('hidden');
                                break;
                            case general.contractStatus.AccountingCensored:
                                _color = 'orange';
                                _status = 'Kê toán đã kiểm duyệt';
                                $('#formCensor').removeClass('hidden');
                                $('#formAccountingCensor').addClass('hidden');
                                break;
                        }
                        $('#txtAdContentStatus').html('<span class="badge bg-' + __color + '" style="font-size:15px;">' + __status + '</span>');
                        $('#txtCensorStatus').html('<span class="badge bg-' + _color + '" style="font-size:15px;">' + _status + '</span>');
                        if (data.Status == general.contractStatus.DepositePaid && _data.CensorStatus == general.censorStatus.ContentCensored) {
                            $('#formAccountingCensor').removeClass('hidden');
                        }
                        $('#AdImg').append('<img src="' + _data.ImageLink + '" style="width:100%">');
                        general.stopLoad();
                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra khi load nội dung quảng cáo', 'error');
                        general.stopLoad();
                    }
                });
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

                    case general.contractStatus.DepositePaid:
                        _color = 'purple';
                        _statusName = 'Đã đóng cọc';
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
                var _prop = '';
                if (item.AdvertisementContentFKNavigation.CensorStatus == general.censorStatus.Unqualified && item.Status == general.contractStatus.DepositePaid) {

                }
                else {
                    _prop = 'disabled';
                }
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    BrandName: item.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName,
                    AdTitle: item.AdvertisementContentFKNavigation.Title,
                    Fromdate: _fromdate,
                    Todate: _todate,
                    ContractValue: _contract,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
                    DateCreated: _dateCreated,
                    Prop: _prop,
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
            general.startLoad();
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
            general.stopLoad();
          
            
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
                if ((moment(item.DateFinish) < moment()) && (item.Status == general.contractStatus.DepositePaid) && (item.AdvertisementContentFKNavigation.CensorStatus == general.censorStatus.Unqualified)) {
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/UpdateStatus',
                        data: {
                            id: item.KeyId,
                            status: general.contractStatus.Unqualified,
                            note: 'Tự cập nhật trạng thái "không thành công" do không chỉnh sửa nội dung cho phù hợp!',

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
                if ((moment(item.DateFinish) < moment()) && (item.Status == general.contractStatus.DepositePaid) && (item.AdvertisementContentFKNavigation.CensorStatus == general.censorStatus.ContentCensored)) {
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
                if ((moment(item.DateFinish) < moment()) && (item.Status == general.contractStatus.Requesting)) {
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertiseContract/UpdateStatus',
                        data: {
                            id: item.KeyId,
                            status: general.contractStatus.Unqualified,
                            note: 'Hệ thống tự cập nhật trạng thái không thành công do không thanh toán tiền cọc!',

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
    $('#txtTitle').val('');
    $('#txtLink').val('');
    $('#txtDescription').val('');
    $('#txtAdContentNote').val('');
    $('#selAdPosition').val('');
    $('#txtAdPositionPrice').val('');
    $('#txtAdContentStatus').empty();
    $('#AdImg').empty();
    $('#txtAdContentKeyId').val('');


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

function loadAdPosition() {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAllAdPosition',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },


        success: function (response) {

            $.each(response, function (i, item) {
                $('#selAdPosition').append("<option value='" + item.KeyId + "'>" + item.Title + "</option>");
                general.stopLoad();

            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load vị trí quảng cáo !', 'error');
            general.stopLoad();
        },
    });

}