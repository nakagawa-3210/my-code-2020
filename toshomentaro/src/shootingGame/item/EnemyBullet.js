phina.define("EnemyBullet", {
  // 継承
  superClass: 'ShootingBaseObject',
  init: function () {    
    const bulletSize = BASE_GRID_HALF/2;
    this.superInit('item/minCabbageBullet', bulletSize, bulletSize);
    this.setCollider();

    // this.rotation = rotation || 0;
    // const speed = 10;
    // const self = this;
    // const top = 20;
    // const middle = 0;
    // const bottom = -20;
    // const bulletArr = [top, middle, bottom];

    // bulletArr.forEach((bulletDegree) => {
    //   const bulletSize = BASE_GRID_HALF/2;
    //   const bullet = BaseObject('item/minMenBullet', bulletSize, bulletSize)
    //   .addChildTo(self).setPosition(self.x, self.y);
    //   bullet.setCollider();
    //   const modification = 180;
    //   const shotDeg =bulletDegree + modification;
    //   const vec = Vector2().fromDegree(shotDeg, speed);
    //   //弾の各々に動きを個別でつける
    //   self.physical.velocity.x = vec.x;
    //   self.physical.velocity.y = vec.y;
    // })
  }
})