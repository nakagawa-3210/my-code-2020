phina.define("ShootingZacook", {
  superClass: 'DisplayElement',
  init: function (scene, zacookBulletGroup) {
    this.superInit();
    const minZacookWidth = 45;
    const minZacookHeight = 75;

    //敗北フラグ
    this.isDefeated = false;
    //ダメージフラグ
    this.isDamaged = false;
    //コック体力
    this.currentHitPoint = ZACOOK_HP;
    //通常コック作成
    this.zacook = ShootingBaseObject("chara/minZacook_ss", minZacookWidth, minZacookHeight).addChildTo(this)
      .setPosition(scene.gridX.center(5), scene.gridY.center(-6));
    //ダメージコック作成
    this.damagedZacook = ShootingBaseObject("chara/damagedMinZacook_ss", minZacookWidth, minZacookHeight).addChildTo(this.zacook)
    //隠す
    const hide = 0;
    this.zacook.alpha = hide;
    this.damagedZacook.alpha = hide;

    //コライダー設置
    this.setupCollider();
    //弾セット(ミリ秒)
    const interval = 5000;
    //弾の動き追加
    this.shotTween = Tweener().clear().call(() => {
      this.shot(zacookBulletGroup);
    })
      .wait(interval)
      .setLoop(true);
    this.shotTween.attachTo(this.zacook);
  },

  shot: function (zacookBulletGroup) {
    const direction = -1;
    const gravity = 1;
    const speed = 10;
    const img = "item/minCabbageBullet";
    const bullet = StraightforwardBullet(direction, gravity, speed, img).addChildTo(zacookBulletGroup)
      .setPosition(this.zacook.x, this.zacook.y);
    const hide = 0;
    if (this.zacook.alpha === hide || this.isDefeated) {
      bullet.alpha = hide;
    }
  },

  setupCollider: function () {
    const colliderW = this.zacook.width;
    const colliderH = this.zacook.height - 5;
    const colliderX = -1;
    const colliderY = -1;
    this.zacook.setCollider(
      colliderW,
      colliderH,
      colliderX,
      colliderY
    );
  },

  setupMotion: function (scene, theNumber) {
    //初期化の際の引数に用意するザコ敵の数をもらう
    //tweenでザコ敵が一周回る時間をザコ敵の数で割る
    //商だけ待たせて、回転させる
    const rotateTime = TOTAL_ROTATE_TIME / 4;
    const waitingTime = TOTAL_ROTATE_TIME / ZACOOK_NUM;
    const position = {
      top: scene.gridY.center(-8),
      centerY: scene.gridY.center(),
      bottom: scene.gridY.center(8),
      left: scene.gridX.center(1),
      centerX: scene.gridX.center(5),
      right: scene.gridX.center(9)
    }
    const rotate = () => {
      this.zacook.tweener.clear()
        .to({
          x: position.left,
          y: position.centerY
        }, rotateTime, "linear")
        .to({
          x: position.centerX,
          y: position.bottom
        }, rotateTime, "linear")
        .to({
          x: position.right,
          y: position.centerY
        }, rotateTime, "linear")
        .to({
          x: position.centerX,
          y: position.top
        }, rotateTime, "linear")
        .setLoop(true);
    }
    //回転し始めるタイミングをずらす
    setTimeout(rotate, waitingTime * theNumber);
  },

  manageHitStatus: function (isHit) {
    const show = 1;
    const hide = 0;
    if (isHit) {
      if (this.damagedZacook.alpha === hide) {
        this.damagedZacook.alpha = show;
        this.currentHitPoint -= PLAYER_ATTACK_POINT;
      }
    } else {
      this.damagedZacook.alpha = hide;
    }
  },
});