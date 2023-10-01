using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameData
{
  [System.Serializable]
  public class MyData
  {
    // ゲームに登場するフラグを名前で保存する
    public string[] allGameFlags = {
      ""
    };
    // 立ったフラグ名を保存
    public string[] currentGameFlags = {

    };

    // 持ってるアイテム(20個までもてる仕様になるコードを書く)
    public string[] havingItemsName = {
      "ふつうのパン",
      "ふつうの水あめ",
      "ふつうのパン",
      "ファイヤーフラワー",
      "ファイヤーフラワー"
    };
    // 預けているアイテム(原作では35個？まで預けられたはず)
    public string[] leavingItemsName = {
      "しゅくはくけん"
    };
    // 使い捨てで、二度と手に入らないアイテム
    public string[] havingImportantThing = {
      "いえのかぎ",
      "じゅうようなデータ",
      "おまもり",
    };
    public string[] havingBadgesName = {
      "ガツーンジャンプ",
      "ガツーンハンマー"
    };
    // havingBadgesNameのうち、何番目のバッジをつけているかを保存している
    // シーンの初回起動時にて参照される
    // バッジの売買の際、バッジのつけ外しの際に更新される
    public int[] puttingBadgeNums = { };
    // シーンの初回起動時よりあとに参照される。初回起動後に装備済みのバッジボタンに乱数を割り振る
    // 同じシーン内にてバッジ所有データが書き変わった際、 何番目のバッジをつけているかがの情報が変更されてしまっているので
    // バッジボタンの持つ乱数を基にどのバッジが装備されているかを判断する
    // 判断ができたらputtingBadgeNumsを更新する
    // puttingBadgeIdは固定ではなく、バッジに関する変更がなくともシーンを立ち上げるたびに変化する
    public int[] puttingBadgeId = { };
    public string[] partnersName = { "サクちゃん" };
    public string savePointSceneName = "StartScene";
    public Vector3 savedPlayerPosition = new Vector3 (0.0f, 0.0f, 0.0f);
    public Vector3 savedPartnerPosition = new Vector3 (2.0f, 3.4f, 2.0f);
    public int playerLevel = 1;
    // 称号
    public string playerTitle = "ハナタレみならい";
    public int playerMaxHp = 10;
    public int playerCurrentHp = 10;
    public int playerHpGrowthPoint = 5;
    public int playerMaxFp = 5;
    public int playerCurrentFp = 5;
    public int playerFpGrowthPoint = 5;
    public int playerMaxBp = 3;
    public int playerBpGrowthPoint = 3;
    public int playerUsingBp = 0;
    public int shoesLevel = 1;
    public int hammerLevel = 1;
    public int havingCoin = 20;
    // experienceが100になるとplayerLevelが1増えて、experience自身は0になる
    public int experience = 90;
    // スペシャル技の星
    public int havingStars = 0;
    // ほしのかけら
    public int havingStarFragments = 0;
    // シャインのかわり
    public int havingSuperFertilizer = 0;
    // 分単位での保存にする
    public int totalPlayingTimeMins = 0;
    public int havingItemsNumRestriction = 20;
    public int leavingItemsNumRestriction = 35;
    public string currentSelectedPartnerName = "サクちゃん";
    public int sakuraLevel = 1;
    public int sakuraCurrentHp = 10;
  }
}