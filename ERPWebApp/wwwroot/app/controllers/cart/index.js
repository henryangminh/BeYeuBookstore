var cartPageController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

function registerEvents() {
    $('body').on('change', '#txtQuantity', function () {
        var qty = $(this).val();
        if (qty <= 0) {
            qty = 1;
            $(this).val(qty);
        }

        if (qty > $(this).attr("max")) {
            qty = $(this).attr("max");
            $(this).val(qty);
        }
        
        var Id = $(this).parent().siblings('input').val();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/Cart/AddToCart',
            data: {
                BookId: Id,
                quantity: qty,
                update: true,
            },
            beforeSend: function () {
                general.startLoad();
            },
            success: function (respond) {
                console.log('AddToCart', respond);
                loadCart();
                general.stopLoad();
            },
            error: function (e) {
                console.log(e);
                general.stopLoad();
            }
        })
    })
}