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
//ゲーム音
const GAME_BGM_VOLUME = 0.2;


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
const PLAYER_STOCK = 5;
const PLAYER_ATTACK_POINT = 1;
const PLAYER_HIT_POINT = 100;
//プレイヤーの移動範囲
	//シューティングゲーム
const PLAYER_MOVE_RANGE_TOP = GAME_SCREEN_TOP + BASE_GRID*2;
const PLAYER_MOVE_RANGE_BOTTOM = GAME_SCREEN_BOTTOM - BASE_GRID;
const SHOOTING_PLAYER_MOVE_RANGE_LEFT = GAME_SCREEN_LEFT + BASE_GRID*1.5;
const SHOOTING_PLAYER_MOVE_RANGE_RIGHT = GAME_SCREEN_WIDTH_HALF*1.25;
	//スクロールゲーム 
const PLAYER_MOVE_RANGE_LEFT = GAME_SCREEN_LEFT;
const PLAYER_MOVE_RANGE_RIGHT = GAME_SCREEN_RIGHT/2;



// 敵情報
const ENEMY_GRID = 64;
const ENEMY_SPEED = 3;
const ENEMY_BERSERK_SPEED = 20;
const ENEMY_GRAVITY = 0.55;
const ENEMY_INTERVAL = 100;

//シューティングボス
const BOSS_HP = 250;
const BOSS_AT = 6;
//ザコ敵
const ZACOOK_NUM = 15;
const ZACOOK_HP = 12;
const ZACOOK_AT = 2;
const TOTAL_ROTATE_TIME = 4000;

// 敵の移動方向マップ
const ENEMY_DIRECTION_MAP = {
	LEFT: -1,
	RIGHT: 1
};

const GAME_SCENE_MAP = {
  SCROLL_GAME: "scrollGame",
  SHOOTING_GAME: "shootingGame",
}

// ゲーム終了マップ
const GAME_EXIT_MAP = {
	SCROLL_GAME_TIME: 100,
	SCROLL_GAME_GOAL: 12800,
	SHOOTING_GAME_TIME: 180,
	SCROLL_GAME_CLEAR: {img: "game/cureClear", gridSize: 340, message: "GAME CLEAR!"},
	SHOOTING_GAME_CLEAR: {img: "game/cureClear", gridSize: 340, message: "GAME CLEAR!"},
	GAME_OVER: {img: "chara/precure", gridSize: BASE_GRID, message: "GAME OVER!"}
}
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
          label: 'scrollResult',
          className: 'ScrollGameResultScene',
        },
        {
          label: 'shootingMain',
          className: 'ShootingGameScene'
        },
        {
          label: 'gameOver',
          className: 'GameOverScene',
        },
        {
          label: 'gameClear',
          className: 'GameClearScene',
        },
        
        {
          label: 'testspace',
          className: 'TestScene'
        }
      ]
    });
  }
});　
/*
* FadeInOut
*画面遷移時のフェードクラス
*/
phina.define("FadeInOut", {
  superClass: 'RectangleShape',
  init: function () {
    this.superInit();
    this.width = GAME_SCREEN_WIDTH;
    this.height = GAME_SCREEN_HEIGHT;
    this.stroke = 'transparent',
    this.strokeWidth = 0;
    this.fill = "black";
    // this.setOrigin(0, 0);
    this.alpha = 0.0;
  },

  fadeIn: function () {
    const show = 1;//fade自身が姿を表す
    const hide = 0;//fade自身が姿を隠す
    this.alpha = show;
    const fadeTime = 1500;
    this.tweener.clear().to({
      alpha: hide
    } ,fadeTime, "linear").call(() => {
      this.flare("finishedFadeIn");
    });
  },

  fadeOut: function () {
    const show = 1;
    const fadeTime = 1500;
    this.tweener.clear().to({
      alpha: show
    } ,fadeTime, "linear").call(()=> {
      this.flare("finishedFadeOut");
    });
  }
});  
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
phina.define("GameClearScene", {
  superClass: 'BaseScene',
  init: function (options) {
    this.superInit(options);
    this.setupBackground();
    this.setupScreenFade();
  },
  setupBackground: function () {
    //メッセージ
    this.message = Sprite("comment/tsuduku", 323, 100)
      .addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(-1));
    //製作者
    this.creators = Label({
      text: 'イラスト: M.K JONES\n\nプログラム: はらぺこちゃん',
      fontSize: 16,
      fill: 'white',
      fontWeight: 'bold',
      stroke: 'black',
      strokeWidth: 6
    }).addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(4));
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },
});
phina.define("GameOverScene", {
  superClass: 'BaseScene',
  init: function (options) {
    this.superInit(options);
    this.options = options;
    this.fromScene = options.from;
    this.options = options;
    this.backgroundColor = "white";
    this.retry = -1;
    this.giveUp = 1;
    this.decision = this.retry;
    this.one('enterframe', () => {
      this.setupWords();
      this.setupScreenFade();
    });
  },

  setupWords: function () {
    const wordsW = 447;
    const wordsH = 102
    //励ましのお言葉
    this.comments = Sprite("comment/resultOne", wordsW, wordsH)
      .addChildTo(this).setPosition(this.gridX.center(), this.gridY.center(-1));
    const optionX = 3;
    const optionY = 3;
    //あきらめない
    const retryW = 190;
    const retryH = 62;
    this.optionRetry = Sprite("option/retry", retryW, retryH)
      .addChildTo(this).setPosition(this.gridX.center(-optionX), this.gridY.center(optionY));
    //あきらめる
    const giveUpW = 141;
    const giveUpH = 59;
    this.optionGiveUp = Sprite("option/giveUp", giveUpW, giveUpH)
      .addChildTo(this).setPosition(this.gridX.center(optionX), this.gridY.center(optionY));
    //選択の丸
    const circleW = 448;
    const circleH = 269;
    const scale = 1.3;
    this.optionDecision = Sprite("option/circle", circleW, circleH)
      .addChildTo(this).setPosition(this.gridX.center(-optionX), this.gridY.center(optionY + 0.5));
    this.optionDecision.scaleX = scale;
    this.optionDecision.scaleY = scale;
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },

  update: function () {
    if (!this.fade.tweener.playing) {
      const key = this.app.keyboard;
      //選択肢変更
      const optionX = 3;
      //あきらめない
      if (key.getKey('left')) {
        this.optionDecision.x = this.gridX.center(-optionX);
        this.decision = this.retry;
      }
      //あきらめる
      if (key.getKey('right')) {
        this.optionDecision.x = this.gridX.center(optionX);
        this.decision = this.giveUp;
      }
      //決定
      if (key.getKeyDown('space')) {
        this.fade.fadeOut();
        this.fade.on("finishedFadeOut", () => {
          //元来たシーンに遷移
          if (this.decision === this.retry) {
            if (this.fromScene === GAME_SCENE_MAP.SCROLL_GAME) {
              this.exit('scrollMain', this.options);
            } else {
              this.exit('shootingMain', this.options);
            }
          }
          //タイトルシーンに遷移
          else {
            this.exit('title', this.options);
          }
        })
      }
    }
  },

});
//タイトル表示前のロードシーン
phina.define('MainLoadScene', {
  superClass: 'DisplayScene',

  init: function (options) {
    this.superInit(options);
    const loader = phina.asset.AssetLoader();

    loader.onload = () => {

      //確認後戻す
      // this.exit('shootingMain', options.screenSize);
      // this.exit('scrollMain', options.screenSize);
      //テスト中のためコメントアウト
      this.exit('title', options.screenSize);

      // this.exit('gameClear', options.screenSize);
    }
    loader.load(options.assets);
  },
});
/*
 * タイトルシーン
 */
