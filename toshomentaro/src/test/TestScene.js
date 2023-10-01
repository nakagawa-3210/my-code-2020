phina.define("TestScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    // console.log("testScene");
    this.backgroundColor = '#6495ed';
    this.one('enterframe', () => {
      this.key = this.app.keyboard;
    });

    //プレイヤーの移動距離
    this.runningDistance = 0;
    //ブロックを作ったかどうか
    this.generatedBlock = false;

    //敵を作ったかどうか
    this.generateEnemy = false;

    //ブロックグループ
    // this.blockGroup = DisplayElement().addChildTo(this)
    //   .setPosition(0, 0);
    

    //敵生成テスト
    //敵を重力なしで配置できるかを試す OK
    
    
    //敵初期設定(空)
    this.enemyGroup = DisplayElement().addChildTo(this).setPosition(0, 0);
    // const enemy = WalkingEnemy("test/zacook")
    //   .addChildTo(this.enemyGroup).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF)




    //初期マップ
    const stageWidth = GAME_INIT_MAP_END/BASE_GRID;
    this.blockGroup = this.setMapBlocksFromFormatArr("map/stageOneInit", stageWidth)
    .setPosition(0, 0).addChildTo(this);
    // const block = Sprite("tile/tiles", BASE_GRID, BASE_GRID)
    //   .addChildTo(this.blockGroup).setPosition(0, 0);
    // console.log("this.blockGroup:", this.blockGroup);
  },

  //jsonファイルの持つ複数の配列がもつ要素すべての中から、
  // プレイヤーが移動する距離から作られる条件に当てはまる要素を参照し、
  // 要素の持つ情報からブロックを一つ

  //毎フレーム処理
  update: function () {
    //走行距離のプラス
    if(this.key.getKey('right')) {
      this.runningDistance += PLAYER_SPEED;
      //子要素の移動
      for (var i = 0; i < this.blockGroup.children.length; i++) {
        const block = this.blockGroup.children[i];
        block.x -= PLAYER_SPEED;
      }
      for (var i = 0; i < this.enemyGroup.children.length; i++) {
        const enemy = this.enemyGroup.children[i];
        enemy.x -= PLAYER_SPEED;
      }
      // console.log("this.runningDistance:", this.runningDistance);
    }


    //64px進むたびにブロックを生成して、生成したことを表すフラグをtrueにする
    if(this.runningDistance !== 0 &&
      this.runningDistance % BASE_GRID === 0 &&
      this.generatedBlock === false) {
        // console.log("generate!!!");
        //ブロックの生成
        this.generateBlock(this.runningDistance, this.blockGroup);
        this.generatedBlock = true;
    }else if (this.runningDistance % BASE_GRID !== 0) {
      this.generatedBlock = false;
    }

    if(this.runningDistance !== 0 &&
      this.runningDistance % BASE_GRID === 0 &&
      this.generateEnemy === false) {
        //敵の生成
      this.generateWalkingEnemy("map/stageOneEnemy" ,"chara/zacook_ss" ,this.runningDistance, this.enemyGroup);
      this.generateEnemy = true;
    }else if (this.runningDistance % BASE_GRID !== 0) {
      this.generateEnemy = false;
    }


    
    if(this.runningDistance !== 0 &&
      this.runningDistance % BASE_GRID === 0 &&
      this.generatedBlock === false) {
        // console.log("generate!!!");
        //ブロックの生成
        this.generateBlock(this.runningDistance, this.blockGroup);
        this.generatedBlock = true;
    }else if (this.runningDistance % BASE_GRID !== 0) {
      this.generatedBlock = false;
    }
    
    

    //子要素の数を調整
    this.removeOldChild(this.blockGroup);


    //配列を用意して、特定の数字の場合のみブロックを生成
    // 複数列の配列を最終的に扱う
  },



  generateBlock: function (distance, group) {
  // 引数に移動距離を渡し、それに応じて画像を表示する関数
  // 例：1を渡せばタイル、2を渡せば柱、3を渡せば何も表示しない
  // 今回の場合は64の倍数の数字を渡す。
  //引数を64で割って、商-1番目の配列要素を読み込んで表示する
  //配列の表示するブロックはブロックグループの子要素にする///ここまでOK
    const mapBaseGridNum = GAME_STAGE_WIDTH;
    const formattedArray = this.getFormattedArray("map/stageOne", mapBaseGridNum)

    //複数の配列にかける関数
    for (var i = 0; i < formattedArray.length; i++) {
      const empty = -1;
      const blockArray = formattedArray[i];
      const blockArrLength = blockArray.length;
      const blockInfo = blockArray[(distance/BASE_GRID-1)];
      //if文内の処理で用いる親クラスが敵作成と異なる
      if(blockInfo !== empty && blockArrLength >= distance/BASE_GRID) {
        const block = ShootingBaseObject("tile/white", BASE_GRID, BASE_GRID)
          .addChildTo(group).setPosition(GAME_INIT_MAP_END - BASE_GRID_HALF, BASE_GRID*i + BASE_GRID_HALF); 
      block.frameIndex = blockInfo;
      block.setCollider();
      }
    }
  },

  generateWalkingEnemy: function (json ,img, distance, group) {
    const mapBaseGridNum = GAME_STAGE_WIDTH;
    const formattedArray = this.getFormattedArray(json, mapBaseGridNum)
    // console.log("formattedArray:", formattedArray);
    for (var i = 0; i < formattedArray.length; i++) {
      const empty = -1;
      const enemyArray = formattedArray[i];
      const enemyArrLength = enemyArray.length;
      const enemyInfo = enemyArray[(distance/BASE_GRID-1)]; 
      if(enemyInfo !== empty && enemyArrLength >= distance/BASE_GRID) {
        const width = 90;
        const height = 150;
        const direction = -1;
        const enemy = WalkingEnemy(img, width, height, direction)
          .addChildTo(group).setPosition(GAME_INIT_MAP_END - BASE_GRID_HALF, BASE_GRID*i + BASE_GRID_HALF);
        enemy.frameIndex = enemyInfo;
        enemy.setCollider();
      }
    }
  },

  
  removeOldChild: function (group) {
    // ブロックグループの子要素の数には制限をつけ、一定数以上になったら古い子要素を削除する
    //子要素の数の確認と削除はwhileループで行う
    // const groupLength = group.children.length;
    const limit = 120;
    // while(group.children.length > limit){
    //   const oldest = 0;
    //   group.children[oldest].remove();
    //   console.log("this.blockGroup:", this.blockGroup);
    // }
    for (var i = 0; i < group.children.length; i++) {
      let child = group.children[i];
      const behindPlayer = -100;
      if(child.x < behindPlayer) {
        child.remove();
        // console.log("this.blockGroup:", this.blockGroup);
      }
    }
    // console.log("next!")
    // if(groupLength < limit){
      
    // }
  },

  
});