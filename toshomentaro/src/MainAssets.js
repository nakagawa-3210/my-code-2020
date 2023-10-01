phina.define("MainAssets", {
  _static: {
    assets: {
      image: {
        //背景
        'background/title':'./assets/img/background/photo/title.jpg',
        'background/titleFrame':'./assets/img/background/photo/titleFrame.png',
        'background/scrollScene':'./assets/img/background/photo/scrollScene.jpg',
        'background/ShootingScene':'./assets/img/background/photo/ShootingScene.jpg',

        //キャラクター
        //img
        'chara/riderMentaro': './assets/img/chara/mentaroRideOnMentoun.png',
        'chara/zacookTest': './assets/img/chara/zacookTest.png',
        'chara/bicCook': './assets/img/chara/bicCook.png',
        'chara/damagedBicCook': './assets/img/chara/damagedBicCook.png',
        //spriteSheetImg
        'chara/mentarou_ss': './assets/img/chara/mentaroSpritesheet.png',
        'chara/zacook_ss': './assets/img/chara/zacookSpritesheet.png',
        'chara/minZacook_ss': './assets/img/chara/minZacookSpritesheet.png',
        'chara/damagedMinZacook_ss': './assets/img/chara/damagedMinZacookSpritesheet.png',
        // 'chara/happo': './assets/img/chara/masterHappo.png',
        
        //アイテム
        'item/tosyomen': './assets/img/item/tousyomenSpritesheet.png',
        'item/mentoun': './assets/img/item/mentoun.png',
        'item/menBullet': './assets/img/item/bullet.png',
        'item/minMenBullet': './assets/img/item/minBullet.png',
        'item/minUmeboshiBullet': './assets/img/item/minUmeboshi.png',
        'item/cabbageBullet': './assets/img/item/Cabbage.png',
        'item/minCabbageBullet': './assets/img/item/minCabbage.png',
        
        //タイルシート
        'tile/white': './assets/img/tiless/whiteTilesSpriteSheet.png',

        //リザルトシーンコメント
        'comment/resultOne': './assets/img/words/gameOverWords/gameOverWords1.png',
        'comment/resultTwo': './assets/img/words/gameOverWords/gameOverWords2.png',
        'comment/tsuduku': './assets/img/words/words8.png',
        //選択肢
        'option/retry': './assets/img/words/gameOverWords/options/retry.png',
        'option/giveUp': './assets/img/words/gameOverWords/options/giveUp.png',
        'option/circle': './assets/img/words/gameOverWords/options/circle.png',
      },
      sound: {
        //bgm
        'bgm/scroll':'./assets/sound/bgm/scroll.mp3',
        'bgm/shooting': './assets/sound/bgm/battleBoss.mp3',
        //voice
        'voice/mentaroAttack': './assets/sound/voice/mentaroAttack.mp3',
        'voice/mentaroDefeated1': './assets/sound/voice/mentaroDefeated1.mp3',
        'voice/mentaroDefeated2': './assets/sound/voice/mentaroDefeated2.mp3',
        'voice/mentaroDefeated3': './assets/sound/voice/mentaroDefeated3.mp3',
        'voice/zacook': './assets/sound/voice/zacook.mp3',
        'voice/bicCookDamaged': './assets/sound/voice/bicCookDamageed.mp3',
        'voice/bicCookDefeated': './assets/sound/voice/bicCookDefeated.mp3',
      },
      json: {
        'map/stageOneInit': './assets/json/stages/stageOne/stageoneInit.json',
        'map/stageOne': './assets/json/stages/stageOne/stageone.json',
        'map/stageOneNoCollider': './assets/json/stages/stageOne/stageoneNoCollider.json',
        'map/stageOneEnemy': './assets/json/stages/stageOne/stageoneEnemy.json',
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
              "frequency": 4,
            },
          }
        },
        "mentaro_ss":
        {
          "frame": {
            "width": 90,
            "height": 150,
            "cols": 3,
            "rows": 1,
          },
          "animations": {
            "walk": {
              "frames": [0, 1, 0, 2],
              "next": "walk",
              "frequency": 4,
            },
          }
        },
        "zacook_ss":
        {
          "frame": {
            "width": 90,
            "height": 150,
            "cols": 3,
            "rows": 1,
          },
          "animations": {
            "walk": {
              "frames": [1, 0, 2, 0],
              "next": "walk",
              "frequency": 4,
            },
          }
        },
        "tosyomen_ss":
        {
          "frame": {
            "width": 120,
            "height": 120,
            "cols": 2,
            "rows": 1,
          },
          "animations": {
            "fly": {
              "frames": [1, 0],
              "next": "fly",
              "frequency": 4,
            },
          }
        },
      }
    }
  }
});
