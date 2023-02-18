google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

function getValue(id) {
    var element = $('#chart_div' + id).val();
    $.ajax({
        type: "GET",
        url: "/Admin/Charts/IdeaData?id=" + id,
        data: JSON.stringify({}),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (chartData) {
            var Data = chartData.jsonList;
            if (Data == "null") {                
                document.getElementById('chart_div_' + id).innerText = "No data";
            }
            else if (Data != null) {
                var arrayData = $.parseJSON(Data);
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Interactive');
                data.addColumn('number', 'Value');


                if (arrayData.length > 0) {
                    for (var i = 0; i < arrayData.length; i++) {
                        //data.addRows(arrayData[i].like, arrayData[i].dislike, arrayData[i].comment, arrayData[i].replies, arrayData[i].view);
                        // data.addRows(arrayData[i].like, arrayData[i].dislike, arrayData[i].comment, arrayData[i].replies, arrayData[i].view);
                        data.addRows([
                            ['Like', arrayData[i].like],
                            ['Dislike', arrayData[i].dislike],
                            ['Comments', arrayData[i].comment],
                            ['Replies', arrayData[i].replies],
                            ['View', arrayData[i].view]
                        ]);
                    }
                }
                var chart = new google.visualization.PieChart(document.getElementById('chart_div_' + id));
                chart.draw(data, {
                    title: "Analyze Idea",
                    pointSize: 5,
                    width: 400,
                    height: 240,
                    is3D: true
                });
            }
        },
        error: function () {
            alert("Loading Failed! Please Try Again.");
        }
    })
}
