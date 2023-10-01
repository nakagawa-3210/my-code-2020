/*
* 敵の基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("BaseEnemy", {
  superClass: 'ScrollBaseObject',
  init: function (file, width, height, scene) {
    this.superInit(file, width, height);
  },

});