/*
* ScrollGameManager
*ゲーム管理の関数をまとめたクラス
*/
phina.define("ScrollGameManager", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さない
  init: function () {},

  manageGameTime: function (scene, time, timerLabel) {
    //経過秒数更新
    time += scene.app.deltaTime;
    let timeLimit = Math.round(GAME_EXIT_MAP.SCROLL_GAME_TIME - Math.floor(time) / 1000);
    timerLabel.text = `TIME\n${timeLimit}`;
    return time;
  },

  //プレイヤーがどっちの向きに向いているかを管理
  managePlayerDirection: function (scene, direction) {
    const key = scene.app.keyboard;
    const right = 1;
    const left = -1;
    if (key.getKeyDown('left')) {
      direction = left;
    }
    if(key.getKeyDown('right')) {
      direction = right;
    }
    return direction;
  },

  //キャラクターの移動距離管理（条件式はmanageStageRangeと共通にしないといけない）
  //戻り値があるのでmanageStageRangeと合わせた一つの関数にはしない
  managePlayerDistance: function (character, isOnTest, distance) {
    //画面半分より右に行こうとしたとき
    if(character.x + character.width/2 > PLAYER_MOVE_RANGE_RIGHT &&
      !isOnTest.isOnLeft) {
      distance += PLAYER_SPEED;
    }
    return distance
  },

  //キャラクターの動ける範囲を制限
  manageCharacterMoveRange: function (character) {
    if (character.x < GAME_SCREEN_LEFT + character.width/2) {
      character.x = GAME_SCREEN_LEFT + character.width/2;
    } else if (character.x > PLAYER_MOVE_RANGE_RIGHT) {
      character.x = PLAYER_MOVE_RANGE_RIGHT;
    }
  },

  //プレイヤーが画面真ん中よりも右側に移動すると、itemsのx軸位置を左に移動させる
  manageStageRange: function (character, items, isOnTest) {
    //プレイヤーが画面半分よりも右側に移動したとき
    if (character.x + character.width/2 > PLAYER_MOVE_RANGE_RIGHT &&
        !isOnTest.isOnLeft) {
      for (var i = 0; i < items.children.length; i++) {
        const object = items.children[i];
        object.x -= PLAYER_SPEED;
      }
    }
  },

  //マップのコライダー付きブロック設置
  manageMapBlocks: function (runDistance, generateFlag, group, json, img) {
    //マップの動的生成
    //64px進むたびにブロックを生成して、生成したことを表すフラグをtrueにする
    if(runDistance !== 0 &&
      runDistance % BASE_GRID === 0 &&
      generateFlag === false) {
      //ブロックの生成
      BaseScene().generateMapBlocks(
        runDistance,
        group,
        json,
        img
      );
      generateFlag = true;
    }else if (runDistance % BASE_GRID !== 0) {
      generateFlag = false;
    }
    this.removeOldChild(group);
    return generateFlag;
  },


  manageMapNoColliderBlocks: function (runDistance, generateFlag, group, json, img) {
    if(runDistance !== 0 &&
      runDistance % BASE_GRID === 0 &&
      generateFlag === false) {
      //ブロックの生成
      BaseScene().generateMapNoColliderBlocks(
        runDistance,
        group,
        json,
        img
      );
      generateFlag = true;
    }else if (runDistance % BASE_GRID !== 0) {
      generateFlag = false;
    }
    this.removeOldChild(group);
    return generateFlag;
  },


  //マップ内の敵の設置と削除
  manageMapEnemies: function (runDistance, generateFlag, group, json, img) {
    if (runDistance !== 0 &&
      runDistance % BASE_GRID === 0 &&
      generateFlag === false) {
        BaseScene().generateMapWalkingEnemy(
          json,
          img,
          runDistance,
          group
        );
        generateFlag = true;
    }else if (runDistance % BASE_GRID !== 0) {
      generateFlag = false;
    }
    this.removeOldChild(group);
    return generateFlag;
  },


  //左側画面外の要素削除
  removeOldChild: function (group) {
    //移動距離が一定に達した際に古い子要素を削除する
    for (var i = 0; i < group.children.length; i++) {
      let child = group.children[i];
      const behindScreenLeft = -100;
      if(child.x < behindScreenLeft) {
        child.remove();
      }
    }
  },

  //床判定のマップと関数
  setupIsOnTest: function (blocks, character) {
    //関数に渡すマップの用意
    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    //地面とプレイヤーのコライダー同士の当たり判定
    blocks.children.each((block) => {
      isOnMap = this.checkIsOnStatus(isOnMap, block, character);
    })
    return isOnMap;
  },

  //タイルごとの接触テスト管理
  checkIsOnStatus: function (isOnMap, item, character) {
    item.accessories.each((itemAccessory) => {
      character.accessories.each((characterAccessory) => {
        if (characterAccessory.id !== undefined &&
          itemAccessory.id !== undefined &&
          itemAccessory.hitTest(characterAccessory)) {
          if (itemAccessory.id === "top" &&
            characterAccessory.id === "bottom" &&
            !character.isJumping) {
            character.physical.velocity.y = 0;
            character.physical.gravity.y = 0;
            isOnMap.isOnTop = true;
            character.y = item.top - character.height / 2;
          } else if (itemAccessory.id === "left" &&
            characterAccessory.id === "right") {
            isOnMap.isOnLeft = true;
          } else if (itemAccessory.id === "right" &&
            characterAccessory.id === "left") {
            isOnMap.isOnRight = true;
          } else if (itemAccessory.id === "bottom" &&
            characterAccessory.id === "top") {
            isOnMap.isOnBottom = true;
          }
        }
      });
    });
    return isOnMap;
  },

   //引数には当たり判定を行う対象の2つを渡す
  manageHitStatus: function (obj1, obj2) {
    let isHit = false;
    obj1.accessories.each((ob1Accessory) => {
      obj2.accessories.each((obj2Accessory) => {
        if (ob1Accessory.id !== undefined &&
          obj2Accessory.id !== undefined && 
          ob1Accessory.hitTest(obj2Accessory)) {
            isHit = true;
        }
      });
    });
    return isHit;
  },

  //プレイヤーの残機管理関数
  managePlayerStock: function (isHit, player, playerStock) {
    //点滅していないかつ、ダメージを受けた時に残機を減らす
    if (isHit && !player.tweener.playing) {
      //player残機を1減らす
      playerStock -= 1;
    }
    return playerStock;
  },

  //ゲームオーバー管理
  gameOverManager: function (scene, playerStock, playerY, time, distance, fade) {
    time += scene.app.deltaTime;
    let timeLimit = 
      Math.round(GAME_EXIT_MAP.SCROLL_GAME_TIME - Math.floor(time) / 1000);
    //残機ゼロ、落下、時間制限切れでゲームオーバー画面に遷移
    if (playerStock === 0 || playerY > GAME_SCREEN_BOTTOM || timeLimit === 0) {
      fade.fadeOut()
      fade.on("finishedFadeOut", () => {
        //BGMのストップ 
        SoundManager.stopMusic();
        //画面遷移
        scene.exit("gameOver", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT,
          from: GAME_SCENE_MAP.SCROLL_GAME
        }) 
      });
    }
    //ボスの間に到着でシューティングゲームへ遷移
    if(distance > GAME_EXIT_MAP.SCROLL_GAME_GOAL) {
      fade.fadeOut()
      fade.on("finishedFadeOut", () => {
        SoundManager.stopMusic();
        scene.exit("shootingMain", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT
        }) 
      });
    }
  },

});