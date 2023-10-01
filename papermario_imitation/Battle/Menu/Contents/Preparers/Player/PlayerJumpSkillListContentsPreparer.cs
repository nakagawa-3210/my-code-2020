using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ジャンプスキルとハンマースキルは同じクラスを使うかも
public class PlayerJumpSkillListContentsPreparer
{
  private PlayerCommonSkillListContentsPreparer playerCommonSkillListContentsPreparer;
  public PlayerJumpSkillListContentsPreparer (
    GameObject buttonForJumpSkill,
    Transform jumpSkillListContainerTransform
  )
  {
    playerCommonSkillListContentsPreparer = new PlayerCommonSkillListContentsPreparer ();
    string skillType = "jump";
    string normalSkillName = "ジャンプ";
    playerCommonSkillListContentsPreparer.SetupSkillListViewButtons (
      buttonForJumpSkill,
      jumpSkillListContainerTransform,
      skillType,
      normalSkillName
    );
  }

  // スキルボタンが使用可能かの情報を更新する
  public void ResetAvailableJumpSkillButtonInformation (int currentBattlePlayerFp)
  {
    playerCommonSkillListContentsPreparer.ResetAvailableBadgeSkillButtonInformation (currentBattlePlayerFp);
  }

}