using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedDetector : MonoBehaviour
{
  [SerializeField] GameObject damageCollider;
  [SerializeField] GameObject actionCollider;
  // プレイヤーが操作するキャラ
  [SerializeField] bool isPlayerMember;
  // 敵のキャラ
  [SerializeField] bool isEnemyMember;
  private int enemyDamageColliderEnteredNum;
  private bool succeededAction;
  private bool hasActionChance;
  void Start ()
  {
    succeededAction = false;
    hasActionChance = true;
  }

  // 攻撃された回数をリセットするのに必要
  public void ResetEnemyAttackedInformation ()
  {
    actionCollider.GetComponent<EnemyBattleCollider> ().ResetPlayerEnteredInfo ();
  }
  public int EnemyDamageColliderEnteredNum
  {
    get { return actionCollider.GetComponent<EnemyBattleCollider> ().EnteredNum; }
  }
  public bool SucceededAction
  {
    set { succeededAction = value; }
    get { return succeededAction; }
  }
  public bool HasActionChance
  {
    set { hasActionChance = value; }
    get { return hasActionChance; }
  }

  void Update ()
  {
    // if (isEnemyMember)
    // {
    //   // Debug.Log ("EnteredNum : " + actionCollider.GetComponent<EnemyBattleCollider> ().EnteredNum);
    // }

    bool isDamageColliderEntered = false;
    bool isActionColliderEntered = false;

    // プレイヤーが敵から攻撃される時のガードコマンド
    if (isPlayerMember)
    {
      isDamageColliderEntered = damageCollider.GetComponent<PlayerBattleCollider> ().EnemyEntered;
      isActionColliderEntered = actionCollider.GetComponent<PlayerBattleCollider> ().EnemyEntered;
      // ActionManager (isDamageColliderEntered, isActionColliderEntered);
    }
    // プレイヤーが敵にジャンプ攻撃するときのアタックコマンド
    else if (isEnemyMember)
    {
      // Debug.Log ("damageCollider : " + damageCollider);
      // Debug.Log ("actionCollider : " + actionCollider);
      isDamageColliderEntered = damageCollider.GetComponent<EnemyBattleCollider> ().PlyayerEntered;
      isActionColliderEntered = actionCollider.GetComponent<EnemyBattleCollider> ().PlyayerEntered;
      // ActionManager (isDamageColliderEntered, isActionColliderEntered);
      // Debug.Log ("hasActionChance : " + hasActionChance);
    }
    // アクションコライダーに触れているかつ、ダメージコライダーに触れていないかつ、スペースキーを押しているかつ、まだ一度もスペースキーを押していなかったとき
    if (isActionColliderEntered && !isDamageColliderEntered && Input.GetKeyDown (KeyCode.Space) && hasActionChance)
    {
      // Debug.Log ("成功！");
      succeededAction = true;
    }
    // チャンスはいちどきり
    if (Input.GetKeyDown (KeyCode.Space))
    {
      // Debug.Log ("おした");
      hasActionChance = false;
    }
  }

  // コード整理でまたコメントアウトを外すかもしれない
  // void ActionManager (bool isDamageColliderEntered, bool isActionColliderEntered)
  // {
  //   // アクションコライダーに触れているかつ、ダメージコライダーに触れていないかつ、スペースキーを押しているかつ、まだ一度もスペースキーを押していなかったとき
  //   if (isActionColliderEntered && !isDamageColliderEntered && Input.GetKeyDown (KeyCode.Space) && hasActionChance)
  //   {
  //     Debug.Log ("成功！");
  //     succeededAction = true;
  //   }
  //   // チャンスはいちどきり
  //   if (Input.GetKeyDown (KeyCode.Space))
  //   {
  //     Debug.Log ("おした");
  //     hasActionChance = false;
  //   }
  // }

  public bool GetEnteredDamageCollider ()
  {
    bool enteredCollider = false;
    if (isPlayerMember)
    {
      enteredCollider = damageCollider.GetComponent<PlayerBattleCollider> ().EnemyEntered;
    }
    else if (isEnemyMember)
    {
      enteredCollider = damageCollider.GetComponent<EnemyBattleCollider> ().PlyayerEntered;
      // Debug.Log ("enteredCollider : " + enteredCollider);
    }
    return enteredCollider;
  }

  public bool GetEnteredActionCollider ()
  {
    bool enteredCollider = false;
    if (isPlayerMember)
    {
      enteredCollider = actionCollider.GetComponent<PlayerBattleCollider> ().EnemyEntered;
    }
    else if (isEnemyMember)
    {
      enteredCollider = actionCollider.GetComponent<EnemyBattleCollider> ().PlyayerEntered;
    }
    return enteredCollider;
  }

  public void ResetDetector ()
  {
    hasActionChance = true;
    succeededAction = false;
  }

}