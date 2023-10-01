phina.define("MainAssets", {
  _static: {
    assets: {
      image: {
        //キャラクター
        'chara/tomapiko': './assets/img/tomapiko_ss.png',
        'chara/takepiyo': './assets/img/takepiyo.png',
        'chara/meropiyo': './assets/img/meropiyo.png',
        'chara/mikapiyo': './assets/img/mikapiyo.png',
        'chara/nasupiyo': './assets/img/nasupiyo.png',
        //タイル(マップオブジェクト)
        'tile/tiles': './assets/img/tiles.png',
        'tile/minTiles': './assets/img/minTiles.png',
      },
      json: {
        'map/blockOne': './assets/json/blockMapOne.json',
        'map/minTileOne': './assets/json/minTileMapOne.json',
        'map/pipesOne': './assets/json/pipeMapOne.json',
      },
      spritesheet: {
        "tomapiko_ss":
        {
          "frame": {
            "width": 64,
            "height": 64,
            "cols": 6,
            "rows": 3,
          },
          "animations": {
            "walk": {
              "frames": [12, 13, 14],
              "next": "walk",
              "frequency": 6,
            },
          }
        },
      }
    }
  }
});
