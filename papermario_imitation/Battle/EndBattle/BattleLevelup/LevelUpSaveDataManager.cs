using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class LevelUpSaveDataManager
{
  private int beforeLevelUpMaxHp;
  private int beforeLevelUpMaxFp;
  private int beforeLevelUpMaxBp;

  public async UniTask LevelUpPlayerStatus (string selectedLevelUpOptionName)
  {
    MyGameData.MyData data = SaveSystem.Instance.userData;

    if (selectedLevelUpOptionName == LevelUpOptionSelectMotionManager.SelectingOption.heart.ToString ())
    {
      int hpGrowth = data.playerHpGrowthPoint;
      int currentMaxHp = data.playerMaxHp;
      beforeLevelUpMaxHp = currentMaxHp;
      int levelUpHp = currentMaxHp + hpGrowth;
      SaveSystem.Instance.userData.playerMaxHp = levelUpHp;
    }
    else if (selectedLevelUpOptionName == LevelUpOptionSelectMotionManager.SelectingOption.flower.ToString ())
    {
      int fpGrowth = data.playerFpGrowthPoint;
      int currentMaxFp = data.playerMaxFp;
      beforeLevelUpMaxFp = currentMaxFp;
      int levelUpFp = currentMaxFp + fpGrowth;
      SaveSystem.Instance.userData.playerMaxFp = levelUpFp;
    }
    else if (selectedLevelUpOptionName == LevelUpOptionSelectMotionManager.SelectingOption.badge.ToString ())
    {
      int bpGrowth = data.playerBpGrowthPoint;
      int currentMaxBp = data.playerMaxBp;
      beforeLevelUpMaxBp = currentMaxBp;
      int levelUpBp = currentMaxBp + bpGrowth;
      SaveSystem.Instance.userData.playerMaxBp = levelUpBp;
    }
    // SaveSystem.Instance.Save ();

    await RecoveringCompletely ();
  }

  async UniTask RecoveringCompletely ()
  {
    MyGameData.MyData data = SaveSystem.Instance.userData;
    data.playerCurrentHp = data.playerMaxHp;
    data.playerCurrentFp = data.playerMaxFp;

    // いまは仲間固定で処理
    string partnerName = "サクちゃん";
    int partnerLevel = data.sakuraLevel;
    PartnerDataArray partnerDataArray = new JsonReaderFromResourcesFolder ().GetPartnerDataArray ("JSON/GamePartnersData");
    data.sakuraCurrentHp = new MenuCommonFunctions ().GetPartnerMaxHp (partnerName, partnerLevel, partnerDataArray);

    // SaveSystem.Instance.Save ();
    await UniTask.WaitUntil (() =>
      beforeLevelUpMaxHp != data.playerMaxHp ||
      beforeLevelUpMaxFp != data.playerMaxFp ||
      beforeLevelUpMaxBp != data.playerMaxBp
    );
  }
}