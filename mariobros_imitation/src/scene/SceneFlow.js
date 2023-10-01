phina.define('MyManagerScene', {
  superClass: 'ManagerScene',
  init: function (option) {
    this.superInit({
      scenes: [
        // {
        //   label: "initialLoad",
        //   className: "InitialLoadScene",
        //   argument: asset,
        // },
        {
          label: "mainLoad",
          className: "MainLoadScene",
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT,
          arguments: {
            assets: MainAssets.assets,
            option: {
              width: GAME_SCREEN_WIDTH,
              height: GAME_SCREEN_HEIGHT
            }
          },
        },
        {
          label: "title",
          className: "TitleScene",
        },
        {
          label: 'main',
          className: 'MainScene',
        },
        {
          label: 'result',
          className: 'ResultScene',
        },
        
        {
          label: 'testspace',
          className: 'TestScene'
        }
      ]
    });
  }
});　