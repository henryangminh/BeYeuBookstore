var ratingController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results != null)
        return results[1] || '';
    return null;
}

function removeStar() {
    for (var i = 1; i <= 5; i++) {
        var id = "#star" + i;
        $(id).css("color", "");
    }
}

function addStar(n) {
    for (var i = 1; i <= n; i++) {
        var id = "#star" + i;
        $(id).css("color", "#00ABDF");
    }
}

function registerEvents() {
    var select = 0;
    $('body').on('click', '#star1', function () {
        removeStar();
        addStar(1);
        select = 1;
    })

    $('body').on('click', '#star2', function () {
        removeStar();
        addStar(2);
        select = 2;
    })

    $('body').on('click', '#star3', function () {
        removeStar();
        addStar(3);
        select = 3;
    })

    $('body').on('click', '#star4', function () {
        removeStar();
        addStar(4);
        select = 4;
    })

    $('body').on('click', '#star5', function () {
        removeStar();
        addStar(5);
        select = 5;
    })

    $('body').on('click', '#btnReview', function () {
        if (select == 0) {
            general.notify("Bạn chưa chọn số điểm", error);
        }
        else {
            $.ajax({
                type: 'POST',
                url: "/BeyeuBookstore/SaveRating",
                data: {
                    KeyId: $('#txtRatingID').val(),
                    BookFK: $.urlParam("id"),
                    CustomerFK: 0,
                    Rating: select,
                    Comment: $('#txtReview').val(),
                },
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (respond) {
                    if (respond == "fail") {
                        general.notify("Không thể rating, vui lòng đăng nhập lại");
                        general.stopLoad();
                    }
                    else {
                        window.location.href = respond;
                    }
                }
            })
        }
    })

    $('body').on('click', '#Review', function () {
        $.ajax({
            url: '/BeyeuBookstore/CheckBought',
            data: {
                BookId: $.urlParam("id"),
            },
            success: function (respond) {
                if (respond) {
                    template = $('#AllowRating').html();
                }
                else {
                    template = $('#UnallowRating').html();
                }
                $('#checkRating').html(template);
            }
        })
    })
}