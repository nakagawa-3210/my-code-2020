phina.define('HitPointGauge', {
  superClass: 'Gauge',
  init: function (options) {
    console.log("options", options)
    this.superInit({
      //体力は基本100
      value: options.value || 100,
      gaugeColor: options.gaugeColor || '#44f',
      stroke: "black",
      strokeWidth: 8,
    });
    console.log("this.clolor", this.fill)
    const scale = options.scale || 0.6;
    this.scaleX = scale;
    this.scaleY = scale;
  },
  managePlayerHitPoint: function (damageValue) {
    this.value -= damageValue;
  }
});