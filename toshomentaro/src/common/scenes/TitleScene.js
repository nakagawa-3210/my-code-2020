/*
 * タイトルシーン
 */
phina.define("TitleScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.setupBackground();
    // テスト
    // SoundManager.playMusic('bgm/shooting');
    //タイトル
    this.titleLabel = Label({
      text: '刀 削 麺 太 郎\n物 語',
      fontSize: 124,
      fill: 'white',
      fontWeight: 'bold',
      stroke: 'black',
      strokeWidth: 20
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_TOP);
    //メッセージ
    this.titleMessage = Label({
      text: `push 'Enter' to start`,
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
    //画面遷移時のフェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());;
  },

  setupBackground: function () {
    this.background = Sprite('background/title').addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center())
    this.backgroundFrame = Sprite('background/titleFrame').addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center())
    this.testFade = FadeInOut().addChildTo(this)
  },

  update: function () {
    const key = this.app.keyboard;
    if (key.getKey('enter') && !this.fade.tweener.playing) {
      this.fade.fadeOut();
      this.fade.on("finishedFadeOut", () => {
        this.exit("scrollMain", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT
        });
        // this.exit("shootingMain", option = {
        //   width: GAME_SCREEN_WIDTH,
        //   height: GAME_SCREEN_HEIGHT
        // });
      })
    }
  },

});