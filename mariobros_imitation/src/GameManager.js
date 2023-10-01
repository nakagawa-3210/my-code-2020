/*
* GameManager
*ゲーム管理の関数をまとめたクラス
*/
phina.define("GameManager", {
  superClass: 'EventDispatcher',

  init: function () {
  },


  //キャラクターの動ける範囲を制限
  manageCharacterMoveRange: function (character) {
    if (character.x < GAME_SCREEN_LEFT) {
      character.x = GAME_SCREEN_RIGHT;
    } else if (character.x > GAME_SCREEN_RIGHT) {
      character.x = GAME_SCREEN_LEFT;
    }
  },


  //床判定のマップと関数
  setupIsOnTest: function (blocks, minTiles, item, character) {
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
    //空中タイルとプレイヤーのコライダー同士の当たり判定
    minTiles.children.each((minTile) => {
      isOnMap = this.checkIsOnStatus(isOnMap, minTile, character);
    })
    //アイテムとプレイヤーのコライダー同士の当たり判定
    isOnMap = this.checkIsOnStatus(isOnMap, item, character);
    return isOnMap;
  },


  //床を打ち上げられた所に敵がいるかの監視
  setupAttackedEnemyStatus: function (powBlock, blocks, minTiles, enemy, player) {
    let isBeingAttacked = false;
    minTiles.children.each((minTile) => {
      let playerIsOnPowMap = {
        "isOnTop": false,
        "isOnBottom": false,
        "isOnLeft": false,
        "isOnRight": false
      }
      let playerIsOnTileMap = {
        "isOnTop": false,
        "isOnBottom": false,
        "isOnLeft": false,
        "isOnRight": false
      };
      let enemyIsOnTileMap = {
        "isOnTop": false,
        "isOnBottom": false,
        "isOnLeft": false,
        "isOnRight": false
      };
      //プレイヤーとpowブロックの当たり判定
      playerIsOnPowMap = this.checkIsOnStatus(playerIsOnPowMap, powBlock, player);
      //プレイヤー、敵たちの空中ブロックに対する当たり判定
      playerIsOnTileMap = this.checkIsOnStatus(playerIsOnTileMap, minTile, player);
      enemyIsOnTileMap = this.checkIsOnStatus(enemyIsOnTileMap, minTile, enemy);

      //プレイヤーのたたいた床に敵が乗っている場合
      if (enemyIsOnTileMap.isOnTop && playerIsOnTileMap.isOnBottom) {
        isBeingAttacked = true;
      }
      //プレイヤーがpowをたたいた場合
      if (playerIsOnPowMap.isOnBottom) {
        //powを動かす
        this.setupMovingTile(powBlock);
        //地面をすべて上に浮かせる
        blocks.children.each((block) => {
          this.setupMovingTile(block);
        });
        minTiles.children.each((minTile) => {
          this.setupMovingTile(minTile);
        });
        //powブロックの高さ調整する

        //地上のブロックに敵が乗っているか判定
        blocks.children.each((block) => {
          enemyIsOnTileMap = this.checkIsOnStatus(enemyIsOnTileMap, block, enemy);
          if (enemyIsOnTileMap.isOnTop) {
            isBeingAttacked = true;
          }
        })
      }
    });
    return isBeingAttacked;
  },


  //ブロックを打ち上げる判定
  setupAttackTiles: function (minTiles, character) {
    //タイル一つ一つを上に打ち上げるかの判定
    minTiles.children.each((minTile) => {
      let isOnMap = {
        "isOnTop": false,
        "isOnBottom": false,
        "isOnLeft": false,
        "isOnRight": false
      };
      isOnMap = this.checkIsOnStatus(isOnMap, minTile, character);
      //タイルが下から叩かれたかつ、タイルが動いていないとき
      if (isOnMap.isOnBottom) {
        //上に打ち上げて元の位置に戻す
        this.setupMovingTile(minTile);
      }
    });
  },
  
  //タイルが上に上がる処理
  setupMovingTile: function (tile) {
    const tweenerTime = 50;//ミリ秒
    tile.tweener.clear()
      .to({
        y: tile.y - tile.height / 2
      }, tweenerTime)
      .to({
        y: tile.y
      }, tweenerTime);
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


  //プレイヤーと敵の当たり判定関数  
  playerHitTestAgainstEnemy: function (player, enemy) {
    let isHit = false;
    hitStatus = { isHit: false, enemyWeakStatus: false }
    enemy.accessories.each((enemyAccessory) => {
      player.accessories.each((playerAccessory) => {
        if (enemyAccessory.id !== undefined &&
          playerAccessory.id !== undefined &&
          enemyAccessory.hitTest(playerAccessory)) {
          isHit = true;
          hitStatus = { isHit: isHit, enemyWeakStatus: enemy.isWeak }
          // console.log("enemy.isWeak:",enemy.isWeak);
        }
      })
    })
    return hitStatus;
  },


  //敵グループに新しい敵の追加
  generateEnemy: function (enemyGroup) {
    let enemy = WalkingEnemy("chara/tomapiko", ENEMY_GRID, ENEMY_GRID);
    enemy.addChildTo(enemyGroup);
  },


  //敵のリスポーン関数(画面下両端のパイプに入ると、上のパイプにリスポーンする) 
  setupEnemyRespawn: function (enemy) {
    if (enemy.y > GAME_SCREEN_BOTTOM - BASE_GRID * 2 &&
      ((enemy.x < GAME_SCREEN_LEFT + BASE_GRID * 2) ||
        (enemy.x > GAME_SCREEN_RIGHT - BASE_GRID * 2))) {
      if (enemy.direction === ENEMY_DIRECTION_MAP.LEFT) {
        //右パイプから登場
        enemy.setPosition(ENEMY_SPAWN_MAP.RIGHT, ENEMY_SPAWN_MAP.APPEAR);
      } else if (enemy.direction === ENEMY_DIRECTION_MAP.RIGHT) {
        //左パイプから登場
        enemy.setPosition(ENEMY_SPAWN_MAP.LEFT, ENEMY_SPAWN_MAP.APPEAR);
      }
    }
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


  //敵の残機管理関数
  manageEnemyrStock: function (isHit, enemy, enemyStock) {
    if (isHit && !enemy.isDefeated) {
      enemyStock -= 1;
      enemy.isDefeated = true;
    }
    return enemyStock;
  },


  //ゲームオーバー管理
  isGameOver: function (scene, playerStock, enemyStock, timeLimit) {
    if (playerStock === 0 || timeLimit < 0) {
      //プレイヤーの残機がなくなったときはゲーム終了
      const exitClear = () => { scene.exit("result", option = GAME_EXIT_MAP.GAME_OVER) }
      setTimeout(exitClear, 100);
    } else if (enemyStock === 0) {
      //敵をすべて倒したときにはゲーム終了
      const exitOver = () => { scene.exit("result", option = GAME_EXIT_MAP.GAME_CLEAR) };
      setTimeout(exitOver, 100);
    }
  },

});