phina.define("TitleScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.setupBackground();
    // テスト
    // SoundManager.playMusic('bgm/shooting');
    //タイトル
    this.titleLabel = Label({
      text: '刀 削 麺 太 郎\n物 語',
      fontSize: 124,
      fill: 'white',
      fontWeight: 'bold',
      stroke: 'black',
      strokeWidth: 20
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_TOP);
    //メッセージ
    this.titleMessage = Label({
      text: `push 'Enter' to start`,
      fontSize: 48,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 10
    }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF * 0.65);

    //タイトルとメッセージに動きの追加
    this.titleLabel.tweener.clear().to({
      y: GAME_SCREEN_HEIGHT_HALF
    }, 800, "easeInBack")
      .to({
        y: GAME_SCREEN_HEIGHT_HALF * 0.85
      }, 500, "easeInBack");

    this.titleMessage.tweener.clear().to({
      y: GAME_SCREEN_HEIGHT_HALF * 1.65
    }, 800, "easeInBack")
      .to({
        y: GAME_SCREEN_HEIGHT_HALF * 1.5
      }, 500, "easeInBack").call(() => {
        this.titleMessage.tweener.clear().fadeOut(800).fadeIn(800).setLoop(true);
      })
    //画面遷移時のフェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());;
  },

  setupBackground: function () {
    this.background = Sprite('background/title').addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center())
    this.backgroundFrame = Sprite('background/titleFrame').addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center())
    this.testFade = FadeInOut().addChildTo(this)
  },

  update: function () {
    const key = this.app.keyboard;
    if (key.getKey('enter') && !this.fade.tweener.playing) {
      this.fade.fadeOut();
      this.fade.on("finishedFadeOut", () => {
        this.exit("scrollMain", option = {
          width: GAME_SCREEN_WIDTH,
          height: GAME_SCREEN_HEIGHT
        });
        // this.exit("shootingMain", option = {
        //   width: GAME_SCREEN_WIDTH,
        //   height: GAME_SCREEN_HEIGHT
        // });
      })
    }
  },

});
/*
 * collider
 */
