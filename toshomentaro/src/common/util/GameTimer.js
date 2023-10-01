phina.define("GameTimer", {
  superClass: 'Label',
  init: function (time) {
    this.superInit({
      text: `TIME\n${time}`,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 15,
      fontSize: 48,
    });
  }

  //時間制限表示
  // this.time = 0;
  // this.timeLabel = Label({
  //   text: `TIME\n${GAME_EXIT_MAP.SCROLL_GAME_TIME}`,
  //   fill: 'white',
  //   stroke: 'black',
  //   strokeWidth: 15,
  //   fontSize: 48,
  // }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, BASE_GRID);
});