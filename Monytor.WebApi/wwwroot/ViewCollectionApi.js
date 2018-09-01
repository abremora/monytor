var getViewCollectionApiUrl = function () {
    return sessionStorage.defaultSourceUrl + "/ViewCollection";
};

var getUrlForViewCollectionApiWithId = function (id) {
    return getViewCollectionApiUrl() + '/' + id;
};

var getUrlForViewForViewCollectionApiWithId = function (id) {
    return window.location.origin + '/?view=' + id;
};

var loadDashboardFromDb = function (viewId) {
    if (viewId == null)
        return;

    var url = getUrlForViewCollectionApiWithId(viewId);

    $.ajax(url)
        .then(function (data) {
            if (data == null) return;

            var viewCount = data.views.length;
            if (viewCount == 0) return;

            var endDate = moment().utc().endOf('day');
            var dashboard = new Dashboard();

            for (var i = 0; i < viewCount; i++) {
                var view = data.views[i];
                var position = view.position;
                var group = view.group;
                var tag = view.tag;
                var range = view.range;
                var timespan = moment.duration(range);
                var start = endDate.clone().subtract(timespan);
                
                var collector = new Collector();                
                collector.group = group;
                collector.tag = tag;
                collector.end = endDate.utc().toISOString();
                collector.start = start.toISOString();
                collector.chartType = view.chartType;
                collector.meanValueType = view.meanValueType;

                var view = new View();
                view.collectors.push(collector);

                dashboard.views.push(view);
            }

            new Dashboard().save(dashboard);

            loadFromStore();
        }).catch((err) => {
            alert("'" + err.status + " " + err.statusText + "' " + "for: " + url);
        });
};

$("#loadViewCollection").click(function () {
    var url = getViewCollectionApiUrl();
    $.ajax({
        url: url
    }).then(function (data) {
        var items = [];
        for (var i = 0; i < data.length; i++) {
            var configUrl = getUrlForViewForViewCollectionApiWithId(data[i].id);
            var viewCount = 0;
            if (data[i].views != null) viewCount = data[i].views.length;

            items.push('<a href="' + configUrl + '" class="list-group">' +
                '<div class="list-group-item">' +
                '<div class="d-flex justify-content-between align-items-center">' +
                '<h5 class="">' + data[i].name + '</h5>' +
                '<span class="badge badge-primary badge-pill">' + viewCount + '</span>' +
                '</div>' +
                '<p class="">' + data[i].description + '</p>' +
                '</div>' +
                '</a>');
        }

        $("#loadViewListGroup").empty();
        $("#loadViewListGroup").append(items.join(''));

    }).catch((err) => {
        alert("'" + err.status + " " + err.statusText + "' " + "for: " + url);
    });
});

$("#saveViewCollectionModal-save").click(function () {
    var name = $("#saveViewCollectionModal-name").val();
    var description = $("#saveViewCollectionModal-description").val();

    var dashboard = new Dashboard().load();
    var data = {
        "Name": name,
        "Description": description,
        "Views": new Array()
    };
    for (var i = 0; i < dashboard.views.length; i++) {
        if (dashboard.views[i].collectors.length == 0)
            continue;
        var collector = dashboard.views[i].collectors[0];
        var end = moment(collector.end);
        var start = moment(collector.start);
        var diff = end.diff(start);
        var timespan = moment(diff).format("D.HH:mm:ss");
        data.Views.push({
            "Position": i,
            "Group": collector.group,
            "Tag": collector.tag,
            "Range": timespan,
            "ChartType": collector.chartType,
            "MeanValueType": collector.meanValueType
        });
    }

    var url = getViewCollectionApiUrl();

    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json'
    });
});