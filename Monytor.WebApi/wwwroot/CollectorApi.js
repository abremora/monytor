var charts = new Array();
var defaultTimeRangeInDays = 3;

var nonNumericConfig = {
    type: 'line',    
    options: {
        responsive: true,
        title: {
            display: true            
        },
        scales: {
            xAxes: [{
                display: true,
                scaleLabel: {
                    display: true
                },
                offset: false,
                bounds: 'ticks',
                type: 'time',
                time: {
                    unit: 'day',
                    distribution: 'linear',
                    stepSize: 1
                }
            }],
            yAxes: [{
                type: 'category',
                position: 'left',
                display: true,
                scaleLabel: {
                    display: false
                },
                ticks: {
                    reverse: true,
                    callback: function (value, index, values) {
                        return value.length > 15 ? value.substring(0, 15) + '...' : value;
                    },
                    autoSkip: true,
                    autoSkipPadding: 10,
                    source: 'auto',
                    maxTicksLimit: 10
                }
            }]
        }
    }
};

var getdefaultApiUrl = function () {
    return window.location.origin + "/api";
};

var getSeriesApiUrl = function () {
    return sessionStorage.defaultSourceUrl + "/series";
};

var cb = function(start, end) {
    $('#dateRangePicker span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
}

var updateAllCharts = function(ev, picker) {
    console.log("it works");

    charts = new Array();
    $("#chartArea").empty();
    var dashboard = new Dashboard().load();

    dashboard.views.forEach(function(x) {
        var collectors = x.collectors;
        collectors.forEach(function(y) {
            y.start = picker.startDate.format('YYYY-MM-DD');
            y.end = picker.endDate.format('YYYY-MM-DD');
            console.log(y.start);
            console.log(y.end);
        });
    });

    new Dashboard().save(dashboard);

    loadFromStore();
}

$(document).ready(function () {

    $('#daterange').daterangepicker({
        startDate: moment(),
        endDate: moment(),
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    $('#daterange').on('apply.daterangepicker', updateAllCharts);

    cb(moment(), moment());

    if (typeof(Storage) === "undefined") {
        alert("No support for Web Storage. Please update your browser.");
    }

    if (!sessionStorage.defaultSourceUrl) {
        sessionStorage.defaultSourceUrl = getdefaultApiUrl();
    }
    if (!sessionStorage.defaultTimeRangeInDays) {
        sessionStorage.defaultTimeRangeInDays = defaultTimeRangeInDays;
    }

    loadAllSeriesGroupsViaUrl(getSeriesApiUrl());

    var currentUrl = new URL(window.location.href);
    var viewId = currentUrl.searchParams.get("view");
    if (viewId !== null) {
        loadDashboardFromDb(viewId);
        return;
    }

    if (!sessionStorage.dashboard)
        sessionStorage.dashboard = JSON.stringify(new Dashboard());
    else {
        loadFromStore();
    }
    updateAllCharts(null, $('#daterange'));
    console.log(charts);
});

$("#addViewButton").click(function () {
    var indexOfNewView = saveNewView();

    var charttype = $("#charttype option:selected").text();
    var linkId = insertChart(indexOfNewView, charttype);
    addCollectorForNewView(indexOfNewView, linkId);
    updateChartConfigDialog(indexOfNewView, linkId);
});

$("#group").change(function () {
    var tagIndex = this.value;
    var groupTagData = getDefaultJsonGroupTagDataFromStore();
    var selectedGroup = groupTagData[tagIndex];
    var tag = $("#tag");
    setTagsFromArray(selectedGroup.value, tag, null);
});

$("#addView").click(function () {
    var today = moment();

    var start = $("#start");
    start.val(moment(today).subtract(sessionStorage.defaultTimeRangeInDays, 'days').format('YYYY-MM-DD'));
    var end = $("#end");
    end.val(today.format('YYYY-MM-DD'));
});

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
    var dashboard = new Dashboard().load();
    for (chartNumber = 0; chartNumber < dashboard.views.length; chartNumber++) {
        var chartType = dashboard.views[chartNumber].collectors[0].chartType;
        var linkId = insertChart(chartNumber, chartType);
        updateChartConfigDialog(chartNumber, linkId);

        var collectorIndex = 0;
        for (collectorIndex = 0; collectorIndex < dashboard.views[chartNumber].collectors.length; collectorIndex++) {
            addCollector(linkId, collectorIndex);
        }
    }
};

var loadAllSeriesGroupsViaUrl = function (url) {
    if (url === null) return;

    $.ajax({
        url: url
    }).then(function (data) {
        setDefaultJsonGroupTagDataToStore(data);

        var groupControl = $(document).find("#group");
        var tagControl = $(document).find("#tag");
        setGroupTagDataToControls(groupControl, tagControl, null, null);
    }).catch((err) => {
        alert("'" + err.status + " " + err.statusText + "' " + "for: " + url);
    });
};

var setGroupTagDataToControls = function (groupControl, tagControl, groupSelected, tagSelected) {
    groupControl.empty();
    tagControl.empty();

    var jsonData = getDefaultJsonGroupTagDataFromStore();
    if (jsonData === null) return;

    var selectedIndex = 0;
    for (var index = 0; index < jsonData.length; ++index) {
        var groupText = jsonData[index].key;
        groupControl.append($('<option>', {
            value: index,
            text: groupText
        }));
        if (groupText === groupSelected) {
            selectedIndex = index;
        }
    }

    var tagValue = jsonData[0].value;
    if (selectedIndex > 0 && selectedIndex < jsonData.length) {
        groupControl.val(selectedIndex);
        tagValue = jsonData[selectedIndex].value;
    }

    setTagsFromArray(tagValue, tagControl, tagSelected);
};

var setDefaultJsonGroupTagDataToStore = function (jsonData) {
    sessionStorage.defaultGroupTagData = JSON.stringify(jsonData);
};

var getDefaultJsonGroupTagDataFromStore = function () {
    if (sessionStorage.defaultGroupTagData === undefined) return null;
    return JSON.parse(sessionStorage.defaultGroupTagData);
};

var createEmptyChart = function (canvas, charttype) {
    if (charttype === "undefined")
        charttype = "line";

    var ctx = canvas.getContext('2d');
    var chart = new Chart(ctx, {
        type: charttype.toLowerCase(),
        options: {
            responsive: true,
            scales: {
                yAxes: [{
                    ticks: {
                        //steps: 5,
                        //stepValue: 5
                    }
                }],
                xAxes: [{
                    display: true,
                    offset: false,
                    bounds: 'ticks',
                    scaleLabel: {
                        display: true                       
                    },
                    type: 'time',
                    time: {
                        unit: 'day',
                        distribution: 'linear',
                        stepSize: 1,
                        displayFormats: {
                            day: 'YY-MM-DD'
                        }
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    ticks: {
                        autoSkip: true,
                        autoSkipPadding: 0,
                        source: 'auto'
                    }
                }]
            }
        }
    });
    return chart;
};

var randomColorGenerator = function () {
    return '#' + (Math.random().toString(16) + '0000000').slice(2, 8);
};

var setTagsFromArray = function (tages, tagElement, tagSelected) {
    tagElement.empty();
    var selectedIndex = 0;
    for (tagIndex = 0; tagIndex < tages.length; ++tagIndex) {
        var tagText = tages[tagIndex];
        tagElement.append($('<option>', {
            value: tagIndex,
            text: tagText
        }));

        if (tagText === tagSelected) {
            selectedIndex = tagIndex;
        }
    }

    if (selectedIndex > 0 && selectedIndex < tages.length) {
        tagElement.val(selectedIndex);        
    } 
};

var insertChart = function (chartNumber, chartType) {
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
        
    var canvas = $(wrapper).find("canvas")[0];
    $("#chartArea").append(wrapper);   
    var newChart = createEmptyChart(canvas, chartType);
    var chartView = {
        key: linkId,
        value: newChart
    };
    charts[linkId] = chartView;
    return linkId;
};

var updateChartConfigDialog = function (chartNumber, linkId) {
    var dashboard = new Dashboard().load();
    if (dashboard.views.length === 0) return;

    var view = dashboard.views[chartNumber];
    if (view.collectors.length === 0) return;

    var collector = view.collectors[0];
    var wrapper = $("#" + linkId);
    var start = $(wrapper).find("#start" + linkId);
    var end = $(wrapper).find("#end" + linkId);
    var group = $(wrapper).find("#group" + linkId);
    var tag = $(wrapper).find("#tag" + linkId);
    var chartType = $(wrapper).find("#charttype" + linkId);

    setGroupTagDataToControls(group, tag, collector.group, collector.tag);

    start.val(moment(collector.start).format(moment.HTML5_FMT.DATE));
    end.val(moment(collector.end).format(moment.HTML5_FMT.DATE));

    chartType.val(collector.chartType);

    group.on('change', function () {
        var tagIndex = this.value;
        var groupTagData = getDefaultJsonGroupTagDataFromStore();
        var selectedGroup = groupTagData[tagIndex];
        var tag = $("#tag" + linkId);
        setTagsFromArray(selectedGroup.value, tag, null);
    });

    var add = $(wrapper).find("#update" + linkId);
    add.click(updateClick);

    var close = $(wrapper).find(".chart-close");
    close.click(closeChart);
};

$("#updateGroupTag").click( function () {
});

$("#closeViews").click(function () {
    charts = new Array();
    $("#chartArea").empty();
    new Dashboard().save(new Dashboard());
});

var addCollector = function (linkId, collectorIndex) {
    var viewRoot = $("#" + linkId);
    var viewIndex = getElementIndex(viewRoot);

    var dashboard = new Dashboard().load();
    var collector = dashboard.views[viewIndex].collectors[collectorIndex];

    var group = collector.group;
    var tag = collector.tag;
    var start = collector.start;
    var end = collector.end;
    var meanValueType = collector.meanValueType;

    addCollectorForValues(linkId, group, tag, start, end, meanValueType);
};

var updateCollector = function (linkId, collectorIndex) {
    var viewRoot = $("#" + linkId);
    var viewIndex = getElementIndex(viewRoot);

    var dashboard = new Dashboard().load();
    var collector = dashboard.views[viewIndex].collectors[collectorIndex];

    var group = collector.group;
    var tag = collector.tag;
    var start = collector.start;
    var end = collector.end;
    var meanValueType = collector.meanValueType;

    var chart = charts[linkId].value;
    chart.data.datasets = [];

    addCollectorForValues(linkId, group, tag, start, end, meanValueType);
};

var addChart = function (linkId, timeLabel, group, tag, result, isNumeric) {
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
    if (!isNumeric) {
        var distinct = [...new Set(result)];
        chart.options.scales.yAxes = nonNumericConfig.options.scales.yAxes;
        chart.data.yLabels = distinct.reverse();
    }
    else {
        chart.options.scales.yAxes = [{ ticks: {} }];
        chart.data.yLabels = null;
    }
    chart.update();
}

var addCollectorForNewView = function (indexOfview, linkId) {
    var dashboard = new Dashboard().load();
    var view = dashboard.views[indexOfview];
    var collector = view.collectors[0];

    var groupSelected = collector.group;
    var tagSelected = collector.tag;
    var start = collector.start;
    var end = collector.end;
    var meanValueType = collector.meanValueType;

    addCollectorForValues(linkId, groupSelected, tagSelected, start, end, meanValueType);

    $("#start" + linkId).val(start);
    $("#end" + linkId).val(end);
    $("#group" + linkId).text(groupSelected);
    $("#tag" + linkId).text(tagSelected);
};

var addCollectorForValues = function (linkId, group, tag, start, end, meanValueType) {
    if (group === "" || tag === "")
        return;

    var apiUrl = getSeriesApiUrl();
    var url = getUrlRequestSeries(apiUrl, start, end, group, tag, meanValueType);

    $.ajax({
        url: url
    }).then(function (data) {

        if (typeof data === 'undefined' && data.length <= 0) {
            console.debug(url + ": result is empty");
            return;
        }

        var result = data.map(a => a.value);
        var time = data.map(a => moment(a.time));

        var isNumeric = result.length > 0 && $.isNumeric(result[0]);

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
        addChart(linkId, timeLabel, group, tag, result, isNumeric);
    });
};

var closeChart = function (event) {
    var viewRoot = $(this).closest(".viewRoot");
    var viewIndex = getElementIndex(viewRoot);

    var dashboard = new Dashboard().load();
    dashboard.views.splice(viewIndex, 1);
    new Dashboard().save(dashboard);

    viewRoot.remove();
};

function getElementIndex(el) {
    var parent = el.parent();
    var children = parent.children();
    return children.index(el);
}

var getUrlRequestSeries = function (apiUrl, start, end, group, tag, meanValueType) {
    var endDay = moment(end).add(1, 'day').subtract(1, 'second').toISOString();
    var tagEscaped = encodeURIComponent(tag);
    var groupEscaped = encodeURIComponent(group);
    var url = apiUrl + "/"
        + start + "/"
        + endDay + "/"
        + groupEscaped + "/"
        + tagEscaped;

    if (meanValueType !== null && meanValueType !== "") {
        url = url + "?meanValueType=" + meanValueType;
    }

    return url;
};

var saveNewView = function () {
    var groupSelected = $("#group option:selected").text();
    var tagSelected = $("#tag option:selected").text();
    var start = $("#start").val();
    var end = $("#end").val();
    var chartType = $("#charttype option:selected").text();
    var meanValueType = $("#meanValueSelect option:selected").val();
    
    var dashboard = new Dashboard().load();
        
    var collectorConfig = new Collector();
    collectorConfig.group = groupSelected;
    collectorConfig.tag = tagSelected;
    collectorConfig.start = start;
    collectorConfig.end = end;
    collectorConfig.chartType = chartType;
    collectorConfig.meanValueType = meanValueType;

    var view = new View();
    view.collectors.push(collectorConfig);
    dashboard.views.push(view);

    new Dashboard().save(dashboard);

    return dashboard.views.length - 1;
};

var updateClick = function (event) {
    var viewRoot = $(this).closest(".viewRoot");

    var linkId = viewRoot.attr("id");

    var group = $(document).find("#group" + linkId + " option:selected").text();
    var tag = $(document).find("#tag" + linkId + " option:selected").text();
    var end = $(document).find("#end" + linkId).val();
    var start = $("#start" + linkId).val();

    var viewIndex = getElementIndex(viewRoot);
    var dashboard = JSON.parse(sessionStorage.dashboard);
    var view = dashboard.views[viewIndex];

    var collector = new Collector();
    collector.group = group;
    collector.tag = tag;
    collector.start = start;
    collector.end = end;

    view.collectors = [];
    view.collectors.push(collector);

    new Dashboard().save(dashboard);

    updateCollector(linkId, view.collectors.length - 1);
};

var delay = (function () {
    var timer = 0;
    return function (callback, args, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms, args);
    };
})();

function Collector() {   
    this.group = "";
    this.tag = "";
    this.start = null;
    this.end = null;
    this.chartType = "line";
    this.meanValueType = null;
}

function View() {
    this.jsonData = null;
    this.collectors = new Array();
}

function Dashboard() {
    this.views = new Array();
}

Dashboard.prototype.load = function () {
    return JSON.parse(sessionStorage.dashboard);
    // TODO: Return type Views instead of anonymous object
};

Dashboard.prototype.save = function (views) {
    // TODO: HACK: Because load() doesnot return typed object 
    sessionStorage.dashboard = JSON.stringify(views);
};