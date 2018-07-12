/// <binding BeforeBuild='default' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rimraf = require("rimraf");
var merge = require('merge-stream');
var runSequence = require('run-sequence');

gulp.task("minify", function () {

    var streams = [
        gulp.src(["wwwroot/js/*.js"])
            .pipe(uglify())
            .pipe(concat("site.min.js"))
            .pipe(gulp.dest("wwwroot/lib/site"))
    ];

    return merge(streams);
});

// Dependency Dirs
var deps = {
    "bootstrap": {
        "dist/**/*": ""
    },
    "jquery": {
        "dist/*": ""
    },
    "popper.js": {
        "dist/**/*": ""
    },
    "moment": {
        "min/*.js": ""
    },
    "mustache": {
        "*.js": ""
    },
    "chart.js": {
        "dist/*": ""
    },
    "@fortawesome": {
        "fontawesome-free/**/*.js": "",
        "fontawesome-free/**/*.css": ""
    },
};

gulp.task("clean", function (cb) {
    return rimraf("wwwroot/vendor/", cb);
});

gulp.task("scripts", function () {

    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/vendor/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});

// Parallel execution
//gulp.task("default", ['clean', 'minify', 'scripts']);

// Gulp 4.0 only
//gulp.task("default", gulp.series('clean', 'minify', 'scripts'));

gulp.task("default", function (callback) {
    runSequence('clean', 'minify', 'scripts');
});
