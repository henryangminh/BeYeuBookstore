var advertisementContentController = function () {
    this.initialize = function () {
        loadData();
        loadAdPosition();
        loadAllAdvertiser();
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
        $('#selAdvertiser').on('change', function () {
            loadData();
        });

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });

        $('#selAdPosition').on('change', function () {
            $.ajax({
                type: 'GET',
                url: '/AdvertisementContent/GetAdPositionById',
                data: {
                    id: $('#selAdPosition option:selected').val(),
                },
                dataType: "json",

                success: function (response) {
                    $('#txtAdPositionPrice').val(general.toMoney(response.AdvertisePrice));
                    $('#txtDeposits').val(general.toMoney(response.AdvertisePrice));
                    $('#txtHeight').val(response.Height);
                    $('#txtWidth').val(response.Width);
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load giá vị trí quảng cáo !', 'error');

                },
            });
        });

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#formCreate').removeClass('hidden');
            $('#UploadFile').removeClass('hidden');
            $('#AdImg').addClass('hidden');
            $('#txtCensorStatus').html('<span class="badge bg-black" style="font-size:15px;">Chưa kiểm duyệt</span>');
            $.ajax({
                type: 'GET',
                url: '/AdvertisementContent/GetAdvertiserInfo',
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

        $('#btnCensored').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AdvertisementContent/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.ContentCensored,
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

        $('#btnAccountingCensored').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AdvertisementContent/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.AccountingCensored,
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
            else
            {
                if ($('#frmMaintainance').valid()) {
                    e.preventDefault();

                    var linkImg = '';
                    var keyId;
                    if ($('#txtId').val() == "") {
                        keyId = 0;
                    }
                    else {
                        keyId = parseInt($('#txtId').val());
                    }
                    var data = new FormData();

                    fileUpload = $('#fileAdImg').get(0);
                    files = fileUpload.files;
                    data.append("files", files[0]);

                    var adPosId = $('#selAdPosition option:selected').val();
                    var advertiserId = $('#txtAdvertiserId').val();
                    var title = $('#txtTitle').val();
                    var link = $('#txtLink').val();
                    var description = $('#txtDescription').val();
                    var note = $('#txtNote').val();
                    var censorStatus = general.censorStatus.Uncensored;
                    var deposits = general.toFloat($('#txtDeposits').val());
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
                    $.ajax({
                            type: 'POST',
                            url: '/AdvertisementContent/ImportFiles',
                            data: data,
                            contentType: false,
                            processData: false,
                            beforeSend: function () {
                            general.startLoad();
                             },
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
                                        general.startLoad();
                                    },
                                    success: function (response) {

                                        $('#modal-add-edit').modal('hide');
                                        general.notify('Ghi thành công!', 'success');
                                        resetForm();
                                        general.stopLoad();
                                        loadData();
                                        general.stopLoad();
                                    },
                                    error: function (err) {
                                        general.notify('Có lỗi trong khi ghi !', 'error');
                                        general.stopLoad();

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
            url: "/AdvertisementContent/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
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
                        $('#formCensor').removeClass('hidden');
                       
                        break;
                    case general.censorStatus.ContentCensored:
                        _color = 'green';
                        _status = 'Đã kiểm duyệt nội dung';
                        $('#formCensor').addClass('hidden');
                        
                        break;
                    case general.censorStatus.Unqualified:
                        _color = 'red';
                        _status = 'Không đủ tiêu chuẩn';
                        $('#formCensor').addClass('hidden');
                        
                        break;
                }
                $('#txtCensorStatus').html('<span class="badge bg-' + _color + '" style="font-size:15px;">' + _status + '</span>');
                $('#AdImg').append('<img src="' + data.ImageLink + '" style="width:100%">');
                $('#chkPaidDeposits').prop('checked', true);
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

    var template = $('#table-template').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {

            keyword: $('#txtKeyword').val(),
            status: $('#selStatus').val(),
            advertiserSort: $('#selAdvertiser').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/AdvertisementContent/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        
        success: function (response) {
            console.log("data", response);

            $.each(response.Results, function (i, item) {
                var _webmasterCensor = item.WebMasterCensorFKNavigation==null?"":item.WebMasterCensorFKNavigation.UserBy.FullName;
                var _color = "";
                var _statusName = '';
                switch (item.CensorStatus) {
                    case general.censorStatus.Uncensored:
                        _color = 'black';
                        _statusName = 'Chưa kiểm duyệt';
                        break;
                    case general.censorStatus.AccountingCensored:
                        _color = 'orange';
                        _statusName = 'Kế toán đã kiểm duyệt';
                        break;
                    case general.censorStatus.ContentCensored:
                        _color = 'green';
                        _statusName = 'Đã kiểm duyệt nội dung';
                        break;
                    case general.censorStatus.Unqualified:
                        _color = 'red';
                        _statusName = 'Không đủ tiêu chuẩn';
                        break;
                   }
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    Id: item.AdvertisementPositionFKNavigation.Title,
                    Advertiser: item.AdvertiserFKNavigation.BrandName,
                    Title: item.Title,
                    Img: '<img src="' + item.ImageLink + '" width="100%">',
                    PageUrl: item.UrlToAdvertisement,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
                    Censor: _webmasterCensor,

                });

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
            general.stopLoad();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
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

function loadAdPosition() {
    $.ajax({
        type: 'GET',
        url: '/AdvertisementContent/GetAllAdPosition',

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
function loadAllAdvertiser() {
    $.ajax({
        type: 'GET',
        url: '/AdvertisementContent/GetAllAdvertiser',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },
        

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selAdvertiser').append("<option value='" + item.KeyId + "'>" + item.BrandName + "</option>");
                general.stopLoad();

            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load người quảng cáo !', 'error');
            general.stopLoad();
        },
    });

}
function loadAdPosition() {
    $.ajax({
        type: 'GET',
        url: '/AdvertisementContent/GetAllAdPosition',

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

