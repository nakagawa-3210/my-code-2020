using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasUsingBadgeCostSignManager
{
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private GameObject allHavingBadgeListContainer;
  private Transform badgeFullCostImgContainerTransform;
  public MenuCanvasUsingBadgeCostSignManager (
    GameObject allHavingBadgeListContainer,
    Transform badgeFullCostImgContainerTransform
  )
  {
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    this.allHavingBadgeListContainer = allHavingBadgeListContainer;
    this.badgeFullCostImgContainerTransform = badgeFullCostImgContainerTransform;
  }

  public void UpdateUsingBadgeCostImg ()
  {
    // 現在の使用中コストの取得
    int usingBadgeCost =
      baseBadgeListFunctions.GetTotalEquippingBadgeCost (allHavingBadgeListContainer);
    baseBadgeListFunctions.SetupBadgeFullConstImgActivity (badgeFullCostImgContainerTransform, usingBadgeCost);
  }
}