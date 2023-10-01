phina.define("StraightforwardBullet", {
  superClass: 'ShootingBaseObject',

  //引数に方向をもらう
  init: function (direction, gravity, bulletSpeed, img) {
    this.direction = direction || 1;
    this.gravity = gravity || 5;
    this.img = img || 'item/minMenBullet'
    const bulletSize = BASE_GRID_HALF/2;
    this.superInit(this.img, bulletSize, bulletSize);
    this.setCollider();
    const speed = bulletSpeed || 30;
    this.physical.velocity.x = speed * this.direction;
    this.physical.velocity.y = this.gravity ;
    // const speed = 30;
    // const gap = 20;
    // const hight = -gap;
    // const low = gap;
    // const self = this;
    //弾のy座標位置
    // const bulletPositionArr = [hight, low];
    //弾の作成
    // bulletPositionArr.forEach((bulletY) => {
    //   const bulletSize = BASE_GRID_HALF/2;
    //   const bullet = BaseObject('item/minMenBullet', bulletSize, bulletSize)
    //     .addChildTo(self).setPosition(self.x, self.y + bulletY);
    //   //弾のコライダー作成
    //   const colliderW = bullet.width;
    //   const colliderH = bullet.height;
    //   const colliderX = this.x;
    //   let colliderY = null;
    //   const modification = 0.5;
    //   if(bulletY === hight) {
    //     //コライダー位置調整
    //     colliderY = this.y + modification;
    //   }else {
    //     colliderY = this.y - modification;
    //   }
    //   bullet.setCollider(colliderW, colliderH, colliderX, colliderY);
    //   // console.log("bullet:", bullet);
    // });
    //弾の移動
    // self.physical.velocity.x = speed * direction;
    // self.physical.velocity.y = 5;
  },

});