phina.define('Application', {
  superClass: 'phina.display.CanvasApp',

  init: function (options) {
    this.superInit(options);
    //ロード終了後処理

    this.replaceScene(MyManagerScene({}));
  },
});