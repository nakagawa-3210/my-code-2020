/*
 * リザルトシーン
 */
phina.define("ResultScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit({
      width: GAME_SCREEN_WIDTH,
      height: GAME_SCREEN_HEIGHT
    });
    this.backgroundColor = 'white';
    this.setupUi(option);
    this.setImgScale(option);
  },
  setupUi: function (option) {
    //イメージ
    this.resultImg = Sprite(option.img, option.gridSize, option.gridSize)
      .addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF / 0.85);
    //メッセージ
    this.resultMessage = Label({
      text: option.message,
      fontSize: 56,
      fontWeight: "bold",
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF / 2);
    //リトライメッセージ
    this.retry = Label({
      text: "press space to Title",
      fontSize: 32,
      fontWeight: "bold",
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF / 0.55);
    //メッセージ点滅
    this.retry.tweener.clear().fadeOut(800).fadeIn(800).setLoop(true);
  },
  //イメージサイズ調整
  setImgScale: function (option) {
    if(option.message === "GAME OVER!") {
      this.resultImg.scaleX = 7.5;
      this.resultImg.scaleY = 7.5;
      this.resultImg.frameIndex = 37;
    }
  },
  //キーボード設定
  update: function () {
    const key = this.app.keyboard;
    if (key.getKey('space')) {
      this.exit("title", option = {
        width: GAME_SCREEN_WIDTH,
        height: GAME_SCREEN_HEIGHT
      });
    }
  },
});