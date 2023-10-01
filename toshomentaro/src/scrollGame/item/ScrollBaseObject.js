/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("ScrollBaseObject", {
  superClass: 'Sprite',
  init: function (file, width, height) {
    this.superInit(file, width, height);
    // this.setCollider();
  },
  setCollider: function (width1, height1, width2, height2, position1, position2) {

    const w1 = width1 || this.width * 0.9;
    const h1 = height1 || this.width / 4;
    const w2 = width2 || this.width / 4;
    const h2 = height2 || this.width * 0.9;
    const p1 = position1 || this.width / 2;
    const p2 = position2 || this.width / 2;

    // コライダー
    const top = Collider().attachTo(this).setSize(w1, h1).offset(0, -p1)
    // .show();
    top.id = 'top';

    const bottom = Collider().attachTo(this).setSize(w1, h1).offset(0, p1)
    // .show();
    bottom.id = 'bottom';

    const left = Collider().attachTo(this).setSize(w2, h2).offset(-p2, 0)
    // .show();
    left.id = 'left';

    const right = Collider().attachTo(this).setSize(w2, h2).offset(p2, 0)
    // .show();
    right.id = 'right';
  },

});