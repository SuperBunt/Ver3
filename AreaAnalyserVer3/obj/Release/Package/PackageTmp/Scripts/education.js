function loadGraph() {
    //alert("Generating graph");

    // var myUrl = "/Education/FeederTable?id=3486";

    //pieChart.dataLoader.loadData()

}

var markers = [];


//var id = '@Model.Town.TownId';



function initMap() {
    var myLatLong = { lat: latitude, lng: longitude };
    var mapOptions = {
        center: myLatLong,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var infoWindow = new google.maps.InfoWindow();
    var latlngbounds = new google.maps.LatLngBounds(new google.maps.LatLng(54.668922, -7.0100274), new google.maps.LatLng(55.121921664, -6.5620904));
    var map = new google.maps.Map(document.getElementById("localMap"), mapOptions);
    setMarkers(map, markers);

}

var filter = [];

function setMarkers(map, markers) {
    console.log("Setting markers");
    console.log(markers);
    for (var i = 0; i < markers.length; i++) {
        //alert("success getting marker "+[i]);
        var data = markers[i];
        var siteLatLng = new google.maps.LatLng(data.Latitude, data.Longitude);
        var marker = new google.maps.Marker({
            position: siteLatLng,
            map: map,
            title: data.Description,
            icon: data.icon,
            category: data.Category
        });
        var contentString = "Some content";

        filter.push(marker);

        google.maps.event.addListener(marker, "click", function (e) {
            //myFunction(this.id);
            infowindow.setContent("Map: " + this.position + "\nName: " + this.title);
            infowindow.open(map, this);
        });
    }
}



function filterMarkers(category) {
    console.log('Filtering markers');
    for (i = 0; i < markers.length; i++) {
        console.log(markers[i].title);
        filterMarker = filter[i];
        // If is same category or category not picked
        if (filterMarker.category == category || category.length === 0) {
            filterMarker.setVisible(true);
        }
            // Categories don't match 
        else {
            filterMarker.setVisible(false);
        }
    }
}

var pieChart = AmCharts.makeChart("graph",
				{
				    "type": "pie",
				    "balloonText": "[[title]]<br><span style='font-size:14px'>[[percents]]%</span>",
				    "gradientType": "linear",
				    "innerRadius": 25,
				    "labelRadius": -45,
				    "labelText": "[[title]]",
				    "minRadius": 80,
				    "baseColor": "#5BCF5B",
				    "gradientRatio": [],
				    "labelTickAlpha": 0,
				    "outlineAlpha": 1,
				    "outlineColor": "#2C1616",
				    "outlineThickness": 2,
				    "titleField": "category",
				    "valueField": "val",
				    "borderAlpha": 0.59,
				    "fontSize": 20,
				    "processCount": 998,
				    "theme": "default",
				    "allLabels": [],
				    "balloon": {},
				    "titles": [
						{
						    "color": "#ffffff",
						    "id": "Title-1",
						    "size": 16,
						    "tabIndex": 0,
						    "text": "% Students who progressed to college"
						}
				    ],
				    marginTop: 0,
				    marginBottom: 0,
				    marginLeft: 0,
				    marginRight: 0,
				    "dataLoader": {
				        "url": "/Education/ProgressionPie?id=189", // controller function call
				        "format": "json",
				        "load": function (options, chart) {
				            console.log('Loaded file: ' + options.url);
				            //alert("Loaded Pie");
				        },
				        "error": function (options, chart) {
				            console.log('Ummm something went wrong loading this file: ' + options.url);
				            alert("Error");
				        },
				    }
				});

var barChart = AmCharts.makeChart("bar",
           {
               "type": "serial",
               "categoryField": "College",
               "columnSpacing": 15,
               "columnWidth": 0.69,
               "rotate": true,
               "autoResize": true,
               "startDuration": 1,
               "categoryAxis": {
                   "gridPosition": "start"
               },
               "chartCursor": {
                   "enabled": true
               },
               "chartScrollbar": {
                   "enabled": true
               },
               "trendLines": [],
               "graphs": [
                   {
                       "fillAlphas": 0.48,
                       "fillColors": "#008000",
                       "id": "AmGraph-1",
                       "lineAlpha": 0.8,
                       "lineColor": "#008000",
                       "lineThickness": 2,
                       "title": "graph 1",
                       "type": "column",
                       "valueField": "Attendees"
                   }
               ],
               "guides": [],
               "valueAxes": [
                   {
                       "id": "ValueAxis-1",
                       "autoGridCount": false,
                       "title": "Attendees"
                   }
               ],
               "allLabels": [],
               "balloon": {},
               "titles": [
                   {
                       "id": "Title-1",
                       "size": 15,
                       "text": "Who went where."
                   }
               ],
               "dataLoader": {
                   "url": "/Education/FeederTable?id=189", // controller function call
                   "format": "json",
                   "load": function (options, chart) {
                       console.log('Loaded file: ' + options.url);
                       // alert("Loaded bar chart");
                   },
                   "error": function (options, chart) {
                       console.log('Ummm something went wrong loading this file: ' + options.url);
                       alert("Error");
                   },
               }
           });

var feederData;

function refreshData(SchoolId, Name, bool) {


    var allCharts = AmCharts.charts;

    $.ajax({
        async: false,
        type: "POST",
        dataType: "json",
        url: "/Education/FeederTable?id=" + SchoolId,
        data: '{}',
        success: function (result) {
            console.log('Success getting feeder table data.');
            //feederData = result;
            barChart.dataProvider = result;
            barChart.validateData();
            $('#schoolName').text(Name);
            transition(bool);
        },
        "error": function (options, chart) {
            console.log('Ummm something went wrong loading this file: ' + options.url);
            alert("Sorry, no data available for this school.");
        }
    });

    $.ajax({
        async: false,
        type: "POST",
        dataType: "json",
        url: "/Education/ProgressionPie?id=" + SchoolId,
        data: '{}',
        success: function (result) {
            console.log('Success loading pie chart data: ' + options.url);
            pieChart.dataProvider = result;
            pieChart.validateData();
        }
    });

}

var tableshowing;

function transition(bool) {
    $("#schoolTable").toggle("slide", { direction: "left" }, 880)
    $("#feederGraph").toggle("slide", { direction: "right" }, 880);
    console.log('transistion between schools');
}

function togglePpr() {
    // Allows for smooth transition between table and graph   
    $("#chartdiv").toggle("slide", { direction: "left" }, 880)
    $("#pprTable").toggle("slide", { direction: "right" }, 880);
    $("#PprBtn").find('i').toggleClass('fa-list fa-line-chart');
    $("#PprBtn").find('span').text(function (i, text) {
        return text === "View table" ? "View graph" : "View table";
    })
    console.log('transistion between ppr');
}

function register() {
    var txt = "Sorry, this option is only available to members.\nSign up now to gain full access to local crime and school data!!";
    var r = confirm(txt);
    if (r == true) {
        window.location = '/Account/Register';
    } else {
        die();
    }
}

function toggleCrime() {
    $("#chartcrime").toggle("slide", { direction: "left" }, 880)
    $("#crimeTable").toggle("slide", { direction: "right" }, 880);
    $("#crimeBtn").find('i').toggleClass('fa-list fa-line-chart');
    console.log('transistion between crime');
}



