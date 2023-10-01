/*
 * メインシーン
 */
phina.define("MainScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = '#6495ed';
    this.setupUi();
    //プレイヤーの残機
    this.player.stock = PLAYER_STOCK;
    //敵の残機
    this.enemyGroup.stock = NUMBER_OF_ENEMY;
    //敵の撃退アニメーション
  },

  //画面内の要素用意
  setupUi: function () {
    //mapセット
    //地面ブロック
    this.blocks = this.setMapBlocks().addChildTo(this).setPosition(0, 0);
    //空中タイル
    this.minTiles = this.setMapMinTiles().addChildTo(this).setPosition(0, 0);
    //パイプ
    this.pipes = this.setMapPipes().addChildTo(this).setPosition(0, 0);
    //powブロック(ラビ)
    this.pow = this.setupItem().addChildTo(this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF / 0.8);
    //プレイヤー
    this.player = Player("chara/precure", PRECURE_GRID, PRECURE_GRID).addChildTo(this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF / 0.6);
    //敵グループの生成
    this.enemyGroup = DisplayElement();
    //敵が画面の一番後ろに表示されるようにする
    this.addChildAt(this.enemyGroup, 0);
    //時間制限表示
    this.time = 0;
    this.timeLabel = Label({
      text: `TIME\n${GAME_EXIT_MAP.GAME_TIME}`,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 15,
      fontSize: 48,
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, BASE_GRID);
  },

  //毎フレーム更新処理
  update: function () {
    //経過秒数更新
    this.time += this.app.deltaTime;
    let timeLimit = Math.round(GAME_EXIT_MAP.GAME_TIME - Math.floor(this.time) / 1000);
    this.timeLabel.text = `TIME\n${timeLimit}`;

    /////////////////プレイヤーの情報管理/////////////////
    //プレイヤーのアニメーション付与
    this.player.setupMotion(this);
    //ジャンプしているかの判定
    this.player.isJumpingTest();
    //地面、空中タイルとプレイヤーのコライダー同士の当たり判定
    this.setupPlayerIsOnStatus =
      GameManager().setupIsOnTest(this.blocks, this.minTiles, this.pow, this.player);
    //ブロックを打ち上げる判定
    GameManager().setupAttackTiles(this.minTiles, this.player);
    //判定を渡して移動動き制御
    this.player.setupOperation(this, this.setupPlayerIsOnStatus);
    //プレイヤーが画面外に出ないよう制御
    GameManager().manageCharacterMoveRange(this.player);

    /////////////////エネミーの情報管理/////////////////
    //フレーム数に応じて敵を生成する
    if ((this.enemyGroup.children.length === 0 || this.app.frame % ENEMY_INTERVAL === 0) &&
      this.enemyGroup.children.length < NUMBER_OF_ENEMY) {
      //敵作成
      GameManager().generateEnemy(this.enemyGroup);
      //最後に追加された敵要素
      let lastChild = this.enemyGroup.children[this.enemyGroup.children.length - 1]
      if (lastChild.direction === ENEMY_DIRECTION_MAP.RIGHT) {
        lastChild.setPosition(ENEMY_SPAWN_MAP.LEFT, ENEMY_SPAWN_MAP.APPEAR);
      } else if (lastChild.direction === ENEMY_DIRECTION_MAP.LEFT) {
        lastChild.setPosition(ENEMY_SPAWN_MAP.RIGHT, ENEMY_SPAWN_MAP.APPEAR);
      }
    }
    //敵グループの動き追加
    this.enemyGroup.children.each((enemy) => {
      //地面判定
      this.setupEnemyIsOnStatus =
        GameManager().setupIsOnTest(this.blocks, this.minTiles, this.pow, enemy);
      //地面判定からの動き管理
      enemy.manageMotion(this.setupEnemyIsOnStatus);
      //動き範囲制御
      GameManager().manageCharacterMoveRange(enemy);
      //敵のリスポーン(画面下両端のパイプに入ると、上のパイプにリスポーンする) 
      GameManager().setupEnemyRespawn(enemy);
    })

    /////////////////プレイヤーとエネミーの情報管理/////////////////
    this.enemyGroup.children.each((enemy) => {
      //プレイヤーと敵の当たり判定監視 
      this.hitStatusPlayerEnemy =
        GameManager().playerHitTestAgainstEnemy(this.player, enemy);

      //敵の残機管理
      //地面からの攻撃を受けたかを判定
      this.attackedStatus =
        GameManager().setupAttackedEnemyStatus(this.pow, this.blocks, this.minTiles, enemy, this.player);
      //攻撃判定からステータス管理
      enemy.manageStatus(this.attackedStatus);

      //敵が弱っている時
      if (this.hitStatusPlayerEnemy.enemyWeakStatus) {
        this.enemyGroup.stock =
          GameManager().manageEnemyrStock(this.hitStatusPlayerEnemy, enemy, this.enemyGroup.stock);
        // enemy.defeatedAnimation(this.hitStatusPlayerEnemy);
      }
      //敵が弱っていない時
      else {
        //プレイヤーの残機管理
        this.player.stock = GameManager().managePlayerStock(this.hitStatusPlayerEnemy.isHit, this.player, this.player.stock);
        //敵にぶつかっていればプレイヤーを点滅させる
        this.player.manageHitStatus(this.hitStatusPlayerEnemy.isHit);
      }

    });

    //////////////////ゲームオーバー管理////////////////////
    GameManager().isGameOver(this, this.player.stock, this.enemyGroup.stock, timeLimit);
    // this.timeLabel.
  },

});