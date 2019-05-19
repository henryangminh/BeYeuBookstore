var beyeubookstoreController = function () {
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

        $('body').on('click', '#ShowDetail', function () {
            var BookId = $(this).parent().parent().siblings('input').val();

            $.ajax({
                type: 'GET',
                url: "/BeyeuBookstore/GetById",
                data: { id: BookId },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    console.log('BookDetail', response);
                    var size = response.Width + "x" + response.Length;
                    if (response.Height != null) size += "x" + response.Height;
                    size += " (cm)";
                    $('#BookId').val(response.KeyId);
                    $('#txtPaperback').text((response.isPaperback) ? "Bìa mềm" : "Bìa cứng");
                    $('#txtBookNameModal').text(response.BookTitle);
                    $('#txtPriceModal').text(general.toMoney(response.UnitPrice));
                    $('#txtDescriptionModal').text(response.Description);
                    $('#txtLinkToProduct').attr('href', '/BeyeuBookstore/BookDetail?id=' + response.KeyId);
                    $('#imgDetail').attr('src', response.Img)
                    $('#txtSize').text("Kích thước: " + size);
                    $('#txtAuthor').text("Tác giả: " + response.Author);
                    $('#txtMerchant').text("Nhà phát hành: " + response.MerchantFKNavigation.MerchantCompanyName);
                    if (response.Quantity > 0) {
                        $('#quantityStatus').html('<i class="fa fa-check"></i>Số lượng: ' + response.Quantity);
                    }
                    else {
                        $('#quantityStatus').html('<i class="fa fa-times" color="red"></i>Hết hàng');
                        $('#divAddToCart').remove();
                    }
                    general.stopLoad();
                }
            })
        })

        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

    }

    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/BeyeuBookstore/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log("loaddetail", response);
                var data = response;

                $('#txtId').val(data.KeyId);
                $('#txtMerchant').val(data.MerchantFKNavigation.MerchantCompanyName);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#txtMerchantKeyId').val(data.MerchantFKNavigation.KeyId);
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

    var template = $('#bookcase').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {
            quantity: 12,
        },
        url: '/BeyeuBookstore/GetAllQuantity',
        dataType: 'json',
        success: function (response) {
            console.log("data", response);
            //var order = 1;
            $.each(response, function (i, item) {
                if (i % 4 == 0) render += '<div class="col-lg-12 col-md-12 col-xs-12">';
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    BookImage: item.Img, //'/images/img/product/10.jpg', //để tạm thời
                    //BookRating: 5.0, //item.BookRating, //Rating //Để tạm thời thôi
                    BookTitle: item.BookTitle,
                    BookPrice: general.toMoney(item.UnitPrice),
                    LinkBook: '?id=' + item.KeyId,
                });
                if ((i + 1) % 4 == 0) render += '</div>';
                //order++;
            });
            //$('#lblTotalRecords').text(response.RowCount);
            $('#LoadBook').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
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
    var template = $('#bookcategory').html();
    var render = "";

    $.ajax({
        type: 'GET',
        url: '/BookCategory/GetAll',
        dataType: "json",
        success: function (response) {
            console.log('BookCategory', response);
            $.each(response, function (i, item) {
                render += Mustache.render(template, {
                    LinkCategoryShop: '?radBookCategory=' + item.KeyId,
                    BookCategoryName: item.BookCategoryName,
                    More: (i > 10) ? true : false,
                });
            });
            if (response.length > 10) {
                render += '<li class="rx-parent"><a class="rx-default" ><span class="cat-thumb fa fa-plus"></span>Thêm</a><a class="rx-show"><span class="cat-thumb fa fa-minus"></span>Đóng</a></li>';
            }
            $('#BookCategory').html(render);
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });
}