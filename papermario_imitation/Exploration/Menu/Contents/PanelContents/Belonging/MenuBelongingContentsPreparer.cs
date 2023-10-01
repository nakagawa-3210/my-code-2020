using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBelongingContentsPreparer
{
  private MenuItemListContentsPreparer menuItemListContentsPreparer;
  private MenuImportantThingListContentsPreparer menuImportantThingListContentsPreparer;
  private MenuTargetListContentsPreparer menuTargetListContentsPreparer;
  private string buttonForBelongingNameTextGameObjectName = "BelongingName";
  private string buttonForBelongingImgGameObjectName = "BelongingImg";

  public MenuBelongingContentsPreparer (
    GameObject buttonForItemsAndImportantThings,
    GameObject buttonForRecoveringTargets,
    Transform itemListContainerTra,
    Transform importantThingListContainerTra,
    Transform recoveringTargetListContainerTra
  )
  {
    // ボタンの情報を保持するコンポーネントをボタンが持っているか確認
    if (buttonForItemsAndImportantThings.GetComponent<BelongingButtonInfoContainer> () == null)
    {
      Debug.LogError ("使用するボタンに「BelongingButtonInfoContainer」をセットしてください");
      return;
    }
    // アイテムリスト作成クラス
    menuItemListContentsPreparer = new MenuItemListContentsPreparer (
      buttonForItemsAndImportantThings,
      itemListContainerTra,
      buttonForBelongingImgGameObjectName,
      buttonForBelongingNameTextGameObjectName
    );
    // たいせつなものリスト作成クラス
    menuImportantThingListContentsPreparer = new MenuImportantThingListContentsPreparer (
      buttonForItemsAndImportantThings,
      importantThingListContainerTra,
      buttonForBelongingImgGameObjectName,
      buttonForBelongingNameTextGameObjectName
    );
    // 回復対象リスト作成クラス
    menuTargetListContentsPreparer = new MenuTargetListContentsPreparer (
      buttonForRecoveringTargets,
      recoveringTargetListContainerTra
    );

  }

  public void ResetItemListViewButtons ()
  {
    // Debug.Log ("ここでアイテムリストぼたんリセットしてるよ");
    menuItemListContentsPreparer.ResetItemListViewButtons ();
  }

  public void UpdateTargetButtonInformation ()
  {
    MyGameData.MyData userData = SaveSystem.Instance.userData;
    menuTargetListContentsPreparer.SetupTargetPlayerButton (userData);
    menuTargetListContentsPreparer.UpDateTargetPartnerButtonInformation (userData);
  }

}