/*
 * タイトルシーン
 */
phina.define("TitleScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = '#6495ed';
    this.setupMapAll();
    //タイトル
    this.titleLabel = Label({
      text: 'Piko Bros',
      fontSize: 124,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 15
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_TOP);
    //メッセージ
    this.titleMessage = Label({
      text: 'space to start',
      fontSize: 48,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 10
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF * 0.65);

    //タイトルとメッセージに動きの追加
    this.titleLabel.tweener.clear().to({
      y: GAME_SCREEN_HEIGHT_HALF
    }, 800, "easeInBack")
      .to({
        y: GAME_SCREEN_HEIGHT_HALF * 0.85
      }, 500, "easeInBack");

    this.titleMessage.tweener.clear().to({
      y: GAME_SCREEN_HEIGHT_HALF * 1.65
    }, 800, "easeInBack")
      .to({
        y: GAME_SCREEN_HEIGHT_HALF * 1.5
      }, 500, "easeInBack").call(() => {
        this.titleMessage.tweener.clear().fadeOut(800).fadeIn(800).setLoop(true);
      })
  },

  update: function () {
    const key = this.app.keyboard;
    if (key.getKey('space')) {
      this.exit("main", option = {
        width: GAME_SCREEN_WIDTH,
        height: GAME_SCREEN_HEIGHT
      });
    }
  },

});