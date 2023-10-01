/*
 * ベースシーン
 */
phina.define("BaseScene", {
  superClass: 'DisplayScene',
  init: function (option) {
    this.superInit(option);
  },

  setupMapAll: function () {

    //画面地面
    this.blockGroup = this.setMapBlocks().addChildTo(this).setPosition(0, 0);
    //画面空中タイル
    this.minTileGroup = this.setMapMinTiles().addChildTo(this).setPosition(0, 0);
    //画面パイプ
    this.pipeGroup = this.setMapPipes().addChildTo(this).setPosition(0, 0);
  },

  //共通の関数を用いてマップのオブジェクトを作成できるようにする
  //下記の三つの関数を一つにまとめる（引数は、ファイル名、画像サイズ）

  setMapBlocks: function () {
    //jsonからデータ取得
    const blockData = AssetManager.get('json', 'map/blockOne').data;
    const blockGroup = DisplayElement()
    for (var i = 0; i < blockData.length; i++) {
      for (var j = 0; j < blockData[i].length; j++) {
        //画像の中心の位置を指定して画像を配置するため、画像幅の半分を計算に含める
        const firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_HALF;
        const gameScreenBottom = GAME_SCREEN_HEIGHT - BASE_GRID / 2;
        const block = BaseObject('tile/tiles', BASE_GRID, BASE_GRID)
        block.frameIndex = blockData[i][j];
        block.addChildTo(blockGroup).setPosition(firstPositionX + BASE_GRID * j, gameScreenBottom);
      }
    }
    return blockGroup;
  },

  setMapMinTiles: function () {
    const minTileData = AssetManager.get('json', 'map/minTileOne').data;
    const minTileGroup = DisplayElement()
    for (var i = 0; i < minTileData.length; i++) {
      for (var j = 0; j < minTileData[i].length; j++) {
        let firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_QUARTER;
        let firstPositionY = GAME_SCREEN_TOP + BASE_GRID_QUARTER;
        const minTile = BaseObject('tile/minTiles', BASE_GRID_HALF, BASE_GRID_HALF);
        const empty = -1;
        if (minTileData[i][j] !== empty) {
          minTile.frameIndex = minTileData[i][j];
          minTile.addChildTo(minTileGroup).setPosition(firstPositionX + BASE_GRID_HALF * j, firstPositionY + BASE_GRID_HALF * i);
        }
      }
    }
    return minTileGroup;
  },

  setMapPipes: function () {
    const pipeData = AssetManager.get('json', 'map/pipesOne').data;
    const pipeGroup = DisplayElement()
    for (var i = 0; i < pipeData.length; i++) {
      for (var j = 0; j < pipeData[i].length; j++) {
        // console.log(pipeData[i][j]);
        let firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_QUARTER;
        let firstPositionY = GAME_SCREEN_TOP + BASE_GRID_QUARTER;
        const pipe = BaseObject('tile/tiles', BASE_GRID, BASE_GRID);
        const empty = -1;
        if (pipeData[i][j] !== empty) {
          pipe.scaleX = 0.5;
          pipe.scaleY = 0.5;
          pipe.rotation += 90;
          pipe.frameIndex = pipeData[i][j];
          pipe.addChildTo(pipeGroup).setPosition(firstPositionX + BASE_GRID_HALF * j, firstPositionY + BASE_GRID_HALF * i);
        }
      }
    }
    return pipeGroup;
  },

  setupItem: function () {
    const item = BaseObject('item/cureFairy', ITEM_GRID, ITEM_GRID);
    return item;
  }
});