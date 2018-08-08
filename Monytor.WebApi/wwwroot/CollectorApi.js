var charts = new Array();
var defaultTimeRangeInDays = 3;

var getdefaultApiUrl = function () {
    return window.location.origin + "/api";
};

var getSeriesApiUrl = function () {
    return sessionStorage.defaultSourceUrl + "/series";
};

$(document).ready(function () {
    if (typeof (Storage) === "undefined") {
        alert("No support for Web Storage. Please update your browser.");
    }

    if (!sessionStorage.defaultSourceUrl) {
        sessionStorage.defaultSourceUrl = getdefaultApiUrl();
    }
    if (!sessionStorage.defaultTimeRangeInDays) {
        sessionStorage.defaultTimeRangeInDays = defaultTimeRangeInDays;
    }

    var currentUrl = new URL(window.location.href);
    var viewId = currentUrl.searchParams.get("view");
    if (viewId != null) {
        loadViewCollection(viewId);
        return;
    }

    if (!sessionStorage.views)
        sessionStorage.views = JSON.stringify(new Views());
    else {
        loadFromStore();
    }
});

$(function () {
    $('[data-tooltip="tooltip"]').tooltip()
})


$("#saveSettingButton").click(function () {
    var url = $("#defaultSourceUrlId").val();
    sessionStorage.defaultSourceUrl = url;

    var timeRange = $("#defaultTimeRangeId").val();
    sessionStorage.defaultTimeRangeInDays = timeRange;
});

$("#settings").click(function () {
    $("#defaultSourceUrlId").val(sessionStorage.defaultSourceUrl);
    $("#defaultTimeRangeId").val(sessionStorage.defaultTimeRangeInDays);
}); 

var loadFromStore = function () {
    var chartNumber = 0;
    var views = JSON.parse(sessionStorage.views);
    for (chartNumber = 0; chartNumber < views.views.length; chartNumber++) {
        var linkId = insertChart(chartNumber);
        loadAllSeriesGroups(linkId);
        var collectorIndex = 0;
        for (collectorIndex = 0; collectorIndex < views.views[chartNumber].collectors.length; collectorIndex++) {
            addCollector(linkId, collectorIndex);
        }
    }
};

var loadAllSeriesGroups = function (linkId) {
    var input = $(document).find("#url" + linkId);
    var url = input.val();
    $.ajax({
        url: url
    }).then(function (data) {
        setJsonGroupTagDataToStore(linkId, data);
        setJsonGroupTagDataToControls(data, linkId);
    }).catch((err) => {
        alert("'" + err.status + " " + err.statusText + "' " + "for: " + url);
        resetGroupTagControls(linkId);
    });
}

var resetGroupTagControls = function (linkId) {
    var groupControl = $(document).find("#group" + linkId);
    var tagControl = $(document).find("#tag" + linkId);

    groupControl.empty();
    tagControl.empty();
}

var setJsonGroupTagDataToControls = function (jsonData, linkId) {
    resetGroupTagControls(linkId);

    var groupControl = $(document).find("#group" + linkId);
    var tagControl = $(document).find("#tag" + linkId);

    for (var index = 0; index < jsonData.length; ++index) {
        groupControl.append($('<option>', {
            value: index,
            text: jsonData[index].key
        }));
    }

    setTagsFromArray(jsonData[0].value, tagControl);
}

var setJsonGroupTagDataToStore = function (linkId, jsonData) {
    var viewRoot = $("#"+ linkId);
    var viewIndex = getElementIndex(viewRoot);

    var views = JSON.parse(sessionStorage.views);
    var viewConfig = views.views[viewIndex];
    viewConfig.jsonData = JSON.stringify(jsonData);
    sessionStorage.views = JSON.stringify(views);
}

