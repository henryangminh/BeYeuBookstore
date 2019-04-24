var bookController = function () {
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

        $('#selBookCategory').on('change', function () {
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

                success: function (response) {
                    console.log("loadInfo", response);
                    $('#txtMerchantKeyId').val(response.KeyId);
                    $('#txtMerchantStatus').val(response.Status);
                    $('#txtMerchant').val(response.MerchantCompanyName);
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
            var that = $(this).data('id');
            loadDetail(that);

        
            //document.getElementById("btnSave").style.display = "block";
        });


        //Delete
        $('body').on('click', '.btn-delete', function (e) {
            var that = $(this).data('id');
            $.ajax({
                type: 'POST',
                url: '/Book/Delete',
                data: { id: that },
                dataType: 'json',
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {

                    general.notify('Xóa thành công!', 'success');
                    loadData();
                    general.stopLoading();

                },
                error: function (status) {
                    general.notify('Có lỗi trong khi xóa !', 'error');
                    general.stopLoading();
                },
            });

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
                    number: true,
                },
                txtWidth:
                {
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
                    var keyId;
                    if ($('#txtId').val() == "") {
                        keyId = 0;
                    }
                    else {
                        keyId = parseInt($('#txtId').val());
                    }
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
                    var price = $('#txtPrice').val();
                    var description = $('#txtDescription').val();
                    var quantity = 0;
                    var status = $('#selStatus option:selected').val();
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
                $('#txtPrice').val(data.UnitPrice);
                $('#txtDescription').val(data.Description);
                $('#selStatus').val(data.Status);
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
                render += Mustache.render(template, {
                    
                    KeyId: item.KeyId,
                    Merchant: item.MerchantFKNavigation.MerchantCompanyName,
                    BookTitle: item.BookTitle,
                    Author: item.Author,
                    BookType: item.BookCategoryFKNavigation.BookCategoryName,
                    UnitPrice: item.UnitPrice,
                    Qty: item.Quantity,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
                    Description: item.Description,
                    
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
                $('#selBookcategory').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");


            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });

}

