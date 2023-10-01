/*
* Player
*optionにはfileName, width, heightを持たせる
*/
phina.define("Player", {
  superClass: 'BaseObject',
  init: function (file, width, height) {
    this.superInit(file, width, height);
    this.isOnBlock = false;
    this.isJumping = false;

    //アニメーション追加
    this.anim = FrameAnimation('precure_ss').attachTo(this);
    // console.log("player this:", this)

    this.isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };

    this.isAttacked = false;
  },

  //アニメーション付与
  setupMotion: function (scene) {
    const key = scene.app.keyboard;
    //キャラの向き
    const directionMap = {
      left: 1,
      right: -1
    };
    if (key.getKey('left')) {
      this.scaleX = directionMap.left;
      this.anim.gotoAndPlay('walk');
    }else if (key.getKey('right')) {
      this.scaleX = directionMap.right;
      this.anim.gotoAndPlay('walk');
    }

    if(key.getKeyUp('left') || key.getKeyUp('right')) {
      this.anim.gotoAndStop('walk');
    }
  },

  
  //空中にいる間のみ自由落下の重力を付与
  //地面、空中タイルに接している時は重力なし
  setupOperation: function (scene, isOnTest) {
    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    //マップ情報引継ぎ
    isOnMap = isOnTest;

    const key = scene.app.keyboard;
    // 上下移動
    //何かの上に接している時
    if (isOnMap.isOnTop === true) {
      if (key.getKey('up')) {
        this.physical.velocity.y -= PLAYER_JUMP_POWER;
        this.physical.gravity.y = PLAYER_GRAVITY;
        //ここでthis.isOnBlockは参照するだけにする
        this.isOnBlock = false;
        this.isJumping = true;
      };
    } else if (isOnMap.isOnTop === false) {
      this.physical.gravity.y = PLAYER_GRAVITY;
    }
    //何かの下に接している時
    if (isOnMap.isOnBottom === true) {
      const bound = 5;
      this.physical.velocity.y = bound;
    }

    //左右移動
    //側面に接触した状態での移動に不具合あり
    //何かの左に接している時
    if (isOnMap.isOnLeft === true) {
      if (key.getKey('left')) { this.x -= PLAYER_SPEED; };
      // console.log("onLeft");
    }
    //何かの右に接している時
    else if (isOnMap.isOnRight === true) {
      if (key.getKey('right')) { this.x += PLAYER_SPEED; };
      // console.log("onRight");
    } 
    //左右のどちらにも接していない時
    if (isOnMap.isOnLeft === false &&
      isOnMap.isOnRight === false) {
      if (key.getKey('left')) { this.x -= PLAYER_SPEED; };
      if (key.getKey('right')) { this.x += PLAYER_SPEED; };
    }
  },

  //ジャンプしているかを監視管理
  isJumpingTest: function () {
    if (this.physical.velocity.y < 0) {
      this.isJumping = true;
    }
    //else分岐いらなかったらコメントアウト
    else {
      this.isJumping = false;
    }
  },

  //敵とのぶつかり判定を受け取って点滅を管理
  manageHitStatus: function (isHit) {
    //攻撃を受けているかつ点滅していない
    if(isHit && !this.tweener.playing) {
      //点滅開始
      this.tweener.clear().fadeOut(100).fadeIn(100).setLoop(true);
      //点滅している(無敵扱い)時間
      const invincibleTime = 2000;
      //点滅終了
      const stopLoop = () =>{
        this.tweener.clear();
        this.alpha = 1.0;
      };
      setTimeout(stopLoop, invincibleTime);
    }
  },

});