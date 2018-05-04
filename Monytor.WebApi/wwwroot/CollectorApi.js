var groupToTagArray = new Array();
var apiUrl = "http://localhost:51736/api/serie";

$(document).ready(function () {
    var today = moment();
    $("#start").val(moment(today).subtract(1, 'days').format('YYYY-MM-DD'));
    $("#end").val(today.format('YYYY-MM-DD'));

    var canvas = document.getElementById("myChart");
    window.myChart = createChart(canvas);

    $.ajax({
        url: apiUrl
    }).then(function (data) {
        var index;

        groupToTagArray = new Array(); 
        for (index = 0; index < data.length; ++index) {
            $("#group").append($('<option>', {
                value: index,
                text: data[index].key
            }));
                                  
            var tages = data[index].value;
            groupToTagArray.push(tages);
        }  

        setTagsFromArray(0);
    });
})

$("#group").on('change', function () {
    var tagIndex = this.value;
    setTagsFromArray(tagIndex);
})

var createChart = function (canvas) {
    var ctx = canvas.getContext('2d');
    return new Chart(ctx, {
        type: 'line',
        options: {
            responsive: true,
            scales: {
                yAxes: [{
                    ticks: {
                        steps: 5,
                        stepValue: 5
                    }
                }],
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true
                    },
                    type: 'time',
                    time: {
                        unit: 'day',
                        distribution: 'linear'
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false,
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                }]
            }
        }
    });
};

var randomColorGenerator = function () {
    return '#' + (Math.random().toString(16) + '0000000').slice(2, 8);
};

var setTagsFromArray = function (tagIndex) {
    var tages = groupToTagArray[tagIndex];
    $("#tag").empty();
    for (tagIndex = 0; tagIndex < tages.length; ++tagIndex) {
        $("#tag").append($('<option>', {
            value: tagIndex,
            text: tages[tagIndex]
        }));
    }
};

$("#addView").click(function () {
    var newCanvas = $('<canvas/>', { id: 'mycanvas', height: 200, width: 150 })[0];
    $("#chartArea").append(newCanvas);
    var newChart = createChart(newCanvas,);   
});

$("#add").click(function () {
    var group = $("#group option:selected").text();
    var tag = $("#tag option:selected").text();

    var end = $("#end").val(); 
    var endDay = moment(end).add(1, 'day').subtract(1, 'second').toISOString();

    var url = $("#url").val() + "/"
        + $("#start").val() + "/"
        + endDay + "/"
        + group + "/"
        + tag;

    $.ajax({
        url: url
    }).then(function (data) {

        if (typeof data === 'undefined' && data.length <= 0) {
            console.debug(url + ": result is empty");
            return;
        }

        var result = data.map(a => a.value);
        var time = data.map(a => moment(a.time));

        var max = moment.max(time);
        var min = moment.min(time);

        var timeLabel = time.map(x => x.toISOString());

        var duration = moment.duration(max.diff(min));
        var unit = 'day';
        if (duration.years() > 0) {
            unit = 'year';
        }
        else if (duration.months() > 0) {
            unit = 'month';
        }

        var day = duration.days();


        var min = Math.min(...result),
            max = Math.max(...result);

        var color = randomColorGenerator();

        window.myChart.data.datasets.push({
            label: group + ": " + tag,
            borderColor: color,
            backgroundColor: Color(color).alpha(0.5).rgbString(),
            fill: false,
            data: result
        });
        window.myChart.data.labels = timeLabel;
        window.myChart.options.scales.xAxes[0].time.unit = unit;
        window.myChart.update();
    });
});