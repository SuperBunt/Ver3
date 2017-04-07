//declare a variable
var markers = [];

$.ajax({
    async: false,
    type: "POST",
    dataType: "json",
    url: '@Url.Action("GetMarkers", "Analysis")',
    data: '{}',
    success: function (result) {
        //get address from controller action.....
        markers = result.AddressResult;
    }

});

function initMap() {
    var myLatLong = { lat: 53.3498, lng: -6.2603 };
    var mapOptions = {
        center: myLatLong,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var infoWindow = new google.maps.InfoWindow();
    var latlngbounds = new google.maps.LatLngBounds(new google.maps.LatLng(54.668922, -7.0100274), new google.maps.LatLng(55.121921664, -6.5620904));
    var map = new google.maps.Map(document.getElementById("selectMap"), mapOptions);
    setMarkers(map, markers);
}




function setMarkers(map, markers) {

    for (var i = 0; i < markers.length; i++) {
        var data = markers[i];
        var siteLatLng = new google.maps.LatLng(data.Latitude, data.Longitude);
        var marker = new google.maps.Marker({
            position: siteLatLng,
            map: map,
            title: data.Description,
            id: data.Id,      
            icon: 'BitmapDescriptorFactory.defaultMarker(BitmapDescriptorFactory.'+data.Colour+'))'
        });

        var contentString = "Some content";

        google.maps.event.addListener(marker, "click", function () {
            infowindow.setContent(this.title);
            infowindow.open(map, this);
        });
    }
}