phina.namespace(function () {
  /**
   * @class Collider
   * Collider
   * @extends phina.accessory.Accessory
   */
  phina.define('Collider', {
    superClass: 'phina.accessory.Accessory',
    /**
     * @constructor
     */
    init: function (target) {
      this.superInit(target);
    },
    // アタッチされた時
    onattached: function () {
      if (!this._collider) {
        this._collider = RectangleShape({
          width: this.target.width,
          height: this.target.height,
          fill: null,
        }).addChildTo(this.target);

        this._collider.hide();
      }
    },

    ondetached: function () {
      if (this._collider) {
        this._collider.remove();
      }
    },
    // 表示
    show: function () {
      this._collider.show();
      return this;
    },
    // 非表示
    hide: function () {
      this._collider.hide();
      return this;
    },
    // 相対位置指定
    offset: function (x, y) {
      this._collider.setPosition(x, y);
      return this;
    },
    // サイズ指定
    setSize: function (width, height) {
      this._collider.setSize(width, height);
      return this;
    },
    // 衝突判定
    hitTest: function (collider) {
      if (!this.target) return;
      // 絶対座標の矩形を計算      
      var rect = this.getAbsoluteRect();
      var rect2 = collider.getAbsoluteRect();
      // 矩形同士で判定
      return phina.geom.Collision.testRectRect(rect, rect2);
    },
    // Colliderの絶対座標の矩形
    getAbsoluteRect: function () {
      var x = this._collider.left + this.target.x;
      var y = this._collider.top + this.target.y;
      return phina.geom.Rect(x, y, this._collider.width, this._collider.height);
    },
  });

  phina.app.Element.prototype.getter('collider', function () {
    if (!this._collider) {
      this._collider = phina.accessory.Collider().attachTo(this);
    }
    return this._collider;
  });
});
phina.define("GameTimer", {
  superClass: 'Label',
  init: function (time) {
    this.superInit({
      text: `TIME\n${time}`,
      fill: 'white',
      stroke: 'black',
      strokeWidth: 15,
      fontSize: 48,
    });
  }

  //時間制限表示
  // this.time = 0;
  // this.timeLabel = Label({
  //   text: `TIME\n${GAME_EXIT_MAP.SCROLL_GAME_TIME}`,
  //   fill: 'white',
  //   stroke: 'black',
  //   strokeWidth: 15,
  //   fontSize: 48,
  // }).addChildTo(this).setPosition(GAME_SCREEN_WIDTH_HALF, BASE_GRID);
});
/*
* ScrollGameController
*ゲーム管理で更新をかける内容の記述
*/
phina.define("ScrollGameController", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さない
  init: function (options) {
    //初期設定
    this.time = 0;
    this.scene = options.scene;
    this.timerLabel = options.timerLabel;
    this.stockLabelNumber = options.stockLabelNumber;
    this.player = options.player;
    this.toushomen = options.toushomen;
    this.blockGroup = options.blockGroup;
    this.withoutCollider = options.withoutColliderBlockGroup;
    this.enemyGroup = options.enemyGroup;
    this.runningDistance = options.runningDistance;
    this.playerDirection = options.playerDirection;
    this.playerStock = options.playerStock;
    this.generatedBlock = options.generatedBlock;
    this.generatedNoColliderBlock = options.generatedNoColliderBlock;
    this.generatedEnemy = options.generatedEnemy;
    this.fade = options.sceneFade;
  },

  //毎フレーム更新処理
  update: function () {
    //時間制限の更新
    this.time = 
      ScrollGameManager().manageGameTime(this.scene, this.time, this.timerLabel);
    
    //playerの更新
    //残機管理
    this.stockLabelNumber.text = `×${this.playerStock}`;
    //地面との当たり判定
    this.setupPlayerIsOnStatus = 
      ScrollGameManager().setupIsOnTest(this.blockGroup, this.player);
    //移動
    this.player.isJumpingTest();
    this.player.setupMotion(this.scene);
    this.player.setupOperation(this.scene, this.setupPlayerIsOnStatus);
    //刀削麺のための、プレイヤーの向き情報更新
    this.playerDirection = 
      ScrollGameManager().managePlayerDirection(this.scene, this.playerDirection);
    //画面内移動範囲制限
    ScrollGameManager().manageCharacterMoveRange(this.player);


    ////刀削麺の更新
    //playerと刀削麺のspriteの向きは逆なのでマイナスで調整する
    const toshomenDirection = -this.playerDirection
    this.toushomen.setupOperation(this.scene, this.player.x, this.player.y, toshomenDirection);


    ////敵の更新
    //動き実装
    for (var i = 0; i < this.enemyGroup.children.length; i++) {
      const enemy = this.enemyGroup.children[i];
      let enemyIsOnTest = ScrollGameManager().setupIsOnTest(this.blockGroup, enemy);
      enemy.manageMotion(enemyIsOnTest);
    }


    //プレイヤーが右に移動しているときに、画面半分より右に行く時、
    // 引数に渡す要素をプレイヤーの移動している間だけ
    if (this.scene.key.getKey('right')) {
      //移動距離更新
      this.runningDistance = 
        ScrollGameManager().managePlayerDistance(this.player, this.setupPlayerIsOnStatus, this.runningDistance);
      //コライダー付きブロック
      ScrollGameManager().manageStageRange(this.player, this.blockGroup, this.setupPlayerIsOnStatus);
      //コライダーなしブロック
      ScrollGameManager().manageStageRange(this.player, this.withoutCollider, this.setupPlayerIsOnStatus);
      //敵
      ScrollGameManager().manageStageRange(this.player, this.enemyGroup, this.setupPlayerIsOnStatus);
    }

    //当たり判定付きblockの生成と削除
    //戻り値にフラグの更新も兼ねている
    this.generatedBlock = ScrollGameManager().manageMapBlocks(
      this.runningDistance,
      this.generatedBlock,
      this.blockGroup,
      "map/stageOne",
      "tile/white"
    );

    //当たり判定のないblockの生成と削除
    this.generatedNoColliderBlock = ScrollGameManager().manageMapNoColliderBlocks(
      this.runningDistance,
      this.generatedNoColliderBlock,
      this.withoutCollider,
      "map/stageOneNoCollider",
      "tile/white"
    );

    //敵の生成と削除
    this.generatedEnemy = ScrollGameManager().manageMapEnemies(
      this.runningDistance,
      this.generatedEnemy,
      this.enemyGroup,
      "map/stageOneEnemy",
      "chara/zacook_ss"
    );


    //敵とプレイヤー、刀削麺の当たり判定を基に敵自身とプレイヤーの管理をする
    this.enemyGroup.children.each((enemy) => {
      //敵と刀削麺の当たり判定監視
      this.hitStatusEnemyToshomen = 
        ScrollGameManager().manageHitStatus(this.toushomen, enemy);
      const show = 1.0;
      if(this.toushomen.alpha === show) {
        enemy.manageStatus(this.hitStatusEnemyToshomen);
      }

      //敵が倒されていないとき
      if (enemy.isWeak === false) {
        //プレイヤーの管理
        //プレイヤーと敵の当たり判定監視 
        this.hitStatusPlayerEnemy =
        ScrollGameManager().manageHitStatus(this.player, enemy);
        //プレイヤーの残機管理(playerが点滅している間は無敵)
        this.playerStock = 
          ScrollGameManager().managePlayerStock(this.hitStatusPlayerEnemy, this.player, this.playerStock);
        //敵にぶつかっていればプレイヤーを点滅させる
        this.player.manageHitStatus(this.hitStatusPlayerEnemy);
      }
    });

    const hide = 0;
    if(this.fade.alpha === hide) {
      // console.log("this.runningDistance:", this.runningDistance);
      ScrollGameManager().gameOverManager(
        this.scene,
        this.playerStock, 
        this.player.y, 
        this.time, 
        this.runningDistance, 
        this.fade
      );
    }
  },
});
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
/*
* 敵の基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("BaseEnemy", {
  superClass: 'ScrollBaseObject',
  init: function (file, width, height, scene) {
    this.superInit(file, width, height);
  },

});
/*
* Player
*optionにはfileName, width, heightを持たせる
*/
phina.define("Player", {
  superClass: 'ScrollBaseObject',
  init: function (file, width, height) {
    this.superInit(file, width, height);
    //コライダー適宜調整
    this.setCollider(
      this.width * 0.5,
      this.height * 0.1,
      this.height*0.1,
      this.height*0.9,
      this.height/2,
      this.width/4
    );
    this.isOnBlock = false;
    this.isJumping = false;

    //アニメーション追加
    this.anim = FrameAnimation('mentaro_ss').attachTo(this);

    this.isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };

    this.isAttacked = false;
  },

  //アニメーション付与
  setupMotion: function (scene) {
    const key = scene.app.keyboard;
    //キャラの向き
    const directionMap = {
      left: -1,
      right: 1
    };
    if (key.getKey('left')) {
      this.scaleX = directionMap.left;
      this.anim.gotoAndPlay('walk');
    }else if (key.getKey('right')) {
      this.scaleX = directionMap.right;
      this.anim.gotoAndPlay('walk');
    }

    if(key.getKeyUp('left') || key.getKeyUp('right')) {
      this.anim.gotoAndStop('walk');
    }
  },

  
  //空中にいる間のみ自由落下の重力を付与
  //地面、空中タイルに接している時は重力なし
  setupOperation: function (scene, isOnTest) {
    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    //マップ情報引継ぎ
    isOnMap = isOnTest;

    const key = scene.app.keyboard;
    // 上下移動
    //何かの上に接している時
    if (isOnMap.isOnTop === true) {
      if (key.getKey('up')) {
        this.physical.velocity.y -= PLAYER_JUMP_POWER;
        this.physical.gravity.y = PLAYER_GRAVITY;
        //ここでthis.isOnBlockは参照するだけにする
        this.isOnBlock = false;
        this.isJumping = true;
      };
    } else if (isOnMap.isOnTop === false) {
      this.physical.gravity.y = PLAYER_GRAVITY;
    }
    //何かの下に接している時
    if (isOnMap.isOnBottom === true) {
      const bound = 5;
      this.physical.velocity.y = bound;
    }

    //左右移動
    //側面に接触した状態での移動に不具合あり
    //何かの左に接している時
    if (isOnMap.isOnLeft === true) {
      if (key.getKey('left')) { this.x -= PLAYER_SPEED; };
      // console.log("onLeft");
    }
    //何かの右に接している時
    else if (isOnMap.isOnRight === true) {
      if (key.getKey('right')) { this.x += PLAYER_SPEED; };
      // console.log("onRight");
    } 
    //左右のどちらにも接していない時
    if (isOnMap.isOnLeft === false &&
      isOnMap.isOnRight === false) {
      if (key.getKey('left')) { this.x -= PLAYER_SPEED; };
      if (key.getKey('right')) { this.x += PLAYER_SPEED; };
    }
  },

  //ジャンプしているかを監視管理
  isJumpingTest: function () {
    if (this.physical.velocity.y < 0) {
      this.isJumping = true;
    }
    //else分岐いらなかったらコメントアウト
    else {
      this.isJumping = false;
    }
  },

  //敵とのぶつかり判定を受け取って点滅を管理
  manageHitStatus: function (isHit) {
    //攻撃を受けているかつ点滅していない
    if(isHit && !this.tweener.playing) {
      //ダメージボイス
      this.setDamagedVoice();
      //点滅開始
      this.tweener.clear().fadeOut(100).fadeIn(100).setLoop(true);
      //点滅している(無敵扱い)時間
      const invincibleTime = 2000;
      //点滅終了
      const stopLoop = () =>{
        this.tweener.clear();
        this.alpha = 1.0;
      };
      setTimeout(stopLoop, invincibleTime);
    }
  },

  setDamagedVoice: function () {
    const voiceArr = [
      'voice/mentaroDefeated1',
      'voice/mentaroDefeated2',
      'voice/mentaroDefeated3'
    ];
    SoundManager.play(voiceArr[Math.floor(Math.random() * voiceArr.length)]);
  },

});
/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("ScrollBaseObject", {
  superClass: 'Sprite',
  init: function (file, width, height) {
    this.superInit(file, width, height);
    // this.setCollider();
  },
  setCollider: function (width1, height1, width2, height2, position1, position2) {

    const w1 = width1 || this.width * 0.9;
    const h1 = height1 || this.width / 4;
    const w2 = width2 || this.width / 4;
    const h2 = height2 || this.width * 0.9;
    const p1 = position1 || this.width / 2;
    const p2 = position2 || this.width / 2;

    // コライダー
    const top = Collider().attachTo(this).setSize(w1, h1).offset(0, -p1)
    // .show();
    top.id = 'top';

    const bottom = Collider().attachTo(this).setSize(w1, h1).offset(0, p1)
    // .show();
    bottom.id = 'bottom';

    const left = Collider().attachTo(this).setSize(w2, h2).offset(-p2, 0)
    // .show();
    left.id = 'left';

    const right = Collider().attachTo(this).setSize(w2, h2).offset(p2, 0)
    // .show();
    right.id = 'right';
  },

});
phina.define("Tousyomen", {
  superClass: 'ScrollBaseObject',
  init: function () {
    const menWidth = 120;
    const menHeight = 120;
    this.superInit("item/tosyomen", menWidth, menHeight);
    this.setCollider();
    this.anim = FrameAnimation('tosyomen_ss').attachTo(this);
    this.alpha = 0;
  },

  //麺の動き
  setupMotion: function (positionX, positionY, direction) {
    //アニメ
    this.anim.gotoAndPlay('fly');
    const time = 300
    const moveRange = positionX + BASE_GRID * 3 * -direction;
    this.tweener.clear();
    this.tweener.clear()
      .to({
        x: moveRange
      }, time, "linear")
      .call(() => {
        //移動後は隠す
        this.alpha = 0;
      });
  },

  // ・麺の実装
  // 実装内容
  // 　スペースキーを押す
  // 	プレイヤーのx軸とy軸に合わせる
  // 	alpha値を0から1に切り替え
  // 　	192px画面右側に向かって進む
  // 　	192px進み終えたらalpha値を0にする

  // 	麺のalpha値が1.0の時に衝突していれば当たりとみなす


  // キーを押すと刀削麺が移動しながら移動する
  setupOperation: function (scene, positionX, positionY, direction) {
    const key = scene.app.keyboard;
    if (key.getKeyDown('space') && this.alpha === 0) {
      //刀削麺発射声
      SoundManager.play("voice/mentaroAttack");
      //プレイヤーの目の前のx軸位置に設置
      this.x = positionX - BASE_GRID * direction;
      // 高さは同じ
      this.y = positionY;
      this.alpha = 1.0;
      this.scaleX = direction;
      this.setupMotion(this.x, positionY, direction);
    }
  },

  hideSelf: function () {
    this.alpha = 0;
    this.tweener.clear();
  }


})
/*
* 歩行敵クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("WalkingEnemy", {
  superClass: 'BaseEnemy',
  init: function (file, width, height, direction) {
    this.enemyWidth = width || 90;
    this.enemyHeight = height || 150;
    this.superInit(file, this.enemyWidth, this.enemyHeight);
    this.direction = direction || -1;
    //コライダー設定
    this.setCollider(
      this.width * 0.5,
      this.height * 0.1,
      this.height*0.1,
      this.height*0.9,
      this.height/2,
      this.width/4
    );
    //アニメーション追加
    //ザコ敵は色が違うだけでモーションは同じ
    this.anim = FrameAnimation('zacook_ss').attachTo(this);
    
    //移動追加
    // this.setupMotion();
    //ダメージ管理
    this.isBeingAttacked = false;
    //弱っているか管理
    this.isWeak = false;
    //倒されたか管理
    this.isDefeated = false;
    //画面外にあるか
    this.isScreenOut = false;
  },

  //初期動き設定
  setupMotion: function (isOnStatus) {
    //重力設定
    this.physical.gravity.y = ENEMY_GRAVITY;
    //移動方向
    // this.direction = [ENEMY_DIRECTION_MAP.RIGHT, ENEMY_DIRECTION_MAP.LEFT].random();
    //キャラの向き設定
    if (this.direction === ENEMY_DIRECTION_MAP.RIGHT) {
      //キャラの向きをx軸反転
      this.scaleX = -1;
    }
  },

  //動き管理
  manageMotion: function (isOnTest) {
    let isOnMap = {
      "isOnTop": false,
      "isOnBottom": false,
      "isOnLeft": false,
      "isOnRight": false
    };
    //マップ情報引継ぎ
    isOnMap = isOnTest;
    //何かの上に接していないとき(空中にいるとき)
    if (isOnMap.isOnTop === false) {
      this.physical.gravity.y = ENEMY_GRAVITY;
      //方向反転
      this.direction *= -1;
      this.scaleX = -this.direction;
    }
    if (isOnMap.isOnLeft === true) {
      this.x -= ENEMY_SPEED;
      //方向転換
      this.direction = -1;
      this.scaleX = -this.direction;
    }
    //何かの右に接している時
    else if (isOnMap.isOnRight === true) {
      this.x += ENEMY_SPEED;
      this.direction = 1;
      this.scaleX = -this.direction;
    }

    //ステータスによる移動管理
    if (this.isWeak) {
      this.rotation = 90;
      this.anim.gotoAndStop('walk');
      if(!this.tweener.playing){
        this.defeatedVoice();
        this.defeatedAnimation();
        this.isDefeated = true;
      }
    } else {
      this.rotation = 0;
      this.x += this.direction * ENEMY_SPEED;
      this.anim.gotoAndPlay('walk');
    }
  },

  //攻撃ステータス管理
  manageStatus: function (isAttacked) {
    if(isAttacked === true) {
      this.isWeak = true;
    }
  },

  //倒された時のアニメーション
  defeatedAnimation: function () {
    const upTime = 100;
    const downTime = 800;
    this.tweener.clear()
    .to({ y: this.y - this.height
    }, upTime, "easeInBack")
    .to({ y: GAME_SCREEN_BOTTOM + this.height
    }, downTime, "easeInBack");
  },

  //倒された時の声
  defeatedVoice: function () {
    if(!this.isDefeated){
      SoundManager.play("voice/zacook");
    }
  }
});
/*
 * メインシーン
 */
