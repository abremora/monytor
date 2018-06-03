var apiUrl = "http://localhost:51736/api/series";
var chartNumber = 0;
var allGroups = new Array();
var groupToTagArray = new Array();
var charts = new Array();

$(document).ready(function () {
    $.ajax({
        url: apiUrl
    }).then(function (data) {
        var index;

        groupToTagArray = new Array(); 
        for (index = 0; index < data.length; ++index) {
            allGroups.push(data[index]);
                                           
            var tages = data[index].value;
            groupToTagArray.push(tages);
        }
    });  
})

var createChart = function (canvas) {
    var ctx = canvas.getContext('2d');
    var chart = new Chart(ctx, {
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
    return chart;
};

var randomColorGenerator = function () {
    return '#' + (Math.random().toString(16) + '0000000').slice(2, 8);
};

var setTagsFromArray = function (tagIndex, tagElement) {
    var tages = groupToTagArray[tagIndex];
    tagElement.empty();
    for (tagIndex = 0; tagIndex < tages.length; ++tagIndex) {
        tagElement.append($('<option>', {
            value: tagIndex,
            text: tages[tagIndex]
        }));
    }
};

$("#addView").click(function () {   
    var data = {
        chartnr: chartNumber
    }    

    var template = $("#chartTemplate")[0];
    var html = Mustache.render(template.innerHTML, data);
    var wrapper = document.createElement('div');
    wrapper.innerHTML = html;

    var today = moment();
    $(wrapper).find("#start").val(moment(today).subtract(1, 'days').format('YYYY-MM-DD'));
    $(wrapper).find("#end").val(today.format('YYYY-MM-DD'));
  
    var canvas = $(wrapper).find("canvas")[0];
    $("#chartArea").append(wrapper);
    var newChart = createChart(canvas); 

    charts.push(newChart);

    chartNumber++;

    var group = $(wrapper).find("#group");
    var tag = $(wrapper).find("#tag");
    var add = $(wrapper).find("#add");

    for (index = 0; index < allGroups.length; ++index) {
        group.append($('<option>', {
            value: index,
            text: allGroups[index].key
        }));
    }   

    group.on('change', function () {
        var tagIndex = this.value;
        setTagsFromArray(tagIndex, tag);
    });

    add.click(addClick);

    setTagsFromArray(0, tag);
});

var addClick = function (event) {
    var modelGroup = $(this).closest(".form-group");
    var group = modelGroup.find("#group option:selected").text();
    var tag = modelGroup.find("#tag option:selected").text();

    var end = modelGroup.find("#end").val(); 
    var endDay = moment(end).add(1, 'day').subtract(1, 'second').toISOString();

    var url = $("#url").val() + "/"
        + $("#start").val() + "/"
        + endDay + "/"
        + group + "/"
        + tag;

    var chartnr = $(this).closest(".chartWrapper").find("canvas").attr("data-chartnr");

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

        

        var chart = charts[chartnr];
        chart.data.datasets.push({
            label: group + ": " + tag,
            borderColor: color,
            backgroundColor: Color(color).alpha(0.5).rgbString(),
            fill: false,
            data: result
        });
        chart.data.labels = timeLabel;
        chart.options.scales.xAxes[0].time.unit = unit;
        chart.update();
    });
};