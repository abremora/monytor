var chartNumber = 0;
var charts = new Array();

var loadAllSeriesGroups = function (chartnr) {
    var input = $(document).find("#url" + chartnr);
    var url = input.val();
    $.ajax({
        url: url
    }).then(function (data) {
        setJsonGroupTagDataAsDataAttribute(chartnr, data);
        setJsonGroupTagDataToControls(data, chartnr);
    }).catch((err) => {
        alert("'" + err.status + " " + err.statusText + "' " + "for: " + url);
        resetGroupTagControls(chartnr);
    });
}

var resetGroupTagControls = function (chartnr) {
    var groupControl = $(document).find("#group" + chartnr);
    var tagControl = $(document).find("#tag" + chartnr);

    groupControl.empty();
    tagControl.empty();
}

var setJsonGroupTagDataToControls = function (jsonData, chartnr) {
    resetGroupTagControls(chartnr);

    var groupControl = $(document).find("#group" + chartnr);
    var tagControl = $(document).find("#tag" + chartnr);

    for (var index = 0; index < jsonData.length; ++index) {
        groupControl.append($('<option>', {
            value: index,
            text: jsonData[index].key
        }));
    }

    setTagsFromArray(jsonData[0].value, tagControl);
}

var setJsonGroupTagDataAsDataAttribute = function (chartnr, jsonData) {
    var groupTagData = getGroupTagFromControl(chartnr);
    groupTagData.data("jsonGroupTag", jsonData);
}

var getGroupTagFromControl = function (chartnr) {
    var groupTagData = $(document).find("#jsonGroupTag" + chartnr);
    return groupTagData;
}

var getJsonGroupTagFromControl = function (chartnr) {
    var groupTagData = getGroupTagFromControl(chartnr);
    var json = groupTagData.data("jsonGroupTag");
    return json;
}

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

var getApiUrl = function () {
    return window.location.origin + "/api/series";
};

var setTagsFromArray = function (tages, tagElement) {
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
   
    var start = $(wrapper).find("#start" + chartNumber);
    start.val(moment(today).subtract(1, 'days').format('YYYY-MM-DD'));
    var end = $(wrapper).find("#end" + chartNumber);
    end.val(today.format('YYYY-MM-DD'));
    var group = $(wrapper).find("#group" + chartNumber);
    var tag = $(wrapper).find("#tag" + chartNumber);

    var currentUrl = getApiUrl();
    var input = $(wrapper).find("#url" + chartNumber);
    input.val(currentUrl);

    input.on('input', function () {
        var chartnr = $(this).data("chartnr");
        delay(function (args) {
            var chartnr = args;
            var input = $(document).find("#url" + chartnr);
            var group = $(document).find("#group" + chartnr);
            var tag = $(document).find("#tag" + chartnr);
            var currentUrl = input.val();

            loadAllSeriesGroups(chartnr);
        }, chartnr, 1000);
    });
    
    var canvas = $(wrapper).find("canvas")[0];
    $("#chartArea").append(wrapper);
    var newChart = createChart(canvas); 

    charts.push(newChart);   
    
    group.on('change', function () {
        var tagIndex = this.value;
        var chartnr = $(this).data("chartnr");
        var groupTagData = getJsonGroupTagFromControl(chartnr);
        var selectedGroup = groupTagData[tagIndex];

        setTagsFromArray(selectedGroup.value, tag);
    });

    loadAllSeriesGroups(chartNumber);

    chartNumber++;
    var add = $(wrapper).find("#add");
    add.click(addClick);
});

var addClick = function (event) {
    var chartnr = $(this).data("chartnr");
    var group = $(document).find("#group" + chartnr + " option:selected").text();
    var groupEscaped = encodeURIComponent(group);
    var tag = $(document).find("#tag" + chartnr + " option:selected").text();
    var tagEscaped = encodeURIComponent(tag);

    var end = $(document).find("#end" + chartnr).val(); 
    var endDay = moment(end).add(1, 'day').subtract(1, 'second').toISOString();

    var url = $("#url" + chartnr).val() + "/"
        + $("#start" + chartnr).val() + "/"
        + endDay + "/"
        + groupEscaped + "/"
        + tagEscaped;   

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

var delay = (function () {
    var timer = 0;
    return function (callback, args, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms, args);
    };
})();