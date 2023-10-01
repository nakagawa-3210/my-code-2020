using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasBadgeEquippingSignManager
{
  private BaseMenuListPreparer baseMenuListPreparer;
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private MenuCommonFunctions menuCommonFunctions;
  private GameObject allHavingBadgeListContainer;
  private GameObject equippingBadgeBadgeListContainer;
  private List<GameObject> allHavingBadgeListViewButtonsList;
  private List<GameObject> equippingBadgeListViewButtonsList;

  public MenuCanvasBadgeEquippingSignManager (
    GameObject allHavingBadgeListContainer,
    GameObject equippingBadgeBadgeListContainer
  )
  {
    baseMenuListPreparer = new BaseMenuListPreparer ();
    menuCommonFunctions = new MenuCommonFunctions ();
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    this.allHavingBadgeListContainer = allHavingBadgeListContainer;
    this.equippingBadgeBadgeListContainer = equippingBadgeBadgeListContainer;
    allHavingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (allHavingBadgeListContainer);
    equippingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (equippingBadgeBadgeListContainer);
    ManageCanNotEquipBadgeColor ();
  }

  public void ResetBadgeListViewButtonsList ()
  {
    equippingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (equippingBadgeBadgeListContainer);
    // Debug.Log ("equippingBadgeListViewButtonsList.Count : " + equippingBadgeListViewButtonsList.Count);
  }

  public void ManageBadgeEquippingSignActivity ()
  {
    ManageBadgeListSign (allHavingBadgeListViewButtonsList);
    ManageBadgeListSign (equippingBadgeListViewButtonsList);
  }

  void ManageBadgeListSign (List<GameObject> badgeList)
  {
    foreach (var badge in badgeList)
    {
      if (badge == null) return;
      BadgeButtonInfoContainer badgeButtonInfoContainer = badge.GetComponent<BadgeButtonInfoContainer> ();
      Transform buttonPointInfo = badge.transform.Find ("BadgePointInfo");
      Transform buttonCostImgContainers = buttonPointInfo.Find ("BadgeCostImgContainers");
      Transform buttonFullCostImgContainer = buttonCostImgContainers.transform.Find ("BadgeFullCostImgContainer");
      Transform puttingBadgeSign = badge.transform.Find ("PuttingBadgeSignImage");
      // 選択されたら選択中のサインを表示する
      // 選択されたら、ではなく、装備されたらにへんこうする
      if (badgeButtonInfoContainer.IsEquipped)
      {
        buttonFullCostImgContainer.gameObject.SetActive (true);
        puttingBadgeSign.gameObject.SetActive (true);
      }
      else
      {
        if (buttonFullCostImgContainer.gameObject.activeSelf || puttingBadgeSign.gameObject.activeSelf)
        {
          buttonFullCostImgContainer.gameObject.SetActive (false);
          puttingBadgeSign.gameObject.SetActive (false);
        }
      }
    }
  }

  // すべてのバッジからは選択できないバッジを分かりやすく表示する
  // バッジポイントが不足していることを分かりやすく
  public void ManageCanNotEquipBadgeColor ()
  {
    // すべてのバッジリストからつけられないもののみの色を変更する
    int playerTotalBadgePoint = SaveSystem.Instance.userData.playerMaxBp;
    int currentUsingBadgeCost = baseBadgeListFunctions.GetTotalEquippingBadgeCost (allHavingBadgeListContainer);
    int restOfTheBadgePoint = playerTotalBadgePoint - currentUsingBadgeCost;
    foreach (var badge in allHavingBadgeListViewButtonsList)
    {
      BadgeButtonInfoContainer badgeButtonInfoContainer = badge.GetComponentInParent<BadgeButtonInfoContainer> ();
      Transform badgeCostInfoTra = badge.transform.Find ("BadgePointInfo");
      Text badgeCostNumText = badgeCostInfoTra.Find ("BadgeCostNum").GetComponent<Text> ();
      Text badgeNameText = badge.transform.Find ("BadgeName").GetComponent<Text> ();
      Image badgeImg = badge.transform.Find ("BadgeImg").GetComponent<Image> ();
      int badgeCost = badgeButtonInfoContainer.Cost;
      bool isEquipped = badgeButtonInfoContainer.IsEquipped;
      // コストがオーバーしているかつ、まだ着けていないとき
      if (restOfTheBadgePoint < badgeCost && !isEquipped)
      {
        // アイテムと共通
        baseMenuListPreparer.SetCanNotSelectColor (badgeNameText, badgeImg, badgeCostNumText);
      }
      else
      {
        baseMenuListPreparer.SetCanSelectColor (badgeNameText, badgeImg, badgeCostNumText);
      }
    }
  }
}