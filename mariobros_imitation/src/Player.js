/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("Player", {
  superClass: 'BaseObject',
  init: function (file, width, height, scene) {
    this.superInit(file, width, height);
    this.setupMotion();
    // this.setupOperation(scene);
    // this.update(scene);
  },
  // update: function (scene) {
  //   this.setupOperation(scene);
  // },
  //アニメーション付与
  setupMotion: function () {

  },
  //キーボード入力操作付与
  setupOperation: function (scene) {
    if (scene.app !== undefined) {
      const key = scene.app.keyboard;
      // 上下左右移動
      if (key.getKey('left')) { this.x -= PLAYER_SPEED; };
      if (key.getKey('right')) { this.x += PLAYER_SPEED; };
      if (key.getKey('up')) { this.y -= PLAYER_SPEED; };
      if (key.getKey('down')) { this.y += PLAYER_SPEED; };
    }
  }

});