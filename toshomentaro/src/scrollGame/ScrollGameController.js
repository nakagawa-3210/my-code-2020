/*
* ScrollGameController
*ゲーム管理で更新をかける内容の記述
*/
phina.define("ScrollGameController", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さない
  init: function (options) {
    //初期設定
    this.time = 0;
    this.scene = options.scene;
    this.timerLabel = options.timerLabel;
    this.stockLabelNumber = options.stockLabelNumber;
    this.player = options.player;
    this.toushomen = options.toushomen;
    this.blockGroup = options.blockGroup;
    this.withoutCollider = options.withoutColliderBlockGroup;
    this.enemyGroup = options.enemyGroup;
    this.runningDistance = options.runningDistance;
    this.playerDirection = options.playerDirection;
    this.playerStock = options.playerStock;
    this.generatedBlock = options.generatedBlock;
    this.generatedNoColliderBlock = options.generatedNoColliderBlock;
    this.generatedEnemy = options.generatedEnemy;
    this.fade = options.sceneFade;
  },

  //毎フレーム更新処理
  update: function () {
    //時間制限の更新
    this.time = 
      ScrollGameManager().manageGameTime(this.scene, this.time, this.timerLabel);
    
    //playerの更新
    //残機管理
    this.stockLabelNumber.text = `×${this.playerStock}`;
    //地面との当たり判定
    this.setupPlayerIsOnStatus = 
      ScrollGameManager().setupIsOnTest(this.blockGroup, this.player);
    //移動
    this.player.isJumpingTest();
    this.player.setupMotion(this.scene);
    this.player.setupOperation(this.scene, this.setupPlayerIsOnStatus);
    //刀削麺のための、プレイヤーの向き情報更新
    this.playerDirection = 
      ScrollGameManager().managePlayerDirection(this.scene, this.playerDirection);
    //画面内移動範囲制限
    ScrollGameManager().manageCharacterMoveRange(this.player);


    ////刀削麺の更新
    //playerと刀削麺のspriteの向きは逆なのでマイナスで調整する
    const toshomenDirection = -this.playerDirection
    this.toushomen.setupOperation(this.scene, this.player.x, this.player.y, toshomenDirection);


    ////敵の更新
    //動き実装
    for (var i = 0; i < this.enemyGroup.children.length; i++) {
      const enemy = this.enemyGroup.children[i];
      let enemyIsOnTest = ScrollGameManager().setupIsOnTest(this.blockGroup, enemy);
      enemy.manageMotion(enemyIsOnTest);
    }


    //プレイヤーが右に移動しているときに、画面半分より右に行く時、
    // 引数に渡す要素をプレイヤーの移動している間だけ
    if (this.scene.key.getKey('right')) {
      //移動距離更新
      this.runningDistance = 
        ScrollGameManager().managePlayerDistance(this.player, this.setupPlayerIsOnStatus, this.runningDistance);
      //コライダー付きブロック
      ScrollGameManager().manageStageRange(this.player, this.blockGroup, this.setupPlayerIsOnStatus);
      //コライダーなしブロック
      ScrollGameManager().manageStageRange(this.player, this.withoutCollider, this.setupPlayerIsOnStatus);
      //敵
      ScrollGameManager().manageStageRange(this.player, this.enemyGroup, this.setupPlayerIsOnStatus);
    }

    //当たり判定付きblockの生成と削除
    //戻り値にフラグの更新も兼ねている
    this.generatedBlock = ScrollGameManager().manageMapBlocks(
      this.runningDistance,
      this.generatedBlock,
      this.blockGroup,
      "map/stageOne",
      "tile/white"
    );

    //当たり判定のないblockの生成と削除
    this.generatedNoColliderBlock = ScrollGameManager().manageMapNoColliderBlocks(
      this.runningDistance,
      this.generatedNoColliderBlock,
      this.withoutCollider,
      "map/stageOneNoCollider",
      "tile/white"
    );

    //敵の生成と削除
    this.generatedEnemy = ScrollGameManager().manageMapEnemies(
      this.runningDistance,
      this.generatedEnemy,
      this.enemyGroup,
      "map/stageOneEnemy",
      "chara/zacook_ss"
    );


    //敵とプレイヤー、刀削麺の当たり判定を基に敵自身とプレイヤーの管理をする
    this.enemyGroup.children.each((enemy) => {
      //敵と刀削麺の当たり判定監視
      this.hitStatusEnemyToshomen = 
        ScrollGameManager().manageHitStatus(this.toushomen, enemy);
      const show = 1.0;
      if(this.toushomen.alpha === show) {
        enemy.manageStatus(this.hitStatusEnemyToshomen);
      }

      //敵が倒されていないとき
      if (enemy.isWeak === false) {
        //プレイヤーの管理
        //プレイヤーと敵の当たり判定監視 
        this.hitStatusPlayerEnemy =
        ScrollGameManager().manageHitStatus(this.player, enemy);
        //プレイヤーの残機管理(playerが点滅している間は無敵)
        this.playerStock = 
          ScrollGameManager().managePlayerStock(this.hitStatusPlayerEnemy, this.player, this.playerStock);
        //敵にぶつかっていればプレイヤーを点滅させる
        this.player.manageHitStatus(this.hitStatusPlayerEnemy);
      }
    });

    const hide = 0;
    if(this.fade.alpha === hide) {
      // console.log("this.runningDistance:", this.runningDistance);
      ScrollGameManager().gameOverManager(
        this.scene,
        this.playerStock, 
        this.player.y, 
        this.time, 
        this.runningDistance, 
        this.fade
      );
    }
  },
});