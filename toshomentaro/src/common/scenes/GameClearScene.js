phina.define("GameClearScene", {
  superClass: 'BaseScene',
  init: function (options) {
    this.superInit(options);
    this.setupBackground();
    this.setupScreenFade();
  },
  setupBackground: function () {
    //メッセージ
    this.message = Sprite("comment/tsuduku", 323, 100)
      .addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(-1));
    //製作者
    this.creators = Label({
      text: 'イラスト: M.K JONES\n\nプログラム: はらぺこちゃん',
      fontSize: 16,
      fill: 'white',
      fontWeight: 'bold',
      stroke: 'black',
      strokeWidth: 6
    }).addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(4));
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },
});