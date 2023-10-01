using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
  private GameObject battleTarget;
  // private EnemyAttackMotionManager enemyAttackMotionManager;

  // 敵の状態がdefeatedの時は倒れるモーションを再生するようにする
  public enum State
  {
    Normal,
    Attack,
    encounteringPlayer,
    defeated
  }
  private State state;
  void Start ()
  {

  }

  void Update ()
  {
    // Debug.Log ("this.state 敵のステータス : " + this.state);
  }
  public void SetBattleTarget (GameObject battleTarget)
  {
    this.battleTarget = battleTarget;
    this.state = EnemyScript.State.encounteringPlayer;
  }

  public void ResetBattleTarget ()
  {
    this.battleTarget = null;
  }
  public State EnemyState
  {
    get { return this.state; }
    set { this.state = value; }
  }
}