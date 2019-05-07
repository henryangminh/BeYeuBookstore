var advertisementContractController = function () {
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
            $('#txtNodate').val(countDate($('#txtFromdate').val(), $('#txtTodate').val()));
            $('#txtTotalPrice').val(general.toMoney(general.toInt($('#txtAdPositionPrice').val()) * parseInt($('#txtNodate').val())));
        });
        
        $('#txtTodate').on('change', function () {
            $('#txtNodate').val(countDate($('#txtFromdate').val(), $('#txtTodate').val()));
            $('#txtTotalPrice').val(general.toMoney(general.toInt($('#txtAdPositionPrice').val()) * parseInt($('#txtNodate').val())));
        });

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#txtCensorStatus').html('<span class="badge bg-black" style="font-size:15px;">Chưa kiểm duyệt</span>');
            $.ajax({
                type: 'GET',
                url: '/AdvertiseContract/GetAdvertiserInfo',
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    $('#txtAdvertiser').val(response.BrandName);
                    $('#txtAdvertiserId').val(response.KeyId);
                    $('#txtAdvertiserStatus').val(response.Status);
                    loadAllAdContent();
                    $('#modal-add-edit').modal('show');
                    
                    general.stopLoading();

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoading();

                },
            });
        });

        $('#btnUnqualified').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            var noteAdContent = $('#txtNote').val();
            $.ajax({
                type: 'POST',
                url: '/AdvertisementContent/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.Unqualified,
                    note: noteAdContent,
                },
                dataType: "json",

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

        $('#btnCensored').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.ContentCensored,
                },
                dataType: "json",

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

        $('#btnAccountingCensored').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AdvertiseContract/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.AccountingCensored,
                },
                dataType: "json",

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

                selAdPosition:
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

                fileAdImg:
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
                    var dateFinish = moment($('#txtTodate').val(), "DD/MM/YYYY").format("YYYY-MM-DD");
                    var noDate = countDate(dateStart, dateFinish);
                    var contractValue = general.toFloat($('#txtTotalPrice').val());
                    var note = $('#txtNote').val();
                    var contractStatus = general.contractStatus.Requesting;

                    if (extension.toUpperCase() != "JPG" && extension.toUpperCase() != "PNG") {
                        general.notify('File ảnh phải ở định dạng JPG hoặc PNG !', 'error');
                        return false;
                    }
                    if ($('#fileAdImg')[0].files[0].size > general.maxSizeAllowed.BookImg) {
                        general.notify('Kích thước ảnh phải nhỏ hơn 2Mb !', 'error');
                        return false;
                    }
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertisementContent/ImportFiles',
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (e) {
                            console.log(e);
                            if ($('#fileAdImg').val() != '') {

                                linkImg = e[0];

                                e.shift();

                            }
                            $.ajax({
                                type: 'POST',
                                url: '/AdvertisementContent/SaveEntity',
                                data: {
                                    KeyId: keyId,
                                    AdvertisementPositionFK: adPosId,
                                    AdvertiserFK: advertiserId,
                                    ImageLink: linkImg,
                                    Title: title,
                                    Description: description,
                                    UrlToAdvertisement: link,
                                    Deposite: deposits,
                                    CensorStatus: censorStatus,
                                    Note: note,

                                },
                                dataType: "json",
                                beforeSend: function () {
                                    general.startLoading();
                                },
                                success: function (response) {

                                    $('#modal-add-edit').modal('hide');
                                    general.notify('Ghi thành công!', 'success');
                                    resetForm();
                                    general.stopLoading();
                                    loadData();
                                },
                                error: function (err) {
                                    general.notify('Có lỗi trong khi ghi !', 'error');
                                    general.stopLoading();

                                },
                            });

                            return false;

                        },
                        error: function (e) {
                            general.notify('Có lỗi trong khi ghi !', 'error');
                            console.log(e);
                        }

                    });

                }
            }
        });




    }

    function resetForm() {
        $('#frmMaintainance').trigger('reset');
    }

    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/AdvertiseContract/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                $('#AdImg').empty();
                console.log("loaddetailbook", response);
                var data = response;
                $('#formCensor').removeClass('hidden');
                $('#txtId').val(data.KeyId);
                $('#txtAdvertiser').val(data.AdvertiserFKNavigation.BrandName);
                $('#txtAdvertiserStatus').val(data.AdvertiserFKNavigation.Status);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#txtWidth').val(data.AdvertisementPositionFKNavigation.Width);
                $('#txtHeight').val(data.AdvertisementPositionFKNavigation.Height);
                $('#txtAdPositionPrice').val(general.toMoney(data.AdvertisementPositionFKNavigation.AdvertisePrice));
                $('#txtDeposits').val(general.toMoney(data.AdvertisementPositionFKNavigation.AdvertisePrice));
                $('#selAdPosition').val(data.AdvertisementPositionFK);
                $('#txtTitle').val(data.Title);
                $('#txtLink').val(data.UrlToAdvertisement);
                $('#txtDescription').val(data.Description);
                $('#txtNote').val(data.Note);
                var _color = '';
                var _status = '';
                switch (data.CensorStatus) {
                    case general.censorStatus.Uncensored:
                        _color = 'black';
                        _status = 'Chưa kiểm duyệt';
                        $('#formCensor').addClass('hidden');
                        break;
                    case general.censorStatus.AccountingCensored:
                        _color = 'orange';
                        _status = 'Kế toán đã kiểm duyệt';
                        $('#formAccountingCensor').addClass('hidden');
                        $('#formCensor').removeClass('hidden');
                        break;
                    case general.censorStatus.ContentCensored:
                        _color = 'green';
                        _status = 'Đã kiểm duyệt nội dung';
                        $('#formCensor').addClass('hidden');
                        $('#formAccountingCensor').addClass('hidden');
                        break;
                    case general.censorStatus.Unqualified:
                        _color = 'red';
                        _status = 'Không đủ tiêu chuẩn';
                        $('#formCensor').addClass('hidden');
                        $('#formAccountingCensor').addClass('hidden');
                        break;
                }
                $('#txtCensorStatus').html('<span class="badge bg-' + _color + '" style="font-size:15px;">' + _status + '</span>');
                $('#AdImg').append('<img src="' + data.ImageLink + '">');
                $('#chkPaidDeposits').prop('checked', true);
                $('#modal-add-edit').modal('show');

                general.stopLoading();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
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
            status: $('#selStatus').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/AdvertiseContract/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log("data", response);

            $.each(response.Results, function (i, item) {
                var _color = "";
                var _statusName = '';
                switch (item.CensorStatus) {
                    case general.contractStatus.Requesting:
                        _color = 'black';
                        _statusName = 'Chưa kiểm duyệt';
                        break;
                    
                    case general.contractStatus.Success:
                        _color = 'green';
                        _statusName = 'Đã kiểm duyệt nội dung';
                        break;

                    case general.contractStatus.Unqualified:
                        _color = 'red';
                        _statusName = 'Không đủ tiêu chuẩn';
                        break;
                }
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                   

                });

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
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

        success: function (response) {

            $('#ImgAdPosition').empty();
            $('#txtAdPosition').val(response.AdvertisementPositionFKNavigation.Title);
            $('#txtAdPositionPrice').val(general.toMoney(response.AdvertisementPositionFKNavigation.AdvertisePrice));
            $('#ImgAdPosition').append('<img src="' + response.ImageLink + '" style="width:100%"> ');

        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nội dung quảng cáo !', 'error');

        },
    });

}
function loadAllAdContent() {
    $.ajax({
        type: 'GET',
        url: '/AdvertiseContract/GetAllAdContentByAdvertiserId',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selAdContent').append("<option value='" + item.KeyId + "'>" + item.Title + "</option>");
            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nội dung quảng cáo !', 'error');

        },
    });

}

function countDate(dateStart, dateFinish) {
    var start = moment(dateFinish, "DD/MM/YYYY");
    var end = moment(dateStart, "DD/MM/YYYY");
    return moment.duration(start.diff(end)).asDays() + 1;
}

