/// <binding BeforeBuild='default' Clean='cleanup' />
var path = require('path'),
    gulp = require('gulp'),
    rename = require('gulp-rename'),
    gp_clean = require('gulp-clean'),
    gp_sass = require('gulp-sass')(require('sass'));

var basePath = path.resolve(__dirname, "");

var srcPaths = {
    sass: [
        path.resolve(basePath, 'scss/custom/build.scss'),
    ]
};

var destPaths = {
    css: path.resolve(basePath, 'css')
};

/* SASS/CSS */
gulp.task('sass_clean', function () {
    return gulp.src(destPaths.css + "*.*", { read: false })
        .pipe(gp_clean({ force: true }));
});

gulp.task('sass', function () {
    return gulp.src(srcPaths.sass)
        .pipe(gp_sass({ outputStyle: 'compressed' }))
        .pipe(rename({
            basename: 'mainStyle',
            suffix: '.min'
        }))
        .pipe(gulp.dest(destPaths.css));
});

/* Defaults */
gulp.task('cleanup', gulp.series(['sass_clean']));

gulp.task('default', gulp.series(['sass']));