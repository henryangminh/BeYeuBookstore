var shopController = function () {
    this.initialize = function () {
        loadData(true);
        LoadMerchant();
        loadBookCategory();
        registerEvents();
    }
}

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results != null)
        return results[1] || '';
    return null;
}
/*
$(window).on('load', function (){
    document.getElementByName('slcMerchant').selectedIndex = ($.urlParam('slcMerchant') != null) ? $.urlParam('slcMerchant') : 0;
    document.getElementByName('slcSortBy').selectedIndex = ($.urlParam('slcSortBy') != null) ? $.urlParam('slcSortBy') : 1;
    document.getElementByName('slcOrder').selectedIndex = ($.urlParam('slcOrder') != null) ? $.urlParam('slcOrder') : 1;
})
*/
function loadData() {

    var template = $('#bookcase').html();
    var templatePaging = $('#paging').html();
    var render = "";
    var renderPaging = "";
    var Page = ($.urlParam('page') != null) ? $.urlParam('page') : 1;

    $.ajax({
        type: 'GET',
        url: '/BeyeuBookstore/GetAllPaging',
        data: {
            "txtSearch": ($.urlParam('txtSearch') != null) ? decodeURI($.urlParam('txtSearch')):'',
            "BookCategoryId": $.urlParam('radBookCategory'),
            "From": ($.urlParam('txtFrom') != null) ? $.urlParam('txtFrom') : null,
            "To": ($.urlParam('txtTo') != null) ? $.urlParam('txtTo') : null,
            "MerchantId": ($.urlParam('slcMerchant') != null) ? $.urlParam('slcMerchant') : null,
            "OrderBy": ($.urlParam('slcSortBy') != null) ? $.urlParam('slcSortBy') : null,
            "Order": ($.urlParam('slcOrder') != null) ? $.urlParam('slcOrder') : null,
            "page": Page,
            "pageSize": 10,
        },
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);
            if (response.Results.length == 0) {
                $('#rsNone').text('Không có kết quả')
            }
            else {
                $('#rsCount').text(response.Results.length + ' trong số ' + response.RowCount + ' kết quả')
                $.each(response.Results, function (i, item) {
                    if (i % 4 == 0) render += '<div class="col-lg-12 col-md-12 col-xs-12">';
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        BookImage: item.Img,
                        BookTitle: item.BookTitle,
                        BookPrice: general.toMoney(item.UnitPrice),
                        LinkBook: '?id=' + item.KeyId,
                    });
                    if ((i + 1) % 4 == 0) render += '</div>';
                });
                $('#LoadBook').html(render);

                for (var i = 1; i <= response.PageCount; i++) {
                    renderPaging += Mustache.render(templatePaging, {
                        LinkPage: GetLinkPaging(i),
                        PageNumber: i,
                        active: (Page == i) ? true : false,
                    })
                }

                document.getElementById('slcMerchant').selectedIndex = ($.urlParam('slcMerchant') != null) ? $.urlParam('slcMerchant') : 0;
                document.getElementById('slcSortBy').selectedIndex = ($.urlParam('slcSortBy') != null) ? $.urlParam('slcSortBy') - 1 : 0;
                document.getElementById('slcOrder').selectedIndex = ($.urlParam('slcOrder') != null) ? $.urlParam('slcOrder') - 1 : 0;

                $('#pageNav').html(renderPaging);
            }
            general.stopLoad();
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoad();
        }
    });
}

function GetLinkPaging(index) {
    var url = window.location.href.split('?')[0];
    var query = window.location.search.substring(1).split('&');
    for (var i = 0; i < query.length; i++) {
        if (query[i].includes('page'))
            query.splice(i, 1);
    }
    query = query.join('&');
    return url + '?' + query + '&page=' + index;
}

function registerEvents() {
    $('#frmAdvanceSearch').submit(function () {
        if (general.toInt($('#txtTo').val()) < general.toInt($('#txtFrom').val())) {
            general.notify('Khoảng giá Đến không thể nhỏ hơn Từ')
            return false;
        }
        if ($('#txtFrom').val() != "") 
            $('#txtFrom').val(general.toInt($('#txtFrom').val()));
        if ($('#txtTo').val() != "") 
            $('#txtTo').val(general.toInt($('#txtTo').val()));
        return true;
    })

    $('body').on('click', '#ShowDetail', function () {
        //var BookId = $('#txtBookKeyId').val();
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
}

function LoadMerchant() {
    $.ajax({
        type: 'GET',
        url: '/Merchant/GetAll',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (respond) {
            $.each(respond, function (i, item) {
                $('#slcMerchant').append(new Option(item.MerchantCompanyName, item.KeyId));
            })
            //LoadSelected();
            general.stopLoad();
        },
        error: function () {

        }
    })
}

function loadBookCategory() {
    var template = $('#bookcategory').html();
    var render = "";
    var checkedId = 0;

    $.ajax({
        type: 'GET',
        url: '/BookCategory/GetAll',
        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log('BookCategory', response);
            if ($.urlParam('radBookCategory') != null) {
                checkedId = $.urlParam('radBookCategory');
            }
            render += '<li><a>Tất cả<span><input type="radio" name="radBookCategory" value="0"' + ((checkedId==0)?'checked':'') + '/></span></a></li>';
            $.each(response, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    BookCategoryName: item.BookCategoryName,
                    checked: (item.KeyId == checkedId) ? true : false,
                });
            });
            $('#BookCategory').html(render);
            general.stopLoad();
        },
        error: function (err) {
            console.log(err);
            general.stopLoad();
        },
    });
}