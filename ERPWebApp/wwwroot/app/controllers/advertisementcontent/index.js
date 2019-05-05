var advertisementContentController = function () {
    this.initialize = function () {
        loadData();
        loadAdPosition();
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
            $.ajax({
                type: 'GET',
                url: '/AdvertisementContent/GetAdvertiserInfo',
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    $('#txtAdvertiser').val(response.BrandName);
                    $('#txtAdvertiserId').val(response.KeyId);
                    $('#txtAdvertiserStatus').val(response.Status);
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
            $.ajax({
                type: 'POST',
                url: '/AdvertisementContent/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.Unqualified,
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
                url: '/AdvertisementContent/UpdateStatus',
                data: {
                    id: keyId,
                    status: general.censorStatus.Censored,
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
            var that = $(this).data('id');
            loadDetail(that);
            $('#formCreate').addClass('hidden');

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
                    var paidDeposite = false;
                    var filename = $('#fileAdImg').val().split('\\').pop();
                    var extension = filename.substr((filename.lastIndexOf('.') + 1));
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
                                        PaidDeposite: paidDeposite,
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
            url: "/AdvertisementContent/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
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
                $('#selAdPosition').val(data.AdvertisementPositionFK);
                $('#txtTitle').val(data.Title);
                $('#txtLink').val(data.UrlToAdvertisement);
                $('#txtDescription').val(data.Description);
                $('#txtNote').val(data.Note);
                switch (data.CensorStatus) {
                    case general.censorStatus.Censored:
                    case general.censorStatus.Unqualified:
                        $('#formCensor').addClass('hidden');
                        break;
                }
                
         
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
        url: '/AdvertisementContent/GetAllPaging',
        dataType: 'json',
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
                    case general.censorStatus.Censored:
                        _color = 'green';
                        _statusName = 'Đã kiểm duyệt';
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
                    Img: '<img src="' + item.ImageLink + '" width="100">',
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

function loadAdPosition() {
    $.ajax({
        type: 'GET',
        url: '/AdvertisementContent/GetAllAdPosition',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selAdPosition').append("<option value='" + item.KeyId + "'>" + item.Title + "</option>");
            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load vị trí quảng cáo !', 'error');

        },
    });

}

