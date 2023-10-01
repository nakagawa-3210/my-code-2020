using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyStatus : MonoBehaviour
{
  // GameObjectをSkillに変える
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private EnemyDataArray enemyDataArray;
  private EnemySkillArray enemySkillArray;
  private List<EnemySkill> enemySkillList;
  private Enemy enemyData;
  private EnemySkill currentEnemySkill;
  private GameObject currentTarget;
  private bool isParalysed = false;
  private bool isPoisoned = false;
  private bool isSleeping = false;
  private bool endEnemyTurn;
  // private int enemyHp;
  // private int enemyFp;
  // private int enemyAt;
  // private int enemyDf;
  // private int enemyCoin;
  // private int enemyExperience;
  // private int enemyHalfExperienceLine;
  // private int enemyNoExperienceLine;
  // private string enemyDescription;

  public List<EnemySkill> EnemySkillList
  {
    get { return enemySkillList; }
  }
  public EnemySkill CurrentEnemySkill
  {
    set { currentEnemySkill = value; }
    get { return currentEnemySkill; }
  }
  public Enemy EnemyData
  {
    get { return enemyData; }
  }
  public GameObject CurrentTarget
  {
    set { currentTarget = value; }
    get { return currentTarget; }
  }
  public bool IsParalysed
  {
    set { isParalysed = value; }
    get { return isParalysed; }
  }
  public bool IsPoisoned
  {
    set { isPoisoned = value; }
    get { return isPoisoned; }
  }
  public bool IsSleeping
  {
    set { isSleeping = value; }
    get { return isSleeping; }
  }
  public bool EndEnemyTurn
  {
    set { endEnemyTurn = value; }
    get { return endEnemyTurn; }
  }

  // Monobehabiorを継承させて、敵のプレファブに直接つける

  public void SetupBattleEnemyStatus (string stageEnemiesFileName, string enemyName)
  {
    enemySkillList = new List<EnemySkill> ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    enemyDataArray = jsonReaderFromResourcesFolder.GetEnemyDataArray ("JSON/EnemyData/EnemyBaseInfo/" + stageEnemiesFileName);
    enemyData = GetSameNameEnemyData (enemyName);
    // Debug.Log ("enemyData : " + enemyData);
    string enemySkillDataFileName = enemyData.skillDataName;
    enemySkillArray = jsonReaderFromResourcesFolder.GetEnemySkillDataArray ("JSON/EnemyData/EnemySkill/" + enemySkillDataFileName);
    // リストに変換
    enemySkillList.AddRange (enemySkillArray.gameEnemySkills);
    endEnemyTurn = false;
  }

  // EnemyDataを渡すからいらない(ステータスに何があるかを見やすくするために下記の方法に変えるかも)
  // void SetupEnemyStatus ()
  // {
  //   enemyHp = enemyData.enemyHp;
  //   enemyFp = enemyData.enemyFp;
  //   enemyAt = enemyData.enemyAt;
  //   enemyDf = enemyData.enemyDf;
  //   enemyCoin = enemyData.enemyCoin;
  //   enemyExperience = enemyData.enemyExperience;
  //   enemyHalfExperienceLine = enemyData.enemyHalfExperienceLine;
  //   enemyNoExperienceLine = enemyData.enemyNoExperienceLine;
  //   enemyDescription = enemyData.enemyDescription;
  // }

  Enemy GetSameNameEnemyData (string enemyName)
  {
    Enemy selectedEnemyData = null;
    foreach (var enemyData in enemyDataArray.gameEnemies)
    {
      if (enemyName == enemyData.enemyName)
      {
        selectedEnemyData = enemyData;
      }
    }
    return selectedEnemyData;
  }

}