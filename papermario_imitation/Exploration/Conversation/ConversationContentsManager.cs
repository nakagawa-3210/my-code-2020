using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConversationContentsManager
{
  private CommonUiManager commonUiManager;
  private GameConversationManager gameConversationManager;
  private CommonUiGUIManager commonUiGUIManager;
  // ------------------- まとめる予定 ------------------- //
  // ActionManagerとかを作ってGUIみたいにまとめるほうがこのクラスがスッキリする？
  public BaseActions baseActions;
  public HanaPlayerActions hanaPlayerActions;
  public ItemShopBuyingActions itemShopBuyingActions;
  public ItemShopSellingActions itemShopSellingActions;
  public ItemShopLeavingActions itemShopLeavingActions;
  public ItemShopGettingActions itemShopGettingActions;
  public ItemShopConversationActions itemShopConversationActions;
  // ------------------- まとめる予定 ------------------- //
  public ShopItemListContentsPreparer shopItemListContentsPreparer;
  private ConversationGUIManager conversationGUIManager;
  private ConversationAnimationManager conversationAnimationManager;
  private ConversationHolder conversationHolder;
  private ConversationReader conversationReader;
  private ConversationScene currentConversationScene;
  private CursorManager cursorManager;
  private CommonUiFunctions commonUiFunctions;
  private MenuCommonFunctions menuCommonFunctions;
  private List < (string text, string nextScene) > options;
  private GameObject optionPlate;
  private GameObject previousFrameSelectedGameObject;
  private GameObject selectedItemButton;
  private ConversationState conversationState;
  private float intervalForcharacterDisplay = 0.05f;
  private string currentText = string.Empty;
  private string previousText = string.Empty;
  private string itemName;
  private string itemPrice;
  private string optionNextSceneId;
  private float timeElapsed = 1.0f;
  private float timeUntilDisplay = 0.0f;
  private int lastUpdateCharacter = -1;

  private bool isCompleteDisplayText = false;
  private bool isOptionsShown = false;
  private bool endConversation = false;
  private bool isPlayingCharaAnimation = false;
  private bool isPlayingCharaAnimationPreviousFrame = false;
  private bool canSetOption = false;
  private bool canSelectOption = false;
  private bool startPlayerAnim = false;
  private bool isItemListShown = false;

  public enum ConversationState
  {
    normalChat,
    selectingYesNo,
    selectingItemShopOptions,
    sellingItem,
    leavingItem,
    gettingItem,
    // 困っているNPCに回復アイテムを渡す
    givingItem,
    // 鍵でドアをあけるとか
    usingImportantThing
  }

  public ConversationContentsManager (
    CommonUiManager commonUiManager,
    GameConversationManager gameConversationManager,
    CommonUiGUIManager commonUiGUIManager,
    string conversatinFileName,
    ConversationGUIManager conversationGUIManager,
    ConversationAnimationManager conversationAnimationManager = null,
    HanaPlayerTalkScript hanaPlayerTalkScript = null
  )
  {
    // マネージャー
    this.commonUiManager = commonUiManager;
    this.gameConversationManager = gameConversationManager;
    this.commonUiGUIManager = commonUiGUIManager;
    this.conversationGUIManager = conversationGUIManager;
    this.conversationAnimationManager = conversationAnimationManager;
    cursorManager = new CursorManager (
      commonUiGUIManager.commonHandCursor,
      conversationGUIManager.eventSystem
    );
    shopItemListContentsPreparer = new ShopItemListContentsPreparer (
      this,
      commonUiGUIManager.commonItemListButtonPrefab.gameObject,
      commonUiGUIManager.commonItemListContainer.transform
    );
    // 会話アクション
    baseActions = new BaseActions (this, hanaPlayerTalkScript);
    hanaPlayerActions = new HanaPlayerActions (this, this.conversationAnimationManager);
    itemShopBuyingActions = new ItemShopBuyingActions (this, hanaPlayerTalkScript);
    itemShopSellingActions = new ItemShopSellingActions (this);
    itemShopLeavingActions = new ItemShopLeavingActions (this);
    itemShopGettingActions = new ItemShopGettingActions (this);
    itemShopConversationActions = new ItemShopConversationActions (this);
    // 会話管理
    conversationHolder = new ConversationHolder (this, conversatinFileName);
    conversationReader = new ConversationReader (this);
    // 便利関数
    commonUiFunctions = new CommonUiFunctions ();
    menuCommonFunctions = new MenuCommonFunctions ();
    // ステート初期化
    conversationState = ConversationState.normalChat;
    // 会話要素
    if (hanaPlayerTalkScript != null)
    {
      itemName = hanaPlayerTalkScript.ShopItemName;
      itemPrice = hanaPlayerTalkScript.ShopItemPrice.ToString ();
    }
    optionPlate = conversationGUIManager.buttonSmallPanel.gameObject;
    // 前フレーム比較
    previousFrameSelectedGameObject = null;
    selectedItemButton = null;
  }

  // テキストファイルが指定するタイミングで呼ばれる関数によって変更される
  // BaseActions.EndTalking();
  public bool EndConversation
  {
    set { endConversation = value; }
    get { return endConversation; }
  }
  public bool StartPlayerAnim
  {
    get { return startPlayerAnim; }
  }
  public bool IsItemListShown
  {
    set { isItemListShown = value; }
    get { return isItemListShown; }
  }
  public bool IsPlayingCharaAnimation
  {
    set { isPlayingCharaAnimation = value; }
  }
  public bool CanSetOption
  {
    set { canSetOption = value; }
    get { return canSetOption; }
  }
  public bool CanSelectOption
  {
    set { canSelectOption = value; }
    get { return canSelectOption; }
  }
  public string OptionNextSceneId
  {
    get { return optionNextSceneId; }
  }
  public string ItemName
  {
    set { itemName = value; }
  }
  public string ItemPrice
  {
    set { itemPrice = value; }
  }
  public ConversationState GameConversationState
  {
    set { conversationState = value; }
    get { return conversationState; }
  }
  public List < (string text, string nextScene) > Options
  {
    set { options = value; }
  }

  public GameObject OptionPlate
  {
    get { return optionPlate; }
  }

  public CursorManager ConversationCursorManager
  {
    get { return cursorManager; }
  }

  public void SetNextProcess ()
  {
    string canLeavingItemNum = itemShopLeavingActions.CanLeavingItemNum.ToString ();
    // 引数に商品の価格、名前を渡して、指定箇所の文字をそれらの引数の値に入れ替える
    // Debug.Log ("currentConversationScene : " + currentConversationScene);
    conversationReader.ReadLines (currentConversationScene, itemName, itemPrice, canLeavingItemNum);
  }

  public void SetScene (string id)
  {
    // Debug.Log ("読み取りid : " + id);
    currentConversationScene = conversationHolder.ConversationScenes.Find (c => c.ID == id);
    currentConversationScene = currentConversationScene.Clone ();
    if (currentConversationScene == null) Debug.LogError ("シナリオがありません");
    SetNextProcess ();
  }

  public void SetOptionPlate (string plateSize)
  {
    // プレート増えるかも？
    if (plateSize == "small")
    {
      optionPlate = conversationGUIManager.buttonSmallPanel.gameObject;
    }
    else
    {
      optionPlate = conversationGUIManager.buttonBigPanel.gameObject;
    }
  }

  public void SetOptions ()
  {
    isOptionsShown = true;
    canSelectOption = true;
    optionPlate.transform.gameObject.SetActive (true);
    conversationGUIManager.conversationText.text = previousText;
    bool firstButton = true;
    foreach (var option in options)
    {
      Button button = Object.Instantiate (conversationGUIManager.OptionButton);
      Text buttonText = button.GetComponentInChildren<Text> ();
      buttonText.text = option.text;
      button.onClick.AddListener (() => OnClickOption (option.nextScene));
      button.transform.SetParent (optionPlate.transform, false);

      Vector3 scale = new Vector3 (1.0f, 1.0f, 1.0f);
      optionPlate.transform.localScale = scale;
      button.transform.localScale = scale;
      // キーで選択できるように最初のボタンをデフォルトで選択する
      if (firstButton)
      {
        firstButton = false;
        button.Select ();
        commonUiGUIManager.commonHandCursor.SetActive (true);
        cursorManager.InitCursorPosition (button.gameObject);
      }
    }
    // 状態変更
    ChangeConversationStateToYesNoOptions ();
  }

  void ChangeConversationStateToYesNoOptions ()
  {
    List<GameObject> optionList = menuCommonFunctions.GetChildList (optionPlate);
    // 選択肢がふたつしかない場合はYesNoの2択から選択中状態として扱う
    if (optionList.Count == 2)
    {
      baseActions.ChangeConversationStateToSelectingYesNo ();
    }
  }

  public void OnClickOption (string nextID = "")
  {
    canSetOption = false;
    optionNextSceneId = nextID;
    // Debug.Log ("optionNextSceneId : " + optionNextSceneId);
  }

  public void SelectDenyOption ()
  {
    // 子要素取得メモ(何度もしらべるので)
    // GameObject obj = parentGameObject.transform.GetChild(0).gameObject;
    // YesとNoの選択しかないので直打ち
    Button noButton = optionPlate.transform.GetChild (1).gameObject.GetComponent<Button> ();
    noButton.onClick.Invoke ();
  }

  public void DestroyButtonAndSetNextScene (string nextID)
  {
    // アニメの再生中は次の会話が表示されない
    // Debug.Log ("canSelectOption : " + canSelectOption);
    if (canSelectOption)
    {
      canSelectOption = false;
      // Debug.Log ("nextID : " + nextID);
      SetScene (nextID);
      isOptionsShown = false;
      optionPlate.transform.gameObject.SetActive (false);
      conversationGUIManager.conversationPanel.SetActive (true);
      // Input.ResetInputAxes ();
    }
    DestroyOptionButtons ();
  }

  public void DestroyOptionButtons ()
  {
    foreach (Transform t in optionPlate.transform)
    {
      UnityEngine.Object.Destroy (t.gameObject);
    }
  }

  // 最後の文字列かどうかのboolの値も引数で受け取るようにする
  public void SetText (string text)
  {
    currentText = text;
    // 想定表示時間と現在の時間をキャッシュ(一時的に保存)
    timeUntilDisplay = currentText.Length * intervalForcharacterDisplay;
    timeElapsed = Time.time;
    // 文字カウント初期化
    lastUpdateCharacter = -1;
  }

  public void ShowCurrentText ()
  {
    isCompleteDisplayText = Time.time > (timeElapsed + timeUntilDisplay);
    if (currentText == string.Empty) return;

    int displayCharacterCount = (int) (Mathf.Clamp01 ((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
    if (displayCharacterCount != lastUpdateCharacter)
    {
      if (displayCharacterCount <= 0) return;
      SEManager.Instance.Play (SEPath.CONVERSATION);
      conversationGUIManager.conversationText.text = currentText.Substring (0, displayCharacterCount);
      lastUpdateCharacter = displayCharacterCount;
    }
  }

  public void WaitingCurrentTextBeShown ()
  {
    // 会話が空でない場合かつ
    if (currentText == string.Empty) return;
    // キャラクターのアニメーションが前フレームで再生中でない場合
    if (isPlayingCharaAnimationPreviousFrame) return;

    // すべての文字が表示済みの場合かつ、選択肢が非表示の場合
    if (isCompleteDisplayText && !isItemListShown)
    {
      // 合図アイコン表示
      conversationGUIManager.nextTalkSign.SetActive (true);
      int allConversatonLine = currentConversationScene.Lines.Count;
      int currentLineIndex = currentConversationScene.Index;
      if (currentLineIndex < allConversatonLine)
      {
        // 次の会話表示
        if (Input.GetKeyDown (KeyCode.Space))
        {
          previousText = conversationGUIManager.conversationText.text;
          conversationGUIManager.conversationText.text = string.Empty;
          conversationGUIManager.nextTalkSign.SetActive (false);
          conversationReader.ReadLines (currentConversationScene);
        }
      }
    }
    // 会話内容が表示途中で、選択肢ボタンが非表示の場合
    else
    {
      // 合図アイコン非表示
      conversationGUIManager.nextTalkSign.SetActive (false);
      if (Input.GetKeyDown (KeyCode.Space) && !isOptionsShown)
      {
        timeUntilDisplay = 0.0f;
        // ボタン押しっぱなし時に新たなキーとして認識されてしまうのでコメントアウト
        // Input.ResetInputAxes ();
      }
    }
  }

  public void WaitingCharaAnimation ()
  {
    // アニメーションの終了監視を行う
    // アニメの終了検知にはAnimatorコンポーネントが必要
    // アニメーションが再生された時
    // Debug.Log ("isPlayingCharaAnimation : " + isPlayingCharaAnimation);
    // Debug.Log ("isPlayingCharaAnimationPreviousFrame : " + isPlayingCharaAnimationPreviousFrame);
    if (isPlayingCharaAnimation && !isPlayingCharaAnimationPreviousFrame)
    {
      startPlayerAnim = true;
      conversationGUIManager.conversationText.text = string.Empty;
    }
    // 前フレームでアニメ再生中かつ、現在のフレームでアニメ再生終了していた時
    if (isPlayingCharaAnimationPreviousFrame && !isPlayingCharaAnimation)
    {
      startPlayerAnim = false;
    }
    isPlayingCharaAnimationPreviousFrame = isPlayingCharaAnimation;
  }

  public void SetDescription ()
  {
    GameObject currentSelectedGameButton = EventSystem.current.currentSelectedGameObject;
    commonUiManager.SetSelectingItemDescriptionText (currentSelectedGameButton);
  }

  public void ManageNextSceneAfterSelectingItem ()
  {
    // アイテムリストコンテナは共通
    bool isAnyItemSelected = commonUiFunctions.IsAnyItemButtonSelected (commonUiGUIManager.commonItemListContainer);
    if (!isAnyItemSelected || selectedItemButton != null) return;
    isItemListShown = false;
    List<GameObject> conversationItemListViewButtonsList = new List<GameObject> ();
    conversationItemListViewButtonsList = menuCommonFunctions.GetChildList (commonUiGUIManager.commonItemListContainer);
    int selectedItemButtonNum = menuCommonFunctions.GetSelectedItemButtonNum (conversationItemListViewButtonsList);
    selectedItemButton = conversationItemListViewButtonsList[selectedItemButtonNum];
    BelongingButtonInfoContainer selectedItemInformation = selectedItemButton.GetComponent<BelongingButtonInfoContainer> ();
    if (conversationState == ConversationState.sellingItem)
    {
      // 会話でのかくにんに価格と名前が必要
      itemPrice = selectedItemInformation.PlayersSellingPrice.ToString ();
      itemName = selectedItemInformation.BelongingName;
      // 売却価格と選ばれたボタン番号が変更に必要
      itemShopSellingActions.SelectedItemButtonNum = selectedItemButtonNum;
      itemShopSellingActions.SelectedItemSellingPrice = selectedItemInformation.PlayersSellingPrice;
      itemShopSellingActions.GoToSellingConfirmationScene ();
    }
    else if (conversationState == ConversationState.leavingItem)
    {
      itemShopLeavingActions.SelectedItemButtonNum = selectedItemButtonNum;
      itemShopLeavingActions.SelectedItemName = selectedItemInformation.BelongingName;
      itemShopLeavingActions.LeavingMyItem ();
    }
    else if (conversationState == ConversationState.gettingItem)
    {
      itemShopGettingActions.SelectedItemButtonNum = selectedItemButtonNum;
      itemShopGettingActions.SelectedItemName = selectedItemInformation.BelongingName;
      itemShopGettingActions.GettingMyItem ();
    }
  }

  public void ManageBackItemShopScene ()
  {
    if (conversationState == ConversationState.selectingItemShopOptions)
    {
      canSetOption = false;
      conversationState = ConversationState.normalChat;
      itemShopConversationActions.GoToEndItemShopConversationScene ();
    }
    else if (conversationState == ConversationState.sellingItem ||
      conversationState == ConversationState.leavingItem ||
      conversationState == ConversationState.gettingItem)
    {
      isItemListShown = false;
      optionNextSceneId = null;
      conversationState = ConversationState.selectingItemShopOptions;
      itemShopConversationActions.GoToSelectingOptionScene ();
      DeleteAllItemButtons ();
    }
  }

  public void DeleteAllItemButtons ()
  {
    commonUiManager.RemoveAllItemButtonsFromListContainer ();
  }

  public void AfterWaitingCharaAnim ()
  {
    conversationReader.ReadLines (currentConversationScene);
  }

  public void InActivateConversationSign ()
  {
    conversationGUIManager.nextTalkSign.SetActive (false);
  }

  public void CloseConversation ()
  {
    conversationGUIManager.conversationText.text = string.Empty;
    currentText = string.Empty;
  }

}