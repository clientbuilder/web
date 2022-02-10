const gulp = require("gulp"),
    concat = require('gulp-concat'),
    sass = require('gulp-sass')(require('sass')),
    uglify = require('gulp-uglify')
minify = require('gulp-minify')
cssmin = require('gulp-cssmin');

gulp.task('styles', () =>
    gulp.src('./Styles/**/*.scss')
        .pipe(sass())
        .pipe(cssmin({ keepSpecialComments: 0 }))
        .pipe(concat('style.min.css'))
        .pipe(gulp.dest('./wwwroot/assets/css/'))
);

gulp.task('scripts', () =>
    gulp.src([
        "node_modules/bootstrap/dist/js/bootstrap.bundle.js"
    ])
        .pipe(uglify())
        .pipe(concat('scripts.min.js'))
        .pipe(gulp.dest('./wwwroot/assets/js'))
);

gulp.task('mdi', () =>
    gulp.src('./node_modules/@mdi/font/fonts/**/*')
        .pipe(gulp.dest('./wwwroot/assets/fonts/mdi/'))
);