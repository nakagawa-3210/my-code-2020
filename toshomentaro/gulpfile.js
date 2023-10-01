const gulp = require("gulp");
const concat = require("gulp-concat");

gulp.task("default", () => {
  return gulp.src([
    "src/*",
    "src/common/*",
    "src/common/scenes/*",
    "src/common/util/*",
    "src/scrollGame/*",
    "src/scrollGame/item/*",
    "src/scrollGame/scenes/*",
    "src/scrollGame/util/*",
    "src/shootingGame/*",
    "src/shootingGame/item/*",
    "src/shootingGame/scenes/*",
    "src/shootingGame/util/*",
    ])
    .pipe(concat('bundle.js'))
    .pipe(gulp.dest('./dest/'));
});