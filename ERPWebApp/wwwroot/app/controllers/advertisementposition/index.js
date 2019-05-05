var advertisementPositionController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        loadBookCategory();
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

        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

        $('#btnCreate').on('click', function () {
            resetForm();
           

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

            
        });


   
        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
         
                txtAdId:
                {
                    required: true
                },
                txtPageUrl:
                {
                    required: true
                },
      
                txtWidth:
                {
                    required: true,
                    number: true,
                },
     
                txtHeight: {
                    required: true,
                    number: true
                },
                selEditStatus: {
                    required: true,
                  
                },
                txtPrice: {
                    required: true,
                    number: true

                },
            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#txtMerchantStatus').val() == 0) {
                general.notify('Nhà cung cấp đã bị Khóa, vui lòng liên hệ Webmaster để biết thêm chi tiết!', 'error');
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
                    var adId = $('#txtAdId').val();
                    var width = $('#txtWidth').val();
                    var height = $('#txtHeight').val();
                    var price = general.toFloat($('#txtPrice').val());
                    var status = $('#selEditStatus option:selected').val();
                    var pageurl = $('#txtPageUrl').val();
                    $.ajax({
                        type: 'POST',
                        url: '/AdvertisementPosition/SaveEntity',
                        data: {
                            KeyId: keyId,
                            PageUrl: pageurl,
                            IdOfPosition: adId,
                            Width: width,
                            Height: height,
                            AdvertisePrice: price,
                            Status: status,
                        },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoading();
                        },
                        success: function (response) {

                            $('#modal-add-edit').modal('hide');
                            general.notify('Ghi thành công!', 'success');
                            resetForm();
                            $('#frmMaintainance').trigger('reset');
                            general.stopLoading();
                            loadData();
                        },
                        error: function (err) {
                            general.notify('Có lỗi trong khi ghi !', 'error');
                            general.stopLoading();

                        },
                    });
                    return false;
                }
            }
        });




    }

    function resetForm() {
        $('#txtId').val('');
        $('#txtStatus').val('')
        $('#dtDateModified').val('');
        $('#dtDateModified').val('');
        $('#txtBooktitle').val('');
        $('#txtAuthor').val('');
        $('#selBookcategory').val('');
        $('#selisPaperback').val('');
        $('#txtLength').val('');
        $('#txtHeight').val('');
        $('#txtWidth').val('');
        $('#txtPageNumber').val('');
        $('#txtPrice').val('');
        $('#txtDescription').val('');




    }

    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/AdvertisementPosition/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log("loaddetailbook", response);
                var data = response;

                $('#txtId').val(data.KeyId);
                $('#txtAdId').val(data.IdOfPosition);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#txtWidth').val(data.Width);
                $('#txtHeight').val(data.Height);
                $('#txtPrice').val(general.toMoney(data.AdvertisePrice));
                $('#selEditStatus').val(data.Status);
                $('#txtPageUrl').val(data.PageUrl);
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
        url: '/AdvertisementPosition/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log("data", response);
        
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
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    Id: item.IdOfPosition,
                    PageUrl: item.PageUrl,
                    WidthxHeight: item.Width+'x'+item.Height,
                    Price: general.toMoney(item.AdvertisePrice),
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
                  

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

function loadBookCategory() {
    $.ajax({
        type: 'GET',
        url: '/Book/GetAllBookCategory',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selBookCategory').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");
                $('#selBookcategory').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");


            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });

}

