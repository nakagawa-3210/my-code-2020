using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCanvasEnableButtonsManager
{
  // 選択肢をグループ分けする
  // フレーム選択、フレーム内選択
  private MenuCommonFunctions menuCommonFunctions;
  private CommonEnableButtonFunction commonEnableButtonFunction;
  // 最初のタグ
  private GameObject[] menuCanvasTagButtonsArray;
  // もちもの、たいせつなもの
  private GameObject[] belongingOptionsArray;
  private GameObject[] badgeOptionsArray;
  private GameObject partnerCursorTargetPosition;
  private GameObject itemScrollViewPanel;
  private GameObject improtantThingScrollViewPanel;
  private GameObject allHavingBadgeScrollViewPanel;
  private GameObject equippingBadgeScrollViewPanel;
  private GameObject skillListContainer;
  private GameObject itemListContainer;
  private GameObject importantThingListContainer;
  private GameObject allHavingBadgeListContainer;
  private GameObject equippingBadgeListContainer;
  private GameObject itemUsingTargetListContainer;
  private GameObject selectedMenuTag;
  private GameObject selectedBelongingOption;
  private GameObject selectedBadgeOption;
  private GameObject selectedItem;
  private List<GameObject> skillListViewButtonList;
  private List<GameObject> itemListViewButtonsList;
  private List<GameObject> importantThingListViewButtonsList;
  private List<GameObject> recoveringTargetList;
  private List<GameObject> allHavingBadgeListViewButtonsList;
  private List<GameObject> equippingBadgeListViewButtonsList;
  private EventSystem eventSystem;
  private bool isBack;
  private bool isMenuClosed;
  private State state;
  public enum State
  {
    close,
    selectingTag,
    selectingParter,
    selectingSkill,
    selectingBelonging,
    selectingItem,
    selectingImportantThingButton,
    selectingRecoveringTarget,
    selectingBadge,
    selectingBadgeFromAll,
    selectingBadgeFromEquipping,
  }

  public State MenuState
  {
    get { return state; }
  }

  public MenuCanvasEnableButtonsManager (
    GameObject[] menuCanvasTagButtonsArray,
    GameObject[] belongingOptionsArray,
    GameObject[] badgeOptionsArray,
    GameObject partnerCursorTargetPosition,
    GameObject itemScrollViewPanel,
    GameObject improtantThingScrollViewPanel,
    GameObject skillListContainer,
    GameObject itemListContainer,
    GameObject importantThingListContainer,
    GameObject allHavingBadgeListContainer,
    GameObject equippingBadgeListContainer,
    GameObject allHavingBadgeScrollViewPanel,
    GameObject equippingBadgeScrollViewPanel,
    GameObject itemUsingTargetListContainer,
    EventSystem eventSystem
  )
  {
    this.menuCanvasTagButtonsArray = menuCanvasTagButtonsArray;
    this.belongingOptionsArray = belongingOptionsArray;
    this.badgeOptionsArray = badgeOptionsArray;
    this.partnerCursorTargetPosition = partnerCursorTargetPosition;
    this.itemScrollViewPanel = itemScrollViewPanel;
    this.improtantThingScrollViewPanel = improtantThingScrollViewPanel;
    this.skillListContainer = skillListContainer;
    this.itemListContainer = itemListContainer;
    this.importantThingListContainer = importantThingListContainer;
    this.allHavingBadgeListContainer = allHavingBadgeListContainer;
    this.equippingBadgeListContainer = equippingBadgeListContainer;
    this.itemUsingTargetListContainer = itemUsingTargetListContainer;
    this.allHavingBadgeScrollViewPanel = allHavingBadgeScrollViewPanel;
    this.equippingBadgeScrollViewPanel = equippingBadgeScrollViewPanel;
    this.eventSystem = eventSystem;
    state = State.close;
    isBack = false;
    isMenuClosed = true;

    SetupButtonLists ();
  }

  async UniTask SetupButtonLists ()
  {
    skillListViewButtonList = null;
    itemListViewButtonsList = null;
    importantThingListViewButtonsList = null;
    recoveringTargetList = null;
    allHavingBadgeListViewButtonsList = null;
    equippingBadgeListViewButtonsList = null;
    // SelectingMenuTagButtons ();

    commonEnableButtonFunction = new CommonEnableButtonFunction ();
    menuCommonFunctions = new MenuCommonFunctions ();

    skillListViewButtonList = menuCommonFunctions.GetChildList (skillListContainer);
    // なかまのスキルは最低でも1つはあるので他とは条件を変える
    await UniTask.WaitUntil (() => skillListViewButtonList != null);

    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
    await UniTask.WaitUntil (() => itemListViewButtonsList != null);

    importantThingListViewButtonsList = menuCommonFunctions.GetChildList (importantThingListContainer);
    await UniTask.WaitUntil (() => importantThingListViewButtonsList != null);

    recoveringTargetList = menuCommonFunctions.GetChildList (itemUsingTargetListContainer);
    await UniTask.WaitUntil (() => recoveringTargetList != null);

    allHavingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (allHavingBadgeListContainer);
    await UniTask.WaitUntil (() => allHavingBadgeListViewButtonsList != null);

    equippingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (equippingBadgeListContainer);
    await UniTask.WaitUntil (() => equippingBadgeListViewButtonsList != null);
    // Debug.Log ("skillListViewButtonList.Count : " + skillListViewButtonList.Count);
    InactivateMenuForClose ();
  }

  public void InactivateMenuForClose ()
  {
    state = State.close;
    isBack = false;
    this.improtantThingScrollViewPanel.SetActive (false);
    this.equippingBadgeScrollViewPanel.SetActive (false);
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingMenuTagButtons ()
  {
    state = State.selectingTag;
    EnableButtons (menuCanvasTagButtonsArray);
    if (isBack)
    {
      // Debug.Log ("もどるからあけてるよー");
      SelectPreviousButton (selectedMenuTag);
    }
    else
    {
      isMenuClosed = false;
      // アイテムのたいせつなもの、バッジの装備中のものを選択中にクローズされた時のリセット用
      itemScrollViewPanel.SetActive (true);
      improtantThingScrollViewPanel.SetActive (false);
      allHavingBadgeScrollViewPanel.SetActive (true);
      equippingBadgeScrollViewPanel.SetActive (false);
      menuCanvasTagButtonsArray[0].GetComponent<Button> ().Select ();
    }
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingPartnerOptions ()
  {
    state = State.selectingParter;
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = true;
    if (isBack)
    {
      partnerCursorTargetPosition.GetComponent<Button> ().Select ();
    }
    else
    {
      selectedMenuTag = GetPreviousFrameSelectedGameObject ();
      partnerCursorTargetPosition.GetComponent<Button> ().Select ();
    }
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectSkills ()
  {
    if (eventSystem.currentSelectedGameObject.tag == "PartnerCursorTargetPosition")
    {
      state = State.selectingSkill;
      commonEnableButtonFunction.EnableListViewButtons (skillListViewButtonList);
      // Debug.Log ("skillListViewButtonList.Count : " + skillListViewButtonList.Count);
      skillListViewButtonList[0].GetComponent<Button> ().Select ();
      partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
      DisableButtons (menuCanvasTagButtonsArray);
      DisableButtons (belongingOptionsArray);
      DisableButtons (badgeOptionsArray);
      commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
      commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
      commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
      commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
      commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
    }
  }

  // スペースキー(またはエンターキー)を使用した進む移動
  public void SelectingBelongingOptions ()
  {
    // state = State.selectingBelonging;
    // EnableButtons (belongingOptionsArray);
    if (isBack)
    {
      // アイテム選択、たいせつなもの選択から帰ってきたときのみ
      if (state == State.selectingItem || state == State.selectingImportantThingButton)
      {
        state = State.selectingBelonging;
        EnableButtons (belongingOptionsArray);
        SelectPreviousButton (selectedBelongingOption);
      }
      // state = State.selectingBelonging;
      // EnableButtons (belongingOptionsArray);
      // SelectPreviousButton (selectedBelongingOption);
    }
    else
    {
      state = State.selectingBelonging;
      EnableButtons (belongingOptionsArray);
      // どのメニュータグから来たかを保存
      selectedMenuTag = GetPreviousFrameSelectedGameObject ();
      belongingOptionsArray[0].GetComponent<Button> ().Select ();
    }
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingItemButton ()
  {
    if (itemListViewButtonsList.Count == 0 || state == State.close) return;
    state = State.selectingItem;
    commonEnableButtonFunction.EnableListViewButtons (itemListViewButtonsList);
    if (isBack)
    {
      SelectPreviousButton (selectedItem);
    }
    else
    {
      selectedBelongingOption = GetPreviousFrameSelectedGameObject ();
      // Debug.Log ("itemListViewButtonsList.Count" + itemListViewButtonsList.Count);

      if (itemListViewButtonsList[0] == null) return;
      itemListViewButtonsList[0].GetComponent<Button> ().Select ();
    }
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    DisableButtons (menuCanvasTagButtonsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingImportantThingButton ()
  {
    // Debug.Log ("たいせつなものをひらいたよ");
    if (importantThingListViewButtonsList.Count == 0) return;
    state = State.selectingImportantThingButton;
    selectedBelongingOption = GetPreviousFrameSelectedGameObject ();
    importantThingListViewButtonsList[0].GetComponent<Button> ().Select ();
    commonEnableButtonFunction.EnableListViewButtons (importantThingListViewButtonsList);
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    DisableButtons (menuCanvasTagButtonsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingBadgeOptions ()
  {
    state = State.selectingBadge;
    EnableButtons (badgeOptionsArray);
    if (isBack)
    {
      SelectPreviousButton (selectedBadgeOption);
    }
    else
    {
      selectedMenuTag = GetPreviousFrameSelectedGameObject ();
      badgeOptionsArray[0].GetComponent<Button> ().Select ();
    }
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (importantThingListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingBadgeButtonFromAll ()
  {
    if (allHavingBadgeListViewButtonsList.Count == 0) return;
    state = State.selectingBadgeFromAll;
    commonEnableButtonFunction.EnableListViewButtons (allHavingBadgeListViewButtonsList);
    selectedBadgeOption = GetPreviousFrameSelectedGameObject ();
    allHavingBadgeListViewButtonsList[0].GetComponent<Button> ().Select ();
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }

  public void SelectingBadgeButtonFromEquipping ()
  {
    if (equippingBadgeListViewButtonsList.Count == 0) return;
    // Debug.Log ("equippingBadgeListViewButtonsList.Count : " + equippingBadgeListViewButtonsList.Count);
    state = State.selectingBadgeFromAll;
    commonEnableButtonFunction.EnableListViewButtons (equippingBadgeListViewButtonsList);
    selectedBadgeOption = GetPreviousFrameSelectedGameObject ();
    equippingBadgeListViewButtonsList[0].GetComponent<Button> ().Select ();
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (recoveringTargetList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
  }

  // 回復対象相手を選んだらメニューは閉じたい
  public void SelectRecoveringTarget ()
  {
    if (itemListViewButtonsList.Count == 0 || state != State.selectingItem) return;
    state = State.selectingRecoveringTarget;
    commonEnableButtonFunction.EnableListViewButtons (recoveringTargetList);
    selectedItem = GetPreviousFrameSelectedGameObject ();
    recoveringTargetList[0].GetComponent<Button> ().Select ();
    partnerCursorTargetPosition.GetComponent<Button> ().enabled = false;
    DisableButtons (menuCanvasTagButtonsArray);
    DisableButtons (belongingOptionsArray);
    DisableButtons (badgeOptionsArray);
    commonEnableButtonFunction.DisableListViewButtons (skillListViewButtonList);
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
    commonEnableButtonFunction.DisableListViewButtons (allHavingBadgeListViewButtonsList);
  }

  void SelectPreviousButton (GameObject previousButton)
  {
    isBack = false;
    previousButton.GetComponent<Button> ().Select ();
  }

  void Close ()
  {
    isMenuClosed = true;
    isBack = false;
    state = State.close;
  }

  GameObject GetPreviousFrameSelectedGameObject ()
  {
    GameObject previousGameObject = eventSystem.currentSelectedGameObject.gameObject;
    return previousGameObject;
  }

  void EnableButtons (GameObject[] buttonsList)
  {
    foreach (var button in buttonsList)
    {
      button.GetComponent<Button> ().enabled = true;
    }
  }
  void DisableButtons (GameObject[] buttonsList)
  {
    foreach (var button in buttonsList)
    {
      button.GetComponent<Button> ().enabled = false;
    }
  }
  // void EnableListViewButtons (List<GameObject> listViewButtonsList)
  // {
  //   foreach (var listViewButton in listViewButtonsList)
  //   {
  //     if (listViewButton == null) return;
  //     listViewButton.GetComponent<Button> ().enabled = true;
  //   }
  // }
  // void DisableListViewButtons (List<GameObject> listViewButtonsList)
  // {
  //   foreach (var listViewButton in listViewButtonsList)
  //   {
  //     if (listViewButton == null) return;
  //     listViewButton.GetComponent<Button> ().enabled = false;
  //   }
  // }

  //  ログ確認用
  // public void TestForSelectedItemName ()
  // {
  //   if (selectedItem == null)
  //   {
  //     Debug.LogError ("selectedItemはnullやで");
  //     return;
  //   };
  //   Debug.Log ("selectedItem.name : " + selectedItem.name);
  // }
  
  // public State MenuState
  // {
  //   get { return state; }
  // }

  public void ManageBelongingScrollViewActive ()
  {
    GameObject cursorIsOnOption = eventSystem.currentSelectedGameObject.gameObject;
    if (cursorIsOnOption.tag == "ItemsButton")
    {
      itemScrollViewPanel.SetActive (true);
      improtantThingScrollViewPanel.SetActive (false);
    }
    else if (cursorIsOnOption.tag == "ImportantThingsButton")
    {
      itemScrollViewPanel.SetActive (false);
      improtantThingScrollViewPanel.SetActive (true);
    }
  }

  public void ManageBadgeScrollViewActivity ()
  {
    GameObject cursorIsOnOption = eventSystem.currentSelectedGameObject.gameObject;
    if (cursorIsOnOption.tag == "AllBadgesButton")
    {
      allHavingBadgeScrollViewPanel.SetActive (true);
      equippingBadgeScrollViewPanel.SetActive (false);
    }
    else if (cursorIsOnOption.tag == "PuttingBadgeButton")
    {
      allHavingBadgeScrollViewPanel.SetActive (false);
      equippingBadgeScrollViewPanel.SetActive (true);
    }
  }

  public void BackButton ()
  {
    // 閉じてない時だけ
    if (state != State.close)
    {
      isBack = true;
      if (state == State.selectingTag)
      {
        Close ();
      }
      else if (state == State.selectingParter)
      {
        SelectingMenuTagButtons ();
      }
      else if (state == State.selectingSkill)
      {
        SelectingPartnerOptions ();
      }
      else if (state == State.selectingBelonging)
      {
        itemScrollViewPanel.SetActive (true);
        improtantThingScrollViewPanel.SetActive (false);
        SelectingMenuTagButtons ();
      }
      else if (state == State.selectingItem)
      {
        SelectingBelongingOptions ();
      }
      else if (state == State.selectingImportantThingButton)
      {
        SelectingBelongingOptions ();
      }
      else if (state == State.selectingRecoveringTarget)
      {
        SelectingItemButton ();
      }
      else if (state == State.selectingBadge)
      {
        allHavingBadgeScrollViewPanel.SetActive (true);
        equippingBadgeScrollViewPanel.SetActive (false);
        SelectingMenuTagButtons ();
      }
      else if (state == State.selectingBadgeFromAll)
      {
        SelectingBadgeOptions ();
      }
      else if (state == State.selectingBadgeFromEquipping)
      {
        SelectingBadgeOptions ();
      }
    }
  }
  public bool IsMenuClosed
  {
    get { return isMenuClosed; }
  }
  public void InactivateEquippingBadgeButtons ()
  {
    commonEnableButtonFunction.DisableListViewButtons (equippingBadgeListViewButtonsList);
  }
  public void InactivateItemButton ()
  {
    commonEnableButtonFunction.DisableListViewButtons (itemListViewButtonsList);
  }
  public void SetNewerItemButtonList ()
  {
    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
    // Debug.Log ("enableでのButtonsList : " + itemListViewButtonsList.Count);
  }

  public void SetNewerEquippingButtonList ()
  {
    equippingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (equippingBadgeListContainer);
    // Debug.Log ("equippingBadgeListViewButtonsList.Count : " + equippingBadgeListViewButtonsList.Count);
  }
  public bool IsSelectingMenuTag ()
  {
    return menuCanvasTagButtonsArray[0].GetComponent<Button> ().enabled;
  }

  public int GetEquippingButtonNum ()
  {
    return equippingBadgeListViewButtonsList.Count;
  }
}