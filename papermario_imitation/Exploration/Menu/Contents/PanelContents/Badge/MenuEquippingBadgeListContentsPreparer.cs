using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class MenuEquippingBadgeListContentsPreparer
{
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private GameObject buttonForBadge;
  private GameObject badgeEmptyCostImage;
  private GameObject badgeFullCostImage;
  private Transform allBadgeListContainerTra;
  private Transform equippingBadgeListContainerTra;
  private BadgeDataArray badgeDataArray;
  private List<GameObject> prevBadgeListViewButtonsList;
  private string imgGameObjectName;
  private string nameTextGameObjectName;
  private string badgePointInfoGameObjectName;
  private string badgePointCostTextInfoGameObjectName;
  private string badgeCostImgContainersName;
  private string badgePointEmptyCostImgContainerName;
  private string badgePointFullCostImgContainerName;
  private string puttingBadgeSignGameObjectName;
  public MenuEquippingBadgeListContentsPreparer (
    GameObject buttonForBadge,
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    Transform allBadgeListContainerTra,
    Transform equippingBadgeListContainerTra,
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
    this.equippingBadgeListContainerTra = equippingBadgeListContainerTra;
    this.imgGameObjectName = imgGameObjectName;
    this.nameTextGameObjectName = nameTextGameObjectName;
    this.badgePointInfoGameObjectName = badgePointInfoGameObjectName;
    this.badgePointCostTextInfoGameObjectName = badgePointCostTextInfoGameObjectName;
    this.badgeCostImgContainersName = badgeCostImgContainersName;
    this.badgePointEmptyCostImgContainerName = badgePointEmptyCostImgContainerName;
    this.badgePointFullCostImgContainerName = badgePointFullCostImgContainerName;
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    menuCommonFunctions = new MenuCommonFunctions ();
    prevBadgeListViewButtonsList = new List<GameObject> ();
    badgeDataArray = jsonReaderFromResourcesFolder.GetBadgeDataArray ("JSON/GameBadgesData");
    SetupEquippingBadgeListViewButtons ();
    prevBadgeListViewButtonsList = menuCommonFunctions.GetChildList (equippingBadgeListContainerTra.gameObject);
  }

  // つけているバッジリストは、バッジのつけ外しに応じて動的に変化する
  // リスト内のボタンをリセットして作り直す関数が欲しい
  public void ResetEquippingBadgeListViewButtons ()
  {
    // Debug.Log ("つけているバッジリストの中身変更");
    menuCommonFunctions.DeleteButtonFromListView (equippingBadgeListContainerTra);
    SetupEquippingBadgeListViewButtons ();
  }

  //上記のように DeleteButtonFromListView を用いたいけど、バッジリストの取得等で上手くいかないので、
  // DeleteButtonFromListViewForEquippingListで対応中(今後改修予定)
  // public void ResetEquippingBadgeListViewButtons ()
  // {
  //   // Debug.Log ("つけているバッジリストの中身変更");
  //   baseBadgeListFunctions.DeleteButtonFromListViewForEquippingList (equippingBadgeListContainerTra);
  //   SetupEquippingBadgeListViewButtons ();
  // }

  void SetupEquippingBadgeListViewButtons ()
  {
    // つけているバッジの名前リスト
    // string[] equippingBadgeNameArr = GetEquippingBadgeNameArray ();
    string[] equippingBadgeNameArr = menuCommonFunctions.GetEquippingBadgeNameArray ();
    // つけているバッジリストのId
    int[] equippingBadgeIdArr = SaveSystem.Instance.userData.puttingBadgeId;
    int[] equippingBadgeNumArr = SaveSystem.Instance.userData.puttingBadgeNums;
    for (var i = 0; i < equippingBadgeNameArr.Length; i++)
    {
      string equippingBadgeName = equippingBadgeNameArr[i];
      GameObject newButton = GameObject.Instantiate<GameObject> (buttonForBadge);
      Sprite buttonSprite = new GetGameSprite ().GetSameNameBadgeSprite (equippingBadgeName);
      baseBadgeListFunctions.SetupHavingBadgeListButton (
        newButton,
        badgeEmptyCostImage,
        badgeFullCostImage,
        equippingBadgeListContainerTra,
        buttonSprite,
        imgGameObjectName,
        nameTextGameObjectName,
        equippingBadgeName,
        badgePointInfoGameObjectName,
        badgePointCostTextInfoGameObjectName,
        badgeCostImgContainersName,
        badgePointEmptyCostImgContainerName,
        badgePointFullCostImgContainerName
      );
      BadgeButtonInfoContainer badgeButtonInfoContainer = newButton.GetComponent<BadgeButtonInfoContainer> ();
      // つけているバッジボタンのIsEquippedをtrueにする
      badgeButtonInfoContainer.IsEquipped = true;
      // つけているバッジのbadgeButtonにBadgeIdをもたせる
      badgeButtonInfoContainer.BadgeId = equippingBadgeIdArr[i];
      // すべてのバッジリストの何番目のバッジなのかを持たせる
      badgeButtonInfoContainer.PrevEquippingBadgeNum = equippingBadgeNumArr[i];
    }

  }

  // MenuCommonFunctionsに移植
  // string[] GetEquippingBadgeNameArray ()
  // {
  //   // つけているバッジの名前リストを作成する
  //   string[] havingBadgeNameArr = SaveSystem.Instance.userData.havingBadgesName;
  //   int[] equippingBadgeNumArr = SaveSystem.Instance.userData.puttingBadgeNums;
  //   List<string> equippingBadgeNameList = new List<string> ();
  //   // 持っているバッジ、つけているバッジが何番目かをもとにつけているバッジのみを探す
  //   foreach (var equippingBadgeNum in equippingBadgeNumArr)
  //   {
  //     for (var i = 0; i < havingBadgeNameArr.Length; i++)
  //     {
  //       int badgeNum = i;
  //       string badgeName = havingBadgeNameArr[i];
  //       if (equippingBadgeNum == badgeNum)
  //       {
  //         equippingBadgeNameList.Add (badgeName);
  //       }
  //     }
  //   }
  //   // つけているバッジの名前リスト
  //   string[] equippingBadgeNameArr = equippingBadgeNameList.ToArray ();
  //   return equippingBadgeNameArr;
  // }

}