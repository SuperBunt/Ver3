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
    });

    chart.addListener("rendered", zoomChart);

    zoomChart();

    function zoomChart() {
        chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
    }

}

function generateCrimeChart() {
    var id = document.getElementById('townId').value
    var myUrl = "/Analysis/GetCrimeData/" + id
    AmCharts.makeChart("chartcrime",
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
				        "categoryBalloonDateFormat": "YYYY"
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
						    "title": "NumAttemptedMurderAssault",
						    "valueField": "NumAttemptedMurderAssault"
						},
						{
						    "balloonText": "[[title]]  [[value]]",
						    "bullet": "square",
						    "id": "AmGraph-2",
						    "title": "NumBurglary",
						    "valueField": "NumBurglary"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-3",
						    "title": "NumDamage",
						    "valueField": "NumDamage"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-4",
						    "title": "NumDangerousActs",
						    "valueField": "NumDangerousActs"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-5",
						    "title": "NumDrugs",
						    "valueField": "NumDrugs"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-6",
						    "title": "NumFraud",
						    "valueField": "NumFraud"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-7",
						    "title": "NumGovernment",
						    "valueField": "NumGovernment"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-8",
						    "title": "NumPublicOrder",
						    "valueField": "NumPublicOrder"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-9",
						    "title": "NumRobbery",
						    "valueField": "NumRobbery"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-10",
						    "lineColor": "#61306A",
						    "title": "NumTheft",
						    "valueField": "NumTheft"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-11",
						    "title": "NumWeapons",
						    "valueField": "NumWeapons"
						},
						{
						    "balloonText": "[[title]] [[value]]",
						    "id": "AmGraph-12",
						    "title": "NumKidnapping",
						    "valueField": "NumKidnapping"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "title": "Axis title"
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
						    "text": "Chart Title"
						}
				    ],
				    "dataLoader": {
				        "url": myUrl, // controller function call
				        "format": "json",
				    }
				});

}