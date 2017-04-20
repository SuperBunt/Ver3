function filterDropdown() {
    // cascading dropdown list for towns by county
    $("#County").change(function () {
        $("#TownID").empty();

        $.ajax({
            type: 'POST',
            url: '@Url.Action("GetTowns")',
            dataType: 'json',
            data: { county: $("#County").val() },
            success: function (townList) {

                $.each(townList, function (i, locale) {
                    $("#TownID").append('<option value ="'
                    + locale.TownId + '">'
                        + locale.Name + ' </option>');
                });
            },
            error: function (ex) {
                alert('Failed' + ex);
            }
        });
        return false;
    })
}
function GoogleMap() {
    src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyBeZsRxVhJ__8kmVandHXR7UvI4kvGRzgs&callback=initMap"
    alert("map");
}
//<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
//<script type="text/javascript">
//    window.onload = 
function initMap() {
    src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyBeZsRxVhJ__8kmVandHXR7UvI4kvGRzgs&callback=initMap"

    var mapOptions = {
        center: new google.maps.LatLng(53.1424, -7.6921),
        zoom: 7,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var infoWindow = new google.maps.InfoWindow();
    var latlngbounds = new google.maps.LatLngBounds();
    var map = new google.maps.Map(document.getElementById("selectMap"), mapOptions);

    google.maps.event.addListener(map, 'click', function (e) {
        // alert("Latitude: " + e.latLng.lat() + "\r\nLongitude: " + e.latLng.lng());
        myFunction(e);
        //$('#divConfirmDialog').dialog('open');
    });
}

function myFunction(e) {
    var txt;
    var lat = e.latLng.lat();
    var lng = e.latLng.lng();

    $.ajax({
        type: "GET",
        url: '@Url.Action("FindNearestTownId", "Analysis")',
        contentType: "application/json; charset=utf-8",
        data: { lat: e.latLng.lat(), lng: e.latLng.lng() },
        dataType: "json",
        success: function (Redirect) {
            // alert(Redirect.to)
            window.location = Redirect.url;
        },
        error: function () {
            alert('error happened');
        }
    });



    //$.ajax({
    //    type: "GET",
    //    url: '@Url.Action("FindNearestTownId", "Analysis")',
    //    contentType: "application/json; charset=utf-8",
    //    data: { lat: e.latLng.lat(), lng: e.latLng.lng() },
    //    dataType: "json",
    //    success: function (Redirect) {
    //        alert(Redirect.to)
    //        window.location = Redirect.url;
    //    },
    //    error: function () {
    //        alert('error happened');
    //    }
    //});
}


function generateChart() {
    var id = document.getElementById('townId').value
    var myUrl = "/Analysis/GetChartData/" + id

    AmCharts.makeChart("chartdiv",
				{
				    "type": "serial",
				    "categoryField": "ds",
				    "dataDateFormat": "YYYY-MM",
				    "backgroundColor": "#BBBBBB",
				    "borderColor": "#CC0000",
				    "percentPrecision": 0,
				    "precision": 0,
				    "theme": "default",
				    "categoryAxis": {
				        "minPeriod": "MM",
				        "parseDates": true
				    },
				    "valueScrollbar": {
				        "enabled": true,
				        "backgroundAlpha": 0,
				        "backgroundColor": "#DA2222",
				        "offset": 12,
				        "oppositeAxis": false,
				        "scrollbarHeight": 10
				    },
				    "chartCursor": {
				        "enabled": true,
				        "categoryBalloonDateFormat": "MMM YYYY",
				        "pan": true,
				        "valueBalloonsEnabled": false,
				        "valueLineAlpha": 0
				    },
				    "chartScrollbar": {
				        "enabled": true
				    },
				    "trendLines": [],
				    "graphs": [
						{
						    "balloonText": "€ [[value]]",
						    "bullet": "round",
						    "bulletAlpha": 0.99,
						    "bulletBorderAlpha": 0.65,
						    "bulletBorderColor": "#FF0000",
						    "bulletColor": "#FFFFFF",
						    "id": "AmGraph-2",
						    "lineColor": "#1A350A",
						    "lineThickness": 2,
						    "title": "Price",
						    "type": "smoothedLine",
						    "valueField": "name"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "autoRotateAngle": 1.8,
						    "fillAlpha": 0.03,
						    "gridAlpha": 0.21,
						    "gridColor": "#1A350A",
						    "gridCount": 0,
						    "offset": 50,
						    "title": "Avg. Price  €"
						}
				    ],
				    "allLabels": [],
				    "balloon": {
				        "animationDuration": 1.02,
				        "fadeOutDuration": 0.52,
				        "fixedPosition": false,
				        "offsetX": 3,
				        "offsetY": 12
				    },
				    "legend": {
				        "enabled": true,
				        "useGraphSettings": true
				    },
				    "titles": [
						{
						    "color": "#2F4F1C",
						    "id": "Title-1",
						    "size": 15,
						    "text": "Property Prices"
						}
				    ],
				    "dataLoader": {
				        "url": myUrl, // controller function call
				        "format": "json",
				    }
				});

    chart.addListener("rendered", zoomChart);

    zoomChart();

    function zoomChart() {
        chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
    }
}

function generateCharts() {
    //var id = document.getElementById('townId').value
    var id = townId;
    var crimeUrl = "/Analysis/GetCrimeData/" + id
    var chart1 = AmCharts.makeChart("chartcrime",
				{
				    "type": "serial",
				    "categoryField": "Year",
				    "dataDateFormat": "YYYY",
				    "theme": "default",
				    "categoryAxis": {
				        "minPeriod": "YYYY",
				        "parseDates": true
				    },
				    "chartCursor": {
				        "enabled": true,
				        "animationDuration": 0,
				        "categoryBalloonDateFormat": "YYYY",
				        "oneBalloonOnly": true
				    },
				    "chartScrollbar": {
				        "enabled": true
				    },
				    "trendLines": [],
				    "graphs": [
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "round",
						    "id": "AmGraph-1",
						    "lineThickness": 2,
						    "title": "Attempted assaults/murder",
						    "type": "smoothedLine",
						    "valueField": "NumAttemptedMurderAssault"
						},
						{
						    "balloonText": "[[title]]  [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-2",
						    "lineThickness": 2,
						    "title": "Burglaries",
						    "type": "smoothedLine",
						    "valueField": "NumBurglary"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "bubble",
						    "id": "AmGraph-3",
						    "lineThickness": 2,
						    "title": "Damage",
						    "type": "smoothedLine",
						    "valueField": "NumDamage"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "bubble",
						    "id": "AmGraph-4",
						    "lineThickness": 2,
						    "title": "Dangerous Acts",
						    "type": "smoothedLine",
						    "valueField": "NumDangerousActs"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "bubble",
						    "id": "AmGraph-5",
						    "lineThickness": 2,
						    "title": "Controlled drug",
						    "type": "smoothedLine",
						    "valueField": "NumDrugs"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "bubble",
						    "hidden": true,
						    "id": "AmGraph-6",
						    "lineThickness": 2,
						    "title": "Fraud",
						    "topRadius": 0,
						    "type": "smoothedLine",
						    "valueField": "NumFraud"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "hidden": true,
						    "id": "AmGraph-7",
						    "lineThickness": 2,
						    "title": "Government",
						    "type": "smoothedLine",
						    "valueField": "NumGovernment"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-8",
						    "lineThickness": 2,
						    "title": "Public Order",
						    "type": "smoothedLine",
						    "valueField": "NumPublicOrder"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-9",
						    "lineThickness": 2,
						    "title": "Robbery",
						    "type": "smoothedLine",
						    "valueField": "NumRobbery"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-10",
						    "lineColor": "#61306A",
						    "lineThickness": 2,
						    "title": "Theft",
						    "type": "smoothedLine",
						    "valueField": "NumTheft"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-11",
						    "lineThickness": 2,
						    "title": "Weapons",
						    "type": "smoothedLine",
						    "valueField": "NumWeapons"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-12",
						    "lineThickness": 2,
						    "title": "Kidnapping",
						    "type": "smoothedLine",
						    "valueField": "NumKidnapping"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "title": "Num.recorded offences"
						}
				    ],
				    "allLabels": [
						{
						    "id": "Label-1"
						}
				    ],
				    "balloon": {},
				    "legend": {
				        "enabled": true,
				        "useGraphSettings": true
				    },
				    "titles": [
						{
						    "id": "Title-1",
						    "size": 15,
						    "text": "Crime Statistics"
						}
				    ],
				    "dataLoader": {
				        "url": crimeUrl, // controller function call
				        "format": "json",
				    }
				});
    var chart2 = AmCharts.makeChart("chartdiv",
				{
				    "type": "serial",
				    "categoryField": "date_sold",
				    "dataDateFormat": "YYYY-MM",
				    "backgroundColor": "#BBBBBB",
				    "borderColor": "#CC0000",
				    "percentPrecision": 0,
				    "precision": 0,
				    "theme": "default",
				    "categoryAxis": {
				        "minPeriod": "MM",
				        "parseDates": true
				    },
				    "valueScrollbar": {
				        "enabled": true,
				        "backgroundAlpha": 0,
				        "backgroundColor": "#DA2222",
				        "offset": 12,
				        "oppositeAxis": false,
				        "scrollbarHeight": 10
				    },
				    "chartCursor": {
				        "enabled": true,
				        "categoryBalloonDateFormat": "MMM YYYY",
				        "pan": true,
				        "valueBalloonsEnabled": false,
				        "valueLineAlpha": 0
				    },
				    "chartScrollbar": {
				        "enabled": true
				    },
				    "trendLines": [],
				    "graphs": [
						{
						    "balloonText": "€ [[value]]",
						    "bullet": "round",
						    "bulletAlpha": 0.99,
						    "bulletBorderAlpha": 0.65,
						    "bulletBorderColor": "#FF0000",
						    "bulletColor": "#FFFFFF",
						    "id": "AmGraph-2",
						    "lineColor": "#1A350A",
						    "lineThickness": 2,
						    "title": "Price",
						    "type": "smoothedLine",
						    "valueField": "avg_price"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "autoRotateAngle": 1.8,
						    "fillAlpha": 0.03,
						    "gridAlpha": 0.21,
						    "gridColor": "#1A350A",
						    "gridCount": 0,
						    "offset": 50,
						    "title": "Avg. Price  €"
						}
				    ],
				    "allLabels": [],
				    "balloon": {
				        "animationDuration": 1.02,
				        "fadeOutDuration": 0.52,
				        "fixedPosition": false,
				        "offsetX": 3,
				        "offsetY": 12
				    },
				    "legend": {
				        "enabled": true,
				        "useGraphSettings": true
				    },
				    "titles": [
						{
						    "color": "#2F4F1C",
						    "id": "Title-1",
						    "size": 15,
						    "text": "Property Prices"
						}
				    ],
				    "dataLoader": {
				        "url": "/Analysis/GetChartData/" + id, // controller function call
				        "format": "json",
				    }
				});

}