var getJsonGroupTagFromStore = function (linkId) {
    var viewRoot = $("#" + linkId);
    var viewIndex = getElementIndex(viewRoot);

    var views = JSON.parse(sessionStorage.views);
    return JSON.parse(views.views[viewIndex].jsonData);    
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

var setTagsFromArray = function (tages, tagElement) {
    tagElement.empty();
    for (tagIndex = 0; tagIndex < tages.length; ++tagIndex) {
        tagElement.append($('<option>', {
            value: tagIndex,
            text: tages[tagIndex]
        }));
    }
};

var insertChart = function (chartNumber) {
    var linkId = Math.floor((Math.random() * 1000000000) + 1);

    var data = {
        chartnr: chartNumber,
        linkId: linkId
    };

    var template = $("#chartTemplate")[0];
    var html = Mustache.render(template.innerHTML, data);
    var wrapper = document.createElement('div');
    $(wrapper).attr("class", "viewRoot");
    $(wrapper).attr("id", linkId);
    $(wrapper).html(html);
    
    var today = moment();

    var start = $(wrapper).find("#start" + linkId);
    start.val(moment(today).subtract(sessionStorage.defaultTimeRangeInDays, 'days').format('YYYY-MM-DD'));
    var end = $(wrapper).find("#end" + linkId);
    end.val(today.format('YYYY-MM-DD'));
    var group = $(wrapper).find("#group" + linkId);
    var tag = $(wrapper).find("#tag" + linkId);

    var currentUrl = getSeriesApiUrl();
    var input = $(wrapper).find("#url" + linkId);
    input.val(currentUrl);

    input.on('input', function () {
        var linkId = $(this).data("linkid");
        delay(function (args) {
            var linkId = args;
            var input = $(document).find("#url" + linkId);
            var group = $(document).find("#group" + linkId);
            var tag = $(document).find("#tag" + linkId);
            var currentUrl = input.val();

            loadAllSeriesGroups(linkId);
        }, linkId, 1000);
    });

    var canvas = $(wrapper).find("canvas")[0];
    $("#chartArea").append(wrapper);
    var newChart = createChart(canvas);
    var chartView = {
        key: linkId,
        value: newChart
    };
    charts[linkId] = chartView;

    group.on('change', function () {
        var tagIndex = this.value;
        var linkId = $(this).data("linkid");
        var groupTagData = getJsonGroupTagFromStore(linkId);
        var selectedGroup = groupTagData[tagIndex];

        setTagsFromArray(selectedGroup.value, tag);
    });

    var add = $(wrapper).find("#add");
    add.click(addClick);

    var close = $(wrapper).find(".chart-close");
    close.click(closeChart);
    return linkId;
};

$("#addView").click(function () {   
    var views = JSON.parse(sessionStorage.views);
    var chartNumber = views.views.length;

    var viewConfig = new ViewConfig();
    views.views.push(viewConfig);

    new Views().save(views);

    var linkId = insertChart(chartNumber); 
    loadAllSeriesGroups(linkId);
});

$("#closeViews").click(function () {
    charts = new Array();
    $("#chartArea").empty();
    new Views().save(new Views());        
});

var addCollector = function (linkId, collectorIndex) {
    var viewRoot = $("#" + linkId);
    var viewIndex = getElementIndex(viewRoot);

    var views = JSON.parse(sessionStorage.views);

    var collectorConfig = views.views[viewIndex].collectors[collectorIndex];
   
    var group = collectorConfig.group;
    var tag = collectorConfig.tag;
    var start = collectorConfig.start;
    var end = collectorConfig.end;

    var apiUrl = getSeriesApiUrl();
    
    if (group === "" || tag === "")
        return;

    var url = getUrlRequestSeries(apiUrl, start, end, group, tag);

    $.ajax({
        url: url
    }).then(function (data) {

        if (typeof data === 'undefined' && data.length <= 0) {
            console.debug(url + ": result is empty");
            return;
        }

        var result = data.map(a => a.value);
        var time = data.map(a => moment(a.time));

        var maxTime = moment.max(time);
        var minTime = moment.min(time);

        var timeLabel = time.map(x => x.toISOString());

        var duration = moment.duration(maxTime.diff(minTime));
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

        var chart = charts[linkId].value;
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

var closeChart = function (event) {
    var viewRoot = $(this).closest(".viewRoot");
    var viewIndex = getElementIndex(viewRoot);

    var views = new Views().load();
    views.views.splice(viewIndex, 1);
    new Views().save(views);
   
    viewRoot.remove();
}

function getElementIndex(el) {
    var parent = el.parent();
    var children = parent.children();
    return children.index(el);    
}

var getUrlRequestSeries = function (apiUrl, start, end, group, tag) {
    var endDay = moment(end).add(1, 'day').subtract(1, 'second').toISOString();
    var tagEscaped = encodeURIComponent(tag);
    var groupEscaped = encodeURIComponent(group);
    var url = apiUrl + "/"
        + start + "/"
        + endDay + "/"
        + groupEscaped + "/"
        + tagEscaped;  

    return url;
}


var addClick = function (event) {
    var view = $(this).closest(".viewRoot");

    var linkId = view.attr("id");  

    var group = $(document).find("#group" + linkId + " option:selected").text();
    var tag = $(document).find("#tag" + linkId + " option:selected").text();
    var end = $(document).find("#end" + linkId).val();   
    var start = $("#start" + linkId).val();
    var apiUrl = $("#url" + linkId).val();

    var url = getUrlRequestSeries(apiUrl, start, end, group, tag);

    var viewIndex = getElementIndex(view);
    var views = JSON.parse(sessionStorage.views);
    var viewConfig = views.views[viewIndex];

    var collectorConfig = new CollectorConfig();
    collectorConfig.url = apiUrl;
    collectorConfig.group = group;
    collectorConfig.tag = tag;
    collectorConfig.start = start;
    collectorConfig.end = end;

    viewConfig.collectors.push(collectorConfig);

    new Views().save(views);

    addCollector(linkId, viewConfig.collectors.length - 1);  
};

var delay = (function () {
    var timer = 0;
    return function (callback, args, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms, args);
    };
})();

function CollectorConfig() {
    this.url = "";
    this.group = "";
    this.tag = "";
    this.start = null;
    this.end = null;
}

function ViewConfig() {
    this.jsonData = null;
    this.collectors = new Array();
}

function Views() {
    this.views = new Array();    
}

Views.prototype.load = function () {
    return JSON.parse(sessionStorage.views);
    // TODO: Return type Views instead of anonymous object
};

Views.prototype.save = function (views) {
    // TODO: HACK: Because load() doesnot return typed object 
    sessionStorage.views = JSON.stringify(views);
};