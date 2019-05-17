var logOutController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

function registerEvents() {
    $('body').on('click', '#LogOut', function () {
        $.ajax({
            type: 'POST',
            url: '/BeyeuBookstore/LogOutAsync',
            success: function (respond) {
                window.location.href = respond;
            }
        })
    })
}