using System.Collections;
using System.Collections.Generic;
// using UniRx.Async;
using UnityEngine;

public class MenuBadgeContentsPreparer
{
  private MenuHavingBadgeListContentsPreparer menuHavingBadgeListContentsPreparer;
  private MenuEquippingBadgeListContentsPreparer menuEquippingBadgeListContentsPreparer;
  private MenuBadgeCostInformationPanelManager menuBadgeCostInformationPanelManager;
  private BadgeDataArray badgeDataArray;
  private string badgeImgGameObjectName = "BadgeImg";
  private string badgeNameTextGameObjectName = "BadgeName";
  private string badgePointInfoGameObjectName = "BadgePointInfo";
  private string badgePointCostTextInfoGameObjectName = "BadgeCostNum";
  private string badgeCostImgContainersName = "BadgeCostImgContainers";
  private string badgePointEmptyCostImgContainerName = "BadgeEmptyCostImgContainer";
  private string badgePointFullCostImgContainerName = "BadgeFullCostImgContainer";
  private string equippingBadgeSignGameObjectName = "PuttingBadgeSignImage";
  public MenuBadgeContentsPreparer (
    // badgeのボタンprefab
    GameObject buttonForBadge,
    GameObject badgeEmptyCostImage,
    // badgeのコストイメージの〇のprefab
    GameObject badgeFullCostImage,
    GameObject playerTotalBadgePointText,
    GameObject playerRestOfTheBadgePoint,
    // badgeのコンテナTransform
    Transform allBadgeListContainerTra,
    Transform equippingBadgeListContainerTra,
    // 使用中バッジコストコンテナ
    Transform playerUsingBadgePointInformationContainerTransform,
    Transform emptyCostImgContainerTransform,
    Transform fullCostImgContainerTransform
  )
  {
    // // すべてのバッジリスト作成クラス
    menuHavingBadgeListContentsPreparer = new MenuHavingBadgeListContentsPreparer (
      buttonForBadge,
      badgeEmptyCostImage,
      badgeFullCostImage,
      allBadgeListContainerTra,
      badgeImgGameObjectName,
      badgeNameTextGameObjectName,
      badgePointInfoGameObjectName,
      badgePointCostTextInfoGameObjectName,
      badgeCostImgContainersName,
      badgePointEmptyCostImgContainerName,
      badgePointFullCostImgContainerName,
      equippingBadgeSignGameObjectName
    );
    // 装備中のバッジリスト作成クラス
    menuEquippingBadgeListContentsPreparer = new MenuEquippingBadgeListContentsPreparer (
      buttonForBadge,
      badgeEmptyCostImage,
      badgeFullCostImage,
      allBadgeListContainerTra,
      equippingBadgeListContainerTra,
      badgeImgGameObjectName,
      badgeNameTextGameObjectName,
      badgePointInfoGameObjectName,
      badgePointCostTextInfoGameObjectName,
      badgeCostImgContainersName,
      badgePointEmptyCostImgContainerName,
      badgePointFullCostImgContainerName,
      equippingBadgeSignGameObjectName
    );
    // 使用中のバッジポイント表示リストの
    menuBadgeCostInformationPanelManager = new MenuBadgeCostInformationPanelManager (
      badgeEmptyCostImage,
      badgeFullCostImage,
      playerTotalBadgePointText,
      playerRestOfTheBadgePoint,
      playerUsingBadgePointInformationContainerTransform,
      allBadgeListContainerTra,
      emptyCostImgContainerTransform,
      fullCostImgContainerTransform
    );
    // メニューの開け閉めに応じて中身を更新する機能を作る予定
    // SetupBadge (
    //   buttonForBadge,
    //   badgeEmptyCostImage,
    //   badgeFullCostImage,
    //   allBadgeListContainerTra,
    //   equippingBadgeListContainerTra,
    //   playerUsingBadgePointInformationContainerTransform,
    //   emptyCostImgContainerTransform,
    //   fullCostImgContainerTransform,
    //   badgeImgGameObjectName,
    //   badgeNameTextGameObjectName,
    //   badgePointInfoGameObjectName,
    //   badgePointCostTextInfoGameObjectName,
    //   badgeCostImgContainersName,
    //   badgePointEmptyCostImgContainerName,
    //   badgePointFullCostImgContainerName,
    //   equippingBadgeSignGameObjectName
    // );
  }

  // メニューの開け閉めに応じて中身を更新する機能を作る予定なので、=>作らないかも？
  // その時に必要になれば使うかも
  void SetupBadge (
    GameObject buttonForBadge,
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    GameObject playerTotalBadgePointText,
    GameObject playerRestOfTheBadgePoint,
    Transform allBadgeListContainerTra,
    Transform equippingBadgeListContainerTra,
    Transform playerUsingBadgePointInformationContainerTransform,
    Transform emptyCostImgContainerTransform,
    Transform fullCostImgContainerTransform,
    string badgeImgGameObjectName,
    string badgeNameTextGameObjectName,
    string badgePointInfoGameObjectName,
    string badgePointCostTextInfoGameObjectName,
    string badgeCostImgContainersName,
    string badgePointEmptyCostImgContainerName,
    string badgePointFullCostImgContainerName,
    string equippingBadgeSignGameObjectName
  )
  {
    // すべてのバッジリスト作成クラス
    menuHavingBadgeListContentsPreparer = new MenuHavingBadgeListContentsPreparer (
      buttonForBadge,
      badgeEmptyCostImage,
      badgeFullCostImage,
      allBadgeListContainerTra,
      badgeImgGameObjectName,
      badgeNameTextGameObjectName,
      badgePointInfoGameObjectName,
      badgePointCostTextInfoGameObjectName,
      badgeCostImgContainersName,
      badgePointEmptyCostImgContainerName,
      badgePointFullCostImgContainerName,
      equippingBadgeSignGameObjectName
    );
    // 装備中のバッジリスト作成クラス
    menuEquippingBadgeListContentsPreparer = new MenuEquippingBadgeListContentsPreparer (
      buttonForBadge,
      badgeEmptyCostImage,
      badgeFullCostImage,
      allBadgeListContainerTra,
      equippingBadgeListContainerTra,
      badgeImgGameObjectName,
      badgeNameTextGameObjectName,
      badgePointInfoGameObjectName,
      badgePointCostTextInfoGameObjectName,
      badgeCostImgContainersName,
      badgePointEmptyCostImgContainerName,
      badgePointFullCostImgContainerName,
      equippingBadgeSignGameObjectName
    );
    // 使用中のバッジポイント表示リストの
    menuBadgeCostInformationPanelManager = new MenuBadgeCostInformationPanelManager (
      badgeEmptyCostImage,
      badgeFullCostImage,
      playerTotalBadgePointText,
      playerRestOfTheBadgePoint,
      playerUsingBadgePointInformationContainerTransform,
      allBadgeListContainerTra,
      emptyCostImgContainerTransform,
      fullCostImgContainerTransform
    );
  }

  public void ManageRestOfBadgePointNumText ()
  {
    menuBadgeCostInformationPanelManager.ManageRestOfTheBadgePointNumText ();
  }

  public void ResetHavingBadgeListViewButtons ()
  {
    menuHavingBadgeListContentsPreparer.ResetHavingbadgeListViewButtons ();
  }

  public void ResetEquippingBadgeListViewButtons ()
  {
    menuEquippingBadgeListContentsPreparer.ResetEquippingBadgeListViewButtons ();
  }

}