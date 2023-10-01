using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class GameConversationManager : MonoBehaviour
{
  [SerializeField] CommonUiManager commonUiManager;
  [SerializeField] CommonUiGUIManager commonUiGUIManager;
  [SerializeField] ConversationGUIManager conversationGUIManager;
  [SerializeField] GameObject conversationCanvas;
  [SerializeField] GameObject conversationTimeLines;

  // 動きのマネージャーを追加する
  private ConversationMotionManager conversationMotionManager;
  private ConversationContentsManager conversationContentsManager;
  private ConversationAnimationManager conversationTimeLinesManager;
  private CursorManager cursorManager;
  private MenuCommonFunctions menuCommonFunctions;
  private List<GameObject> itemButtonList;
  private string[] scenarios;
  private float intervalForcharacterDisplay = 0.05f;
  private int currentLine = 0;
  private string currentText;
  private float timeUntilDisplay = 0.0f;
  private float timeElapsed = 1.0f;
  private int lastUpdateCharacter = -1;
  private TextAsset textAsset;
  private string textAssetName;
  private string nextSceneId;
  private bool showOptions;
  private bool showItemList;
  private bool isPlayingPlayerAnim;
  private bool isShowingItemList;
  private bool temporarilyCloseConversation;
  private bool endGameConversaion;
  // public bool EndConversaion
  // {
  //   get { return endGameConversaion; }
  // }

  void Start ()
  {
    conversationMotionManager = new ConversationMotionManager (
      commonUiManager,
      conversationGUIManager
    );
    conversationTimeLinesManager = conversationTimeLines.GetComponent<ConversationAnimationManager> ();
    menuCommonFunctions = new MenuCommonFunctions ();
    itemButtonList = new List<GameObject> ();
    currentText = string.Empty;
    showOptions = false;
    showItemList = false;
    isPlayingPlayerAnim = false;
    isShowingItemList = false;
    temporarilyCloseConversation = false;
    endGameConversaion = true;
    nextSceneId = null;

    // currentGameData = GameObject.FindWithTag ("CurrentGameData").GetComponent<CurrentGameData> ();
    // await UniTask.WaitUntil (() => currentGameData.EndReadingData ());
  }

  public bool IsCompleteDisplayText
  {
    get { return Time.time > timeElapsed + timeUntilDisplay; }
  }

  async UniTask Update ()
  {
    if (conversationContentsManager == null) return;
    bool endConversation = conversationContentsManager.EndConversation;
    // 新しいテキスト更新関数
    ManageBackScene ();
    conversationContentsManager.ShowCurrentText ();
    conversationContentsManager.WaitingCurrentTextBeShown ();
    conversationContentsManager.WaitingCharaAnimation ();
    conversationContentsManager.SetDescription ();
    conversationContentsManager.ManageNextSceneAfterSelectingItem ();
    await ManageNextSceneAfterOption ();
    cursorManager.MoveCursorTween ();
    await TemporarilyShowHideConversationPanel ();
    ShowAndHideConversationItemList ();
    await CloseConversationCanvas ();
    // test
    // ConversationContentsManager.ConversationState conversationState = conversationContentsManager.GameConversationState;
    // Debug.Log ("conversationState : " + conversationState);
  }

  void LateUpdate ()
  {
    bool isItemDataUpdated = menuCommonFunctions.IsItemDataUpdated ();
    if (conversationContentsManager != null && isItemDataUpdated)
    {
      conversationContentsManager.DeleteAllItemButtons ();
    }
  }

  void ManageBackScene ()
  {
    ConversationContentsManager.ConversationState conversationstate = conversationContentsManager.GameConversationState;
    // 二択の選択肢かアイテムリストが表示されているとき
    if (showOptions || showItemList)
    {
      if (Input.GetKeyDown (KeyCode.Backspace) &&
        conversationstate == ConversationContentsManager.ConversationState.selectingYesNo)
      {
        conversationContentsManager.SelectDenyOption ();
      }
      else if (Input.GetKeyDown (KeyCode.Backspace) &&
        conversationstate != ConversationContentsManager.ConversationState.normalChat)
      {
        conversationContentsManager.CanSelectOption = false;
        conversationContentsManager.ManageBackItemShopScene ();
        // Input.ResetInputAxes ();
      }
    }
  }

  // HanaPlayerScriptのもつ会話終了関数を受け取る 
  // プレイヤーが購入しようとしているアイテムの名前と価格を受け取る
  public async void OpenConversationCanvas (string conversationFileName, HanaPlayerTalkScript hanaPlayerTalkScript = null)
  {
    // CurrentGameData currentGameData = GameObject.FindWithTag ("CurrentGameData").GetComponent<CurrentGameData> ();
    // Debug.Log ("currentGameData.EndReadingData () : " + CurrentGameData.EndReadingData ());
    // await UniTask.WaitUntil (() => CurrentGameData.EndReadingData ());

    conversationContentsManager = new ConversationContentsManager (
      commonUiManager,
      this,
      commonUiGUIManager,
      conversationFileName,
      conversationGUIManager,
      conversationTimeLinesManager,
      hanaPlayerTalkScript
    );

    cursorManager = conversationContentsManager.ConversationCursorManager;
    cursorManager.SetMyTweenSpeed (0.6f);
    GameObject optionPanel = conversationContentsManager.OptionPlate;
    conversationMotionManager.FadeoutOptionPanel (conversationGUIManager.buttonBigPanel.gameObject);
    conversationMotionManager.FadeoutOptionPanel (conversationGUIManager.buttonSmallPanel.gameObject);
    await conversationMotionManager.ShowingConversationPanel ();
    string firstScene = "001";
    conversationContentsManager.SetScene (firstScene);
    endGameConversaion = false;
  }

  public async UniTask CloseConversationCanvas ()
  {
    bool endConversation = conversationContentsManager.EndConversation;
    if (endConversation && !endGameConversaion)
    {
      endGameConversaion = true;
      conversationContentsManager.InActivateConversationSign ();
      await conversationMotionManager.HidingConversationPanel ();
      conversationContentsManager.CloseConversation ();
      // Debug.Log ("ゲームの会話終了！！");
    }
  }

  async UniTask TemporarilyShowHideConversationPanel ()
  {
    bool startPlayerAnim = conversationContentsManager.StartPlayerAnim;
    bool isItemListShown = conversationContentsManager.IsItemListShown;
    if (!isPlayingPlayerAnim && startPlayerAnim && !temporarilyCloseConversation)
    {
      isPlayingPlayerAnim = true;
      temporarilyCloseConversation = true;
      await conversationMotionManager.HidingConversationPanel ();
    }
    else if (!isShowingItemList && isItemListShown && !temporarilyCloseConversation)
    {
      isShowingItemList = true;
      temporarilyCloseConversation = true;
      await conversationMotionManager.HidingConversationPanel ();
    }
    else if (isPlayingPlayerAnim && !startPlayerAnim && temporarilyCloseConversation)
    {
      isPlayingPlayerAnim = false;
      temporarilyCloseConversation = false;
      conversationContentsManager.AfterWaitingCharaAnim ();
      await conversationMotionManager.ShowingConversationPanel ();
    }
    else if (isShowingItemList && !isItemListShown && temporarilyCloseConversation)
    {
      isShowingItemList = false;
      temporarilyCloseConversation = false;
      await conversationMotionManager.ShowingConversationPanel ();
    }
  }

  async UniTask ManageNextSceneAfterOption ()
  {
    await ShowAndHideConversationOption ();
    ManageOptionsNextScene ();
  }

  // 閉じる機能にシーンをセットする役割を持たせているのを直したい
  public async UniTask ShowAndHideConversationOption ()
  {
    bool canSetOption = conversationContentsManager.CanSetOption;
    GameObject optionPanel = conversationContentsManager.OptionPlate;
    if (canSetOption && !showOptions)
    {
      showOptions = true;
      conversationContentsManager.SetOptions ();
      await conversationMotionManager.FadeInOptionPanel (optionPanel);
    }
    else if (!canSetOption && showOptions)
    {
      conversationContentsManager.CanSetOption = false;
      nextSceneId = conversationContentsManager.OptionNextSceneId;
      // Debug.Log ("nextSceneId : " + nextSceneId);
      await conversationMotionManager.FadeoutOptionPanel (optionPanel);
      commonUiGUIManager.commonHandCursor.SetActive (false);
    }
  }

  void ManageOptionsNextScene ()
  {
    // 選択肢選択によるcanSetOption=falseのときは
    bool canSetOption = conversationContentsManager.CanSetOption;
    if (!canSetOption && showOptions)
    {
      showOptions = false;
      if (nextSceneId != null)
      {
        // Debug.Log ("nextSceneId : " + nextSceneId);
        conversationContentsManager.DestroyButtonAndSetNextScene (nextSceneId);
      }
      else
      {
        conversationContentsManager.DestroyOptionButtons ();
      }
    }
  }

  // 一部だけアイテムを拾う処理と共通
  public async UniTask ShowAndHideConversationItemList ()
  {
    bool isItemListShown = conversationContentsManager.IsItemListShown;
    ConversationContentsManager.ConversationState conversationState = conversationContentsManager.GameConversationState;
    if (isItemListShown && !showItemList)
    {
      // Debug.Log ("いっかいしか押せないよ");
      showItemList = true;
      // 会話内容に合わせてリストに用意するボタン変更
      await conversationContentsManager.shopItemListContentsPreparer.SetupItemListButtons (conversationState);
      await conversationMotionManager.ShowingItemList ();
      commonUiGUIManager.commonHandCursor.SetActive (true);
      itemButtonList = menuCommonFunctions.GetChildList (commonUiGUIManager.commonItemListContainer);
      cursorManager.InitCursorPosition (itemButtonList[0]);
      itemButtonList[0].GetComponent<Button> ().Select ();
    }
    else if (!isItemListShown && showItemList)
    {
      showItemList = false;
      commonUiGUIManager.commonHandCursor.SetActive (false);
      await conversationMotionManager.HidingItemList ();
    }
  }

  public string TextAssetName
  {
    set { textAssetName = value; }
  }

  public bool EndConversation
  {
    // 会話キャンバス表示の取得
    get
    {
      bool endConversaion = true;
      if (conversationContentsManager != null)
      {
        if (!conversationContentsManager.EndConversation || !conversationMotionManager.ClosedConversationPanel)
        {
          // endConversaion = conversationContentsManager.EndConversation;
          // Debug.Log("conversationContentsManager.EndConversation : " + conversationContentsManager.EndConversation);
          // Debug.Log("conversationMotionManager.ClosedConversationPanel : " + conversationMotionManager.ClosedConversationPanel);
          endConversaion = false;
        }
      }
      return endConversaion;
    }
  }
}