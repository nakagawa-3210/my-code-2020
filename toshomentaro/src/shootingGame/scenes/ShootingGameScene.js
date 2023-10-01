phina.define("ShootingGameScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = "white";
    this.setupUi();
    this.one("enterframe", () => {
      //原因はわからないが、最初に表示するシーンでは音楽が流せない
      SoundManager.playMusic('bgm/shooting');
      SoundManager.setVolumeMusic(GAME_BGM_VOLUME);
      this.activateGameController();
      console.log("this.currentMusic:", this.currentMusic);
    });
  },

  setupUi: function () {
    //背景設定
    this.setupBackground();
    //ゲーム内プレイヤー設定
    this.setupPlayer();
    //ゲーム内ザコキャラ設定
    this.setupZakoGroup();
    //ゲーム内ボス設定
    this.setupBoss();
    //画面遷移用フェード
    this.setupScreenFade();
    //時間制限
    this.setupTimer();
  },

  setupBackground: function () {
    //背景の画像
    this.background = Sprite("background/ShootingScene", GAME_SCREEN_WIDTH, GAME_SCREEN_HEIGHT)
      .addChildTo(this)
      .setOrigin(0, 0);
    //背景の床
    const self = this;
    const generateTiles = () => {
      const numberOfTiles = GAME_SCREEN_WIDTH / BASE_GRID;
      for (var i = 0; i < numberOfTiles; i++) {
        const tile = Sprite("tile/white", BASE_GRID, BASE_GRID).addChildTo(self);
        tile.setOrigin(0, 0.5);
        tile.setPosition(BASE_GRID * i, GAME_SCREEN_HEIGHT)
      }
    };
    //床の設置
    generateTiles();
  },

  setupPlayer: function () {
    this.playerBullet = DisplayElement().addChildTo(this);
    this.player = ShootingPlayer(this, this.playerBullet).addChildTo(this);
  },

  setupZakoGroup: function () {
    this.zacooksBullet = DisplayElement().addChildTo(this);
    this.zacookGroup = DisplayElement().addChildTo(this);
    for (var i = 0; i < ZACOOK_NUM; i++) {
      let number = i + 1;
      const zacook = ShootingZacook(this, this.zacooksBullet)
        .addChildTo(this.zacookGroup);
    }
  },

  setupBoss: function () {
    this.bossBullet = DisplayElement().addChildTo(this);
    this.boss = ShootingBoss(this, this.bossBullet).addChildTo(this);
    //位置固定のためここで作成
    this.bossHitPointGauge = HitPointGauge({ value: PLAYER_HIT_POINT, scale: 1.5, gaugeColor: "red" }).addChildTo(this)
      .setPosition(this.gridX.center(5), this.gridY.center(-7));
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    this.fade.fadeIn();
  },

  setupTimer:function () {
    this.timerLabel = GameTimer(GAME_EXIT_MAP.SHOOTING_GAME_TIME).addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center(-6.5));
  },


  activateGameController: function () {
    this.gameController = ShootingGameController({
      scene: this,
      timerLabel: this.timerLabel,
      playerBullet: this.playerBullet,
      player: this.player,
      zacooksBullet: this.zacooksBullet,
      zacookGroup: this.zacookGroup,
      bossBullet: this.bossBullet,
      boss: this.boss,
      bossHitPointGauge: this.bossHitPointGauge,
      sceneFade: this.fade
    });
    this.on("enterframe", () => {
      this.gameController.update();
    })
  }
});