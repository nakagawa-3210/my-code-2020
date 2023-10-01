using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MenuCanvasMotionManager
{
  // [SerializeField] float doTweenDurationTime; // 0.8f
  // [SerializeField] GameObject menuCanvasGUI;
  // [SerializeField] GameObject player;

  private float doTweenDurationTime; // 0.8f
  // private GameObject menuCanvasGUI;

  private MenuCanvasGUI menuCanvasGuiScript;
  private HanaPlayerScript hanaPlayerScript;
  private MenuCommonFunctions menuCommonFunctions;
  private MenuCanvasOpenCloseManager menuCanvasOpenCloseManager;
  private MenuCanvasPanelsManager menuCanvasPanelsManager;
  private CursorManager cursorManager;
  private MenuCanvasEnableButtonsManager menuCanvasEnableButtonsManager;
  private TargetListOpenCloseManager targetListOpenCloseManager;
  private MenuCanvasBadgeEquippingSignManager menuCanvasBadgeEquippingSignManager;
  private MenuCanvasUsingBadgeCostSignManager menuCanvasUsingBadgeCostSignManager;
  private List<Vector3> tagInitialPositionList;
  private List<GameObject> itemList;
  private List<GameObject> recoveringTargetList;
  private Vector3 contentsPanelsInitialPosition;
  private Vector3 descriptionPanelInitialPosition;
  private Vector3 tagsOutOfScreenPosition;
  private Vector3 contentsPanelsOutOfScreenPosition;
  private Vector3 descriptionPanelOutOfScreenPosition;
  private int previousItemListButtonNum;
  private bool openRecoveringTargetList;

  public MenuCanvasMotionManager (MenuCanvasGUI menuCanvasGuiScript, HanaPlayerScript hanaPlayerScript, float doTweenDurationTime)
  {
    this.doTweenDurationTime = doTweenDurationTime;
    this.menuCanvasGuiScript = menuCanvasGuiScript;
    this.hanaPlayerScript = hanaPlayerScript;

    menuCanvasOpenCloseManager = new MenuCanvasOpenCloseManager (
      menuCanvasGuiScript.defaultSelectedTagButton,
      menuCanvasGuiScript.defaultContentsPanel,
      menuCanvasGuiScript.descriptionPanel,
      menuCanvasGuiScript.tagButtonArray,
      menuCanvasGuiScript.contentsPanelArray,
      doTweenDurationTime
    );
    menuCanvasPanelsManager = new MenuCanvasPanelsManager (
      menuCanvasGuiScript.tagButtonArray,
      menuCanvasGuiScript.contentsPanelArray,
      menuCanvasGuiScript.defaultSelectedTagButton,
      menuCanvasOpenCloseManager,
      menuCanvasGuiScript.eventSystem
    );
    cursorManager = new CursorManager (
      menuCanvasGuiScript.cursor,
      menuCanvasGuiScript.eventSystem
    );
    targetListOpenCloseManager = new TargetListOpenCloseManager (
      menuCanvasGuiScript.whoPanel,
      menuCanvasGuiScript.recoveringTargetScrollViewPanel
    );
    // ボタンの情報に応じてSetActiveを操作してバッジボタンの見た目を変化させる挙動をつくる
    menuCanvasBadgeEquippingSignManager = new MenuCanvasBadgeEquippingSignManager (
      menuCanvasGuiScript.allHavingBadgeListContainer,
      menuCanvasGuiScript.equippingBadgeListContainer
    );
    // セーブデータの使用しているバッジコストに応じて、使用バッジコストの見た目を変化させる
    menuCanvasUsingBadgeCostSignManager = new MenuCanvasUsingBadgeCostSignManager (
      menuCanvasGuiScript.allHavingBadgeListContainer,
      menuCanvasGuiScript.badgeFullCostImgContainer.transform
    );
    // ボタンのenableを操作しているが、selectの操作によってカーソル位置を変えるので
    // コンテンツマネージャーではなくモーションマネージャにて管理

    menuCanvasEnableButtonsManager = new MenuCanvasEnableButtonsManager (
      menuCanvasGuiScript.tagButtonArray,
      menuCanvasGuiScript.belongingOptionsArray,
      menuCanvasGuiScript.badgeOptionsArray,
      menuCanvasGuiScript.partnerCursorTarget,
      menuCanvasGuiScript.itemScrollViewPanel,
      menuCanvasGuiScript.improtantThingScrollViewPanel,
      menuCanvasGuiScript.skillListContainer,
      menuCanvasGuiScript.itemListContainer,
      menuCanvasGuiScript.improtantThingListContainer,
      menuCanvasGuiScript.allHavingBadgeListContainer,
      menuCanvasGuiScript.equippingBadgeListContainer,
      menuCanvasGuiScript.allHavingBadgeScrollViewPanel,
      menuCanvasGuiScript.equippingBadgeScrollViewPanel,
      menuCanvasGuiScript.recoveringTargetListContainer,
      menuCanvasGuiScript.eventSystem
    );
    menuCommonFunctions = new MenuCommonFunctions ();
    itemList = new List<GameObject> ();
    recoveringTargetList = new List<GameObject> ();
    openRecoveringTargetList = false;

    previousItemListButtonNum = menuCanvasGuiScript.itemListContainer.transform.childCount;
    cursorManager.InitCursorPosition (menuCanvasGuiScript.defaultSelectedTagButton);
  }

  public void MotionUpdate ()
  {
    // 画面がクリックされてeventの選択フォーカスがはずれてnullになっていたら、
    // 前のフレームで選択されていたボタンを選択するようにすればエラーが出ないかも

    ManageOpenCloseMenu ();
    ManageCursorPosition ();
    MangeShowingPanel ();
    ManageShowingScrollView ();
    ManageOpenCloseRecoveringTargetList ();
    // // セーブが書かれるたびに更新したい
    ManageNewerItemButtonList ();
    ManageUsingBadgeCostInfo ();
    ManageBadgeCanEquipColor ();
    
  }

  public void MotionLateUpdate ()
  {
    // バッジオプションのすべてのバッジの上にカーソルが来たフレーム
    ManageNewerEquippingBadgeButtonList ();
  }

  void ManageOpenCloseMenu ()
  {
    // bool isTargetListTween = targetListOpenCloseManager.IsTweening ();
    // キーによる開け閉め
    if (Input.GetKeyDown (KeyCode.O) && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
    {
      menuCanvasOpenCloseManager.OpenMenu ();
      menuCanvasEnableButtonsManager.SelectingMenuTagButtons ();
      UpDateItemButtonList ();
    }
    else if (Input.GetKeyDown (KeyCode.Backspace)) //isTargetListTweenを条件式に加えてもダメだった
    {
      // 戻るボタン検知
      menuCanvasEnableButtonsManager.BackButton ();
    }

    // 戻るボタンがメニュータグ選択中に押されたときもメニューを閉じる
    bool isMenuClosed = menuCanvasEnableButtonsManager.IsMenuClosed;
    bool isStartMenuTween = menuCanvasOpenCloseManager.StartOpenCloseMenuTween ();
    // メニュー開け閉めのtweenが動いていない時かつCキーを押す
    if ((Input.GetKeyDown (KeyCode.C) && !isStartMenuTween) || (isMenuClosed && hanaPlayerScript.PlayerState == HanaPlayerScript.State.OpenMenu))
    {
      menuCanvasOpenCloseManager.CloseMenu ();
      menuCanvasEnableButtonsManager.InactivateMenuForClose ();
    }
  }

  public void ManageCloseMenuAfterSelectRecoveringTarget ()
  {
    // アイテム
    if (menuCanvasGuiScript.itemListContainer == null) return;
    itemList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.itemListContainer);
    recoveringTargetList = menuCommonFunctions.GetChildList (menuCanvasGuiScript.recoveringTargetListContainer);
    bool isRecoveringTargedSelected = menuCommonFunctions.GetIsSelectableTargetButtonSelected (recoveringTargetList, itemList);
    if (isRecoveringTargedSelected)
    {
      menuCanvasOpenCloseManager.CloseMenu ();
      menuCanvasEnableButtonsManager.InactivateMenuForClose ();
    }
  }

  void MangeShowingPanel ()
  {
    if (menuCanvasEnableButtonsManager.IsSelectingMenuTag () && hanaPlayerScript.PlayerState == HanaPlayerScript.State.OpenMenu)
    {
      Debug.Log ("パネルを表示");
      // 選択されているタグに対応するパネルを表示する
      menuCanvasPanelsManager.ShowSelectedPanel ();
    }
  }
  void ManageShowingScrollView ()
  {
    if (!menuCanvasOpenCloseManager.IsMenuOpen ()) return;
    menuCanvasEnableButtonsManager.ManageBelongingScrollViewActive ();
    menuCanvasEnableButtonsManager.ManageBadgeScrollViewActivity ();
    menuCanvasBadgeEquippingSignManager.ManageBadgeEquippingSignActivity ();
  }
  void ManageCursorPosition ()
  {
    if (menuCanvasOpenCloseManager.IsMenuClosedCompletely ())
    {
      cursorManager.InitCursorPosition (menuCanvasGuiScript.defaultSelectedTagButton);
    }
    else
    {
      cursorManager.MoveCursorTween ();
    }
    bool isMenuOpen = menuCanvasOpenCloseManager.IsMenuOpen ();
    cursorManager.ManageTweenSpeed (isMenuOpen);
  }
  async void ManageOpenCloseRecoveringTargetList ()
  {
    // リストビュー内のアイテムボタンが選ばれたかを検知
    // 動的に作られるボタンは更新処理にて対応
    // アイテムの配列は動的に変わる可能性がある
    bool IsItemSelected = IsAnyItemButtonSelected ();
    if (IsItemSelected && !openRecoveringTargetList)
    {
      // Debug.Log ("ボタンが選ばれたよ");
      openRecoveringTargetList = true;
      await targetListOpenCloseManager.OpenRecoveringTargetList ();
      // ターゲットリストの中身の色を変更する
      targetListOpenCloseManager.ManageTargetCanSelectColor (menuCanvasGuiScript.itemListContainer, menuCanvasGuiScript.recoveringTargetListContainer);
      menuCanvasEnableButtonsManager.SelectRecoveringTarget ();
    }
    // アイテムが選ばれていない状態でターゲットリストが開いていた時
    // コンテンツマネージャーにてバックスペースで戻って回復対象を選択しているとき。
    else if (!IsItemSelected && openRecoveringTargetList)
    {
      await targetListOpenCloseManager.CloseRecoveringTargetList ();
      openRecoveringTargetList = false;
    }
    // 確認用
    // Debug.Log ("IsItemSelected :" + IsItemSelected);
    // Debug.Log ("currentSelectedGameObject.name :" + EventSystem.current.currentSelectedGameObject.name);
  }

  void ManageNewerEquippingBadgeButtonList ()
  {
    // メニューが開いているときのみ
    // 一回だけの更新にするには、MenuCanvasManagerを作成して、
    // モーションとコンテンツの関数を順番を決めてこうしんするように改修しないとできなさそう
    if (!menuCanvasOpenCloseManager.IsMenuOpen ()) return;
    // updatedEquippingBadgeListを条件式に加えていたけど上手くいかない。
    // コンテンツでリストを作成し直す際に、Destroyよりも新しいボタンの作成の方が先に処理されるので一時的にリストの要素が倍になる
    // 倍になったリストを下記の処理で保存してしまい、リストを用いる際には削除されて無くなった要素を参照してnullエラーが出る
    // なので毎フレーム更新で何度もリストを更新して、正しいリストを無理やり保存している
    if (menuCanvasGuiScript.eventSystem.currentSelectedGameObject.tag == "AllBadgesButton")
    {
      // Debug.Log ("バッジリストがここのときだけ更新");
      int equippingBadgeNum = SaveSystem.Instance.userData.puttingBadgeId.Length;
      // if (equippingBadgeNum == menuCanvasEnableButtonsManager.GetEquippingButtonNum ())
      // {
      // updatedEquippingBadgeList = true;
      // }
      // updatedEquippingBadgeList = true;
      // カーソルの移動に必要なバッジリストの更新
      menuCanvasEnableButtonsManager.SetNewerEquippingButtonList ();
      // バッジのコストイメージ変更に必要なバッジリストの更新
      menuCanvasBadgeEquippingSignManager.ResetBadgeListViewButtonsList ();
      // 装備ボタンを非アクティブ化
      menuCanvasEnableButtonsManager.InactivateEquippingBadgeButtons ();
    }
  }

  void ManageNewerItemButtonList ()
  {
    bool updated = menuCommonFunctions.IsItemDataUpdated ();
    if (updated)
    {
      UpDateItemButtonList ();
    }
  }

  // ボタンの数が変わったら新しいボタンリストを作成する
  void UpDateItemButtonList ()
  {
    // Debug.Log ("ボタンリストのリスト内容更新");
    int currentItemListNum = menuCanvasGuiScript.itemListContainer.transform.childCount;
    previousItemListButtonNum = currentItemListNum;
    menuCanvasEnableButtonsManager.SetNewerItemButtonList ();
  }

  void ManageUsingBadgeCostInfo ()
  {
    bool badgeIsChanged = menuCommonFunctions.IsBadgeDataUpdated ();
    if (badgeIsChanged)
    {
      // Debug.Log ("バッジの中身が変えられたよ！");
      menuCanvasUsingBadgeCostSignManager.UpdateUsingBadgeCostImg ();
    }
  }

  void ManageBadgeCanEquipColor ()
  {
    // セーブデータが変更されたタイミングのみに改修する
    if (menuCanvasGuiScript.allHavingBadgeScrollViewPanel.activeSelf)
    {
      menuCanvasBadgeEquippingSignManager.ManageCanNotEquipBadgeColor ();
    }
  }

  public string MenuState
  {
    get { return menuCanvasEnableButtonsManager.MenuState.ToString (); }
  }
  // ログ確認用
  // public bool IsBack
  // {
  //   get { return menuCanvasEnableButtonsManager.IsBack; }
  // }

  public bool IsAnyItemButtonSelected ()
  {
    CommonUiFunctions commonUiFunction = new CommonUiFunctions ();
    return commonUiFunction.IsAnyItemButtonSelected (menuCanvasGuiScript.itemListContainer);
    // 共通関数に移動(バグがでるかもなので確認が十分に出来たらコメントアウト削除)
    // return targetListOpenCloseManager.IsAnyItemButtonSelected (menuCanvasGuiScript.itemListContainer);
  }

  // デフォルトで用意されているボタンは下記の関数を持ちいてOnClick処理をさせる
  // Tween中は操作できないように条件を加えるif(!tween) {処理}とかに変更
  public void SelectingMenuTags ()
  {
    menuCanvasEnableButtonsManager.SelectingMenuTagButtons ();
  }
  public void SelectingPartnerOptions ()
  {
    menuCanvasEnableButtonsManager.SelectingPartnerOptions ();
  }
  // 他と違い、「e」ボタンで開くようにする
  public void SelectingSkill ()
  {
    menuCanvasEnableButtonsManager.SelectSkills ();
  }
  public void SelectingBelongingOptions ()
  {
    menuCanvasEnableButtonsManager.SelectingBelongingOptions ();
  }
  public void SelectingItem ()
  {
    menuCanvasEnableButtonsManager.SelectingItemButton ();
  }
  public void SelectingImportantThing ()
  {
    menuCanvasEnableButtonsManager.SelectingImportantThingButton ();
  }
  public void SelectingBadgeOptions ()
  {
    menuCanvasEnableButtonsManager.SelectingBadgeOptions ();
  }
  public void SelectingBadgesFromAll ()
  {
    menuCanvasEnableButtonsManager.SelectingBadgeButtonFromAll ();
  }
  public void SelectingBadgeFromEquipping ()
  {
    menuCanvasEnableButtonsManager.SelectingBadgeButtonFromEquipping ();
  }

  public bool IsOpenMenu ()
  {
    // メニューが開いている状態、またはtween途中の状態
    return menuCanvasOpenCloseManager.IsMenuOpen ();
  }
  public bool IsMenuClosedCompletely ()
  {
    // メニューが開かれていないかつ、tweenアニメも終わっている状態
    return menuCanvasOpenCloseManager.IsMenuClosedCompletely ();
  }

}