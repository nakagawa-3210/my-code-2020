/*
* 歩行敵クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("WalkingEnemy", {
  superClass: 'BaseEnemy',
  init: function (file, width, height, scene) {
    this.superInit(file, width, height);
    //アニメーション追加
    this.anim = FrameAnimation('tomapiko_ss').attachTo(this);
    //移動追加
    this.setupMotion();
    //ダメージ管理
    this.isBeingAttacked = false;
    //弱っているか管理
    this.isWeak = false;
    //倒されたか管理
    this.isDefeated = false;
    //画面外にあるか
    this.isScreenOut = false;
  },

  //初期動き設定
  setupMotion: function () {
    //重力設定
    this.physical.gravity.y = ENEMY_GRAVITY;
    //移動方向
    this.direction = [ENEMY_DIRECTION_MAP.RIGHT, ENEMY_DIRECTION_MAP.LEFT].random();
    //キャラの向き設定
    if (this.direction === ENEMY_DIRECTION_MAP.RIGHT) {
      //キャラの向きをx軸反転
      this.scaleX = -1;
    }
  },

  //動き管理
  manageMotion: function (isOnTest) {
    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    //マップ情報引継ぎ
    isOnMap = isOnTest;
    //何かの上に接していないとき(空中にいるとき)
    if (isOnMap.isOnTop === false) {
      this.physical.gravity.y = ENEMY_GRAVITY;
    }
    if (isOnMap.isOnLeft === true) {
      this.x -= ENEMY_SPEED;
    }
    //何かの右に接している時
    else if (isOnMap.isOnRight === true) {
      this.x += ENEMY_SPEED;
    }
    //ステータスによる移動管理
    if (this.isDefeated) {
      this.rotation = 0;
      this.frameIndex = 4;
      //下記のやり方ではうまくいかない
      if(!this.tweener.playing){
        this.defeatedAnimation();
      }
    } else {
      if (this.isWeak) {
        this.rotation = 90;
        this.anim.gotoAndStop('walk');
      } else {
        this.rotation = 0;
        this.x += this.direction * ENEMY_SPEED;
        this.anim.gotoAndPlay('walk');
        // this.defeatedAnimation();
      }
    }
  },

  //攻撃ステータス管理
  manageStatus: function (isBeingAttackedStatus) {
    if (isBeingAttackedStatus && !this.isBeingAttacked) {
      this.isBeingAttacked = true;
      if (this.isWeak && !this.isDefeated) {
        this.isWeak = false;
      } else {
        this.isWeak = true;
      }
      // console.log("enemy is attacked");
    } else if (!isBeingAttackedStatus && this.isBeingAttacked) {
      this.isBeingAttacked = false;
    }
  },

  //倒された時のアニメーション
  defeatedAnimation: function () {
    const upTime = 100;
    const downTime = 800;
    this.tweener.clear()
    .to({ y: this.y - this.height
    }, upTime, "easeInBack")
    .to({ y: GAME_SCREEN_BOTTOM + this.height
    }, downTime, "easeInBack");
  },

});