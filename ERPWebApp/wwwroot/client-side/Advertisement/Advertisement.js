var advertisementController = function () {
    this.initialize = function () {
        getAdvertisement();       
    }
}

function getAdvertisement() {
    $.ajax({
        type: "GET",
        url: "/BeyeuBookstore/GetAdvertisement",
        dataType: 'json',
        data: { url: window.location.pathname.toLowerCase() },
        success: function (respond) {
            console.log("Advertiser", respond);
            $.each(respond, function (i, item) {
                var idPosition = '#' + item.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation.IdOfPosition;
                $(idPosition).attr("src", item.AdvertisementContentFKNavigation.ImageLink);
                $(idPosition).parent().attr("href", item.AdvertisementContentFKNavigation.UrlToAdvertisement);
                $(idPosition).parent().attr("target", "_blank");
            })
        },
        error: function (e) {
            console.log(e);
        }
    })
}