phina.define("ShootingGameManager", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さない
  init: function () { },

  manageGameTime: function (scene, time, timerLabel, bossCurrentHpValue) {
    if (bossCurrentHpValue !== 0) {
      //経過秒数更新
      time += scene.app.deltaTime;
      let timeLimit = Math.round(GAME_EXIT_MAP.SHOOTING_GAME_TIME - Math.floor(time) / 1000);
      if (timeLimit < 0) {
        timeLimit = 0;
      }
      timerLabel.text = `TIME\n${timeLimit}`;
      return time;
    }
  },

  //プレイヤーの移動範囲制限
  managePlayerMoveRange: function (player) {
    const playerSelf = player.player;
    const playerGauge = player.hitPointGauge;
    const playerSelfGaugeGap = playerSelf.y - playerGauge.y;
    if (playerSelf.y < PLAYER_MOVE_RANGE_TOP) {
      playerSelf.y = PLAYER_MOVE_RANGE_TOP;
      playerGauge.y = PLAYER_MOVE_RANGE_TOP - playerSelfGaugeGap;
    }
    if (playerSelf.y > PLAYER_MOVE_RANGE_BOTTOM - BASE_GRID) {
      playerSelf.y = PLAYER_MOVE_RANGE_BOTTOM - BASE_GRID;
      playerGauge.y = PLAYER_MOVE_RANGE_BOTTOM - BASE_GRID - playerSelfGaugeGap;
    }
    if (playerSelf.x < SHOOTING_PLAYER_MOVE_RANGE_LEFT) {
      playerSelf.x = SHOOTING_PLAYER_MOVE_RANGE_LEFT;
      playerGauge.x = SHOOTING_PLAYER_MOVE_RANGE_LEFT;
    }
    if (playerSelf.x > SHOOTING_PLAYER_MOVE_RANGE_RIGHT) {
      playerSelf.x = SHOOTING_PLAYER_MOVE_RANGE_RIGHT;
      playerGauge.x = SHOOTING_PLAYER_MOVE_RANGE_RIGHT;
    }
  },

  //プレイヤー、ボス敵、ザコ敵の当たり判定管理
  managePlayerIsHitStatus: function (player, bossBullet, boss, zakoBullet, zakoGroup) {
    let isHit = false;
    //テスト中のためコメントアウト(ザコテキの弾)
    //ボスから受ける攻撃
    isHit = this.hitTestEnemyBulletPlayer(
      bossBullet,
      player,
      isHit
    );
    isHit = this.hitTestPlayerEnemy(
      player,
      boss,
      isHit
    );
    //ザコ敵から受ける攻撃
    isHit = this.hitTestEnemyBulletPlayer(
      zakoBullet,
      player,
      isHit
    );
    for (var i = 0; i < zakoGroup.children.length; i++) {
      const zacook = zakoGroup.children[i].children[0];
      isHit = this.hitTestPlayerEnemy(
        player,
        zacook,
        isHit
      )
    }
    return isHit
  },


  manageZacook: function (zacookGroup, playerBullet) {
    for (var i = 0; i < zacookGroup.children.length; i++) {
      let zacook = zacookGroup.children[i];
      let zacookSelf = zacook.children[0];
      zacookSelf.isDamaged = this.hitTestPlayerBulletZacook(
        playerBullet,
        zacookSelf
      );
      zacook.manageHitStatus(zacookSelf.isDamaged, PLAYER_ATTACK_POINT);
      this.removeZacook(zacook);
    }
  },


  //敵本体とプレイヤーの当たり判定
  hitTestPlayerEnemy: function (player, enemy, playerHitStatus) {
    let isHit = playerHitStatus;
    player.accessories.each((playerAccessory) => {
      enemy.accessories.each((enemyAccessory) => {
        if (playerAccessory.id !== undefined &&
          enemyAccessory.id !== undefined &&
          playerAccessory.hitTest(enemyAccessory)) {
          isHit = true;
        }
      });
    });
    return isHit;
  },

  //弾の削除
  removeBullet: function (bullet) {
    bullet.remove();
  },

  //プレイヤーと敵の弾の当たり判定
  hitTestEnemyBulletPlayer: function (bulletGroup, player, hitStatus) {
    let isHit = hitStatus;
    bulletGroup.children.each((bullet) => {
      bullet.accessories.each((bulletAccessory) => {
        player.accessories.each((playerAccessory) => {
          if (bulletAccessory.id !== undefined &&
            playerAccessory.id !== undefined &&
            bulletAccessory.hitTest(playerAccessory)) {
            //攻撃が当たった判定を記録
            isHit = true;
            //被弾した弾の削除
            this.removeBullet(bullet);
          }
        })
      })
    });
    return isHit;
  },

  // ボスの鼻とプレイヤーの弾の当たり判定
  hitTestPlayerBulletBossNose: function (bulletGroup, boss) {
    let isHit = false;
    bulletGroup.children.each((bullet) => {
      bullet.accessories.each((bulletAccessory) => {
        boss.accessories.each((bossAccessory) => {
          if (bulletAccessory.id !== undefined &&
            bossAccessory.id !== undefined &&
            bossAccessory.id === "nose" &&
            bulletAccessory.hitTest(bossAccessory)) {
            isHit = true;
            this.removeBullet(bullet)
          }
        })
      })
    });
    return isHit;
  },

  //ザコ敵とプレイヤーの弾の当たり判定
  hitTestPlayerBulletZacook: function (bulletGroup, zacook) {
    let isHit = false;
    bulletGroup.children.each((bullet) => {
      bullet.accessories.each((bulletAccessory) => {
        zacook.accessories.each((zacookAccessory) => {
          if (bulletAccessory.id !== undefined &&
            zacookAccessory.id !== undefined &&
            bulletAccessory.hitTest(zacookAccessory)) {
            isHit = true;
            this.removeBullet(bullet);
          }
        })
      });
    });
    return isHit;
  },

  manageBossHitPointValue: function (bossCurrentHpValue, bossHpGauge, damagedBoss) {
    const show = 1;
    // let currentGaugeValue = bossCurrentHpValue;
    if (damagedBoss.alpha === show) {
      bossCurrentHpValue -= PLAYER_ATTACK_POINT;
      currentGaugeValue = Math.round((bossCurrentHpValue / BOSS_HP) * 100);
      bossHpGauge.value = currentGaugeValue;
    }
    return bossCurrentHpValue;
  },

  setupZacookMotion: function (scene, zacookGroup) {
    for (var i = 0; i < ZACOOK_NUM; i++) {
      let number = i + 1;
      let zacook = zacookGroup.children[i];
      let zacookSelf = zacook.children[0];
      //表示
      const show = 1;
      zacookSelf.alpha = show;
      //動き追加
      zacook.setupMotion(scene, number);
    }
  },

  removeZacook: function (zacook) {
    if (zacook.currentHitPoint === 0) {
      zacook.remove();
    }
  },

  manageBossMotion: function (boss, bossCurrentHpValue, bossWeak, motionCounter) {
    if (!boss.normalBoss.tweener.playing) {
      if (bossCurrentHpValue > bossWeak) {
        motionCounter = this.manageBossFirstMotion(motionCounter, boss);
      } else {
        motionCounter = this.manageBossSecondMotion(motionCounter, boss);
      }
    }
    return motionCounter;
  },

  //ボスの動き1
  manageBossFirstMotion: function (counter, boss) {
    if (counter === 1) {
      boss.motionOne();
      counter += 1;
    } else {
      boss.motionTwo();
      counter = 1;
    }
    return counter;
  },

  //ボスの動き2
  manageBossSecondMotion: function (counter, boss) {
    if (counter === 1) {
      boss.motionThree();
      counter += 1;
    } else {
      boss.motionFour();
      counter = 1;
    }
    return counter;
  },

  //キャラクターはisDefeatedがtureの時に弾が非表示になる
  manageIsDefeatedMotion: function (player, boss, zacookGroup) {
    const removeSpeed = 5;
    //ボスが勝利した時
    if (player.hitPointGauge.value == 0) {
      const playerSelf = player.player;
      if (!playerSelf.tweener.playing) {
        //敗北にする
        player.isDefeated = true;
        //敗北モーション起動
        player.defeatedMotion();
      }
      if (player.isDefeated) {
        //画面の左外側に移動
        player.x -= removeSpeed;
      }
      //画面外に出たことを検知で画面遷移
    }
    //プレイヤーが勝利した時
    else {
      if (!boss.normalBoss.tweener.playing) {
        //ボス敗北設定
        boss.isDefeated = true;
        boss.motionDefeated();
        SoundManager.play("voice/bicCookDefeated");
        //ザコ敵敗北設定
        for (var i = 0; i < zacookGroup.children.length; i++) {
          zacookGroup.children[i].isDefeated = true;
        }
      }
      //敵たちを画面右外側へ移動
      if (boss.isDefeated) {
        zacookGroup.x += removeSpeed;
        boss.x += removeSpeed;
      }
    }
  },

  gameOverManager: function (scene, player, boss, timeLimit, fade) {
    if ((player.x < GAME_SCREEN_LEFT || timeLimit === 0) && !fade.tweener.playing) {
      //ゲームオーバーシーンに移動
      fade.fadeOut()
      fade.on("finishedFadeOut", () => {
        //BGMのストップ 
        SoundManager.stopMusic();
        //画面遷移
        scene.exit("gameOver", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT,
          from: GAME_SCENE_MAP.SHOOTING_GAME
        })
      });
    }
    if (boss.x > GAME_SCREEN_RIGHT - BASE_GRID * 10 && !fade.tweener.playing) {
      //ゲームクリアシーンに移動
      fade.fadeOut()
      fade.on("finishedFadeOut", () => {
        //BGMのストップ 
        SoundManager.stopMusic();
        //画面遷移
        scene.exit("gameClear", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT,
          from: GAME_SCENE_MAP.SHOOTING_GAME
        })
      });
    }
  }

});