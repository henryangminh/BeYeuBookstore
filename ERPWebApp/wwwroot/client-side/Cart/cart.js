var cartController = function () {
    this.initialize = function () {
        loadCart();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '#AddToCart', function () {
            var Id = $(this).parent().parent().siblings('input').val();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'Cart/AddToCart',
                data: {
                    BookId: Id,
                    quantity: 1,
                    update: false,
                },
                success: function (respond) {
                    console.log('AddToCart', respond);
                    loadCart();
                },
                error: function (e) {
                    console.log(e);
                }
            })
        })
    }
}

function loadCart() {
    var template = $('#LoadCart').html();
    var render = "";
    var total = 0;
    var itemCount = 0;

    $.ajax({
        url: 'Cart/GetCart',
        dataType: 'json',
        success: function (respond) {
            itemCount = respond.length;
            console.log('cart', respond);
            $.each(respond, function (i, item) {
                var subtotal = parseInt(item.Quantity) * parseInt(item.UnitPrice);
                total += subtotal;
                render += Mustache.render(template, {
                    LinkToProduct: '#',
                    LinkImage: item.Book.Img, //'/images/img/product/10.jpg', //để tạm thời
                    BookName: item.Book.BookTitle,
                    Quantity: item.Quantity,
                    UnitPrice: general.toMoney(item.UnitPrice),
                    SubTotal: general.toMoney(subtotal),
                })
            })
            $('#CartDetail').html(render);
            $('#txtTotalPriceCart').text(general.toMoney(total));
            $('#txtTotalItemCart').text(itemCount);
        },
        error: function (e) {
            console.log(e);
        }
    })
}