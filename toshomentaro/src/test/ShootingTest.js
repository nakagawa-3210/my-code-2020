phina.define("ShootingTest", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = "black";
    // this.setupBackground();

    const bossPositionX = this.gridX.center(5);
    const bossPositionY = this.gridY.center();
    this.bossBullet = DisplayElement().addChildTo(this);
    this.testBoss = ShootingBoss(this.bossBullet).addChildTo(this)
      .setPosition(bossPositionX, bossPositionY);
    // console.log("this.testBoss.x:", this.testBoss.x)
    // this.testBoss.motionTwo()
    //敵を作成するために一旦コメントアウト
    this.playerBullet = DisplayElement().addChildTo(this);
    this.player = ShootingPlayer(this.playerBullet).addChildTo(this)
    .setPosition(this.gridX.center(), this.gridY.center(3));
  },

  setupBackground: function (){
    const width =1600;
    const height = 1066;
    this.background = 
      Sprite("background/hongkong", width, height)
        .addChildTo(this).setOrigin(0, 0);
  },

  update: function () {
    //敵を作成するために一旦コメントアウト
    var direction = this.app.keyboard.getKeyDirection();
    // 移動する向きとスピードを代入する
    this.player.moveBy(direction.x * this.player.speed, direction.y * this.player.speed);


    //現在のフレーム数に応じて敵の動きを選択
    // if(!this.testBoss.tweener.playing) {
    //   const moveTime = 90;
    //   const attackTime = 200;
    //   if(this.app.frame % attackTime === 0) {
    //     this.testBoss.motionTwo();
    //   }else if (this.app.frame % moveTime === 0) {
    //     this.testBoss.motionOne();
    //   }
    // }

    //乱数の値に応じて動きを選択
    if(!this.testBoss.tweener.playing) {
      const motionNum = Math.random();
      if(motionNum < 0.3) {
        this.testBoss.motionTwo();
      }else if (motionNum < 0.6) {
        this.testBoss.motionThree();
      }else {
        this.testBoss.motionOne();
      }
    }

    // if(!this.testBoss.tweener.playing) {
      // const attackTime = 300;
      // if(this.app.frame % attackTime === 0) {
      //   this.testBoss.motionTwo();
      // }else {
      //   this.testBoss.motionOne();
      // }
    // }
  }

});