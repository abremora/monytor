$("#saveViewCollectionModal-save").click(function () {
    var name = $("#saveViewCollectionModal-name").val();

    var views = new Views().load();
    var data = {
        "Name": name,
        "Description": null,
        "Views": new Array()
    };
    for (var i = 0; i < views.views.length; i++) {
        if (views.views[i].collectors.length == 0)
            continue;
        var collector = views.views[i].collectors[0];
        var end = moment(collector.end);
        var start = moment(collector.start);
        var diff = end.subtract(start);
        var timespan = diff.format("D.HH:mm:ss");
        data.Views.push({
            "Position": i,
            "Group": collector.group,
            "Tag": collector.tag,
            "Range": timespan
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

var getViewCollectionApiUrl = function () {
    return window.location.origin + "/api/ViewCollection";
};