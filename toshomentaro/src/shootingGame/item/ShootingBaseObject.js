/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("ShootingBaseObject", {
  superClass: 'Sprite',
  init: function (file, width, height) {
    this.superInit(file, width, height);
  },
  setCollider: function (colliderW, colliderH, colliderX, colliderY, colliderId) {
    const width = colliderW || this.width;
    const height = colliderH || this.height;
    const x = colliderX || this.x;
    const y = colliderY || this.y;
    const id = colliderId || "self";
    const selfCollider = 
      Collider().attachTo(this).setSize(width, height).offset(x, y)
      // .show();
    selfCollider.id = id;
  },

});