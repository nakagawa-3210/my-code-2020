using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleCollider : MonoBehaviour
{
  private int enteredNum;
  private bool plyayerEntered;

  public int EnteredNum
  {
    get { return enteredNum; }
  }
  public bool PlyayerEntered
  {
    get { return plyayerEntered; }
  }

  void Start ()
  {
    ResetPlayerEnteredInfo ();
  }

  public void ResetPlayerEnteredInfo ()
  {
    enteredNum = 0;
    plyayerEntered = false;
  }

  // PlayerColliderはプレイヤーのジャンプ等で自分の体で攻撃する際に用いる
  // PlayerHammerColliderは道具を使って攻撃する際に用いる
  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "PlayerCollider" || other.tag == "PlayerHammerCollider")
    {
      plyayerEntered = true;
      enteredNum++;
      // Debug.Log ("plyayerEntered : " + plyayerEntered);
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "PlayerCollider" || other.tag == "PlayerHammerCollider")
    {
      plyayerEntered = false;
    }
  }
}