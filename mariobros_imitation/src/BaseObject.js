/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("BaseObject", {
  superClass: 'Sprite',
  init: function (file, width, height) {
    this.superInit(file, width, height);
    this.setCollider();
  },
  setCollider: function () {
    var s1 = this.width * 0.6;
    var s2 = this.width / 10;
    var half = this.width / 2.5;
    // コライダー
    const top = Collider().attachTo(this).setSize(s1, s2).offset(0, -half)
    .show();
    top.id = 'top';
    const bottom = Collider().attachTo(this).setSize(s1, s2).offset(0, half)
    .show();
    bottom.id = 'bottom';
    const left = Collider().attachTo(this).setSize(s2, s1).offset(-half, 0)
    .show();
    left.id = 'left';
    const right = Collider().attachTo(this).setSize(s2, s1).offset(half, 0)
    .show();
    right.id = 'right';
  },
});