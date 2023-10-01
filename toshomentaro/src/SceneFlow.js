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
            screenSize: {
              width: GAME_SCREEN_WIDTH,
              height: GAME_SCREEN_HEIGHT
            }
          },
        },
        //マップ生成用テストシーン
        {
          label: "test",
          className: "TestScene",
        },
        {
          label: 'shootTest',
          className: 'ShootingTest'
        },
        {
          label: "title",
          className: "TitleScene",
        },
        {
          label: 'scrollMain',
          className: 'ScrollGameScene',
        },
        {
          label: 'scrollResult',
          className: 'ScrollGameResultScene',
        },
        {
          label: 'shootingMain',
          className: 'ShootingGameScene'
        },
        {
          label: 'gameOver',
          className: 'GameOverScene',
        },
        {
          label: 'gameClear',
          className: 'GameClearScene',
        },
        
        {
          label: 'testspace',
          className: 'TestScene'
        }
      ]
    });
  }
});　