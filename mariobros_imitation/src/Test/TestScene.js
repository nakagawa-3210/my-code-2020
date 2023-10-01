phina.define("TestScene", {
  superClass: 'DisplayScene',
  init: function (option) {
    this.superInit(option);
    console.log("test")
    this.backgroundColor = '#6495ed';

    this.player = Player("chara/precure", PRECURE_GRID, PRECURE_GRID, this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF)
      .addChildTo(this);
    this.isPlayerOn = false;

    this.player.physical.gravity.y = 1;

    this.setupBlock();

    // console.log(this);
    // console.log(this.block)
  },

  update: function () {
    //プレイヤーに動き追加
    // this.player.setupOperation(this);
    this.player.isJumpingTest();
    // this.gravityManager(this.isPlayerOn);

    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    this.block.accessories.each((blockAccessory) => {
      this.player.accessories.each((characterAccessory) => {
        if (characterAccessory.id !== undefined
          && blockAccessory.hitTest(characterAccessory)) {
          console.log("hit!!");
          if (blockAccessory.id === "top" &&
            characterAccessory.id === "bottom" &&
            !this.player.isJumping) {
            this.player.physical.velocity.y = 0;
            this.player.physical.gravity.y = 0;
            this.player.isOnTest(true);
            isOnMap.isOnTop = true;
            this.player.y = this.block.top - this.player.height / 2;
          } else if (blockAccessory.id === "left" &&
            characterAccessory.id === "right") {
            isOnMap.isOnLeft = true;
          } else if (blockAccessory.id === "right" &&
          characterAccessory.id === "left") {
            isOnMap.isOnRight = true;
          } else if (blockAccessory.id === "bottom" &&
            characterAccessory.id === "top") {
            isOnMap.isOnBottom = true;
            console.log("isOnMap", isOnMap);
          }
        }
      });
    });
    console.log("isOnMap:", isOnMap);
    this.player.setupOperation(this, isOnMap);
  },

  gravityManager: function (isOn) {
    if (isOn) {
      // this.player.physical.force(0, 0);
      // this.player.physical.gravity.set(0, 0);


      // console.log("isON", this.player);
    } else {
      this.player.physical.gravity.y = 1;
      // console.log("isNOT", this.player);
    }
  },

  setupBlock: function () {
    this.block = Sprite('tile/tiles', BASE_GRID, BASE_GRID)
    this.block.frameIndex = 1;
    this.block.setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF * 1.5).addChildTo(this);


    var s1 = this.block.width * 0.9;
    var s2 = this.block.width / 4;
    var half = this.block.width / 2;
    // コライダー
    const top = Collider().attachTo(this.block).setSize(s1, s2).offset(0, -half)
      .show();
    top.id = 'top';
    // const bottom = Collider().attachTo(this.block).setSize(s1, s2).offset(0, half)
    //   .show();
    // bottom.id = 'bottom';
    // const left = Collider().attachTo(this.block).setSize(s2, s1).offset(-half, 0)
    //   .show();
    // left.id = 'left';
    // const right = Collider().attachTo(this.block).setSize(s2, s1).offset(half, 0)
    //   .show();
    // right.id = 'right';
  }

})