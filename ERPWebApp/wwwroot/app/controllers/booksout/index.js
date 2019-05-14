var booksoutController = function () {
    this.initialize = function () {
        loadData();
       // sendMail();
        registerEvents();
    }
    function registerEvents() {
        loadAllMerchant();
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });
        

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });


        $('#btnMore').on('click', function () {

            loadBooksOut($('#selBook option:selected').val());
            $("#selBook option:selected").remove();

        });
        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

        $('#btnCreate').on('click', function () {
            resetForm();
            loadAllBookByMerchantId();
            $('#formAdd').removeClass('hidden');
            $('#formWacth').addClass('hidden');
            $('#formEditBooksOut').removeClass('hidden');
            $('#formSaveBooksOut').removeClass('hidden');
            $.ajax({
                type: 'GET',
                url: '/BooksOut/GetMerchantInfo',

                dataType: "json",

                success: function (response) {
                    console.log("loadInfo", response);
                    $('#txtMerchantKeyId').val(response.KeyId);
                    $('#txtMerchantStatus').val(response.Status);
                    $('#txtMerchant').val(response.MerchantCompanyName);
                    $('#dtDateCreated').val(moment().format("DD/MM/YYYY"));
                    $('#modal-add-edit').modal('show');

                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load thông tin nhà cung cấp!', 'error');

                },
            });
           
            
        });

        $('body').on('click', '#BooksOutDetailDelete', function () {
            var bookId = $(this).parent().parent().find('td:eq(0)').text();
            var bookTitle = $(this).parent().parent().find('td:eq(1)').text();
            alert(bookId + '|' + bookTitle);
            $('#selBook').append("<option value='" + bookId + "'>" + bookTitle + "</option>");

            $(this).parent().parent().remove();
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
                
                booksoutQty: {

                    required: true,
                    number: true,
                    integer: true,
                }
            }
        });

        //Save
        $('#btnSave').on('click', function (e) {
            if ($('#txtMerchantStatus').val() == 0) {
                general.notify('Nhà cung cấp đã bị Khóa, hoặc bạn không có quyền thêm mới vui lòng liên hệ Webmaster để biết thêm chi tiết!', 'error');
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
                    var listBooksOutDetail = [];
                    $('#tblBooksOut > tbody > tr').each(function () {
                        var BooksOutDetail = new Object();
                        BooksOutDetail.BookFK = $(this).find('td:eq(0)').text();
                        BooksOutDetail.Qty = $(this).find('td:eq(6)').find('input').val();
                        listBooksOutDetail.push(BooksOutDetail);
                    });
                    $.ajax({
                        type: 'POST',
                        url: '/BooksOut/SaveEntity',

                        dataType: "json",
                        beforeSend: function () {
                            general.startLoad();
                        },
                        data: {
                            listBooksOutDetailVms: listBooksOutDetail,
                        },
                        success: function (response) {

                            general.notify('Ghi thành công!', 'success');
                            loadData();
                            $('#modal-add-edit').modal('hide');

                            general.stopLoad();
                            loadData();
                        },
                        error: function (err) {

                            general.stopLoad();
                            general.notify('Có lỗi trong khi ghi phiếu nhập!', 'error');

                        },
                    });


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
        $('#tbl-booksoutcontent').empty();
    }

    function loadDetail(that) {

        $('#formAdd').removeClass('hidden');
        $('#formWacth').addClass('hidden');
        $('#formEditBooksOut').addClass('hidden');
        $('#formSaveBooksOut').addClass('hidden');
        resetForm();
        var template = $('#tableBooksOutDetail-template').html();
        var render = "";
        $.ajax({
            type: "GET",
            url: "/BooksOut/GetById",
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

                $.ajax({
                    type: "GET",
                    url: "/BooksOut/GetAllDetailById",
                    data: { id: that },
                    dataType: "json",
                    success: function (response) {
                        $.each(response, function (i, item) {
                            render += Mustache.render(template, {

                                BookId: item.BookFK,
                                BookName: item.BookFKNavigation.BookTitle,
                                Author: item.BookFKNavigation.Author,
                                Img: '<img src="' + item.BookFKNavigation.Img + '" width="100">',
                                Category: item.BookFKNavigation.BookCategoryFKNavigation.BookCategoryName,
                                Qty: item.BookFKNavigation.Quantity,
                                QtyOut: item.Qty,
                            });
                        });
                        $('#tbl-booksoutcontent').html(render);
                        general.stopLoad();

                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra', 'error');
                        general.stopLoad();
                    }
                });

                general.stopLoad();
                $('#modal-add-edit').modal('show');
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
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/BooksOut/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log("data", response);
      
            $.each(response.Results, function (i, item) {
         

                var _dateCreated = moment(item.DateCreated).format("DD/MM/YYYY HH:mm:ss");
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    Merchant: item.MerchantFKNavigation.MerchantCompanyName,
                    DateCreated: _dateCreated,
                });


           

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
function loadAllBookByMerchantId() {
    $.ajax({
        type: 'GET',
        url: '/BooksOut/GetAllBookByMerchantId',

        dataType: "json",

        success: function (response) {
            $.each(response, function (i, item) {
                $('#selBook').append("<option value='" + item.KeyId + "'>" + item.BookTitle + "</option>");

            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi sách !', 'error');

        },
    });

}


function loadAllMerchant() {
    $.ajax({
        type: 'GET',
        url: '/BooksOut/GetAllMerchantInfo',

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
function loadBooksOut(that) {

    var template = $('#tableBooksOut-template').html();
    var render = "";
    $.ajax({
        type: 'GET',
        url: '/BooksOut/GetBookById',

        dataType: "json",
        data: { id: that },
        success: function (response) {
            render += Mustache.render(template, {

                BookId: response.KeyId,
                BookName: response.BookTitle,
                Author: response.Author,
                Img: '<img src="' + response.Img + '" width="100">',
                Category: response.BookCategoryFKNavigation.BookCategoryName,
                Qty: response.Quantity,
            });
            $('#tbl-booksoutcontent').append(render);


        },
        error: function (err) {
            general.notify('Có lỗi trong khi load nhà cung cấp !', 'error');

        },
    });

}