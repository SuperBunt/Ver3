function loadGraph() {
    //alert("Generating graph");

    // var myUrl = "/Education/FeederTable?id=3486";

    //pieChart.dataLoader.loadData()
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
						    "color": "#008000",
						    "id": "Title-1",
						    "size": 16,
						    "tabIndex": 0,
						    "text": "% Leavers who progressed to college"
						}
				    ],
				    "dataLoader": {
				        "url": "/Education/ProgressionPie?id=3323", // controller function call
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
				        "url": "/Education/FeederTable?id=3323", // controller function call
				        "format": "json",
				        "load": function (options, chart) {
				            console.log('Loaded file: ' + options.url);
				            //alert("Loaded bar chart");
				        },
				        "error": function (options, chart) {
				            console.log('Ummm something went wrong loading this file: ' + options.url);
				            alert("Error");
				        },
				    }
				});
    
}

function refreshData(SchoolId, Name) {
    var allCharts = AmCharts.charts;
    //alert("Refresh graph: " + SchoolId);
    
    $('#schoolName').text(Name);
    $("#charts").show();
    allCharts[0].dataLoader.url = "/Education/ProgressionPie?id=" + SchoolId;
    allCharts[0].dataLoader.loadData();

    allCharts[1].dataLoader.url = "/Education/FeederTable?id=" + SchoolId;
    allCharts[1].dataLoader.loadData();
    //$.ajax({
    //    type: 'GET',
    //    url: myUrl,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: 'json',
    //    //data: { id: SchoolId },
    //    success: function (pieData) {
    //        alert('Success:  \n' + pieData);
    //        // update chart data
    //        //var pieChart = document.getElementById("graph").innerHTML;
    //        pieChart.dataProvider = pieData;
    //        //pieChart.validateData();
                     
    //    },
    //    error: function (ex) {
    //        alert('Failed' + ex);
    //    }
    //});

}