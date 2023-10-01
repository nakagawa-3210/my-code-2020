using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class MenuHavingBadgeListContentsPreparer
{
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private GameObject buttonForBadge;
  private GameObject badgeEmptyCostImage;
  private GameObject badgeFullCostImage;
  private Transform allBadgeListContainerTra;
  private string imgGameObjectName;
  private string nameTextGameObjectName;
  private string badgePointInfoGameObjectName;
  private string badgePointCostTextInfoGameObjectName;
  private string badgeCostImgContainersName;
  private string badgePointEmptyCostImgContainerName;
  private string badgePointFullCostImgContainerName;
  private string puttingBadgeSignGameObjectName;
  public MenuHavingBadgeListContentsPreparer (
    GameObject buttonForBadge,
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    Transform allBadgeListContainerTra,
    string imgGameObjectName,
    string nameTextGameObjectName,
    string badgePointInfoGameObjectName,
    string badgePointCostTextInfoGameObjectName,
    string badgeCostImgContainersName,
    string badgePointEmptyCostImgContainerName,
    string badgePointFullCostImgContainerName,
    string puttingBadgeSignGameObjectName
  )
  {
    this.buttonForBadge = buttonForBadge;
    this.badgeEmptyCostImage = badgeEmptyCostImage;
    this.badgeFullCostImage = badgeFullCostImage;
    this.puttingBadgeSignGameObjectName = puttingBadgeSignGameObjectName;
    this.allBadgeListContainerTra = allBadgeListContainerTra;
    this.imgGameObjectName = imgGameObjectName;
    this.nameTextGameObjectName = nameTextGameObjectName;
    this.badgePointInfoGameObjectName = badgePointInfoGameObjectName;
    this.badgePointCostTextInfoGameObjectName = badgePointCostTextInfoGameObjectName;
    this.badgeCostImgContainersName = badgeCostImgContainersName;
    this.badgePointEmptyCostImgContainerName = badgePointEmptyCostImgContainerName;
    this.badgePointFullCostImgContainerName = badgePointFullCostImgContainerName;
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    SetupAllBadgeListViewButtons ();
    SetupEquippingInfo ();
  }

  public void ResetHavingbadgeListViewButtons ()
  {
    // baseBadgeListFunctions.DeleteButtonFromListView (allBadgeListContainerTra);
    // allBadgeListContainerTra = baseBadgeListFunctions.DeleteButtonFromListViewForHavingBadge (allBadgeListContainerTra);
    // SetupAllBadgeListViewButtons ();

    SetupEquippingInfo ();
    // Debug.Log ("せっていかんりょう");
    // Debug.Log ("ながさ : " + menuCommonFunctions.GetChildList (allBadgeListContainerTra.gameObject).Count);
  }

  void SetupAllBadgeListViewButtons ()
  {
    string[] havingBadgeNameArr = SaveSystem.Instance.userData.havingBadgesName;
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    // Debug.Log ("ケシタアトの数 : " + menuCommonFunctions.GetChildList (allBadgeListContainerTra.gameObject).Count);
    for (var i = 0; i < havingBadgeNameArr.Length; i++)
    {
      string havingBadgeName = havingBadgeNameArr[i];
      GameObject newButton = GameObject.Instantiate<GameObject> (buttonForBadge);
      Sprite buttonSprite = new GetGameSprite ().GetSameNameBadgeSprite (havingBadgeName);
      baseBadgeListFunctions.SetupHavingBadgeListButton (
        newButton,
        badgeEmptyCostImage,
        badgeFullCostImage,
        allBadgeListContainerTra,
        buttonSprite,
        imgGameObjectName,
        nameTextGameObjectName,
        havingBadgeName,
        badgePointInfoGameObjectName,
        badgePointCostTextInfoGameObjectName,
        badgeCostImgContainersName,
        badgePointEmptyCostImgContainerName,
        badgePointFullCostImgContainerName
      );
    }

    // // 装備済みのバッジボタンのみ情報変更
    // // ボタンの持つ特定のGameObjectを有効にし、装着しているという情報をボタンに持たせる
    // baseBadgeListFunctions.SetupBadgeEquippedBadgeButtonInformation (
    //   allBadgeListContainerTra,
    //   puttingBadgeSignGameObjectName,
    //   badgePointInfoGameObjectName,
    //   badgeCostImgContainersName,
    //   badgePointFullCostImgContainerName
    // );
  }

  void SetupEquippingInfo ()
  {
    // 装備済みのバッジボタンのみ情報変更
    // ボタンの持つ特定のGameObjectを有効にし、装着しているという情報をボタンに持たせる
    baseBadgeListFunctions.SetupBadgeEquippedBadgeButtonInformation (
      allBadgeListContainerTra,
      puttingBadgeSignGameObjectName,
      badgePointInfoGameObjectName,
      badgeCostImgContainersName,
      badgePointFullCostImgContainerName
    );
  }

}