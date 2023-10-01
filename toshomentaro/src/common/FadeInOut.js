/*
* FadeInOut
*画面遷移時のフェードクラス
*/
phina.define("FadeInOut", {
  superClass: 'RectangleShape',
  init: function () {
    this.superInit();
    this.width = GAME_SCREEN_WIDTH;
    this.height = GAME_SCREEN_HEIGHT;
    this.stroke = 'transparent',
    this.strokeWidth = 0;
    this.fill = "black";
    // this.setOrigin(0, 0);
    this.alpha = 0.0;
  },

  fadeIn: function () {
    const show = 1;//fade自身が姿を表す
    const hide = 0;//fade自身が姿を隠す
    this.alpha = show;
    const fadeTime = 1500;
    this.tweener.clear().to({
      alpha: hide
    } ,fadeTime, "linear").call(() => {
      this.flare("finishedFadeIn");
    });
  },

  fadeOut: function () {
    const show = 1;
    const fadeTime = 1500;
    this.tweener.clear().to({
      alpha: show
    } ,fadeTime, "linear").call(()=> {
      this.flare("finishedFadeOut");
    });
  }
});  