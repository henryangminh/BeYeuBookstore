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
            $.ajax({
                type: 'GET',
                url: '/Book/GetMerchantInfo',

                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },

                success: function (response) {
                    console.log("loadInfo", response);
                    $('#txtMerchantKeyId').val(response.KeyId);
                    $('#txtMerchantStatus').val(response.Status);
                    $('#txtMerchant').val(response.MerchantCompanyName);
                    $('#modal-add-edit').modal('show');
                    general.stopLoad();

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load thông tin nhà cung cấp!', 'error');
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
                },
                txtWidth:
                {
                    required: true,
                    number: true,
                },
                txtHeight:
                {
                    number: true,
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
                        var bookCategoryFK = $('#selBookcategory option:selected').val();
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
                        var status = $('#selStatus option:selected').val();
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
                                                general.stopLoaading();
                                                general.stopLoaad();

                                            },
                                        });

                                        return false;

                                    },
                                    error: function (e) {
                                        general.notify('Có lỗi trong khi ghi !', 'error');
                                        console.log(e);
                                        general.stopLoad();
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
                                    general.stopLoad();
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
        $('#txtDescription').val('');
    }

    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/Book/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
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
                $('#selBookcategory').val(data.BookCategoryFK);
                if (data.isPaperback) {
                    $('#selisPaperback').val(0);
                }
                else {
                    $('#selisPaperback').val(1);
                }
                $('#txtLength').val(data.Length);
                $('#txtWidth').val(data.Width);
                $('#txtHeight').val(data.Height);
                $('#txtPageNumber').val(data.PageNumber);
                $('#txtPrice').val(general.toMoney(data.UnitPrice));
                $('#txtDescription').val(data.Description);
                $('#selStatus').val(data.Status);
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
            general.stopLoad();
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (XMLHttpRequest,textStatus,errorThrown) {
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

function loadBookCategory() {
    $.ajax({
        type: 'GET',
        url: '/Book/GetAllBookCategory',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selBookCategory').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");
                $('#selBookcategoryDetail').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");
                general.stopLoad();


            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');
            general.stopLoad();;
        },
    });

}
function loadAllMerchant() {
    $.ajax({
        type: 'GET',
        url: '/Book/GetAllMerchantInfo',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selMerchant').append("<option value='" + item.KeyId + "'>" + item.MerchantCompanyName + "</option>");
                general.stopLoad();
            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nhà cung cấp !', 'error');
            general.stopLoad();

        },
    });

}

//function sendMail() {
//    $.ajax({
//        type: 'POST',
//        url: '/Book/UpdateBookRatingById',
//        data: {
//            id: 1,
//        },
//        dataType: "json",

//        success: function (response) {
            
//            general.notify('Gửi mail thành công', 'success');
            
//        },
//        error: function (err) {
//            console.log(err);
//            general.notify('Có lỗi trong khi gửi mail', 'error');

//        },
//    });
//}