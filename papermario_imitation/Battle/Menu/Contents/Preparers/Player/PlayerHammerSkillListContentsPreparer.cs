using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHammerSkillListContentsPreparer
{
  private PlayerCommonSkillListContentsPreparer playerCommonSkillListContentsPreparer;
  public PlayerHammerSkillListContentsPreparer (
    GameObject buttonForHammerSkill,
    Transform hammerSkillListContainerTransform
  )
  {
    playerCommonSkillListContentsPreparer = new PlayerCommonSkillListContentsPreparer ();
    string skillType = "hammer";
    string normalSkillName = "ハンマー";
    playerCommonSkillListContentsPreparer.SetupSkillListViewButtons (
      buttonForHammerSkill,
      hammerSkillListContainerTransform,
      skillType,
      normalSkillName
    );
  }

  public void ResetAvailableJumpSkillButtonInformation (int currentBattlePlayerFp)
  {
    playerCommonSkillListContentsPreparer.ResetAvailableBadgeSkillButtonInformation (currentBattlePlayerFp);
  }
}