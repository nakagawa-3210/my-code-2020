using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一旦はなかまがサクラに依存してつくりきる
public class BattleResultSaveManager
{
  public void SaveBattleResultAfterWinningBattle (
    int playerHp,
    int playerFp,
    int currentPartnerHp,
    int acquiringExperience,
    string[] havingItemNameArray
  )
  {
    CommonSave (
      playerHp,
      playerFp,
      currentPartnerHp,
      havingItemNameArray
    );
    // 経験値
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    int currentAcquiringExperience = saveData.experience;
    int totalExperience = currentAcquiringExperience + acquiringExperience;
    // Debug.Log ("totalExperience : " + totalExperience);
    if (totalExperience >= 100)
    {
      // Debug.Log ("レベルアップ");
      totalExperience -= 100;
      // Debug.Log ("totalExperience : " + totalExperience);
      saveData.playerLevel += 1;
    }
    saveData.experience = totalExperience;
    // Debug.Log ("saveData.experience : " + saveData.experience);
    // SaveSystem.Instance.Save ();
  }

  public void SaveBattleResultAfterEscapingBattle (
    int playerHp,
    int playerFp,
    int currentPartnerHp,
    string[] havingItemNameArray
  )
  {
    CommonSave (
      playerHp,
      playerFp,
      currentPartnerHp,
      havingItemNameArray
    );
  }

  void CommonSave (
    int playerHp,
    int playerFp,
    int currentPartnerHp,
    string[] havingItemNameArray
  )
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    // プレイヤーのHP、FP
    saveData.playerCurrentHp = playerHp;
    saveData.playerCurrentFp = playerFp;
    // なかまのHP
    if (currentPartnerHp <= 0)
    {
      // なかまが倒されていた場合は体力1で復活させる
      currentPartnerHp = 1;
    }
    saveData.sakuraCurrentHp = currentPartnerHp;
    // アイテム
    saveData.havingItemsName = havingItemNameArray;
    // SaveSystem.Instance.Save ();
  }
}