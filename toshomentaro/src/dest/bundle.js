phina.define('Application', {
  superClass: 'phina.display.CanvasApp',

  init: function (options) {
    this.superInit(options);
    //ロード終了後処理

    this.replaceScene(MyManagerScene({}));
  },
});
'use strict';
console.log("hoge");
phina.globalize();
// 画面サイズ情報
const GAME_SCREEN_WIDTH = 1280;
const GAME_SCREEN_HEIGHT = 768;
const GAME_SCREEN_WIDTH_HALF = GAME_SCREEN_WIDTH / 2;
const GAME_SCREEN_HEIGHT_HALF = GAME_SCREEN_HEIGHT / 2;
const GAME_SCREEN_TOP = 0;
const GAME_SCREEN_BOTTOM = GAME_SCREEN_HEIGHT;
const GAME_SCREEN_LEFT = 0;
const GAME_SCREEN_RIGHT = GAME_SCREEN_WIDTH;
const GAME_STAGE_WIDTH = 200;
const GAME_INIT_MAP_END = GAME_SCREEN_WIDTH*1.5;

// ステージ内共通サイズ
const BASE_GRID = 64;
const BASE_GRID_HALF = BASE_GRID / 2;
const BASE_GRID_QUARTER = BASE_GRID_HALF / 2;
const CHARACTER_GRID = 64;
const ITEM_GRID = 48;

// 共通重力
const ZERO_GRAVITY = 0;


// player情報
const MENTARO_FILE_SIZE = 750;
const MENTARO_SCALE = 0.2;
const MENTARO_GRID = MENTARO_FILE_SIZE*MENTARO_SCALE;

const MENTARO_HEIGHT = 150;
const MENTARO_WIDTH = 90;
const PLAYER_JUMP_POWER = 15;
const PLAYER_SPEED = 8;
const PLAYER_GRAVITY = 0.55;
const PLAYER_STOCK = 3;
//プレイヤーの移動範囲
const PLAYER_MOVE_RANGE_LEFT = GAME_SCREEN_LEFT;
const PLAYER_MOVE_RANGE_RIGHT = GAME_SCREEN_RIGHT/2;



// 敵情報
const ENEMY_GRID = 64;
const ENEMY_SPEED = 5;
const ENEMY_BERSERK_SPEED = 20;
const ENEMY_GRAVITY = 0.55;
const NUMBER_OF_ENEMY = 5;
const ENEMY_INTERVAL = 100;

//シューティングボス
const BOSS_HP = 300;

// 敵の位置マップ
const ENEMY_SPAWN_MAP = {
	LEFT: GAME_SCREEN_LEFT + BASE_GRID * 2, 
	RIGHT: GAME_SCREEN_RIGHT - BASE_GRID * 2,
	APPEAR: BASE_GRID/2,
	DISAPPEAR: GAME_SCREEN_BOTTOM - BASE_GRID * 2,
};

// 敵の移動方向マップ
const ENEMY_DIRECTION_MAP = {
	LEFT: -1,
	RIGHT: 1
};

// ゲーム終了マップ
const GAME_EXIT_MAP = {
	GAME_TIME: 100,
	GAME_CLEAR: {img: "game/cureClear", gridSize: 340, message: "GAME CLEAR!"},
	GAME_OVER: {img: "chara/precure", gridSize: BASE_GRID, message: "GAME OVER!"}
}
phina.define("MainAssets", {
  _static: {
    assets: {
      image: {
        //背景
        'background/title': './assets/img/background/title.jpg',
        'background/game': './assets/img/background/notredame.jpg',
        'background/hongkong': './assets/img/background/hongkong.jpg',

        //キャラクター
        //img
        'chara/riderMentaro': './assets/img/chara/mentaroRideOnMentoun.png',
        'chara/zacookTest': './assets/img/chara/zacookTest.png',
        'chara/bicCook': './assets/img/chara/bicCook.png',
        'chara/damagedBicCokk': './assets/img/chara/damagedBicCook.png',
        //spriteSheetImg
        'chara/mentarou_ss': './assets/img/chara/mentaroSpritesheet.png',
        'chara/zacook_ss': './assets/img/chara/zacookSpritesheet.png',
        // 'chara/happo': './assets/img/chara/masterHappo.png',
        
        //アイテム
        'item/tosyomen': './assets/img/item/tousyomenSpritesheet.png',
        'item/mentoun': './assets/img/item/mentoun.png',
        'item/menBullet': './assets/img/item/bullet.png',
        'item/minMenBullet': './assets/img/item/minBullet.png',

        //タイルシート
        'tile/white': './assets/img/tiless/whiteTilesSpriteSheet.png',

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

phina.define('MyManagerScene', {
  superClass: 'ManagerScene',
  init: function (option) {
    this.superInit({
      scenes: [
        // {
        //   label: "initialLoad",
        //   className: "InitialLoadScene",
        //   argument: asset,
        // },
        {
          label: "mainLoad",
          className: "MainLoadScene",
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT,
          arguments: {
            assets: MainAssets.assets,
            screenSize: {
              width: GAME_SCREEN_WIDTH,
              height: GAME_SCREEN_HEIGHT
            }
          },
        },
        //マップ生成用テストシーン
        {
          label: "test",
          className: "TestScene",
        },
        {
          label: 'shootTest',
          className: 'ShootingTest'
        },
        {
          label: "title",
          className: "TitleScene",
        },
        {
          label: 'scrollMain',
          className: 'ScrollGameScene',
        },
        {
          label: 'shootingMain',
          className: 'ShootingGameScene'
        },
        {
          label: 'result',
          className: 'ResultScene',
        },
        
        {
          label: 'testspace',
          className: 'TestScene'
        }
      ]
    });
  }
});　