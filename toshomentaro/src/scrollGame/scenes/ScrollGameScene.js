/*
 * メインシーン
 */
//メインシーンですべきこと
// 使用する要素の宣言のみを行う
// 横スクロールゲームにて使用されるものが何なのかを説明するのみ

phina.define("ScrollGameScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);

    //画面に登場する要素のセットアップ
    this.setupUi();
    //プレイヤーの残機
    //プレイヤーの移動距離
    this.runningDistance = 0;
    //プレイヤーがどっちに向いているか
    this.playerDirection = 1;
    //プレイヤーの残機
    this.player.stock = PLAYER_STOCK;
    //ブロックを作ったかどうか
    this.generatedBlock = false;
    //コライダーなしのブロック
    this.generatedNoColliderBlock = false;
    //敵を作ったかどうか
    this.generatedEnemy = false;
    //ゲーム開始
    this.one('enterframe', () => {
      this.key = this.app.keyboard;
      SoundManager.playMusic('bgm/scroll');
      SoundManager.setVolumeMusic(GAME_BGM_VOLUME);
      this.activateGameController();
    });
  },

  //画面内の要素用意
  setupUi: function () {
    //ゲーム内の背景設置
    this.setupBackground();
    //ゲーム内のブロック設置
    this.setupStageBlocks();
    //プレイヤー設置
    this.setupPlayre();
    //プレイヤーの残機
    this.setupPlayerStockImg();
    //敵設置
    this.setupStageEnemies();
    //時間制限
    this.setupTimer();
    //画面遷移用フェード
    this.setupScreenFade();
  },

  setupBackground: function () {
    this.background = Sprite("background/scrollScene", GAME_SCREEN_WIDTH, GAME_SCREEN_HEIGHT).addChildTo(this)
      .setOrigin(0.0, 0.0)
      .setPosition(0, 0);
  },

  setupStageBlocks: function () {
    //マップの幅をタイルの数で示した数字
    const mapWidth = GAME_INIT_MAP_END / BASE_GRID;
    //初期表示されるマップ作成
    this.blockGroup =
      this.setMapBlocksFromFormatArr("map/stageOneInit", mapWidth).addChildTo(this).setPosition(0, 0);
    //コライダーなしのブロック
    this.withoutCollider = DisplayElement().addChildTo(this).setPosition(0, 0);
  },

  //プレイヤー設定
  setupPlayre: function () {
    //プレイヤー
    const initialX = 200;
    const initialY = 500;
    this.player = Player("chara/mentarou_ss", MENTARO_WIDTH, MENTARO_HEIGHT)
      .addChildTo(this)
      .setPosition(initialX, initialY);
    const justStanding = 0;
    this.player.frameIndex = justStanding;
    //刀削麺
    this.toushomen = Tousyomen().addChildTo(this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF);
  },

  //プレイヤーの残機表示
  setupPlayerStockImg: function () {
    this.stockInfo = DisplayElement().addChildTo(this).setPosition(0, 0);
    //残機イメージ
    const stockImg = Sprite("chara/mentarou_ss", MENTARO_WIDTH, MENTARO_HEIGHT)
      .addChildTo(this.stockInfo).setPosition(BASE_GRID, BASE_GRID);
    const scale = 0.5;
    stockImg.scaleX = scale;
    stockImg.scaleY = scale;
    const justStanding = 0;
    stockImg.frameIndex = justStanding;
    //残機数字情報
    this.stockLabelNumber = Label({
      text: `× ${this.player.stock}`,
      fill: "white",
      stroke: 'black',
      strokeWidth: 15,
    }).addChildTo(this.stockInfo)
    .setPosition(BASE_GRID*2, BASE_GRID);
  },

  setupStageEnemies: function () {
    //敵設置
    this.enemyGroup = DisplayElement().addChildTo(this).setPosition(0, 0);
  },

  setupTimer:function () {
    this.timerLabel = GameTimer(GAME_EXIT_MAP.SCROLL_GAME_TIME).addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center(-6.5));
  },

  

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },

  //コントローラーにてゲーム更新処理を行う
  activateGameController: function () {
    //更新処理に必要なものを引数に渡す
    this.gameController = ScrollGameController(
      {
        scene: this,
        timerLabel: this.timerLabel,
        stockLabelNumber: this.stockLabelNumber,
        player: this.player,
        toushomen: this.toushomen,
        blockGroup: this.blockGroup,
        withoutColliderBlockGroup: this.withoutCollider,
        enemyGroup: this.enemyGroup,
        runningDistance: this.runningDistance,
        playerDirection: this.playerDirection,
        playerStock: this.player.stock,
        generatedBlock: this.generatedBlock,
        generatedNoColliderBlock: this.generatedNoColliderBlock,
        generatedEnemy: this.generatedEnemy,
        sceneFade: this.fade,
      }
    )
    this.on("enterframe", () => {
      // 毎フレーム処理
      this.gameController.update();
    })
  },
});