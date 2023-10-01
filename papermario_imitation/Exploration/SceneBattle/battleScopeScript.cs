using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScopeScript : MonoBehaviour
{
  private GameObject player;
  private GameObject enemy;
  private EnemyScript enemyScript;
  private BattleEnemyInformationHolder battleEnemyInformationHolder;
  private HanaPlayerScript hanaPlayerScript;

  private bool encounteredEnemy;

  void Start ()
  {
    encounteredEnemy = false;
    player = GameObject.Find ("Player");
    hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
    enemy = transform.parent.gameObject;
    enemyScript = enemy.GetComponent<EnemyScript> ();
    battleEnemyInformationHolder = enemy.GetComponent<BattleEnemyInformationHolder> ();
    // Debug.Log ("battleEnemyInformationHolder : " + battleEnemyInformationHolder);
  }

  // void Update ()
  // {

  // }


  //EnemyScriptの

  void OnTriggerEnter (Collider other)
  {
    // タグにて先制攻撃の有無を判断する 
    // 脚のコライダーとハンマーのコライダーにタグかも
    if (other.tag == "Player")
    {
      // バトルシーンから逃げた、勝った後に衝突した場合は無視
      if (hanaPlayerScript.PlayerState != HanaPlayerScript.State.EncounteringEnemy && !hanaPlayerScript.EscapedFromTheBattle)
      {
        enemyScript.SetBattleTarget (player);
        HanaPlayerBattleScript hanaPlayerBattleScript = player.GetComponent<HanaPlayerBattleScript> ();
        hanaPlayerBattleScript.SetBattleOpponent (enemy, "Player");
        hanaPlayerBattleScript.HanaPlayerBattleEnemyInformationHolder = battleEnemyInformationHolder;
        hanaPlayerBattleScript.EnemyName = enemy.name;
      }
    }
  }

  // バトルから逃げてきたときに、敵との遭遇コライダーに既に入っている状態でバトル開始可能になった時用
  void OnTriggerStay (Collider other)
  {
    if (other.tag == "Player")
    {
      if (hanaPlayerScript.PlayerState != HanaPlayerScript.State.EncounteringEnemy && !hanaPlayerScript.EscapedFromTheBattle && !encounteredEnemy)
      {
        encounteredEnemy = false;
        enemyScript.SetBattleTarget (player);
        HanaPlayerBattleScript hanaPlayerBattleScript = player.GetComponent<HanaPlayerBattleScript> ();
        hanaPlayerBattleScript.SetBattleOpponent (enemy, "Player");
        hanaPlayerBattleScript.HanaPlayerBattleEnemyInformationHolder = battleEnemyInformationHolder;
        hanaPlayerBattleScript.EnemyName = enemy.name;
      }
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.EncounteringEnemy)
      {
        enemyScript.ResetBattleTarget ();
      }
    }
  }

}