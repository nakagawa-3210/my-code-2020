using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCanvasContentsManager
{
  // メニューキャンバス内のテキストであるゲームオブジェクト名と、
  // セーブデータのキー名が一致した場合のみにテキストの内容を変更する
  private MenuCanvasGUI menuCanvasGuiScript;

  private MenuCommonFunctions menuCommonFunctions;
  private List<GameObject> itemListViewButtonsList;
  private List<GameObject> targetListViewButtonsList;
  private GameObject currentSelectedGameObject;
  private GameObject previousSelectedGameObject;
  private MenuPlayerContentsPreparer menuPlayerContentsPreparer;
  private MenuPartnerContentsPreparer menuPartnerContentsPreparer;
  private MenuBelongingContentsPreparer menuBelongingContentsPreparer;
  private MenuBadgeContentsPreparer menuBadgeContentsPreparer;
  private MenuDescriptionContentsManager menuDescriptionContentsManager;
  private int previousItemListButtonNum;
  private bool isOnAllBadgesButton;
  private bool isOnEquippingBadgeButton;

  private bool endSetup;

  public bool EndSetup
  {
    get { return endSetup; }
  }

  public MenuCanvasContentsManager (MenuCanvasGUI menuCanvasGuiScript)
  {
    endSetup = false;
    this.menuCanvasGuiScript = menuCanvasGuiScript;
    menuPlayerContentsPreparer = new MenuPlayerContentsPreparer (
      menuCanvasGuiScript.playerLevel,
      menuCanvasGuiScript.playerTitle,
      menuCanvasGuiScript.playerCurrentHp,
      menuCanvasGuiScript.playerMaxHp,
      menuCanvasGuiScript.playerCurrentFp,
      menuCanvasGuiScript.playerMaxFp,
      menuCanvasGuiScript.playerMaxBp,
      menuCanvasGuiScript.playerExperiencePoints,
      menuCanvasGuiScript.playerCoin,
      menuCanvasGuiScript.playerStarFragments,
      menuCanvasGuiScript.playerSuperFertilizer,
      menuCanvasGuiScript.playerTotalPlayingTimeHour,
      menuCanvasGuiScript.playerTotalPlayingTimeMin
    );

    menuPartnerContentsPreparer = new MenuPartnerContentsPreparer (
      menuCanvasGuiScript.partnerCursorTarget,
      menuCanvasGuiScript.skillButton,
      menuCanvasGuiScript.currentPartnerCurrentHp,
      menuCanvasGuiScript.currentPartnerMaxHp,
      menuCanvasGuiScript.currentPartnerNameText,
      menuCanvasGuiScript.skillListContainer.transform,
      menuCanvasGuiScript.eventSystem
    );

    menuBelongingContentsPreparer = new MenuBelongingContentsPreparer (
      menuCanvasGuiScript.itemListButton,
      menuCanvasGuiScript.recoveringTargetListButton,
      menuCanvasGuiScript.itemListContainer.transform,
      menuCanvasGuiScript.improtantThingListContainer.transform,
      menuCanvasGuiScript.recoveringTargetListContainer.transform
    );

    menuBadgeContentsPreparer = new MenuBadgeContentsPreparer (
      menuCanvasGuiScript.badgeListButton,
      menuCanvasGuiScript.badgeEmptyCostImage,
      menuCanvasGuiScript.badgeFullCostImage,
      menuCanvasGuiScript.playerTotalBadgePointNumText,
      menuCanvasGuiScript.playerRestOfTheBadgePoint,
      menuCanvasGuiScript.allHavingBadgeListContainer.transform,
      menuCanvasGuiScript.equippingBadgeListContainer.transform,
      menuCanvasGuiScript.playerUsingBadgePointInformationPanel.transform,
      menuCanvasGuiScript.badgeEmptyCostImgContainer.transform,
      menuCanvasGuiScript.badgeFullCostImgContainer.transform
    );

    // DescriptionContentsManagerのSetStaticButtonInformation関数の中で参照しているGameObjectを共通にする
    menuDescriptionContentsManager = new MenuDescriptionContentsManager (
      menuCanvasGuiScript.descriptionText,
      menuCanvasGuiScript.descriptionSelectImage,
      menuCanvasGuiScript.eventSystem
    );
    // itemArray = menuBelongingContentsPreparer.GetItemArray ();
    menuCommonFunctions = new MenuCommonFunctions ();
    menuPlayerContentsPreparer.SetupMenuPanelsContentsText ();
    SetupButtonLists ();
    isOnAllBadgesButton = false;
    isOnEquippingBadgeButton = false;
    previousItemListButtonNum = menuCanvasGuiScript.itemListContainer.transform.childCount;

    CheckSetup ();
  }

  async UniTask CheckSetup ()
  {
    List<GameObject> skillListViewButtonList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.skillListContainer);
    List<GameObject> itemListViewButtonsList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.itemListContainer);
    List<GameObject> importantThingListViewButtonsList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.improtantThingListContainer);
    List<GameObject> recoveringTargetList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.recoveringTargetListContainer);
    List<GameObject> allHavingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.allHavingBadgeListContainer);
    List<GameObject> equippingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.equippingBadgeListContainer);

    await UniTask.WaitUntil (() => skillListViewButtonList != null && itemListViewButtonsList != null && importantThingListViewButtonsList != null &&
      recoveringTargetList != null && allHavingBadgeListViewButtonsList != null && equippingBadgeListViewButtonsList != null);
    endSetup = true;
  }

  public void ContentsUpdate ()
  {
    // currentSelectedGameObjectの更新
    ManageCurrentSelectedGameObject ();
    menuDescriptionContentsManager.SetDescriptionContents (currentSelectedGameObject, previousSelectedGameObject);
    ManagePreviousSelectedGameObject ();
  }
  public void ContentsLateUpdate ()
  {
    // UpdateCanvasContentsのアイテムリストの削除処理は他の処理よりも後に行われてほしい
    UpdateCanvasContents ();
    ManageSelectSelectableTarget ();
  }

  public void ManageBackToItemListFromTargetList ()
  {
    if (Input.GetKeyDown (KeyCode.Backspace))
    {
      // Debug.Log ("あいてむの押された情報削除");
      ResetIsSelectedInformation ();
    }
  }

  void ManageSelectSelectableTarget ()
  {
    bool isSelectableTargetSelected = menuCommonFunctions.GetIsSelectableTargetButtonSelected (targetListViewButtonsList, itemListViewButtonsList);
    // 回復アイテムを選択可能なキャラに使用した時
    if (isSelectableTargetSelected)
    {
      // 最新のアイテム保持情報をリストビューに反映したい
      // 持っているアイテムボタンを更新(増減)
      // Debug.Log ("アイテム使用後のコンテンツ更新");
      // メニュー画面外でのセーブデータの変更に対応していない
      // (アイテムを購入した後にメニューを開いても更新されない)
      // 使用したアイテムボタンの削除
      DeleteUsedItemButton ();
      // 選択したボタンの情報リセット
      ResetIsSelectedInformation ();
      // 回復対象ボタンのテキスト内容をデータに合わせて更新
    }
  }

  void DeleteUsedItemButton ()
  {
    int usedItemNumber = menuCommonFunctions.GetSelectedItemButtonNum (itemListViewButtonsList);
    // あとで確認改修
    GameObject itemListContainer = menuCanvasGuiScript.itemListContainer;
    GameObject usedItemButton = itemListContainer.transform.GetChild (usedItemNumber).gameObject;
    MonoBehaviour.Destroy (usedItemButton);
  }

  // セーブデータに変更があるたびに更新してほしい
  void UpdateCanvasContents ()
  {
    bool isItemDataUpdated = menuCommonFunctions.IsItemDataUpdated ();
    if (isItemDataUpdated)
    {
      menuBelongingContentsPreparer.ResetItemListViewButtons ();
      menuBelongingContentsPreparer.UpdateTargetButtonInformation ();
    }
    bool isSaveDataUpdated = menuCommonFunctions.IsSaveDataUpdated ();
    if (isSaveDataUpdated)
    {
      // リストの再設定
      SetupButtonLists ();
      // アイテムの使用で変更された体力等の情報をテキストに反映
      menuPlayerContentsPreparer.SetupMenuPanelsContentsText ();
      menuPartnerContentsPreparer.SetupPartnerHp ();
      // 宿屋、回復ボックス等で回復したとき
      menuBelongingContentsPreparer.UpdateTargetButtonInformation ();
      menuBadgeContentsPreparer.ManageRestOfBadgePointNumText ();
    }

    // バッジリストの更新
    if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject == null) return;
    if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject.tag == "AllBadgesButton" && !isOnAllBadgesButton)
    {
      // Debug.Log ("つけているバッジリストの更新");
      isOnAllBadgesButton = true;
      // つけているバッジリストの更新
      menuBadgeContentsPreparer.ResetEquippingBadgeListViewButtons ();
    }
    else if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject.tag != "AllBadgesButton")
    {
      isOnAllBadgesButton = false;
    }

    if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject.tag == "PuttingBadgeButton" && !isOnEquippingBadgeButton)
    {
      isOnEquippingBadgeButton = true;
      menuBadgeContentsPreparer.ResetHavingBadgeListViewButtons ();
    }
    else if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject.tag != "PuttingBadgeButton")
    {
      isOnEquippingBadgeButton = false;
    }

  }

  void SetupButtonLists ()
  {
    GameObject recoveringTargetListContainer = menuCanvasGuiScript.recoveringTargetListContainer;
    targetListViewButtonsList = menuCommonFunctions.GetChildList (recoveringTargetListContainer);
    GameObject itemListContainer = menuCanvasGuiScript.itemListContainer;
    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
  }

  // ボタンがもつ自身が選ばれていたという情報の削除
  public void ResetIsSelectedInformation ()
  {
    SetupButtonLists ();
    // 回復対象ボタン自身がもつ自分が選ばれたかの判断情報リセット
    menuCommonFunctions.ResetTargetButtonIsSelected (targetListViewButtonsList);
    // アイテムボタン自身がもつ自分が選ばれたかの判断情報リセット
    menuCommonFunctions.ResetItemButtonIsSelected (itemListViewButtonsList);
  }

  void ManageCurrentSelectedGameObject ()
  {
    currentSelectedGameObject = menuCanvasGuiScript.eventSystem.currentSelectedGameObject;
  }
  void ManagePreviousSelectedGameObject ()
  {
    if (currentSelectedGameObject != previousSelectedGameObject)
    {
      previousSelectedGameObject = currentSelectedGameObject;
    }
  }
}