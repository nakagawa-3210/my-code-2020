/*
* Player
*optionにはfileName, width, heightを持たせる
*/
phina.define("ShootingPlayer", {
  superClass: 'ShootingBaseObject',
  init: function (playerBulletGroup) {
    const width =128;
    const height = 133;
    this.superInit("chara/riderMentaro", width, height);
    //当たり判定
    this.setupCollider();
    this.speed = PLAYER_SPEED*2;

    this.hitPointGauge = HitPointGauge({value: 100}).addChildTo(this)
      .setPosition(0, -BASE_GRID*1.5);
    this.hitPointGauge.value -= 30;

    const interval = 120;
    //tweener初期化
    this.tweener.clear();
    //弾の動き設定
    this.shotTween = Tweener().clear().call(() => {
      this.shot(playerBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.shotTween.attachTo(this);
  },

  setupCollider: function () {
    const colliderW = this.width/5;
    const colliderH = this.height/1.8;
    const colliderX = this.x-10;
    const colliderY = this.y-15;
    this.setCollider(
      colliderW,
      colliderH,
      colliderX,
      colliderY,
    );
  },

  setupMotion: function (scene) {
    const direction = scene.app.keyboard.getKeyDirection();
    // 移動する向きとスピードを代入する
    this.moveBy(direction.x * this.speed, direction.y * this.speed);
  },

  shot: function (playerBulletGroup) {
    //弾の生成
    const gap = 20;
    const hight = -gap;
    const low = gap;
    const bulletArr = [hight, low];
    bulletArr.forEach((position) => {
      const bullet = StraightforwardBullet()
      .addChildTo(playerBulletGroup).setPosition(this.x, this.y + BASE_GRID_HALF + position);
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



  //敵とのぶつかり判定を受け取って点滅を管理
  manageHitStatus: function (isHit) {
    //攻撃を受けているかつ点滅していない
    if (isHit && !this.tweener.playing) {
      //点滅開始
      this.tweener.clear().fadeOut(100).fadeIn(100).setLoop(true);
      //点滅している(無敵扱い)時間
      const invincibleTime = 2000;
      //点滅終了
      const stopLoop = () => {
        this.tweener.clear();
        this.alpha = 1.0;
      };
      setTimeout(stopLoop, invincibleTime);
    }
  },

});