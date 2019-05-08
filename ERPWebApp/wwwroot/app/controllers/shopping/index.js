var shopController = function () {
    this.initialize = function () {
        loadData(true);
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
            "txtSearch": ($.urlParam('txtSearch') != null) ? $.urlParam('txtSearch'):'',
            "BookCategoryId": $.urlParam('radBookCategory'),
            "From": ($.urlParam('txtFrom') != null) ? $.urlParam('txtFrom') : null,
            "To": ($.urlParam('txtTo') != null) ? $.urlParam('txtTo') : null,
            "page": Page,
            "pageSize": 10,
        },
        dataType: 'json',
        success: function (response) {
            console.log("data", response);
            if (response.Results.length == 0) {
                $('#rsNone').text('Không có kết quả')
            }
            else {
                $('#rsCount').text(response.Results.length + ' kết quả trong số ' + response.RowCount + ' kết quả')
                $.each(response.Results, function (i, item) {
                    if (i % 4 == 0) render += '<div class="col-lg-12 col-md-12 col-xs-12">';
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        BookImage: item.Img,
                        BookTitle: item.BookTitle,
                        BookPrice: general.toMoney(item.UnitPrice),
                        LinkBook: '#',
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
                $('#pageNav').html(renderPaging);
            }
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
}

function GetLinkPaging(index) {
    var a = window.location.search.substring(1).split('&');
    for (var i = 0; i < a.length; i++) {
        if (a.includes('page'))
            a.splice(i, 1);
    }
    if (window.location.href.includes('?')) {
        return window.location.href + "&page=" + index;
    }
    return window.location.href + "?page=" + index;
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
                general.startLoading();
            },
            success: function (response) {
                console.log('BookDetail', response);

                $('#txtBookNameModal').text(response.BookTitle);
                $('#txtPriceModal').text(general.toMoney(response.UnitPrice));
                $('#txtDescriptionModal').text(response.Description);
                $('#imgDetail').attr('src', response.Img)
            }
        })
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
        },
        error: function (err) {
            console.log(err);
        },
    });
}