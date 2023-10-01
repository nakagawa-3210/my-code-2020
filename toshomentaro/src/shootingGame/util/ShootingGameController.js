phina.define("ShootingGameController", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さないv
  init: function (options) {
    // SoundManager.playMusic('bgm/shooting');
    //初期設定
    this.time = 0;
    this.scene = options.scene;
    this.timerLabel = options.timerLabel;
    this.playerBullet = options.playerBullet;
    this.player = options.player;
    this.zacooksBullet = options.zacooksBullet;
    this.zacookGroup = options.zacookGroup;
    this.bossBullet = options.bossBullet;
    this.boss = options.boss;
    this.bossHitPointGauge = options.bossHitPointGauge;
    this.fade = options.sceneFade;
    //テスト
    // this.testBullet = options.testBullet;
    // this.testPlayer = options.testPlayer;

    this.isDamegedPlayer = false;
    this.playerAttackPoint = PLAYER_ATTACK_POINT;

    this.spawnZacook = false;

    this.isDamegedBoss = false;
    this.bossCurrentHpValue = BOSS_HP;
    this.bossWeak = BOSS_HP / 6;
    this.bossAttackPoint = BOSS_AT;
    //ボスの攻撃パターンカウント
    this.bossMotionCounter = 0;

  },

  //毎フレーム更新処理
  update: function () {
    //時間制限の更新
    this.time =
      ShootingGameManager().manageGameTime(this.scene, this.time, this.timerLabel, this.bossCurrentHpValue);
    const timeLimit = Math.round(GAME_EXIT_MAP.SHOOTING_GAME_TIME - Math.floor(this.time) / 1000);
    //プレイヤーとボスが戦っている時
    if (this.player.hitPointGauge.value !== 0 &&
      this.bossHitPointGauge.value !== 0 &&
      timeLimit !== 0) {
      this.player.setupMotion(this.scene);
      ShootingGameManager().managePlayerMoveRange(this.player);
      //プレイヤーが敵から受ける攻撃判定
      this.isDamegedPlayer = ShootingGameManager().managePlayerIsHitStatus(
        this.player.player,
        //本実装までは文字にしておく
        this.bossBullet,
        this.boss.normalBoss,
        this.zacooksBullet,
        this.zacookGroup
      );
      //プレイヤーの体力を減らす
      this.player.manageHitStatus(this.isDamegedPlayer, this.bossAttackPoint);

      //ザコックを追加
      if (this.bossCurrentHpValue < this.bossWeak) {
        if (!this.spawnZacook) {
          this.spawnZacook = true;
          //ザコックの透明化を解除と移動モーションの追加
          ShootingGameManager().setupZacookMotion(this.scene, this.zacookGroup);
        }
        //ザコックがプレイヤーから受ける攻撃判定
        ShootingGameManager().manageZacook(this.zacookGroup, this.playerBullet);
      }

      //ボスが受けた攻撃を管理
      this.isDamegedBoss = ShootingGameManager().hitTestPlayerBulletBossNose(
        this.playerBullet,
        this.boss.normalBoss
      );
      //ボスの体力ゲージ管理
      this.boss.manageHitStatus(this.isDamegedBoss);
      this.bossCurrentHpValue = ShootingGameManager().manageBossHitPointValue(
        this.bossCurrentHpValue,
        this.bossHitPointGauge,
        this.boss.damagedBoss
      );

      // ボスに動き付与
      this.bossMotionCounter = ShootingGameManager().manageBossMotion(
        this.boss,
        this.bossCurrentHpValue,
        this.bossWeak,
        this.bossMotionCounter
      );

    }
    //戦いに決着がついたとき
    else {
      //敗北モーション
      ShootingGameManager()
        .manageIsDefeatedMotion(this.player, this.boss, this.zacookGroup);
      //画面遷移
      ShootingGameManager().gameOverManager(
        this.scene,
        this.player,
        this.boss,
        timeLimit,
        this.fade)
    }

  },

  // //キャラクターはisDefeatedがtureの時に弾が非表示になる
  // gameOver: function (player, boss, zacookGroup) {
  //   const removeSpeed = 5;
  //   //ボスが勝利した時
  //   if(player.hitPointGauge.value == 0) {
  //     const playerSelf = player.player;
  //     if(!playerSelf.tweener.playing){
  //       //敗北にする
  //       player.isDefated = true;
  //       //敗北モーション起動
  //       player.defeatedMotion();
  //     }
  //     if(player.isDefeated) {
  //       //画面の左外側に移動
  //       player.x -= removeSpeed;
  //     }
  //     //画面外に出たことを検知で画面遷移
  //   }
  //   //プレイヤーが勝利した時
  //   else{
  //     if(!boss.normalBoss.tweener.playing) {
  //       //ボス敗北設定
  //       boss.isDefeated = true;
  //       boss.motionDefeated();
  //       //ザコ敵敗北設定
  //       for (var i = 0; i < zacookGroup.children.length; i++) {
  //         zacookGroup.children[i].isDefeated = true;
  //       }
  //     }
  //     //敵たちを画面右外側へ移動
  //     if(boss.isDefeated) {
  //       zacookGroup.x -= removeSpeed;
  //       boss.x -= removeSpeed;
  //     }
  //   }
  // }

});