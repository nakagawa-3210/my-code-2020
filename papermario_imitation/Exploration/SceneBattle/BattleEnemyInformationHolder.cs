using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyInformationHolder : MonoBehaviour
{
  [SerializeField] List<GameObject> battleEnemyList;

  public List<GameObject> BattleEnemyList
  {
    get { return battleEnemyList; }
    set { battleEnemyList = value; }
  }
}