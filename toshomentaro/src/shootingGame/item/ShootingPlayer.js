phina.define("ShootingPlayer", {
  superClass: 'DisplayElement',
  init: function (scene, playerBulletGroup) {
    this.superInit();
    this.isDefeated = false;
    const width =128;
    const height = 133;
    const playerX = scene.gridX.center(-6);
    const playerY = scene.gridY.center();
    this.player = ShootingBaseObject("chara/riderMentaro", width, height).addChildTo(this)
      .setPosition(playerX, playerY);
    //コライダー
      this.setupCollider();
    //体力ゲージ
    this.hitPointGauge = HitPointGauge({value: PLAYER_HIT_POINT}).addChildTo(this)
      .setPosition(scene.gridX.center(-6), scene.gridY.center(-2));

    //弾の動き
    const interval = 120;
    //tweener初期化
    this.player.tweener.clear();
    //弾の動き設定
    this.shotTween = Tweener().clear().call(() => {
      this.shot(playerBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.shotTween.attachTo(this.player);
  },

  setupCollider: function () {
    const colliderW = this.player.width/5;
    const colliderH = this.player.height/1.8;
    const colliderX = -BASE_GRID/4;
    const colliderY = -GAME_SCREEN_HEIGHT/25;
    this.player.setCollider(
      colliderW,
      colliderH,
      colliderX,
      colliderY,
    );
  },

  setupMotion: function (scene) {
    const speed = PLAYER_SPEED*2;
    const direction = scene.app.keyboard.getKeyDirection();
    // 移動する向きとスピードを代入する
    this.player.moveBy(direction.x * speed, direction.y * speed);
    this.hitPointGauge.moveBy(direction.x * speed, direction.y * speed);
  },

  shot: function (playerBulletGroup) {
    //弾の生成
    const gap = 20;
    const hight = -gap;
    const low = gap;
    const bulletArr = [hight, low];
    bulletArr.forEach((position) => {
      const bullet = StraightforwardBullet()
      .addChildTo(playerBulletGroup).setPosition(this.player.x, this.player.y + BASE_GRID_HALF + position);
      if(this.isDefeated) {
        const hide = 0;
        bullet.alpha = hide;
      }
    })
    
    // console.log("playerBulletGroup.children.length:", playerBulletGroup.children.length);
    //弾の削除
    if(playerBulletGroup.children.length > 0) {
      for (var i = 0; i < playerBulletGroup.children.length; i++) {
        const child = playerBulletGroup.children[i];
        // console.log("playerBulletGroup.children.length:", playerBulletGroup.children.length)
        // console.log("child.x:", child.x);
        if(child.x > GAME_SCREEN_RIGHT) {
          child.remove();
        }
      }  
    }
  },

  defeatedMotion: function () {
    const moveTime = 80;
    this.player.tweener.clear().to({
      y: this.player.y + 5
    }, moveTime, "linear")
    .to({
      y: this.player.y
    }, moveTime, "linear")
    .setLoop(true);
  },

  //敵とのぶつかり判定を受け取って点滅を管理
  manageHitStatus: function (isHit, damageValue) {
    //攻撃を受けているかつ点滅していない
    if (isHit && !this.player.tweener.playing) {
      //ダメージ管理
      this.hitPointGauge.managePlayerHitPoint(damageValue);
      //ダメージボイス
      this.setDamagedVoice();
      //点滅開始
      this.player.tweener.clear().fadeOut(100).fadeIn(100).setLoop(true);
      //点滅している(無敵扱い)時間
      const invincibleTime = 2000;
      //点滅終了
      const stopLoop = () => {
        this.player.tweener.clear();
        this.player.alpha = 1.0;
      };
      setTimeout(stopLoop, invincibleTime);
    }
  },

  setDamagedVoice: function () {
    const voiceArr = [
      'voice/mentaroDefeated1',
      'voice/mentaroDefeated2',
      'voice/mentaroDefeated3'
    ];
    SoundManager.play(voiceArr[Math.floor(Math.random() * voiceArr.length)]);
  },
  
});