
phina.define("ShootingBoss", {
  superClass: 'ShootingBaseObject',
  init: function (bossBulletGroup) {
    const width = 416;
    const height = 750;
    
    this.superInit("chara/bicCook", width, height);
    this.setupCollider();
    // console.log("this:", this);

    const interval = 800;
    this.firstShotTweeen = Tweener().clear().call(() => {
      this.shot(bossBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.firstShotTweeen.attachTo(this);

    console.log("boss.x:", this.x)

    //第二の攻撃を考える
    // this.secondShotTween = Tweener().clear().call(() => {
    //   this.shot(bossBulletGroup);
    // })
    // .wait(interval)
    // .setLoop(true);
  },

  setupCollider: function () {
    const bodyW = this.width/4;
    const bodyH = this.height;
    const bodyX = this.x;
    const bodyY = this.y;    
    const bodyCollider = this.setCollider(
      bodyW,
      bodyH,
      bodyX,
      bodyY
    );
    const stomachW = this.width/2.1
    const stomachH = BASE_GRID*3
    const stomachX = this.x+BASE_GRID_QUARTER;
    const stomachY = this.y-BASE_GRID;
    const stomachCollider = this.setCollider(
      stomachW,
      stomachH,
      stomachX,
      stomachY
    );
    const forkW = BASE_GRID*6;
    const forkH = this.width/3.5;
    const forkX = this.x;
    const forkY = this.y+BASE_GRID_HALF;
    const forkCollider = this.setCollider(
      forkW,
      forkH,
      forkX,
      forkY
    );
    const noseW = BASE_GRID;
    const noseH = BASE_GRID_HALF;
    const noseX = this.x-BASE_GRID_HALF;
    const noseY = this.y-BASE_GRID*3.4;
    const noseId = "nose";
    const noseCollider = this.setCollider(
      noseW,
      noseH,
      noseX,
      noseY,
      noseId
    )
  },

  shot: function (bossBulletGroup) {
    //this避難
    const self = this;
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
    })

    //弾の削除
    if(bossBulletGroup.children.length > 0) {
      // console.log("bossBulletGroup.children:", bossBulletGroup.children)
      for (var i = 0; i < bossBulletGroup.children.length; i++) {
        const bullet = bossBulletGroup.children[i];
        // console.log("bullet:", bullet)
        if(bullet.x < GAME_SCREEN_LEFT) {
          bullet.remove();
          // console.log("bossBulletGroup.children.length:", bossBulletGroup.children.length)
        }
      }
    }
  },

  motionOne: function () {
    const moveTime = 2000;
    const jumpTime = 600;
    const fallTime = 400;
    this.tweener.clear().to({
      x: this.x - BASE_GRID*3
    }, moveTime, "linear")
    .to({
      x: this.x 
    }, moveTime, "linear")
    .to({
      y: this.y - BASE_GRID*9
    }, jumpTime, "linear")
    .to({
      y: this.y
    }, fallTime, "linear")
  },

  motionTwo: function () {
    const littleBack = 500;
    const waitTime = 400
    const attackTime = 1000;
    const backTime = 1000;
    this.tweener.clear()
    .to({
      x: this.x + BASE_GRID
    }, littleBack, "linear")
    .wait(waitTime)
    .to({
      x: this.x - BASE_GRID * 8
    }, attackTime, "easeOutElastic")
    .to({
      x: this.x
    }, backTime, "linear")
  },

  motionThree: function () {
    const stopTime = 1500;
    this.tweener.clear()
    .to({
      rotation: 30
    }, stopTime, "linear")
    .to({
      rotation: 0
    }, stopTime, "linear");
  },
});



