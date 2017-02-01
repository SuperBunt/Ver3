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
        //            "dataProvider": [
        //  {
        //    "ds": "9-2014",
        //    "name": 525000
        //},
        //{
        //    "ds": "9-2013",
        //    "name": 880000
        //},
        //{
        //    "ds": "9-2012",
        //    "name": 302000
        //},
        //{
        //    "ds": "9-2011",
        //    "name": 670000
        //},
        //{
        //    "ds": "8-2014",
        //    "name": 596000
        //},
        //{
        //    "ds": "8-2013",
        //    "name": 280000
        //},
        //{
        //    "ds": "8-2012",
        //    "name": 145000
        //},
        //{
        //    "ds": "8-2011",
        //    "name": 753303.96
        //},
        //{
        //    "ds": "7-2014",
        //    "name": 325000
        //},
        //{
        //    "ds": "7-2013",
        //    "name": 252000
        //},
        //{
        //    "ds": "7-2012",
        //    "name": 383259
        //},
        //{
        //    "ds": "7-2011",
        //    "name": 365000
        //},
        //{
        //    "ds": "6-2014",
        //    "name": 475000
        //},
        //{
        //    "ds": "6-2013",
        //    "name": 295000
        //},
        //{
        //    "ds": "6-2012",
        //    "name": 335000
        //},
        //{
        //    "ds": "6-2011",
        //    "name": 190000
        //},
        //{
        //    "ds": "5-2014",
        //    "name": 232000
        //},
        //{
        //    "ds": "5-2013",
        //    "name": 370000
        //},
        //{
        //    "ds": "5-2012",
        //    "name": 215000
        //},
        //{
        //    "ds": "5-2011",
        //    "name": 263000
        //},
        //{
        //    "ds": "4-2014",
        //    "name": 440529
        //},
        //{
        //    "ds": "4-2013",
        //    "name": 300000
        //},
        //{
        //    "ds": "4-2012",
        //    "name": 220000
        //},
        //{
        //    "ds": "4-2011",
        //    "name": 157500
        //},
        //{
        //    "ds": "3-2014",
        //    "name": 550000
        //},
        //{
        //    "ds": "3-2013",
        //    "name": 275428
        //},
        //{
        //    "ds": "3-2012",
        //    "name": 340000
        //},
        //{
        //    "ds": "3-2011",
        //    "name": 165000
        //},
        //{
        //    "ds": "2-2015",
        //    "name": 370044
        //},
        //{
        //    "ds": "2-2014",
        //    "name": 237500
        //},
        //{
        //    "ds": "2-2013",
        //    "name": 216000
        //},
        //{
        //    "ds": "2-2012",
        //    "name": 325991.19
        //},
        //{
        //    "ds": "2-2011",
        //    "name": 234000
        //},
        //{
        //    "ds": "12-2014",
        //    "name": 950000
        //},
        //{
        //    "ds": "12-2013",
        //    "name": 655000
        //},
        //{
        //    "ds": "12-2012",
        //    "name": 260000
        //},
        //{
        //    "ds": "12-2011",
        //    "name": 400000
        //},
        //{
        //    "ds": "1-2015",
        //    "name": 320010
        //},
        //{
        //    "ds": "1-2014",
        //    "name": 195000
        //},
        //{
        //    "ds": "1-2013",
        //    "name": 343612.33
        //},
        //{
        //    "ds": "1-2012",
        //    "name": 285000
        //},
        //{
        //    "ds": "1-2011",
        //    "name": 270000
        //},
        //{
        //    "ds": "11-2014",
        //    "name": 682819
        //},
        //{
        //    "ds": "11-2013",
        //    "name": 315000
        //},
        //{
        //    "ds": "11-2012",
        //    "name": 630000
        //},
        //{
        //    "ds": "11-2011",
        //    "name": 771000
        //},
        //{
        //    "ds": "10-2014",
        //    "name": 615000
        //},
        //{
        //    "ds": "10-2013",
        //    "name": 398230.07
        //},
        //{
        //    "ds": "10-2012",
        //    "name": 250000
        //},
        //{
        //    "ds": "10-2011",
        //    "name": 260000
        //}
        //]
    });

    chart.addListener("rendered", zoomChart);

    zoomChart();

    function zoomChart() {
        chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
    }

}

function generatePie() {
    

    alert("hello");
    AmCharts.makeChart("chartdiv", {
        "type": "pie",
        "balloonText": "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>",
        "titleField": "category",
        "valueField": "column-1",
        "allLabels": [],
        "balloon": {},
        "legend": {
            "enabled": true,
            "align": "center",
            "markerType": "circle"
        },
        "titles": [],
        "dataProvider": [
            {
                "category": "category 1",
                "column-1": 8
            },
            {
                "category": "category 2",
                "column-1": 6
            }
        ]
    });

}