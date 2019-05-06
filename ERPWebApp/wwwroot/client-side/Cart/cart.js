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
                    general.notify("Thêm vào giỏ hàng thành công",'success');
                    loadCart();
                },
                error: function (e) {
                    console.log(e);
                }
            })
        })

        $('body').on('click', '#DeleteCart', function () {
            var Id = $(this).parent().siblings('input').val();

            $.ajax({
                url: 'Cart/ClearCart',
                success: function (respond) {
                    console.log('clearCart',respond);
                    loadCart();
                },
                error: function (e) {
                    console.log(e);
                }
            })
        })

        $('body').on('click', '#RemoveBookFromCart', function () {
            var Id = $(this).parent().siblings('input').val();

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'Cart/RemoveFromCart',
                data: {
                    BookId: Id
                },
                success: function (respond) {
                    console.log('Delete from cart',respond);
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
    var templateCartPage = $('#CartPageDetailTemplate').html();
    var render = "";
    var renderCartPage = "";
    var total = 0;
    var itemCount = 0;

    $.ajax({
        url: 'Cart/GetCart',
        dataType: 'json',
        success: function (respond) {
            itemCount = respond.length;
            console.log('cart', respond);
            if (itemCount == 0) {
                $('#CartDetail').html('<p>Bạn chưa mua hàng. Hãy đến cửa hàng để mua thêm nhé</p>');
                $('#CartPageDetail').html('<td class="product-name" style="font-size: 16px;" colspan="6">Bạn chưa mua hàng. Hãy đến cửa hàng để mua thêm nhé</td>');
            }
            else {
                $.each(respond, function (i, item) {
                    var subtotal = parseInt(item.Quantity) * parseInt(item.UnitPrice);
                    total += subtotal;
                    render += Mustache.render(template, {
                        KeyId: item.Book.KeyId,
                        LinkToProduct: '#',
                        LinkImage: item.Book.Img, //'/images/img/product/10.jpg', //để tạm thời
                        BookName: item.Book.BookTitle,
                        Quantity: item.Quantity,
                        UnitPrice: general.toMoney(item.UnitPrice),
                        SubTotal: general.toMoney(subtotal),
                    })

                    if (templateCartPage != null) {
                        renderCartPage += Mustache.render(templateCartPage, {
                            KeyId: item.Book.KeyId,
                            LinkToProduct: '#',
                            LinkImage: item.Book.Img, //'/images/img/product/10.jpg', //để tạm thời
                            BookName: item.Book.BookTitle,
                            Quantity: item.Quantity,
                            UnitPrice: general.toMoney(item.UnitPrice),
                            SubTotal: general.toMoney(subtotal),
                        })
                    }
                })
                $('#CartDetail').html(render);
                $('#CartPageDetail').html(renderCartPage);
            }
            $('#txtTotalItemCart').text(itemCount);
            $('#txtTotalPriceCart').text(general.toMoney(total));
            $('#txtCartAmount').text(general.toMoney(total));
            $('#txtCartTotal').text(general.toMoney(total));
        },
        error: function (e) {
            console.log(e);
        }
    })
}