//何も表示せずメインロードシーンに必要な最低限のロードを行う
phina.define('InitialLoadScene', {
  superClass: 'DisplayScene',

  init: function (option) {
    this.superInit(option);
    Label({
      text: 'InitialLoadScene',
      fontSize: 64,

    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF);
    new Promise((resolve, reject) => {
      const loader = phina.asset.AssetLoader();
      loader.load(asset);
      resolve();
    }).then(() => {
      this.exit();
    })
  },
});