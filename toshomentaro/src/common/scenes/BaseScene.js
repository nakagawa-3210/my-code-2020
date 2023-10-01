/*
 * ベースシーン
 */
phina.define("BaseScene", {
  superClass: 'DisplayScene',
  init: function (option) {
    this.superInit(option);
  },

  setupMapAll: function () {
    
  },


  //「分割したい配列」と「分割する長さ」を渡す
  getFormattedArray: function (mapJson, n) {
    let mapBlocksData = AssetManager.get('json', mapJson)
      .data.layers[0].data;
    //配列のframeIndexを調整
    const indexedArr = mapBlocksData.map(x => x-1);
    let formattedArr = [];
    let idx = 0;
    while (idx < indexedArr.length) {
      formattedArr.push(indexedArr.splice(idx, idx + n));
    }
    return formattedArr;
  },

  // マップの中でもコライダー付きのブロックを生成
  generateMapBlocks: function (distance, group, json, img) {
    // 引数に移動距離を渡し、それに応じて画像を表示する関数
    // 例：1を渡せばタイル、2を渡せば柱、3を渡せば何も表示しない
    // 今回の場合は64の倍数の数字を渡す。
    //引数を64で割って、商-1番目の配列要素を読み込んで表示する
    //配列の表示するブロックはブロックグループの子要素にする

    const mapBaseGridNum = GAME_STAGE_WIDTH;
    const formattedArray = this.getFormattedArray(json, mapBaseGridNum);
    //複数の配列にかける関数
    for (var i = 0; i < formattedArray.length; i++) {
      const empty = -1;
      const blockArray = formattedArray[i];
      const blockArrLength = blockArray.length;
      const blockInfo = blockArray[(distance/BASE_GRID)-1];
      if(blockInfo !== empty && blockArrLength >= distance/BASE_GRID) {
        const block = ScrollBaseObject(img, BASE_GRID, BASE_GRID).addChildTo(group)
          .setPosition(GAME_INIT_MAP_END - BASE_GRID_HALF, BASE_GRID*i + BASE_GRID_HALF); 
        block.frameIndex = blockInfo;
        block.setCollider();
      }
    }
  },

  //マップの中でもコライダーなしのブロックを生成
  generateMapNoColliderBlocks: function (distance, group, json, img) {
    const mapBaseGridNum = GAME_STAGE_WIDTH;
    const formattedArray = this.getFormattedArray(json, mapBaseGridNum);
    for (var i = 0; i < formattedArray.length; i++) {
      const empty = -1;
      const noColliderArray = formattedArray[i];
      const noColliderArrLength = noColliderArray.length;
      const noColliderInfo = noColliderArray[(distance/BASE_GRID)-1];
      if(noColliderInfo !== empty && noColliderArrLength >= distance/BASE_GRID) {
        const noCollider = Sprite(img, BASE_GRID, BASE_GRID).addChildTo(group)
          .setPosition(GAME_INIT_MAP_END - BASE_GRID_HALF, BASE_GRID*i + BASE_GRID_HALF);
        noCollider.frameIndex = noColliderInfo;
      }
    }
  },
    
  //歩く敵を生成
  generateMapWalkingEnemy: function (json ,img, distance, group, width, height, direction) {
    const mapBaseGridNum = GAME_STAGE_WIDTH;
    const formattedArray = this.getFormattedArray(json, mapBaseGridNum);
    for (var i = 0; i < formattedArray.length; i++) {
      const empty = -1;
      const enemyArray = formattedArray[i];
      const enemyArrLength = enemyArray.length;
      const enemyInfo = enemyArray[(distance/BASE_GRID)-1]; 
      if(enemyInfo !== empty && enemyArrLength >= distance/BASE_GRID) {
        const imgWidth = width || 90;
        const imgHeight = height || 150;
        const imgDirection = direction || -1;
        const enemy = WalkingEnemy(img, imgWidth, imgHeight, imgDirection)
          .addChildTo(group).setPosition(GAME_INIT_MAP_END - BASE_GRID_HALF, BASE_GRID*i + BASE_GRID_HALF);
        enemy.frameIndex = enemyInfo;
      }
    }
  },
    


  //マップエディタで作成したjsonファイルから配列を作成し、
  // 配列を元にマップを作成する
  setMapBlocksFromFormatArr: function (mapJson, n) {
    const formmatedArr = this.getFormattedArray(mapJson, n);
    const mapBlocks = DisplayElement();
    //配列達を確認
    for (var i = 0; i < formmatedArr.length; i++) {
      //配列ごとの要素確認
      for (var j = 0; j < formmatedArr[i].length; j++) {
        let firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_HALF;
        let firstPositionY = GAME_SCREEN_TOP + BASE_GRID_HALF;
        let blockInfo = formmatedArr[i][j];
        const block = ScrollBaseObject('tile/white', BASE_GRID, BASE_GRID);
        block.setCollider();
        const empty = -1;
        if(blockInfo !== empty) {
          //表示画像位置設定
          block.frameIndex = blockInfo;
          block.addChildTo(mapBlocks)
            .setPosition(firstPositionX + BASE_GRID * j, firstPositionY + BASE_GRID * i);
        }
      }
    }
    return mapBlocks;
  },

  //敵の画像ファイル
  setMapenemiesFromFormatArr: function (enemyImgKey, enemyWidth, enemyHeight, mapJson, n) {
    const formmatedArr = this.getFormattedArray(mapJson, n);
    const mapEnemies = DisplayElement();
    //配列達を確認
    for (var i = 0; i < formmatedArr.length; i++) {
      //配列ごとの要素確認
      for (var j = 0; j < formmatedArr[i].length; j++) {
        let firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_HALF;
        let firstPositionY = GAME_SCREEN_TOP + BASE_GRID_HALF;
        let enemyInfo = formmatedArr[i][j];
        const enemy = WalkingEnemy(enemyImgKey, enemyWidth, enemyHeight);
        // enemy.setCollider();
        const enemyNum = 6;
        if(enemyInfo === enemyNum) {
          //表示画像位置設定
          enemy.frameIndex = enemyInfo;
          enemy.addChildTo(mapEnemies)
            .setPosition(firstPositionX + BASE_GRID * j, firstPositionY + (BASE_GRID - 5) * i);
        }
      }
    }
    return mapEnemies;
  },


  //共通の関数を用いてマップのオブジェクトを作成できるようにする
  //下記の三つの関数を一つにまとめる（引数は、ファイル名、画像サイズ）

  setMapBlocks: function (mapJson) {
    //jsonからデータ取得
    const mapBlocksData = AssetManager.get('json', mapJson).data;
    const mapBlocks = DisplayElement();
    //配列達を確認
    for (var i = 0; i < mapBlocksData.length; i++) {
      //配列ごとの要素確認
      for (var j = 0; j < mapBlocksData[i].length; j++) {
        let firstPositionX = GAME_SCREEN_LEFT + BASE_GRID_HALF;
        let firstPositionY = GAME_SCREEN_TOP + BASE_GRID_HALF;
        let blockInfo = mapBlocksData[i][j];
        const block = ScrollBaseObject('tile/tiles', BASE_GRID, BASE_GRID);
        const empty = -1;
        if(blockInfo !== empty) {
          //表示画像位置設定
          block.frameIndex = blockInfo;
          block.addChildTo(mapBlocks)
            .setPosition(firstPositionX + BASE_GRID * j, firstPositionY + BASE_GRID * i);
        }
      }
    }
    return mapBlocks;
  },

});