//メインシーンですべきこと
// 使用する要素の宣言のみを行う
// 横スクロールゲームにて使用されるものが何なのかを説明するのみ

phina.define("ScrollGameScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);

    //画面に登場する要素のセットアップ
    this.setupUi();
    //プレイヤーの残機
    //プレイヤーの移動距離
    this.runningDistance = 0;
    //プレイヤーがどっちに向いているか
    this.playerDirection = 1;
    //プレイヤーの残機
    this.player.stock = PLAYER_STOCK;
    //ブロックを作ったかどうか
    this.generatedBlock = false;
    //コライダーなしのブロック
    this.generatedNoColliderBlock = false;
    //敵を作ったかどうか
    this.generatedEnemy = false;
    //ゲーム開始
    this.one('enterframe', () => {
      this.key = this.app.keyboard;
      SoundManager.playMusic('bgm/scroll');
      SoundManager.setVolumeMusic(GAME_BGM_VOLUME);
      this.activateGameController();
    });
  },

  //画面内の要素用意
  setupUi: function () {
    //ゲーム内の背景設置
    this.setupBackground();
    //ゲーム内のブロック設置
    this.setupStageBlocks();
    //プレイヤー設置
    this.setupPlayre();
    //プレイヤーの残機
    this.setupPlayerStockImg();
    //敵設置
    this.setupStageEnemies();
    //時間制限
    this.setupTimer();
    //画面遷移用フェード
    this.setupScreenFade();
  },

  setupBackground: function () {
    this.background = Sprite("background/scrollScene", GAME_SCREEN_WIDTH, GAME_SCREEN_HEIGHT).addChildTo(this)
      .setOrigin(0.0, 0.0)
      .setPosition(0, 0);
  },

  setupStageBlocks: function () {
    //マップの幅をタイルの数で示した数字
    const mapWidth = GAME_INIT_MAP_END / BASE_GRID;
    //初期表示されるマップ作成
    this.blockGroup =
      this.setMapBlocksFromFormatArr("map/stageOneInit", mapWidth).addChildTo(this).setPosition(0, 0);
    //コライダーなしのブロック
    this.withoutCollider = DisplayElement().addChildTo(this).setPosition(0, 0);
  },

  //プレイヤー設定
  setupPlayre: function () {
    //プレイヤー
    const initialX = 200;
    const initialY = 500;
    this.player = Player("chara/mentarou_ss", MENTARO_WIDTH, MENTARO_HEIGHT)
      .addChildTo(this)
      .setPosition(initialX, initialY);
    const justStanding = 0;
    this.player.frameIndex = justStanding;
    //刀削麺
    this.toushomen = Tousyomen().addChildTo(this)
      .setPosition(GAME_SCREEN_WIDTH_HALF, GAME_SCREEN_HEIGHT_HALF);
  },

  //プレイヤーの残機表示
  setupPlayerStockImg: function () {
    this.stockInfo = DisplayElement().addChildTo(this).setPosition(0, 0);
    //残機イメージ
    const stockImg = Sprite("chara/mentarou_ss", MENTARO_WIDTH, MENTARO_HEIGHT)
      .addChildTo(this.stockInfo).setPosition(BASE_GRID, BASE_GRID);
    const scale = 0.5;
    stockImg.scaleX = scale;
    stockImg.scaleY = scale;
    const justStanding = 0;
    stockImg.frameIndex = justStanding;
    //残機数字情報
    this.stockLabelNumber = Label({
      text: `× ${this.player.stock}`,
      fill: "white",
      stroke: 'black',
      strokeWidth: 15,
    }).addChildTo(this.stockInfo)
    .setPosition(BASE_GRID*2, BASE_GRID);
  },

  setupStageEnemies: function () {
    //敵設置
    this.enemyGroup = DisplayElement().addChildTo(this).setPosition(0, 0);
  },

  setupTimer:function () {
    this.timerLabel = GameTimer(GAME_EXIT_MAP.SCROLL_GAME_TIME).addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center(-6.5));
  },

  

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    console.log("this.fade:", this.fade)
    this.fade.fadeIn();
  },

  //コントローラーにてゲーム更新処理を行う
  activateGameController: function () {
    //更新処理に必要なものを引数に渡す
    this.gameController = ScrollGameController(
      {
        scene: this,
        timerLabel: this.timerLabel,
        stockLabelNumber: this.stockLabelNumber,
        player: this.player,
        toushomen: this.toushomen,
        blockGroup: this.blockGroup,
        withoutColliderBlockGroup: this.withoutCollider,
        enemyGroup: this.enemyGroup,
        runningDistance: this.runningDistance,
        playerDirection: this.playerDirection,
        playerStock: this.player.stock,
        generatedBlock: this.generatedBlock,
        generatedNoColliderBlock: this.generatedNoColliderBlock,
        generatedEnemy: this.generatedEnemy,
        sceneFade: this.fade,
      }
    )
    this.on("enterframe", () => {
      // 毎フレーム処理
      this.gameController.update();
    })
  },
});
// ソース元アドレス
// https://qiita.com/alkn203/items/8c24d433e9eb8ea0f4bc

