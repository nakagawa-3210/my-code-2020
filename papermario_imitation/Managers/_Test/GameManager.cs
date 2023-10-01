using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  // 探索シーンから遷移したとき
  //    
  // プレイヤーの位置をバトルシーン遷移時にキャッシュする
  // 仲間の位置をバトルシーン遷移時にキャッシュする

  private HanaPlayerScript[] hanaPlayerScripts;
  private HanaPlayerScript hanaPlayerScript;
  private bool startLoadingScene;

  // ゲーム進行での確認したい内容
  // 	プレイヤーの状態確認
  // 		「エンカウントエネミー」だとバトルシーンに入る(マップ探索シーンにバトルシーンを加える)
  // 		「ディフィートエネミー」だとマップ探索シーンに入る(加えたバトルシーンを削除する)
  // 		「デッドセルフ」だとゲームオーバーシーンに入る

  // 何をしているときにはプレイヤーが動けないのか
  //    プレイヤーが会話中は移動できない
  //    プレイヤーがマップ上でアイテムを拾った際はプレイヤーは動けない
  //    プレイヤーがメニュー画面を開いている時はフィールド上のプレイヤー、仲間、敵、マップの仕掛けは動けない
  //      タイムスケールをゼロにする以外の方法で考える(メニューをupdateを用いて動かすことが出来ないから)
  //    プレイヤーが移動アニメ再生中の時はプレイヤーは動けない
  //      マップ上の建物に入る
  //      マップ上の罠にかかる

  void Start ()
  {
    // Debug.Log ("マネージャーが作られたよ");
    // startLoadingScene = false;
    // SceneManager.sceneLoaded += OnSceneLoaded;
    // InitHanaPlayerScript ();

    // // JSONファイルテスト作成
    // int test = SaveSystem.Instance.userData.playerCurrentHp;
    // Debug.Log("hp : " + test);
  }

  void Update ()
  {
    // if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.EncounteringEnemy && !startLoadingScene)
    // {
    //   startLoadingScene = true;
    //   // 遷移先直打ち中なので、改修予定
    //   SceneManager.LoadScene ("SteppeBattle");
    // }
    // 条件式にメニューを開いた状態を加える
    // ゲーム内のキャラやステージの動きに制限をかける
    // ステータスにストップとかを加えてこのクラス内から変更する
    // hanaPlayerScript.PlayerState = HanaPlayerScript.State.OpenMenu;
  }

  void OnSceneLoaded (Scene scene, LoadSceneMode mode)
  {
    Debug.Log ("新しいシーンがロードされたよ");
    InitHanaPlayerScript ();
    if (SceneManager.GetActiveScene ().name == "SteppeBattle")
    {
      // 変数引き渡しテストOK
      var battleManager = GameObject.FindWithTag ("BattleManager").GetComponent<BattleManager> ();
      // battleManager.Test = "テストテキスト変更";
    }

    // シーン内のプレイヤー、仲間、敵を初期化する
  }

  void InitHanaPlayerScript ()
  {
    // このクラス内でプレイヤーについて初期位置設定と操作制限をすることはあっても、
    // プレイヤーの操作自体をすることはない
    // シーン内にプレイヤーがいるのか、複数いた場合プレイヤーはどれなのか、を明確にする
    hanaPlayerScripts = FindObjectsOfType<HanaPlayerScript> ();
    if (hanaPlayerScripts.Length > 0)
    {
      if (!hanaPlayerScript) hanaPlayerScript = hanaPlayerScripts[0];
    }
    DebugUtility.HandleErrorIfNullFindObject<HanaPlayerScript, GameManager> (hanaPlayerScript, this);
  }

  // デリゲートをどんどん使ってみる
  // プレイヤーのコライダー判定をマネージャークラスのhoge関数に加える等を試す
}