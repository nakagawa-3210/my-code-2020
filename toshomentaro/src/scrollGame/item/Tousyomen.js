phina.define("Tousyomen", {
  superClass: 'ScrollBaseObject',
  init: function () {
    const menWidth = 120;
    const menHeight = 120;
    this.superInit("item/tosyomen", menWidth, menHeight);
    this.setCollider();
    this.anim = FrameAnimation('tosyomen_ss').attachTo(this);
    this.alpha = 0;
  },

  //麺の動き
  setupMotion: function (positionX, positionY, direction) {
    //アニメ
    this.anim.gotoAndPlay('fly');
    const time = 300
    const moveRange = positionX + BASE_GRID * 3 * -direction;
    this.tweener.clear();
    this.tweener.clear()
      .to({
        x: moveRange
      }, time, "linear")
      .call(() => {
        //移動後は隠す
        this.alpha = 0;
      });
  },

  // ・麺の実装
  // 実装内容
  // 　スペースキーを押す
  // 	プレイヤーのx軸とy軸に合わせる
  // 	alpha値を0から1に切り替え
  // 　	192px画面右側に向かって進む
  // 　	192px進み終えたらalpha値を0にする

  // 	麺のalpha値が1.0の時に衝突していれば当たりとみなす


  // キーを押すと刀削麺が移動しながら移動する
  setupOperation: function (scene, positionX, positionY, direction) {
    const key = scene.app.keyboard;
    if (key.getKeyDown('space') && this.alpha === 0) {
      //刀削麺発射声
      SoundManager.play("voice/mentaroAttack");
      //プレイヤーの目の前のx軸位置に設置
      this.x = positionX - BASE_GRID * direction;
      // 高さは同じ
      this.y = positionY;
      this.alpha = 1.0;
      this.scaleX = direction;
      this.setupMotion(this.x, positionY, direction);
    }
  },

  hideSelf: function () {
    this.alpha = 0;
    this.tweener.clear();
  }


})