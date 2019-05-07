var shopController = function () {
    this.initialize = function () {
        loadCategory();
        registerEvents();
    }
}

function loadCategory() {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: '/BookCategory/GetAll',
        success: function (respond) {
            $.each(respond, function (i, item) {

            })
        }
    })
}