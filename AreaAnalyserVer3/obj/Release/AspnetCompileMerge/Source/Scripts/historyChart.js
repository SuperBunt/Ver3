function generateChart() {
    var myUrl = "PriceRegister/GetChartData";
    AmCharts.makeChart("chartdiv", {
        "type": "serial",
        "categoryField": "date",
        "dataDateFormat": "MM-YYYY",
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
                "bullet": "round",
                "bulletSize": 3,
                "cursorBulletAlpha": 0,
                "dateFormat": "MMM , YYYY",
                "id": "AmGraph-1",
                "title": "Cork",
                "type": "smoothedLine",
                "valueField": "Cork"
            },
            {
                "bullet": "round",
                "bulletSize": 2,
                "id": "AmGraph-2",
                "title": "Dublin",
                "type": "smoothedLine",
                "valueField": "Dublin"
            },
            {
                "id": "AmGraph-3",
                "title": "Galway",
                "type": "smoothedLine",
                "valueField": "Galway"
            }
        ],
        "guides": [],
        "valueAxes": [
            {
                "id": "ValueAxis-1",
                "offset": 50,
                "title": "Axis title"
            }
        ],
        "allLabels": [],
        "balloon": {},
        "legend": {
            "enabled": true,
            "useGraphSettings": true
        },
        "titles": [
            {
                "id": "Title-1",
                "size": 15,
                "text": "PPR History"
            }
        ],
        "dataLoader": {
            "url": myUrl, // controller function call
            "format": "json"
        }
        //"dataProvider": [
        //    {
        //        "date": "01-2015",
        //        "Dublin": 5,
        //        "Cork": 8,
        //        "Galway": 3
        //    },
        //    {
        //        "date": "02-2015",
        //        "Dublin": 7,
        //        "Cork": 6,
        //        "Galway": 4
        //    },
        //    {
        //        "date": "03-2015",
        //        "Dublin": 3,
        //        "Cork": 2,
        //        "Galway": 3
        //    },
        //    {
        //        "date": "04-2015",
        //        "Dublin": 3,
        //        "Cork": 1,
        //        "Galway": 4
        //    },
        //    {
        //        "date": "05-2015",
        //        "Dublin": 1,
        //        "Cork": 2,
        //        "Galway": 2
        //    },
        //    {
        //        "date": "06-2015",
        //        "Dublin": 2,
        //        "Cork": 3,
        //        "Galway": 8
        //    },
        //    {
        //        "date": "07-2015",
        //        "Dublin": 8,
        //        "Cork": 6,
        //        "Galway": 8
        //    }
        //]
    });

    chart.addListener("rendered", zoomChart);

    //zoomChart();

    //function zoomChart() {
    //    chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
    //}

}