/*
 * collider
 */
phina.namespace(function () {
  /**
   * @class Collider
   * Collider
   * @extends phina.accessory.Accessory
   */
  phina.define('Collider', {
    superClass: 'phina.accessory.Accessory',
    /**
     * @constructor
     */
    init: function (target) {
      this.superInit(target);
    },
    // アタッチされた時
    onattached: function () {
      if (!this._collider) {
        this._collider = RectangleShape({
          width: this.target.width,
          height: this.target.height,
          fill: null,
        }).addChildTo(this.target);

        this._collider.hide();
      }
    },

    ondetached: function () {
      if (this._collider) {
        this._collider.remove();
      }
    },
    // 表示
    show: function () {
      this._collider.show();
      return this;
    },
    // 非表示
    hide: function () {
      this._collider.hide();
      return this;
    },
    // 相対位置指定
    offset: function (x, y) {
      this._collider.setPosition(x, y);
      return this;
    },
    // サイズ指定
    setSize: function (width, height) {
      this._collider.setSize(width, height);
      return this;
    },
    // 衝突判定
    hitTest: function (collider) {
      if (!this.target) return;
      // 絶対座標の矩形を計算      
      var rect = this.getAbsoluteRect();
      var rect2 = collider.getAbsoluteRect();
      // 矩形同士で判定
      return phina.geom.Collision.testRectRect(rect, rect2);
    },
    // Colliderの絶対座標の矩形
    getAbsoluteRect: function () {
      var x = this._collider.left + this.target.x;
      var y = this._collider.top + this.target.y;
      return phina.geom.Rect(x, y, this._collider.width, this._collider.height);
    },
  });

  phina.app.Element.prototype.getter('collider', function () {
    if (!this._collider) {
      this._collider = phina.accessory.Collider().attachTo(this);
    }
    return this._collider;
  });
});
phina.define("ArcBullet", {
  superClass: "ShootingBaseObject",
  init: function () {
    this.superInit("bullet", 24, 24);
    this.velocity = 
      phina.geom.Vector2(Math.cos(rad) * speed, Math.sin(rad) * speed);
  },

  setup: function (frameIndex, x, y, direction, power) {
    // this.frameIndex = frameIndex;
    // this.x = x;
    // this.y = y;
    // this.rotation = direction;
    // this.power = power;
    // var degree = degree * ( Math.PI / 180 ) ;
    // var rad = direction * Math.DEG_TO_RAD;
    this.velocity = phina.geom.Vector2(Math.cos(rad) * speed, Math.sin(rad) * speed);
    return this;
  },
});

