/*
* 歩行敵クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("WalkingEnemy", {
  superClass: 'BaseEnemy',
  init: function (file, width, height, direction) {
    this.enemyWidth = width || 90;
    this.enemyHeight = height || 150;
    this.superInit(file, this.enemyWidth, this.enemyHeight);
    this.direction = direction || -1;
    //コライダー設定
    this.setCollider(
      this.width * 0.5,
      this.height * 0.1,
      this.height*0.1,
      this.height*0.9,
      this.height/2,
      this.width/4
    );
    //アニメーション追加
    //ザコ敵は色が違うだけでモーションは同じ
    this.anim = FrameAnimation('zacook_ss').attachTo(this);
    
    //移動追加
    // this.setupMotion();
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
  setupMotion: function (isOnStatus) {
    //重力設定
    this.physical.gravity.y = ENEMY_GRAVITY;
    //移動方向
    // this.direction = [ENEMY_DIRECTION_MAP.RIGHT, ENEMY_DIRECTION_MAP.LEFT].random();
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
      //方向反転
      this.direction *= -1;
      this.scaleX = -this.direction;
    }
    if (isOnMap.isOnLeft === true) {
      this.x -= ENEMY_SPEED;
      //方向転換
      this.direction = -1;
      this.scaleX = -this.direction;
    }
    //何かの右に接している時
    else if (isOnMap.isOnRight === true) {
      this.x += ENEMY_SPEED;
      this.direction = 1;
      this.scaleX = -this.direction;
    }

    //ステータスによる移動管理
    if (this.isWeak) {
      this.rotation = 90;
      this.anim.gotoAndStop('walk');
      if(!this.tweener.playing){
        this.defeatedVoice();
        this.defeatedAnimation();
        this.isDefeated = true;
      }
    } else {
      this.rotation = 0;
      this.x += this.direction * ENEMY_SPEED;
      this.anim.gotoAndPlay('walk');
    }
  },

  //攻撃ステータス管理
  manageStatus: function (isAttacked) {
    if(isAttacked === true) {
      this.isWeak = true;
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

  //倒された時の声
  defeatedVoice: function () {
    if(!this.isDefeated){
      SoundManager.play("voice/zacook");
    }
  }
});