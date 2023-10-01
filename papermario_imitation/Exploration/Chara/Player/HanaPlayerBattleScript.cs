using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanaPlayerBattleScript : MonoBehaviour
{
  private GameObject battleOpponent;
  private HanaPlayerScript hanaPlayerScript;

  // 敵の情報を持つコンポーネントを受け取る
  private BattleEnemyInformationHolder battleEnemyInformationHolder;
  private string enemyName;

  public BattleEnemyInformationHolder HanaPlayerBattleEnemyInformationHolder
  {
    set { battleEnemyInformationHolder = value; }
    get { return battleEnemyInformationHolder; }
  }
  public string EnemyName
  {
    set { enemyName = value; }
    get { return enemyName; }
  }

  void Start ()
  {
    hanaPlayerScript = gameObject.GetComponent<HanaPlayerScript> ();
  }

  void Update ()
  {
    if (battleOpponent == null) return;
    Debug.Log ("battleOpponent : " + battleOpponent);
  }

  // 接敵したときかつ、!Defeatedのときに呼んでもらう
  // 敵が受け取ったコライダーのタグ名と敵の状態を受け取る
  public void SetBattleOpponent (GameObject battleOpponent, string colliderTagName)
  {
    Debug.Log ("てきと衝突");
    // まずは普通にプレイヤーと敵が衝突した時の処理を考える
    this.battleOpponent = battleOpponent;
    hanaPlayerScript.PlayerState = HanaPlayerScript.State.EncounteringEnemy;
  }

  public void ResetBattleOpponent ()
  {
    this.battleOpponent = null;
    battleEnemyInformationHolder = null;
    enemyName = null;
  }

}