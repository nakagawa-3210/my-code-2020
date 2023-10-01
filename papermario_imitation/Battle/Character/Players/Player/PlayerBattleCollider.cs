using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleCollider : MonoBehaviour
{
  private bool enemyEntered;
  public bool EnemyEntered
  {
    get { return enemyEntered; }
  }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "EnemyCollider")
    {
      enemyEntered = true;
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "EnemyCollider")
    {
      enemyEntered = false;
    }
  }
}