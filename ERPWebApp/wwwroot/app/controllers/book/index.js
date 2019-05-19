var bookController = function () {
    this.initialize = function () {
        loadData();
        loadAllMerchant();
       // sendMail();
        registerEvents();
    }
    function registerEvents() {
        loadBookCategory();
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selBookCategory').on('change', function () {
            loadData();
        });
        $('#selMerchant').on('change', function () {
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
            $('#DateInfo').addClass('hidden');
            $('#ShowBookImg').addClass('hidden');
            $('#ImportBookImg').removeClass('hidden');
            $.ajax({
                type: 'GET',
                url: '/Book/GetMerchantInfo',

                dataType: "json",

                success: function (response) {
                    console.log("loadInfo", response);
                    $('#txtMerchantKeyId').val(response.KeyId);
                    $('#txtMerchantStatus').val(response.Status);
                    $('#dtDateCreated').val(moment().format("DD/MM/YYYY"));
                    $('#txtMerchant').val(response.MerchantCompanyName);
                    $('#selStatus').html('<span class="badge bg-black" style="font-size:15px;">Đang nhập</span>');
                    $('#modal-add-edit').modal('show');

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load thông tin nhà cung cấp!', 'error');

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
            $('#DateInfo').removeClass('hidden');
            var that = $(this).data('id');
            loadDetail(that);

        
            //document.getElementById("btnSave").style.display = "block";
        });
        //UpdateStatus
        $('body').on('click', '.btn-lock', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            updateBookStatus(that,0);
            
        })
        $('body').on('click', '.btn-unlock', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            updateBookStatus(that,1);
            
        });


        
        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtBooktitle:
                {
                    required: true
                },
                txtAuthor:
                {
                    required: true
                },
                txtLength:
                {
                    required: true,
                    number: true,
                    integer: true,
                },
                txtWidth:
                {
                    required: true,
                    number: true,
                    integer: true,
                },
                txtHeight:
                {
                    number: true,
                    integer: true,
                },
                selBookcategory: {
                    required: true
                },
                selisPaperback: {
                    required: true
                },
                txtPageNumber: {
                    required: true,
                    number: true,
                    integer: true,
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
                general.notify('Nhà cung cấp đã bị Khóa hoặc bạn không có quyền thêm mới, vui lòng liên hệ Webmaster để biết thêm chi tiết!', 'error');
                return false;
            }

            else {
                
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

                        fileUpload = $('#fileBookImg').get(0);
                        files = fileUpload.files;
                        data.append("files", files[0]);

                        var bookTitle = $('#txtBooktitle').val();
                        var merchantFK = $('#txtMerchantKeyId').val();
                        var author = $('#txtAuthor').val();
                        var bookCategoryFK = $('#selBookcategoryDetail option:selected').val();
                        var ispaperback = $('#selisPaperback option:selected').val();
                        if (ispaperback == 0) {
                            ispaperback = true;
                        }
                        else {
                            ispaperback = false;
                        }
                        var length = $('#txtLength').val();
                        var width = $('#txtWidth').val();
                        var height = $('#txtHeight').val();
                        var pageNo = $('#txtPageNumber').val();
                        var price = general.toFloat($('#txtPrice').val());
                        var description = $('#txtDescription').val();
                        var quantity = 0;
                        if ($('#txtQty').val() != 0) { quantity = parseInt($('#txtQty').val()) }
                        var status = 0;
                        if ($('#fileBookImg').val() != "")
                        {
                            var filename = $('#fileBookImg').val().split('\\').pop();
                            var extension = filename.substr((filename.lastIndexOf('.') + 1));
                            if (extension.toUpperCase() != "JPG" && extension.toUpperCase() != "PNG") {
                                general.notify('File ảnh phải ở định dạng JPG hoặc PNG !', 'error');
                                return false;
                            }
                            if ($('#fileBookImg')[0].files[0].size > general.maxSizeAllowed.BookImg) {
                                general.notify('Kích thước ảnh phải nhỏ hơn 2Mb !', 'error');
                                return false;
                            }
                            else {
                                $.ajax({
                                    type: 'POST',
                                    url: '/Book/ImportFiles',
                                    data: data,
                                    contentType: false,
                                    processData: false,

                                    beforeSend: function () {
                                        general.startLoad();
                                        general.startLoading();
                                    },
                                    success: function (e) {
                                        console.log(e);
                                        if ($('#fileBookImg').val() != '') {

                                            linkImg = e[0];

                                            e.shift();

                                        }
                                        $.ajax({
                                            type: 'POST',
                                            url: '/Book/SaveEntity',
                                            data: {
                                                KeyId: keyId,
                                                BookTitle: bookTitle,
                                                MerchantFK: merchantFK,
                                                Author: author,
                                                BookCategoryFK: bookCategoryFK,
                                                isPaperback: ispaperback,
                                                Length: length,
                                                Width: width,
                                                Height: height,
                                                PageNumber: pageNo,
                                                UnitPrice: price,
                                                Description: description,
                                                Quantity: quantity,
                                                Status: status,
                                                Img: linkImg,
                                            },
                                            dataType: "json",
                                            beforeSend: function () {
                                                general.startLoad();
                                                general.startLoading();
                                            },
                                            success: function (response) {

                                                $('#modal-add-edit').modal('hide');
                                                general.notify('Ghi thành công!', 'success');
                                                resetForm();
                                                $('#frmMaintainance').trigger('reset');
                                                general.stopLoading();
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

                                    },
                                    error: function (e) {
                                        general.stopLoaad();
                                        general.notify('Có lỗi trong khi ghi !', 'error');
                                        console.log(e);

                                    }

                                });
                            }
                        }
                        else {
                            var linkImg = $('#BookImg').val();
                            $.ajax({
                                type: 'POST',
                                url: '/Book/SaveEntity',
                                data: {
                                    KeyId: keyId,
                                    BookTitle: bookTitle,
                                    MerchantFK: merchantFK,
                                    Author: author,
                                    BookCategoryFK: bookCategoryFK,
                                    isPaperback: ispaperback,
                                    Length: length,
                                    Width: width,
                                    Height: height,
                                    PageNumber: pageNo,
                                    UnitPrice: price,
                                    Description: description,
                                    Quantity: quantity,
                                    Status: status,
                                    Img: linkImg,
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
                                    general.stopLoading();
                                    loadData();
                                },
                                error: function (err) {
                                    general.notify('Có lỗi trong khi ghi !', 'error');
                                    general.stopLoad();

                                },
                            });
                        }
                    }               
            }
            general.stopLoad();
        })
        
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
        $('#ShowBookImg').empty();
        $('#txtDescription').val('');
        $('#txtQty').val('');
    }

    function loadDetail(that) {
        resetForm();
        $('#ShowBookImg').removeClass('hidden');
        $.ajax({
            type: "GET",
            url: "/Book/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log("loaddetailbook", response);
                var data = response;
                
                $('#txtId').val(data.KeyId);
                $('#txtMerchant').val(data.MerchantFKNavigation.MerchantCompanyName);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#txtMerchantKeyId').val(data.MerchantFKNavigation.KeyId);
                $('#txtMerchantStatus').val(data.MerchantFKNavigation.Status);
                $('#txtBooktitle').val(data.BookTitle);
                $('#txtAuthor').val(data.Author);
                $('#BookImg').val(data.Img);
                $('#selBookcategoryDetail').val(data.BookCategoryFK);
                if (data.isPaperback) {
                    $('#selisPaperback').val(0);
                }
                else {
                    $('#selisPaperback').val(1);
                }
                $('#txtLength').val(data.Length);
                $('#txtQty').val(data.Quantity);
                $('#txtWidth').val(data.Width);
                $('#txtHeight').val(data.Height);
                $('#txtPageNumber').val(data.PageNumber);
                $('#txtPrice').val(general.toMoney(data.UnitPrice));
                $('#txtDescription').val(data.Description);
                var _color = '';
                var _status = '';
                switch (data.Status) {
                    case general.status.InActive:
                        _color = 'red';
                        _status = 'Khóa';
                        break;
                    case general.status.Active:
                        _color = 'green';
                        _status = 'Kích hoạt';
                        break;
                }
                $('#selStatus').html('<span class="badge bg-' + _color + '" style="font-size:15px;">' + _status + '</span>');
                $('#modal-add-edit').modal('show');
                $('#ShowBookImg').append('<img src="' + data.Img + '" width="100%">')
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
            mId: $('#selMerchant option:selected').val(),
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            bookcategoryid: $('#selBookCategory').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Book/GetAllPaging',
        dataType: 'json',
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
                var _price = general.toMoney(item.UnitPrice);
                render += Mustache.render(template, {
                    
                    KeyId: item.KeyId,
                    Merchant: item.MerchantFKNavigation.MerchantCompanyName,
                    BookTitle: item.BookTitle,
                    Author: item.Author,
                    Img: '<img src="'+item.Img+'" width="100">',
                    BookType: item.BookCategoryFKNavigation.BookCategoryName,
                    UnitPrice: _price,
                    Qty: item.Quantity,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',

                    
                });
                order++;

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (XMLHttpRequest,textStatus,errorThrown) {
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
                $('#selBookcategoryDetail').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");


            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });

}
function updateBookStatus(bookthat, statusthat) {
    $.ajax({
        type: 'POST',
        url: '/Book/UpdateBookStatus',

        dataType: "json",
        data: {
            id: bookthat,
            status: statusthat,
        },
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {
            general.notify('Cập nhật trạng thái sách thành công!', 'success');
            
            general.stopLoad();
            loadData();
        },
        error: function (err) {
            general.notify('Có lỗi trong khi cập nhật trạng thái sách !', 'error');
            general.stopLoad();
        },
    });

}
function loadAllMerchant() {
    $.ajax({
        type: 'GET',
        url: '/Book/GetAllMerchantInfo',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selMerchant').append("<option value='" + item.KeyId + "'>" + item.MerchantCompanyName + "</option>");
            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nhà cung cấp !', 'error');

        },
    });

}