//タイトル表示前のロードシーン
phina.define('MainLoadScene', {
  superClass: 'DisplayScene',

  init: function (options) {
    this.superInit(options);
    const loader = phina.asset.AssetLoader();
    // new Promise((resolve, reject) => {
    loader.onload = () => {
      this.exit('title', options.option);
    }
    loader.load(options.assets);
    //   resolve();
    // }).then(() => {
    // this.exit('title');
    // });
  },
});