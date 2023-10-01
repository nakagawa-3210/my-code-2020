phina.define("ShootingBoss", {
  superClass: 'DisplayElement',
  init: function (scene, bossBulletGroup) {
    this.superInit();

    //敗北フラグ
    this.isDefeated = false;
    const width = 415;
    const height = 750;
    //通常状態のボス
    this.normalBoss = ShootingBaseObject("chara/bicCook", width, height).addChildTo(this)
      .setPosition(scene.gridX.center(5), scene.gridY.center());
    //当たり判定コライダー設置
    this.setupCollider();
    //攻撃を受けたボス
    this.damagedBoss = ShootingBaseObject("chara/damagedBicCook", width, height).addChildTo(this.normalBoss)
    //通常は透明で隠す
    const hide = 0;
    this.damagedBoss.alpha = hide;
    
    //弾の動き
    const interval = 800;
    this.firstShotTweeen = Tweener().clear().call(() => {
      this.shot(bossBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.firstShotTweeen.attachTo(this);
    
  },

  shot: function (bossBulletGroup) {
    //this避難
    const self = this.normalBoss;
    const top = 20;
    const middle = 0;
    const bottom = -20;
    const bulletArr = [top, middle, bottom];
    //弾の作成
    bulletArr.forEach((degree) => {
      const bullet = EnemyBullet().addChildTo(bossBulletGroup)
      .setPosition(self.x, self.y);
      const speed = 10;
      const modification = 180;
      const shotDegree = self.rotation + degree + modification;
      const vector = Vector2().fromDegree(shotDegree, speed);
      bullet.physical.velocity = vector;
      if(this.isDefeated){
        const hide = 0;
        bullet.alpha = hide;
      }
    })

    //画面外に出た弾の削除
    if(bossBulletGroup.children.length > 0) {
      for (var i = 0; i < bossBulletGroup.children.length; i++) {
        const bullet = bossBulletGroup.children[i];
        if(bullet.x < GAME_SCREEN_LEFT) {
          bullet.remove();
        }
      }
    }
  },


  //コライダー設置
  setupCollider: function () {
    const bodyW = this.normalBoss.width;
    const bodyH = this.normalBoss.height;
    const bodyX = 110;
    const bodyY = -6;    
    const bodyCollider = this.normalBoss.setCollider(
      bodyW,
      bodyH,
      bodyX,
      bodyY
    );
    const noseW = BASE_GRID;
    const noseH = BASE_GRID_HALF;
    const noseX = -50;
    const noseY = -220;
    const noseId = "nose";
    const noseCollider = this.normalBoss.setCollider(
      noseW,
      noseH,
      noseX,
      noseY,
      noseId
    )
  },


  //動きパターン
  motionOne: function () {
    const moveTime = 2000;
    const jumpTime = 600;
    const fallTime = 400;
    // const self = this;
    // console.log("this:", this);
    this.normalBoss.tweener.clear().to({
      x: this.normalBoss.x - BASE_GRID*3
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x 
    }, moveTime, "linear")
    .to({
      y: this.normalBoss.y - BASE_GRID*9
    }, jumpTime, "linear")
    .to({
      y: this.normalBoss.y
    }, fallTime, "linear");
  },

  motionTwo: function () {
    const littleBack = 500;
    const waitTime = 400
    const attackTime = 1000;
    const backTime = 1000;
    // console.log("this:", this);
    this.normalBoss.tweener.clear()
    .to({
      x: this.normalBoss.x + BASE_GRID
    }, littleBack, "linear")
    .wait(waitTime)
    .to({
      x: this.normalBoss.x - BASE_GRID * 8
    }, attackTime, "easeOutElastic")
    .to({
      x: this.normalBoss.x
    }, backTime, "linear");
  },

  motionThree: function () {
    const jumpTime = 300;
    const fallTime = 100;
    this.normalBoss.tweener.clear()
    .to({
      y: this.normalBoss.y - BASE_GRID*2
    }, jumpTime, "linear")
    .to({
      y: this.normalBoss.y
    }, fallTime, "linear");
  },

  motionFour: function () {
    const moveTime = 300;
    this.normalBoss.tweener.clear().to({
      x: this.normalBoss.x - BASE_GRID
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x + BASE_GRID
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x
    }, moveTime, "linear");
  },

  motionDefeated: function () {
    const moveTime = 100;
    this.normalBoss.tweener.clear().to({
      y: this.normalBoss.y - BASE_GRID_QUARTER,
    }, moveTime, "linear")
    .to({
      y: this.normalBoss.y,
    }, moveTime, "linear")
    .setLoop(true);
  },

  manageHitStatus: function (isHit) {
    //攻撃を受けているかつダメージボスが表示されていない
    const show = 1;
    const hide = 0;
    if (isHit) {
      if(this.damagedBoss.alpha === hide){
        this.damagedBoss.alpha = show;
      }
    }else {
      this.damagedBoss.alpha = hide;
    }
  },
})