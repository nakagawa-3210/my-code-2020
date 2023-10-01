'use strict';
console.log("hoge");
phina.globalize();
//画面サイズ情報
const GAME_SCREEN_WIDTH = 1280;
const GAME_SCREEN_HEIGHT = 768;
const GAME_SCREEN_WIDTH_HALF = GAME_SCREEN_WIDTH / 2;
const GAME_SCREEN_HEIGHT_HALF = GAME_SCREEN_HEIGHT / 2;
const GAME_SCREEN_TOP = 0;
const GAME_SCREEN_BOTTOM = GAME_SCREEN_HEIGHT;
const GAME_SCREEN_LEFT = 0;
const GAME_SCREEN_RIGHT = GAME_SCREEN_WIDTH;

//ステージ内共通サイズ
const BASE_GRID = 64;
const BASE_GRID_HALF = BASE_GRID / 2;
const BASE_GRID_QUARTER = BASE_GRID_HALF / 2;
const CHARACTER_GRID = 64;
// const MIN_TILES = 32;
const ITEM_GRID = 48;

//共通重力
const ZERO_GRAVITY = 0;

//player情報 
const PRECURE_GRID = 48;
const PLAYER_JUMP_POWER = 15;
const PLAYER_SPEED = 10;
const PLAYER_GRAVITY = 0.55;
const PLAYER_STOCK = 1;


//敵情報
const ENEMY_GRID = 64;
const ENEMY_SPEED = 5;
const ENEMY_BERSERK_SPEED = 20;
const ENEMY_GRAVITY = 0.55;
const NUMBER_OF_ENEMY = 5;
const ENEMY_INTERVAL = 100;
//敵の位置マップ
const ENEMY_SPAWN_MAP = {
	LEFT: GAME_SCREEN_LEFT + BASE_GRID * 2, 
	RIGHT: GAME_SCREEN_RIGHT - BASE_GRID * 2,
	APPEAR: BASE_GRID/2,
	DISAPPEAR: GAME_SCREEN_BOTTOM - BASE_GRID * 2,
};
//敵の移動方向マップ
const ENEMY_DIRECTION_MAP = {
	LEFT: -1,
	RIGHT: 1
};

//ゲーム終了マップ
const GAME_EXIT_MAP = {
	GAME_TIME: 100,
	GAME_CLEAR: {img: "game/cureClear", gridSize: 340, message: "GAME CLEAR!"},
	GAME_OVER: {img: "chara/precure", gridSize: PRECURE_GRID, message: "GAME OVER!"}
}