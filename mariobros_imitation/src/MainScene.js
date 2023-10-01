/*
 * メインシーン
 */
phina.define("MainScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = '#6495ed';

    this.setupUi();
  },

  update: function () {
    //プレイヤーに動き追加
    this.player.setupOperation(this);

    //地面との当たり判定
    //プレイヤーの地面判定
    //敵の地面判定

    //敵との当たり判定

    //プレイヤーの残機管理

    //
  },

  //画面内の要素用意
  setupUi: function () {
    //mapセット
    this.blocks = this.setMapBlocks().addChildTo(this).setPosition(0, 0);
    this.minTiles = this.setMapMinTiles().addChildTo(this).setPosition(0, 0);
    this.pipes = this.setMapPipes().addChildTo(this).setPosition(0, 0);
  
    //プレイヤーセット
    this.player = Player("chara/tomapiko", BASE_GRID, BASE_GRID, this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF)
      .addChildTo(this);
  },

});