//タイトル表示前のロードシーン
phina.define('MainLoadScene', {
  superClass: 'DisplayScene',

  init: function (options) {
    this.superInit(options);
    const loader = phina.asset.AssetLoader();

    loader.onload = () => {

      //確認後戻す
      // this.exit('shootingMain', options.screenSize);
      // this.exit('scrollMain', options.screenSize);
      //テスト中のためコメントアウト
      this.exit('title', options.screenSize);

      // this.exit('gameClear', options.screenSize);
    }
    loader.load(options.assets);
  },
});