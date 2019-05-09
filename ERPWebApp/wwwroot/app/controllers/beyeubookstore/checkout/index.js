var checkOutController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

/*
function loadCheckOut() {
    var template = $('#CartItem');

    $.ajax({
        url: '/Cart/GetCart',
        dataType: 'json',
        success: function (respond) {
            itemCount = respond.length;
            console.log('checkout', respond);
        }
    })
}
*/