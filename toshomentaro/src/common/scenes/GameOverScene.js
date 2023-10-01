phina.define("GameOverScene", {
  superClass: 'BaseScene',
  init: function (options) {
    this.superInit(options);
    this.options = options;
    this.fromScene = options.from;
    this.options = options;
    this.backgroundColor = "white";
    this.retry = -1;
    this.giveUp = 1;
    this.decision = this.retry;
    this.one('enterframe', () => {
      this.setupWords();
      this.setupScreenFade();
    });
  },

  setupWords: function () {
    const wordsW = 447;
    const wordsH = 102
    //励ましのお言葉
    this.comments = Sprite("comment/resultOne", wordsW, wordsH)
      .addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(-1));
    const optionX = 3;
    const optionY = 3;
    //あきらめない
    const retryW = 190;
    const retryH = 62;
    this.optionRetry = Sprite("option/retry", retryW, retryH)
      .addChildTo(this).setPosition(this.gridX.center(-optionX), this.gridY.center(optionY));
    //あきらめる
    const giveUpW = 141;
    const giveUpH = 59;
    this.optionGiveUp = Sprite("option/giveUp", giveUpW, giveUpH)
      .addChildTo(this).setPosition(this.gridX.center(optionX), this.gridY.center(optionY));
    //選択の丸
    const circleW = 448;
    const circleH = 269;
    const scale = 1.3;
    this.optionDecision = Sprite("option/circle", circleW, circleH)
      .addChildTo(this).setPosition(this.gridX.center(-optionX), this.gridY.center(optionY + 0.5));
    this.optionDecision.scaleX = scale;
    this.optionDecision.scaleY = scale;
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },

  update: function () {
    if (!this.fade.tweener.playing) {
      const key = this.app.keyboard;
      //選択肢変更
      const optionX = 3;
      //あきらめない
      if (key.getKey('left')) {
        this.optionDecision.x = this.gridX.center(-optionX);
        this.decision = this.retry;
      }
      //あきらめる
      if (key.getKey('right')) {
        this.optionDecision.x = this.gridX.center(optionX);
        this.decision = this.giveUp;
      }
      //決定
      if (key.getKeyDown('space')) {
        this.fade.fadeOut();
        this.fade.on("finishedFadeOut", () => {
          //元来たシーンに遷移
          if (this.decision === this.retry) {
            if (this.fromScene === GAME_SCENE_MAP.SCROLL_GAME) {
              this.exit('scrollMain', this.options);
            } else {
              this.exit('shootingMain', this.options);
            }
          }
          //タイトルシーンに遷移
          else {
            this.exit('title', this.options);
          }
        })
      }
    }
  },

});