phina.define("EnemyBullet", {
  // 継承
  superClass: 'ShootingBaseObject',
  init: function () {    
    const bulletSize = BASE_GRID_HALF/2;
    this.superInit('item/minCabbageBullet', bulletSize, bulletSize);
    this.setCollider();

    // this.rotation = rotation || 0;
    // const speed = 10;
    // const self = this;
    // const top = 20;
    // const middle = 0;
    // const bottom = -20;
    // const bulletArr = [top, middle, bottom];

    // bulletArr.forEach((bulletDegree) => {
    //   const bulletSize = BASE_GRID_HALF/2;
    //   const bullet = BaseObject('item/minMenBullet', bulletSize, bulletSize)
    //   .addChildTo(self).setPosition(self.x, self.y);
    //   bullet.setCollider();
    //   const modification = 180;
    //   const shotDeg =bulletDegree + modification;
    //   const vec = Vector2().fromDegree(shotDeg, speed);
    //   //弾の各々に動きを個別でつける
    //   self.physical.velocity.x = vec.x;
    //   self.physical.velocity.y = vec.y;
    // })
  }
})
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
/*
* 画面内要素基礎クラス
*optionにはfileName, width, heightを持たせる
*/
phina.define("ShootingBaseObject", {
  superClass: 'Sprite',
  init: function (file, width, height) {
    this.superInit(file, width, height);
  },
  setCollider: function (colliderW, colliderH, colliderX, colliderY, colliderId) {
    const width = colliderW || this.width;
    const height = colliderH || this.height;
    const x = colliderX || this.x;
    const y = colliderY || this.y;
    const id = colliderId || "self";
    const selfCollider = 
      Collider().attachTo(this).setSize(width, height).offset(x, y)
      // .show();
    selfCollider.id = id;
  },

});
phina.define("ShootingBoss", {
  superClass: 'DisplayElement',
  init: function (scene, bossBulletGroup) {
    this.superInit();

    //敗北フラグ
    this.isDefeated = false;
    const width = 415;
    const height = 750;
    //通常状態のボス
    this.normalBoss = ShootingBaseObject("chara/bicCook", width, height).addChildTo(this)
      .setPosition(scene.gridX.center(5), scene.gridY.center());
    //当たり判定コライダー設置
    this.setupCollider();
    //攻撃を受けたボス
    this.damagedBoss = ShootingBaseObject("chara/damagedBicCook", width, height).addChildTo(this.normalBoss)
    //通常は透明で隠す
    const hide = 0;
    this.damagedBoss.alpha = hide;
    
    //弾の動き
    const interval = 800;
    this.firstShotTweeen = Tweener().clear().call(() => {
      this.shot(bossBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.firstShotTweeen.attachTo(this);
    
  },

  shot: function (bossBulletGroup) {
    //this避難
    const self = this.normalBoss;
    const top = 20;
    const middle = 0;
    const bottom = -20;
    const bulletArr = [top, middle, bottom];
    //弾の作成
    bulletArr.forEach((degree) => {
      const bullet = EnemyBullet().addChildTo(bossBulletGroup)
      .setPosition(self.x, self.y);
      const speed = 10;
      const modification = 180;
      const shotDegree = self.rotation + degree + modification;
      const vector = Vector2().fromDegree(shotDegree, speed);
      bullet.physical.velocity = vector;
      if(this.isDefeated){
        const hide = 0;
        bullet.alpha = hide;
      }
    })

    //画面外に出た弾の削除
    if(bossBulletGroup.children.length > 0) {
      for (var i = 0; i < bossBulletGroup.children.length; i++) {
        const bullet = bossBulletGroup.children[i];
        if(bullet.x < GAME_SCREEN_LEFT) {
          bullet.remove();
        }
      }
    }
  },


  //コライダー設置
  setupCollider: function () {
    const bodyW = this.normalBoss.width;
    const bodyH = this.normalBoss.height;
    const bodyX = 110;
    const bodyY = -6;    
    const bodyCollider = this.normalBoss.setCollider(
      bodyW,
      bodyH,
      bodyX,
      bodyY
    );
    const noseW = BASE_GRID;
    const noseH = BASE_GRID_HALF;
    const noseX = -50;
    const noseY = -220;
    const noseId = "nose";
    const noseCollider = this.normalBoss.setCollider(
      noseW,
      noseH,
      noseX,
      noseY,
      noseId
    )
  },


  //動きパターン
  motionOne: function () {
    const moveTime = 2000;
    const jumpTime = 600;
    const fallTime = 400;
    // const self = this;
    // console.log("this:", this);
    this.normalBoss.tweener.clear().to({
      x: this.normalBoss.x - BASE_GRID*3
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x 
    }, moveTime, "linear")
    .to({
      y: this.normalBoss.y - BASE_GRID*9
    }, jumpTime, "linear")
    .to({
      y: this.normalBoss.y
    }, fallTime, "linear");
  },

  motionTwo: function () {
    const littleBack = 500;
    const waitTime = 400
    const attackTime = 1000;
    const backTime = 1000;
    // console.log("this:", this);
    this.normalBoss.tweener.clear()
    .to({
      x: this.normalBoss.x + BASE_GRID
    }, littleBack, "linear")
    .wait(waitTime)
    .to({
      x: this.normalBoss.x - BASE_GRID * 8
    }, attackTime, "easeOutElastic")
    .to({
      x: this.normalBoss.x
    }, backTime, "linear");
  },

  motionThree: function () {
    const jumpTime = 300;
    const fallTime = 100;
    this.normalBoss.tweener.clear()
    .to({
      y: this.normalBoss.y - BASE_GRID*2
    }, jumpTime, "linear")
    .to({
      y: this.normalBoss.y
    }, fallTime, "linear");
  },

  motionFour: function () {
    const moveTime = 300;
    this.normalBoss.tweener.clear().to({
      x: this.normalBoss.x - BASE_GRID
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x + BASE_GRID
    }, moveTime, "linear")
    .to({
      x: this.normalBoss.x
    }, moveTime, "linear");
  },

  motionDefeated: function () {
    const moveTime = 100;
    this.normalBoss.tweener.clear().to({
      y: this.normalBoss.y - BASE_GRID_QUARTER,
    }, moveTime, "linear")
    .to({
      y: this.normalBoss.y,
    }, moveTime, "linear")
    .setLoop(true);
  },

  manageHitStatus: function (isHit) {
    //攻撃を受けているかつダメージボスが表示されていない
    const show = 1;
    const hide = 0;
    if (isHit) {
      if(this.damagedBoss.alpha === hide){
        this.damagedBoss.alpha = show;
      }
    }else {
      this.damagedBoss.alpha = hide;
    }
  },
})
phina.define("ShootingPlayer", {
  superClass: 'DisplayElement',
  init: function (scene, playerBulletGroup) {
    this.superInit();
    this.isDefeated = false;
    const width =128;
    const height = 133;
    const playerX = scene.gridX.center(-6);
    const playerY = scene.gridY.center();
    this.player = ShootingBaseObject("chara/riderMentaro", width, height).addChildTo(this)
      .setPosition(playerX, playerY);
    //コライダー
      this.setupCollider();
    //体力ゲージ
    this.hitPointGauge = HitPointGauge({value: PLAYER_HIT_POINT}).addChildTo(this)
      .setPosition(scene.gridX.center(-6), scene.gridY.center(-2));

    //弾の動き
    const interval = 120;
    //tweener初期化
    this.player.tweener.clear();
    //弾の動き設定
    this.shotTween = Tweener().clear().call(() => {
      this.shot(playerBulletGroup);
    })
    .wait(interval)
    .setLoop(true);
    this.shotTween.attachTo(this.player);
  },

  setupCollider: function () {
    const colliderW = this.player.width/5;
    const colliderH = this.player.height/1.8;
    const colliderX = -BASE_GRID/4;
    const colliderY = -GAME_SCREEN_HEIGHT/25;
    this.player.setCollider(
      colliderW,
      colliderH,
      colliderX,
      colliderY,
    );
  },

  setupMotion: function (scene) {
    const speed = PLAYER_SPEED*2;
    const direction = scene.app.keyboard.getKeyDirection();
    // 移動する向きとスピードを代入する
    this.player.moveBy(direction.x * speed, direction.y * speed);
    this.hitPointGauge.moveBy(direction.x * speed, direction.y * speed);
  },

  shot: function (playerBulletGroup) {
    //弾の生成
    const gap = 20;
    const hight = -gap;
    const low = gap;
    const bulletArr = [hight, low];
    bulletArr.forEach((position) => {
      const bullet = StraightforwardBullet()
      .addChildTo(playerBulletGroup).setPosition(this.player.x, this.player.y + BASE_GRID_HALF + position);
      if(this.isDefeated) {
        const hide = 0;
        bullet.alpha = hide;
      }
    })
    
    // console.log("playerBulletGroup.children.length:", playerBulletGroup.children.length);
    //弾の削除
    if(playerBulletGroup.children.length > 0) {
      for (var i = 0; i < playerBulletGroup.children.length; i++) {
        const child = playerBulletGroup.children[i];
        // console.log("playerBulletGroup.children.length:", playerBulletGroup.children.length)
        // console.log("child.x:", child.x);
        if(child.x > GAME_SCREEN_RIGHT) {
          child.remove();
        }
      }  
    }
  },

  defeatedMotion: function () {
    const moveTime = 80;
    this.player.tweener.clear().to({
      y: this.player.y + 5
    }, moveTime, "linear")
    .to({
      y: this.player.y
    }, moveTime, "linear")
    .setLoop(true);
  },

  //敵とのぶつかり判定を受け取って点滅を管理
  manageHitStatus: function (isHit, damageValue) {
    //攻撃を受けているかつ点滅していない
    if (isHit && !this.player.tweener.playing) {
      //ダメージ管理
      this.hitPointGauge.managePlayerHitPoint(damageValue);
      //ダメージボイス
      this.setDamagedVoice();
      //点滅開始
      this.player.tweener.clear().fadeOut(100).fadeIn(100).setLoop(true);
      //点滅している(無敵扱い)時間
      const invincibleTime = 2000;
      //点滅終了
      const stopLoop = () => {
        this.player.tweener.clear();
        this.player.alpha = 1.0;
      };
      setTimeout(stopLoop, invincibleTime);
    }
  },

  setDamagedVoice: function () {
    const voiceArr = [
      'voice/mentaroDefeated1',
      'voice/mentaroDefeated2',
      'voice/mentaroDefeated3'
    ];
    SoundManager.play(voiceArr[Math.floor(Math.random() * voiceArr.length)]);
  },
  
});
phina.define("ShootingZacook", {
  superClass: 'DisplayElement',
  init: function (scene, zacookBulletGroup) {
    this.superInit();
    const minZacookWidth = 45;
    const minZacookHeight = 75;

    //敗北フラグ
    this.isDefeated = false;
    //ダメージフラグ
    this.isDamaged = false;
    //コック体力
    this.currentHitPoint = ZACOOK_HP;
    //通常コック作成
    this.zacook = ShootingBaseObject("chara/minZacook_ss", minZacookWidth, minZacookHeight).addChildTo(this)
      .setPosition(scene.gridX.center(5), scene.gridY.center(-6));
    //ダメージコック作成
    this.damagedZacook = ShootingBaseObject("chara/damagedMinZacook_ss", minZacookWidth, minZacookHeight).addChildTo(this.zacook)
    //隠す
    const hide = 0;
    this.zacook.alpha = hide;
    this.damagedZacook.alpha = hide;

    //コライダー設置
    this.setupCollider();
    //弾セット(ミリ秒)
    const interval = 5000;
    //弾の動き追加
    this.shotTween = Tweener().clear().call(() => {
      this.shot(zacookBulletGroup);
    })
      .wait(interval)
      .setLoop(true);
    this.shotTween.attachTo(this.zacook);
  },

  shot: function (zacookBulletGroup) {
    const direction = -1;
    const gravity = 1;
    const speed = 10;
    const img = "item/minCabbageBullet";
    const bullet = StraightforwardBullet(direction, gravity, speed, img).addChildTo(zacookBulletGroup)
      .setPosition(this.zacook.x, this.zacook.y);
    const hide = 0;
    if (this.zacook.alpha === hide || this.isDefeated) {
      bullet.alpha = hide;
    }
  },

  setupCollider: function () {
    const colliderW = this.zacook.width;
    const colliderH = this.zacook.height - 5;
    const colliderX = -1;
    const colliderY = -1;
    this.zacook.setCollider(
      colliderW,
      colliderH,
      colliderX,
      colliderY
    );
  },

  setupMotion: function (scene, theNumber) {
    //初期化の際の引数に用意するザコ敵の数をもらう
    //tweenでザコ敵が一周回る時間をザコ敵の数で割る
    //商だけ待たせて、回転させる
    const rotateTime = TOTAL_ROTATE_TIME / 4;
    const waitingTime = TOTAL_ROTATE_TIME / ZACOOK_NUM;
    const position = {
      top: scene.gridY.center(-8),
      centerY: scene.gridY.center(),
      bottom: scene.gridY.center(8),
      left: scene.gridX.center(1),
      centerX: scene.gridX.center(5),
      right: scene.gridX.center(9)
    }
    const rotate = () => {
      this.zacook.tweener.clear()
        .to({
          x: position.left,
          y: position.centerY
        }, rotateTime, "linear")
        .to({
          x: position.centerX,
          y: position.bottom
        }, rotateTime, "linear")
        .to({
          x: position.right,
          y: position.centerY
        }, rotateTime, "linear")
        .to({
          x: position.centerX,
          y: position.top
        }, rotateTime, "linear")
        .setLoop(true);
    }
    //回転し始めるタイミングをずらす
    setTimeout(rotate, waitingTime * theNumber);
  },

  manageHitStatus: function (isHit) {
    const show = 1;
    const hide = 0;
    if (isHit) {
      if (this.damagedZacook.alpha === hide) {
        this.damagedZacook.alpha = show;
        this.currentHitPoint -= PLAYER_ATTACK_POINT;
      }
    } else {
      this.damagedZacook.alpha = hide;
    }
  },
});
phina.define("StraightforwardBullet", {
  superClass: 'ShootingBaseObject',

  //引数に方向をもらう
  init: function (direction, gravity, bulletSpeed, img) {
    this.direction = direction || 1;
    this.gravity = gravity || 5;
    this.img = img || 'item/minMenBullet'
    const bulletSize = BASE_GRID_HALF/2;
    this.superInit(this.img, bulletSize, bulletSize);
    this.setCollider();
    const speed = bulletSpeed || 30;
    this.physical.velocity.x = speed * this.direction;
    this.physical.velocity.y = this.gravity ;
    // const speed = 30;
    // const gap = 20;
    // const hight = -gap;
    // const low = gap;
    // const self = this;
    //弾のy座標位置
    // const bulletPositionArr = [hight, low];
    //弾の作成
    // bulletPositionArr.forEach((bulletY) => {
    //   const bulletSize = BASE_GRID_HALF/2;
    //   const bullet = BaseObject('item/minMenBullet', bulletSize, bulletSize)
    //     .addChildTo(self).setPosition(self.x, self.y + bulletY);
    //   //弾のコライダー作成
    //   const colliderW = bullet.width;
    //   const colliderH = bullet.height;
    //   const colliderX = this.x;
    //   let colliderY = null;
    //   const modification = 0.5;
    //   if(bulletY === hight) {
    //     //コライダー位置調整
    //     colliderY = this.y + modification;
    //   }else {
    //     colliderY = this.y - modification;
    //   }
    //   bullet.setCollider(colliderW, colliderH, colliderX, colliderY);
    //   // console.log("bullet:", bullet);
    // });
    //弾の移動
    // self.physical.velocity.x = speed * direction;
    // self.physical.velocity.y = 5;
  },

});
phina.define("ShootingGameScene", {
  superClass: 'BaseScene',
  init: function (option) {
    this.superInit(option);
    this.backgroundColor = "white";
    this.setupUi();
    this.one("enterframe", () => {
      //原因はわからないが、最初に表示するシーンでは音楽が流せない
      SoundManager.playMusic('bgm/shooting');
      SoundManager.setVolumeMusic(GAME_BGM_VOLUME);
      this.activateGameController();
      console.log("this.currentMusic:", this.currentMusic);
    });
  },

  setupUi: function () {
    //背景設定
    this.setupBackground();
    //ゲーム内プレイヤー設定
    this.setupPlayer();
    //ゲーム内ザコキャラ設定
    this.setupZakoGroup();
    //ゲーム内ボス設定
    this.setupBoss();
    //画面遷移用フェード
    this.setupScreenFade();
    //時間制限
    this.setupTimer();
  },

  setupBackground: function () {
    //背景の画像
    this.background = Sprite("background/ShootingScene", GAME_SCREEN_WIDTH, GAME_SCREEN_HEIGHT)
      .addChildTo(this)
      .setOrigin(0, 0);
    //背景の床
    const self = this;
    const generateTiles = () => {
      const numberOfTiles = GAME_SCREEN_WIDTH / BASE_GRID;
      for (var i = 0; i < numberOfTiles; i++) {
        const tile = Sprite("tile/white", BASE_GRID, BASE_GRID).addChildTo(self);
        tile.setOrigin(0, 0.5);
        tile.setPosition(BASE_GRID * i, GAME_SCREEN_HEIGHT)
      }
    };
    //床の設置
    generateTiles();
  },

  setupPlayer: function () {
    this.playerBullet = DisplayElement().addChildTo(this);
    this.player = ShootingPlayer(this, this.playerBullet).addChildTo(this);
  },

  setupZakoGroup: function () {
    this.zacooksBullet = DisplayElement().addChildTo(this);
    this.zacookGroup = DisplayElement().addChildTo(this);
    for (var i = 0; i < ZACOOK_NUM; i++) {
      let number = i + 1;
      const zacook = ShootingZacook(this, this.zacooksBullet)
        .addChildTo(this.zacookGroup);
    }
  },

  setupBoss: function () {
    this.bossBullet = DisplayElement().addChildTo(this);
    this.boss = ShootingBoss(this, this.bossBullet).addChildTo(this);
    //位置固定のためここで作成
    this.bossHitPointGauge = HitPointGauge({ value: PLAYER_HIT_POINT, scale: 1.5, gaugeColor: "red" }).addChildTo(this)
      .setPosition(this.gridX.center(5), this.gridY.center(-7));
  },

  setupScreenFade: function () {
    //画面遷移用フェード
    this.fade = FadeInOut().addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center());
    this.fade.fadeIn();
  },

  setupTimer:function () {
    this.timerLabel = GameTimer(GAME_EXIT_MAP.SHOOTING_GAME_TIME).addChildTo(this)
      .setPosition(this.gridX.center(), this.gridY.center(-6.5));
  },


  activateGameController: function () {
    this.gameController = ShootingGameController({
      scene: this,
      timerLabel: this.timerLabel,
      playerBullet: this.playerBullet,
      player: this.player,
      zacooksBullet: this.zacooksBullet,
      zacookGroup: this.zacookGroup,
      bossBullet: this.bossBullet,
      boss: this.boss,
      bossHitPointGauge: this.bossHitPointGauge,
      sceneFade: this.fade
    });
    this.on("enterframe", () => {
      this.gameController.update();
    })
  }
});
phina.define("ShootingGameController", {
  superClass: 'EventDispatcher',

  //クラスの初期化なので消さないv
  init: function (options) {
    // SoundManager.playMusic('bgm/shooting');
    //初期設定
    this.time = 0;
    this.scene = options.scene;
    this.timerLabel = options.timerLabel;
    this.playerBullet = options.playerBullet;
    this.player = options.player;
    this.zacooksBullet = options.zacooksBullet;
    this.zacookGroup = options.zacookGroup;
    this.bossBullet = options.bossBullet;
    this.boss = options.boss;
    this.bossHitPointGauge = options.bossHitPointGauge;
    this.fade = options.sceneFade;
    //テスト
    // this.testBullet = options.testBullet;
    // this.testPlayer = options.testPlayer;

    this.isDamegedPlayer = false;
    this.playerAttackPoint = PLAYER_ATTACK_POINT;

    this.spawnZacook = false;

    this.isDamegedBoss = false;
    this.bossCurrentHpValue = BOSS_HP;
    this.bossWeak = BOSS_HP / 6;
    this.bossAttackPoint = BOSS_AT;
    //ボスの攻撃パターンカウント
    this.bossMotionCounter = 0;

  },

  //毎フレーム更新処理
  update: function () {
    //時間制限の更新
    this.time =
      ShootingGameManager().manageGameTime(this.scene, this.time, this.timerLabel, this.bossCurrentHpValue);
    const timeLimit = Math.round(GAME_EXIT_MAP.SHOOTING_GAME_TIME - Math.floor(this.time) / 1000);
    //プレイヤーとボスが戦っている時
    if (this.player.hitPointGauge.value !== 0 &&
      this.bossHitPointGauge.value !== 0 &&
      timeLimit !== 0) {
      this.player.setupMotion(this.scene);
      ShootingGameManager().managePlayerMoveRange(this.player);
      //プレイヤーが敵から受ける攻撃判定
      this.isDamegedPlayer = ShootingGameManager().managePlayerIsHitStatus(
        this.player.player,
        //本実装までは文字にしておく
        this.bossBullet,
        this.boss.normalBoss,
        this.zacooksBullet,
        this.zacookGroup
      );
      //プレイヤーの体力を減らす
      this.player.manageHitStatus(this.isDamegedPlayer, this.bossAttackPoint);

      //ザコックを追加
      if (this.bossCurrentHpValue < this.bossWeak) {
        if (!this.spawnZacook) {
          this.spawnZacook = true;
          //ザコックの透明化を解除と移動モーションの追加
          ShootingGameManager().setupZacookMotion(this.scene, this.zacookGroup);
        }
        //ザコックがプレイヤーから受ける攻撃判定
        ShootingGameManager().manageZacook(this.zacookGroup, this.playerBullet);
      }

      //ボスが受けた攻撃を管理
      this.isDamegedBoss = ShootingGameManager().hitTestPlayerBulletBossNose(
        this.playerBullet,
        this.boss.normalBoss
      );
      //ボスの体力ゲージ管理
      this.boss.manageHitStatus(this.isDamegedBoss);
      this.bossCurrentHpValue = ShootingGameManager().manageBossHitPointValue(
        this.bossCurrentHpValue,
        this.bossHitPointGauge,
        this.boss.damagedBoss
      );

      // ボスに動き付与
      this.bossMotionCounter = ShootingGameManager().manageBossMotion(
        this.boss,
        this.bossCurrentHpValue,
        this.bossWeak,
        this.bossMotionCounter
      );

    }
    //戦いに決着がついたとき
    else {
      //敗北モーション
      ShootingGameManager()
        .manageIsDefeatedMotion(this.player, this.boss, this.zacookGroup);
      //画面遷移
      ShootingGameManager().gameOverManager(
        this.scene,
        this.player,
        this.boss,
        timeLimit,
        this.fade)
    }

  },

  // //キャラクターはisDefeatedがtureの時に弾が非表示になる
  // gameOver: function (player, boss, zacookGroup) {
  //   const removeSpeed = 5;
  //   //ボスが勝利した時
  //   if(player.hitPointGauge.value == 0) {
  //     const playerSelf = player.player;
  //     if(!playerSelf.tweener.playing){
  //       //敗北にする
  //       player.isDefated = true;
  //       //敗北モーション起動
  //       player.defeatedMotion();
  //     }
  //     if(player.isDefeated) {
  //       //画面の左外側に移動
  //       player.x -= removeSpeed;
  //     }
  //     //画面外に出たことを検知で画面遷移
  //   }
  //   //プレイヤーが勝利した時
  //   else{
  //     if(!boss.normalBoss.tweener.playing) {
  //       //ボス敗北設定
  //       boss.isDefeated = true;
  //       boss.motionDefeated();
  //       //ザコ敵敗北設定
  //       for (var i = 0; i < zacookGroup.children.length; i++) {
  //         zacookGroup.children[i].isDefeated = true;
  //       }
  //     }
  //     //敵たちを画面右外側へ移動
  //     if(boss.isDefeated) {
  //       zacookGroup.x -= removeSpeed;
  //       boss.x -= removeSpeed;
  //     }
  //   }
  // }

});
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