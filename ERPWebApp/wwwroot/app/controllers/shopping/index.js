var shopController = function () {
    this.initialize = function () {
        loadBookCategory();
        registerEvents();
    }
}

function registerEvents() {
    $('#frmAdvanceSearch').submit(function () {
        if (general.toInt($('#txtTo').val()) < general.toInt($('#txtFrom').val())) {
            general.notify('Khoảng giá Đến không thể nhỏ hơn Từ')
            return false;
        }
        $('#txtFrom').val(general.toInt($('#txtFrom').val()));
        $('#txtTo').val(general.toInt($('#txtTo').val()));
        return true;
    })
}

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results != null)
        return results[1] || 0;
    return null;
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
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });
}