phina.define("ArcBullet", {
  superClass: "ShootingBaseObject",
  init: function () {
    this.superInit("bullet", 24, 24);
    this.velocity = 
      phina.geom.Vector2(Math.cos(rad) * speed, Math.sin(rad) * speed);
  },

  setup: function (frameIndex, x, y, direction, power) {
    // this.frameIndex = frameIndex;
    // this.x = x;
    // this.y = y;
    // this.rotation = direction;
    // this.power = power;
    // var degree = degree * ( Math.PI / 180 ) ;
    // var rad = direction * Math.DEG_TO_RAD;
    this.velocity = phina.geom.Vector2(Math.cos(rad) * speed, Math.sin(rad) * speed);
    return this;
  },
});
