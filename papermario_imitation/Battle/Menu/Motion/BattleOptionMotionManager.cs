using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

//  SEManager.Instanceの改修予定

public class BattleOptionMotionManager : MonoBehaviour
{
  [SerializeField] bool isPlayer;
  [SerializeField] bool isPartner;
  [SerializeField] GameObject battleCommonOptionUi;
  [SerializeField] GameObject playerBattleOptionUi;
  [SerializeField] GameObject partnerBattleOptionUi;
  [SerializeField] GameObject battleManager;
  // 500.0f
  [SerializeField] float battleMenuRotationSpeed = 500.0f;
  // 0.5f
  [SerializeField] float firstOptionShowingRotatingDuration = 0.5f;
  // 0.1f
  [SerializeField] float firstOptionShrinkingDuration = 0.1f;
  // 0.1f
  [SerializeField] float listRotatingDuration = 0.1f;
  private MenuCommonFunctions menuCommonFunctions;
  private OptionGroupRotationManager optionGroupRotationManager;
  private TargetOptionCursorManager targetOptionCursorManager;
  private BattleManager battleManagerScript;
  private BattleCommonOptionUi battleCommonOptionUiScript;
  private PlayerBattleOptionUi playerBattleOptionUiScript;
  private PartnerBattleOptionUi partnerBattleOptionUiScript;
  private BattleMenuState battleMenuState;
  // 攻撃対象、回復対象の選択から戻るときにどのリストを再表示するかに必要
  private BattleMenuState previousBattleMenuState;
  private BattleOptionTweenManager battleOptionTweenManager;
  private SkillButtonInformationContainer selectedSkillButtonInfoContainer;
  private CursorManager listHandCursorManager;
  private Animator optionBalloonAnimator;
  private List<GameObject> jumpSkillList;
  private List<GameObject> hammerSkillList;
  private List<GameObject> itemOptionList;
  private List<GameObject> strategyOptionList;
  private List<GameObject> partnerSkillList;
  private List<GameObject> battlePlayersCharacterList;
  private List<GameObject> battleEnemyCharacterList;
  private List<GameObject> selectabelBattleTargetCharacterList;
  private List<GameObject> cursorTargetList;
  public List<GameObject> optionList;
  private GameObject currentSelectOptionBalloon;
  private GameObject selectedOptionButton;
  private GameObject selectedTarget;
  private GameObject selectedItemOptionButton;
  private int selectedOptionNum;
  private int selectedTargetNum;
  private bool wantInteractableValueBeTrue;
  private bool isSelectingFromPlayerParty;

  // 全選択の時にはBattleCommonOptionMultipleTargetをSetActive(true)にするだけ
  public enum BattleMenuState
  {
    firstOption,
    jumpOption,
    hammerOption,
    partnerSkillOption,
    itemOption,
    strategyOption,
    targetOption,
    allTargetOption,
    runAwayTime,
    guardTime,
    actionTime,
    waitingTime
  }
  public BattleMenuState CurrentBattleMenuState
  {
    // バトルマネージャーで敵の攻撃や仲間の選択を待つとき、自分の出番が来た時などに変更する(ステート等を初期化するために)
    // もしくはプレイヤーのアクションが終了したときにこのクラスのリセット関数等を呼ぶようにする
    set { battleMenuState = value; }
    get { return battleMenuState; }
  }
  public BattleMenuState PreviousBattleMenuState
  {
    get { return previousBattleMenuState; }
  }
  public GameObject SelectedOptionButton
  {
    get { return selectedOptionButton; }
  }
  public GameObject SelectedTarget
  {
    get { return selectedTarget; }
  }
  public List<GameObject> BattlePlayersCharacterList
  {
    set { battlePlayersCharacterList = value; }
  }
  public List<GameObject> BattleEnemyCharacterList
  {
    set { battleEnemyCharacterList = value; }
  }
  public List<GameObject> SelectabelBattleTargetCharacterList
  {
    get { return selectabelBattleTargetCharacterList; }
  }
  public bool IsSelectingFromPlayerParty
  {
    get { return isSelectingFromPlayerParty; }
  }

  // バトルマネージャーにも渡せる
  public int SelectedTargetNum
  {
    get { return selectedTargetNum; }
  }

  void Start ()
  {
    battleMenuState = BattleMenuState.waitingTime;
    menuCommonFunctions = new MenuCommonFunctions ();
    battleManagerScript = battleManager.GetComponent<BattleManager> ();
    battleCommonOptionUiScript = battleCommonOptionUi.GetComponent<BattleCommonOptionUi> ();
    playerBattleOptionUiScript = playerBattleOptionUi.GetComponent<PlayerBattleOptionUi> ();
    partnerBattleOptionUiScript = partnerBattleOptionUi.GetComponent<PartnerBattleOptionUi> ();
    // 分岐箇所をSetup()に移動中
    // 共通
    // プレイヤーの攻撃、回復対象選択のターゲットハンドカーソル初期化
    targetOptionCursorManager = new TargetOptionCursorManager (
      battleCommonOptionUiScript.targetHandCursor
    );
    // リストハンドカーソル初期化
    listHandCursorManager = new CursorManager (
      battleCommonOptionUiScript.listHandCursor,
      battleCommonOptionUiScript.eventSystem
    );
    battleCommonOptionUiScript.listHandCursor.SetActive (false);
    battleCommonOptionUiScript.targetHandCursor.SetActive (false);
    battleCommonOptionUiScript.selectedEnemyNameFrame.SetActive (false);
    battleCommonOptionUiScript.attackDescriptionFrame.SetActive (false);
    battleCommonOptionUiScript.optionButtonDescriptionFrame.SetActive (false);
    battleCommonOptionUiScript.battleCommonOptionMultipleTarget.SetActive (false);
    cursorTargetList = new List<GameObject> ();
    // 待機状態
    battleMenuState = BattleMenuState.waitingTime;
    previousBattleMenuState = BattleMenuState.waitingTime;
    // リストのボタン無効
    wantInteractableValueBeTrue = false;
    Setup ();
  }

  void Update ()
  {
    ManageSelectedTargetNum ();
    listHandCursorManager.MoveCursorTween ();
    UpdateSelectedOptionNum ();
    ManageShowingOptionBalloonForRotating ();
    ManageSelectedFirstOptionRotation ();
    ManageSelectFirstListButton ();
    ManageTargetHandCursorPosition ();
  }

  void Setup ()
  {
    if (isPlayer)
    {
      optionBalloonAnimator = playerBattleOptionUiScript.currentSelectOptionBalloon.GetComponent<Animator> ();
      optionGroupRotationManager = new OptionGroupRotationManager (
        playerBattleOptionUiScript.optionGroup,
        playerBattleOptionUiScript.optionList
      );
      battleOptionTweenManager = new BattleOptionTweenManager (
        playerBattleOptionUiScript.optionList,
        playerBattleOptionUiScript.forShowingRotation,
        playerBattleOptionUiScript.currentSelectOptionBalloon,
        battleCommonOptionUiScript.optionBalloonMovePositionTransform,
        firstOptionShowingRotatingDuration,
        firstOptionShrinkingDuration,
        listRotatingDuration
      );
      battleOptionTweenManager.SetupPlayerList (
        playerBattleOptionUiScript.playerJumpSkillListContainer,
        playerBattleOptionUiScript.playerHammerSkillListContainer
      );
      playerBattleOptionUiScript.currentSelectOptionBalloon.SetActive (false);
      currentSelectOptionBalloon = playerBattleOptionUiScript.currentSelectOptionBalloon;
      optionList = playerBattleOptionUiScript.optionList;
      jumpSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerJumpSkillListContainer);
      hammerSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerHammerSkillListContainer);
    }
    else if (isPartner)
    {
      optionBalloonAnimator = partnerBattleOptionUiScript.currentSelectOptionBalloon.GetComponent<Animator> ();
      optionGroupRotationManager = new OptionGroupRotationManager (
        partnerBattleOptionUiScript.optionGroup,
        partnerBattleOptionUiScript.optionList
      );
      battleOptionTweenManager = new BattleOptionTweenManager (
        partnerBattleOptionUiScript.optionList,
        partnerBattleOptionUiScript.forShowingRotation,
        partnerBattleOptionUiScript.currentSelectOptionBalloon,
        battleCommonOptionUiScript.optionBalloonMovePositionTransform,
        firstOptionShowingRotatingDuration,
        firstOptionShrinkingDuration,
        listRotatingDuration
      );
      battleOptionTweenManager.SetupPartnerList (partnerBattleOptionUiScript.partnerSkillListContainer);
      partnerBattleOptionUiScript.currentSelectOptionBalloon.SetActive (false);
      currentSelectOptionBalloon = partnerBattleOptionUiScript.currentSelectOptionBalloon;
      optionList = partnerBattleOptionUiScript.optionList;
      partnerSkillList = menuCommonFunctions.GetChildList (partnerBattleOptionUiScript.partnerSkillListContainer);
    }
    // 共通
    //   アイテム
    SetupItemList ();
    battleOptionTweenManager.SetupCommonList (battleCommonOptionUiScript.itemOptionListContainer);
    // さくせん
    strategyOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.strategyOptionListContainer);
    battleOptionTweenManager.SetupCommonList (battleCommonOptionUiScript.strategyOptionListContainer);
  }

  // プレイヤーのターンが来た時に呼ぶ
  public void StartPlayerTurn ()
  {
    // Debug.Log ("じぶんのターン");
    battleMenuState = BattleMenuState.firstOption;
    battleOptionTweenManager.ShowFirstOptionsWithRotating ();
  }

  // 敵選択後に見えないところで初期の状態に戻し、次ターン獲得時の表示に備える処理
  public async UniTask ResetPlayerFirstOption ()
  {
    // 吹き出しの位置リセット
    await battleOptionTweenManager.MoveOptionBalloonToFirstPosition ();
    battleOptionTweenManager.ResetFirstOption ();
    optionGroupRotationManager.ResetOptionRotation ();
    void commonResetOption (List<GameObject> optionList)
    {
      foreach (var option in optionList)
      {
        option.SetActive (true);
      }
    }
    commonResetOption (playerBattleOptionUiScript.optionList);
    commonResetOption (partnerBattleOptionUiScript.optionList);
    optionGroupRotationManager.SelectedOptionNum = 0;
    selectedOptionNum = 0;
    playerBattleOptionUiScript.optionList[optionGroupRotationManager.SelectedOptionNum].SetActive (false);
  }

  // 他クラスからも用いたいので関数化している
  public async UniTask SetupItemList ()
  {
    itemOptionList = null;
    itemOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.itemOptionListContainer);
    await UniTask.WaitUntil(() => itemOptionList != null);
  }

  // 選択中の敵番号更新
  void ManageSelectedTargetNum ()
  {
    selectedTargetNum = targetOptionCursorManager.SelectedTargetNum;
  }

  void UpdateSelectedOptionNum ()
  {
    bool endShowingFirstOptionTween = battleOptionTweenManager.EndShowingFirstOptionTween;
    if (battleMenuState == BattleMenuState.firstOption && endShowingFirstOptionTween)
    {
      selectedOptionNum = optionGroupRotationManager.SelectedOptionNum;
    }
  }

  // 最初の選択肢が回転中の時の吹き出し表示非表示管理(あくまでも"選択"している時の回転)
  // 選択肢が回転しながら登場する動きを作成したら、下記関数の処理をするためには条件を加える必要がある
  void ManageShowingOptionBalloonForRotating ()
  {
    bool isRotating = optionGroupRotationManager.IsRotating;
    if (!isRotating && battleMenuState == BattleMenuState.firstOption)
    {
      currentSelectOptionBalloon.SetActive (true);
      // playerBattleOptionUiScriptに入れている順番に依存するので、Unityエディタで
      // Optionの順番を要確認
      optionList[selectedOptionNum].SetActive (false);
    }
    else if (isRotating && battleMenuState == BattleMenuState.firstOption)
    {
      currentSelectOptionBalloon.SetActive (false);
      foreach (var option in optionList)
      {
        option.SetActive (true);
      }
    }
  }

  void ManageSelectedFirstOptionRotation ()
  {

    bool endShowingFirstOptionTween = battleOptionTweenManager.EndShowingFirstOptionTween;
    bool endBalloonRotating = EndBalloonRotation ();
    if (battleMenuState == BattleMenuState.firstOption && endBalloonRotating && endShowingFirstOptionTween)
    {
      optionGroupRotationManager.ManageSelectedOptionNum ();
      optionGroupRotationManager.ManageOptionRotationBasedOnNum (battleMenuRotationSpeed);
    }
  }

  void ManageSelectFirstListButton ()
  {
    // ボタンが選択可能かを管理する
    // ボタンコンポーネントの値を変更するのでContentsManagerの役割にするか迷った
    // BattleOptionManagerにてContentsManagerとMotionManagerを合わせた処理を無理に作るとMotionManagerからステートをとったりでコードが冗長になるのでやめた
    // 回転していないかつ、表示のtweenが回った後
    bool endBalloonRotating = EndBalloonRotation ();
    if (endBalloonRotating && wantInteractableValueBeTrue)
    {
      wantInteractableValueBeTrue = false;
      if (isPlayer)
      {
        ManageSelectPlayerFirstButton ();
      }
      else if (isPartner)
      {
        ManageSelectPartnerFirstButton ();
      }
    }
  }

  // 押せないようにしていたボタンを押せるように戻し、ステートに応じてデフォルト選択のボタンを設定
  void ManageSelectPlayerFirstButton ()
  {
    if (battleMenuState == BattleMenuState.jumpOption)
    {
      ManageFirstListButtonAndCursor (jumpSkillList);
    }
    else if (battleMenuState == BattleMenuState.hammerOption)
    {
      ManageFirstListButtonAndCursor (hammerSkillList);
    }
    else if (battleMenuState == BattleMenuState.itemOption)
    {
      ManageFirstListButtonAndCursor (itemOptionList);
    }
    else if (battleMenuState == BattleMenuState.strategyOption)
    {
      ManageFirstListButtonAndCursor (strategyOptionList);
    }
  }

  void ManageSelectPartnerFirstButton ()
  {
    if (battleMenuState == BattleMenuState.partnerSkillOption)
    {
      ManageFirstListButtonAndCursor (partnerSkillList);
    }
    else if (battleMenuState == BattleMenuState.itemOption)
    {
      ManageFirstListButtonAndCursor (itemOptionList);
    }
    else if (battleMenuState == BattleMenuState.strategyOption)
    {
      ManageFirstListButtonAndCursor (strategyOptionList);
    }
  }

  void ManageFirstListButtonAndCursor (List<GameObject> list)
  {
    menuCommonFunctions.ActivateButtonInteractable (list);
    GameObject firstButton = list[0];
    firstButton.GetComponent<Button> ().Select ();
    listHandCursorManager.InitCursorPosition (firstButton);
    listHandCursorManager.ManageTweenSpeed (true);
    battleCommonOptionUiScript.listHandCursor.SetActive (true);
  }

  void InactivateAllListButtonsInteractable ()
  {
    if (isPlayer)
    {
      // リスト非表示のときはすべてのリストがボタンを押せない状態になっていればいいので分岐無し
      menuCommonFunctions.InactivateButtonInteractable (jumpSkillList);
      menuCommonFunctions.InactivateButtonInteractable (hammerSkillList);
    }
    else if (isPartner)
    {
      menuCommonFunctions.InactivateButtonInteractable (partnerSkillList);
    }
    menuCommonFunctions.InactivateButtonInteractable (itemOptionList);
    listHandCursorManager.ManageTweenSpeed (false);
  }

  void ManageTargetHandCursorPosition ()
  {
    bool endHideTween = battleOptionTweenManager.EndHideListTween;
    if (battleMenuState == BattleMenuState.targetOption && endHideTween)
    {
      targetOptionCursorManager.ManageTargetHandCurSorPosition ();
    }
  }

  // ボタンコンポーネントを用いない選択なのでplayerBattleOptionで呼び出し方法を決める
  // 最初の選択後、リストを表示する際の処理
  // プレイヤーと仲間で異なる
  public async UniTask ManageFirstOption ()
  {
    bool endBalloonRotating = EndBalloonRotation ();
    bool isRotating = optionGroupRotationManager.IsRotating;
    bool endShowingFirstOptionTween = battleOptionTweenManager.EndShowingFirstOptionTween;
    if (isRotating || !endBalloonRotating || !endShowingFirstOptionTween || battleMenuState != BattleMenuState.firstOption) return;
    // 決定ボタン音再生
    SEManager.Instance.Play (SEPath.MENU_DECISION);
    // 吹き出しの移動
    battleOptionTweenManager.MoveOptionBalloonToListPosition ();
    // ふきだしの回転開始
    optionBalloonAnimator.SetBool ("isRotating", true);
    // 最初の選択肢を縮小させる動きで隠す
    await battleOptionTweenManager.ShrinkFirstOptions ();
    // ふきだしの回転終了
    optionBalloonAnimator.SetBool ("isRotating", false);
    if (isPlayer)
    {
      await ManagePlayerFirstOption ();
    }
    else if (isPartner)
    {
      await ManagePartnerFirstOption ();
    }
  }
  public async UniTask ManagePlayerFirstOption ()
  {
    // 選択した選択肢に応じての処理
    int jump = 0;
    int hammer = 1;
    int item = 2;
    int strategy = 3;
    if (selectedOptionNum == jump)
    {
      // Debug.Log ("ジャンプの選択");
      battleMenuState = BattleMenuState.jumpOption;
      ShowList (playerBattleOptionUiScript.playerJumpSkillListContainer);
    }
    else if (selectedOptionNum == hammer)
    {
      // Debug.Log ("ハンマーの選択");
      battleMenuState = BattleMenuState.hammerOption;
      ShowList (playerBattleOptionUiScript.playerHammerSkillListContainer);
    }
    else if (selectedOptionNum == item)
    {
      // Debug.Log ("アイテムの選択");
      battleMenuState = BattleMenuState.itemOption;
      ShowList (battleCommonOptionUiScript.itemOptionListContainer, true);
    }
    else if (selectedOptionNum == strategy)
    {
      // Debug.Log ("戦術の選択");
      battleMenuState = BattleMenuState.strategyOption;
      ShowList (battleCommonOptionUiScript.strategyOptionListContainer);
    }
  }
  public async UniTask ManagePartnerFirstOption ()
  {
    int skill = 0;
    int item = 1;
    int strategy = 2;
    if (selectedOptionNum == skill)
    {
      battleMenuState = BattleMenuState.partnerSkillOption;
      // Debug.Log ("スキルリストを表示");
      ShowList (partnerBattleOptionUiScript.partnerSkillListContainer);
    }
    else if (selectedOptionNum == item)
    {
      battleMenuState = BattleMenuState.itemOption;
      ShowList (battleCommonOptionUiScript.itemOptionListContainer, true);
    }
    else if (selectedOptionNum == strategy)
    {
      battleMenuState = BattleMenuState.strategyOption;
      ShowList (battleCommonOptionUiScript.strategyOptionListContainer);
    }
  }

  // 最初の選択肢を選んだ後に表示されるリストのオプションの管理
  // リストのボタンの内のいづれかが選択された場合に呼ぶ
  public async UniTask ManageButtonListOption ()
  {
    if (battleMenuState == BattleMenuState.firstOption || battleMenuState == BattleMenuState.targetOption) return;
    if (isPlayer)
    {
      ManagePlayerButtonListOption ();
    }
    else if (isPartner)
    {
      ManagePartnerButtonListOption ();
    }
  }

  async UniTask ManagePlayerButtonListOption ()
  {
    // 選ばれたボタン確認 (リストはコンテンツマネージャーが編集したものをうけとるようにする)
    jumpSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerJumpSkillListContainer);
    hammerSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerHammerSkillListContainer);
    itemOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.itemOptionListContainer);
    strategyOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.strategyOptionListContainer);
    if (battleMenuState == BattleMenuState.jumpOption)
    {
      selectedOptionButton = menuCommonFunctions.GetSelectedSelectableSkillButton (jumpSkillList);
      if (selectedOptionButton == null) return;
      battleManagerScript.CurrentSelectedOptionButton = selectedOptionButton;

      selectabelBattleTargetCharacterList = battleEnemyCharacterList;
      isSelectingFromPlayerParty = false;

      await ManageButtonListCommonProcess (
        BattleMenuState.jumpOption,
        playerBattleOptionUiScript.playerJumpSkillListContainer
      );
      // 画面下にアクションコマンドの操作方法を表示(a値を下げて半透明状態)
      ShowFadeCommandDescription ();
    }
    else if (battleMenuState == BattleMenuState.hammerOption)
    {
      selectedOptionButton = menuCommonFunctions.GetSelectedSelectableSkillButton (hammerSkillList);
      if (selectedOptionButton == null) return;
      battleManagerScript.CurrentSelectedOptionButton = selectedOptionButton;

      selectabelBattleTargetCharacterList = battleEnemyCharacterList;
      isSelectingFromPlayerParty = false;

      await ManageButtonListCommonProcess (
        BattleMenuState.hammerOption,
        playerBattleOptionUiScript.playerHammerSkillListContainer
      );
      ShowFadeCommandDescription ();
    }
    await ManageCommonListOption ();
  }
  // 仲間用
  async UniTask ManagePartnerButtonListOption ()
  {
    partnerSkillList = menuCommonFunctions.GetChildList (partnerBattleOptionUiScript.partnerSkillListContainer);
    selectedOptionButton = menuCommonFunctions.GetSelectedSelectableSkillButton (partnerSkillList);
    if (battleMenuState == BattleMenuState.partnerSkillOption && selectedOptionButton != null)
    {
      battleManagerScript.CurrentSelectedOptionButton = selectedOptionButton;

      selectabelBattleTargetCharacterList = battleEnemyCharacterList;
      isSelectingFromPlayerParty = false;

      await ManageButtonListCommonProcess (
        BattleMenuState.partnerSkillOption,
        partnerBattleOptionUiScript.partnerSkillListContainer
      );
      ShowFadeCommandDescription ();
    }
    await ManageCommonListOption ();
  }

  // 下記player,partner共通分岐処理
  // アイテムかさくせんのどちらかの選択肢リストの中から行動を選択した処理
  async UniTask ManageCommonListOption ()
  {
    // 下記共通
    // アイテム選択
    if (battleMenuState == BattleMenuState.itemOption)
    {
      selectedOptionButton = menuCommonFunctions.GetSelectedItemButton (itemOptionList);
      if (selectedOptionButton == null) return;
      battleManagerScript.CurrentSelectedOptionButton = selectedOptionButton;
      BelongingButtonInfoContainer selectedItemButtonInfoContainer = selectedOptionButton.GetComponent<BelongingButtonInfoContainer> ();
      // アイテムのタイプに合わせてManageButtonListCommonProcessの最後の引数変更
      if (selectedItemButtonInfoContainer.Type == BelongingButtonInfoContainer.State.recoverHp.ToString () ||
        selectedItemButtonInfoContainer.Type == BelongingButtonInfoContainer.State.recoverFp.ToString ())
      {
        selectabelBattleTargetCharacterList = GetSelectablePlayerPartyList (selectedItemButtonInfoContainer.Type);
        isSelectingFromPlayerParty = true;

        await ManageButtonListCommonProcess (
          BattleMenuState.itemOption,
          battleCommonOptionUiScript.itemOptionListContainer,
          true
        );
      }
      // こうげきアイテムは一旦原作通りに敵全選択でつくる
      else if (selectedItemButtonInfoContainer.Type == BelongingButtonInfoContainer.State.makeEnemySleep.ToString () ||
        selectedItemButtonInfoContainer.Type == BelongingButtonInfoContainer.State.threat.ToString () ||
        selectedItemButtonInfoContainer.Type == BelongingButtonInfoContainer.State.flame.ToString ())
      {
        battleMenuState = BattleMenuState.allTargetOption;
        previousBattleMenuState = BattleMenuState.itemOption;
        isSelectingFromPlayerParty = false;

        InactivateAllListButtonsInteractable ();
        currentSelectOptionBalloon.SetActive (false);
        await HideList (battleCommonOptionUiScript.itemOptionListContainer, true);
        ShowAllTargetsSelectingInformation ();
      }
    }
    // さくせん選択
    else if (battleMenuState == BattleMenuState.strategyOption)
    {
      // 仲間チェンジ、ガード、逃げるによって異なる
      selectedOptionButton = menuCommonFunctions.GetSelectedStrategyButton (strategyOptionList);
      if (selectedOptionButton == null) return;
      battleManagerScript.CurrentSelectedOptionButton = selectedOptionButton;
      // なかまチェンジは一旦作らないので、ストラテジーオプション選択後はactionTimeに移る
      battleMenuState = BattleMenuState.actionTime;
      InactivateAllListButtonsInteractable ();
      currentSelectOptionBalloon.SetActive (false);
      await HideList (battleCommonOptionUiScript.strategyOptionListContainer);
    }
  }
  async UniTask ManageButtonListCommonProcess (BattleMenuState prevState, GameObject listContainer, bool isItemList = false)
  {
    battleMenuState = BattleMenuState.targetOption;
    previousBattleMenuState = prevState;
    InactivateAllListButtonsInteractable ();
    // 吹き出し、ジャンプリスト、リストハンドカーソルをSetActive(false)で非表示
    currentSelectOptionBalloon.SetActive (false);
    await HideList (listContainer, isItemList);
    // 敵の中から対象の選択をするよう指定(バトルマネージャー側からリストを更新しても対応できる)
    // Debug.Log ("SetupCursorPositionListsがよばれてるよ");
    targetOptionCursorManager.SetupCursorPositionLists (selectabelBattleTargetCharacterList, battleEnemyCharacterList, isSelectingFromPlayerParty);
    // 敵選択情報表示
    ShowTargetSelectingTargetInformation ();
  }

  // アイテム使用可能な対象を絞り込む
  List<GameObject> GetSelectablePlayerPartyList (string itemType)
  {
    List<GameObject> selectablePlayerPartyList = new List<GameObject> ();
    if (itemType == BelongingButtonInfoContainer.State.recoverHp.ToString ())
    {
      foreach (var playerMember in battlePlayersCharacterList)
      {
        if (playerMember.GetComponent<BattlePlayer> () != null)
        {
          BattlePlayer battlePlayerInfo = playerMember.GetComponent<BattlePlayer> ();
          if (battlePlayerInfo.Hp < battlePlayerInfo.MaxHp)
          {
            selectablePlayerPartyList.Add (playerMember);
          }
        }
        else if (playerMember.GetComponent<BattleSakura> () != null)
        {
          if (playerMember.GetComponent<BattleSakura> () != null)
          {
            // 敵のクラスのように共通のなかまクラスコンポーネントを持たせてそれを読み込ませるように改修する
            BattlePartnerStatus battlePartnerInfo = playerMember.GetComponent<BattlePartnerStatus> ();
            if (battlePartnerInfo.Hp < battlePartnerInfo.MaxHp)
            {
              selectablePlayerPartyList.Add (playerMember);
            }
          }
        }
      }
    }
    else if (itemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      foreach (var playerMember in battlePlayersCharacterList)
      {
        if (playerMember.GetComponent<BattlePlayer> () != null)
        {
          BattlePlayer battlePlayerInfo = playerMember.GetComponent<BattlePlayer> ();
          if (battlePlayerInfo.Fp < battlePlayerInfo.MaxFp)
          {
            selectablePlayerPartyList.Add (playerMember);
          }
        }
      }
    }
    return selectablePlayerPartyList;
  }

  // ManagePlayerFirstOptionと同様、ターゲットの選択はボタンコンポーネントを用いていないのでplayerBattleOptionManagerに呼び出し方法を任せている
  // 攻撃、回復対象選択後の処理
  public void ManageTargetOption ()
  {
    // 表示が終わる前に非表示の処理を動かせてしまう時があったので条件式追加
    bool isTargetOptionShown = battleCommonOptionUiScript.selectedEnemyNameFrame.activeSelf;
    if (battleMenuState == BattleMenuState.targetOption && isTargetOptionShown)
    {
      SEManager.Instance.Play (SEPath.MENU_DECISION);
      battleMenuState = BattleMenuState.actionTime;
      selectedTarget = selectabelBattleTargetCharacterList[selectedTargetNum];
      HideTargetSelectingTargetInformation ();
    }
  }

  // 敵の全選択中に決定した際の処理
  public void ManageAllTargetsOption ()
  {
    bool isAllTargetsOptionShown = battleCommonOptionUiScript.battleCommonOptionMultipleTarget.activeSelf;
    if (battleMenuState == BattleMenuState.allTargetOption && isAllTargetsOptionShown)
    {
      SEManager.Instance.Play (SEPath.MENU_DECISION);
      battleMenuState = BattleMenuState.actionTime;
      HideAllTargetsSelectingInformation ();
    }
  }

  // リストから最初の選択に戻る際の処理
  public async UniTask ManageBackToFirstPlayerOption ()
  {
    bool endBalloonRotating = EndBalloonRotation ();
    if (endBalloonRotating && battleMenuState != BattleMenuState.firstOption && battleMenuState != BattleMenuState.waitingTime)
    {
      SEManager.Instance.Play (SEPath.MENU_BACK);
      if (battleMenuState == BattleMenuState.jumpOption)
      {
        await BackToFirstPlayerOption (playerBattleOptionUiScript.playerJumpSkillListContainer);
      }
      else if (battleMenuState == BattleMenuState.hammerOption)
      {
        await BackToFirstPlayerOption (playerBattleOptionUiScript.playerHammerSkillListContainer);
      }
      else if (battleMenuState == BattleMenuState.itemOption)
      {
        await BackToFirstPlayerOption (battleCommonOptionUiScript.itemOptionListContainer, true);
      }
      else if (battleMenuState == BattleMenuState.strategyOption)
      {
        await BackToFirstPlayerOption (battleCommonOptionUiScript.strategyOptionListContainer);
      }
      else if (battleMenuState == BattleMenuState.partnerSkillOption)
      {
        await BackToFirstPlayerOption (partnerBattleOptionUiScript.partnerSkillListContainer);
      }
    }
  }

  async UniTask BackToFirstPlayerOption (GameObject listContainer, bool isItemList = false)
  {
    battleMenuState = BattleMenuState.firstOption;
    // ボタン無効化
    InactivateAllListButtonsInteractable ();
    // リストの非表示
    HideList (listContainer, isItemList);
    // 吹き出しの移動
    battleOptionTweenManager.MoveOptionBalloonToFirstPosition ();
    // ふきだしの回転開始
    optionBalloonAnimator.SetBool ("isRotating", true);
    await battleOptionTweenManager.ExpandFirstOptions ();
    // ふきだしの回転終了
    optionBalloonAnimator.SetBool ("isRotating", false);
  }

  // 敵選択からリスト選択に戻る際の処理
  public void ManageBackToListOption ()
  {
    bool endHideTween = battleOptionTweenManager.EndHideListTween;
    if (battleMenuState == BattleMenuState.targetOption && endHideTween || battleMenuState == BattleMenuState.allTargetOption && endHideTween)
    {
      SEManager.Instance.Play (SEPath.MENU_BACK);
      if (previousBattleMenuState == BattleMenuState.jumpOption)
      {
        battleMenuState = BattleMenuState.jumpOption;
        BackToListOption (playerBattleOptionUiScript.playerJumpSkillListContainer);
      }
      else if (previousBattleMenuState == BattleMenuState.hammerOption)
      {
        battleMenuState = BattleMenuState.hammerOption;
        BackToListOption (playerBattleOptionUiScript.playerHammerSkillListContainer);
      }
      else if (previousBattleMenuState == BattleMenuState.itemOption)
      {
        battleMenuState = BattleMenuState.itemOption;
        BackToListOption (battleCommonOptionUiScript.itemOptionListContainer, true);
      }
      else if (previousBattleMenuState == BattleMenuState.strategyOption)
      {
        battleMenuState = BattleMenuState.strategyOption;
        // BackToListOption ();
      }
      else if (previousBattleMenuState == BattleMenuState.partnerSkillOption)
      {
        battleMenuState = BattleMenuState.partnerSkillOption;
        BackToListOption (partnerBattleOptionUiScript.partnerSkillListContainer);
      }
    }
  }

  void BackToListOption (GameObject skillLIstContainer, bool isItemList = false)
  {
    // 吹き出し再表示
    currentSelectOptionBalloon.SetActive (true);
    ShowList (skillLIstContainer, isItemList);
    // 敵選択UI非表示
    HideTargetSelectingTargetInformation ();
    HideCommandDescription ();
    // 選択中の対象番号のリセット
    ResetSelectedTargetNum ();
  }

  public void ResetSelectedTargetNum ()
  {
    targetOptionCursorManager.ResetSelectedTargetNum ();
  }

  async UniTask ShowList (GameObject listContainer, bool isItemList = false)
  {
    await battleOptionTweenManager.ShowListButtons (listContainer, isItemList);
    wantInteractableValueBeTrue = true;
    battleCommonOptionUiScript.optionButtonDescriptionFrame.SetActive (true);
  }

  async UniTask HideList (GameObject listContainer, bool isItemList = false)
  {
    battleCommonOptionUiScript.listHandCursor.SetActive (false);
    battleCommonOptionUiScript.optionButtonDescriptionFrame.SetActive (false);
    await battleOptionTweenManager.HideListButtons (listContainer, isItemList);
  }

  void ShowTargetSelectingTargetInformation ()
  {
    // 表示
    battleCommonOptionUiScript.selectedEnemyNameFrame.SetActive (true);
    battleCommonOptionUiScript.targetHandCursor.SetActive (true);
    // ハンドカーソルを初期位置に移動
    targetOptionCursorManager.ManageTargetHandCurSorPosition ();
  }

  void HideTargetSelectingTargetInformation ()
  {
    battleCommonOptionUiScript.selectedEnemyNameFrame.SetActive (false);
    battleCommonOptionUiScript.targetHandCursor.SetActive (false);
  }

  void ShowAllTargetsSelectingInformation ()
  {
    battleCommonOptionUiScript.battleCommonOptionMultipleTarget.SetActive (true);
  }

  // 中身が動的に必要な時のみ変わるのでBattleOptionManagerからも呼び出し 
  public void HideAllTargetsSelectingInformation ()
  {
    battleCommonOptionUiScript.battleCommonOptionMultipleTarget.SetActive (false);
  }

  public void ShowCommandDescription (float alpha = 1.0f)
  {
    battleCommonOptionUiScript.attackDescriptionFrame.GetComponent<CanvasGroup> ().alpha = alpha;
    battleCommonOptionUiScript.attackDescriptionFrame.SetActive (true);
  }

  void ShowFadeCommandDescription ()
  {
    float littleFade = 0.6f;
    ShowCommandDescription (littleFade);
  }

  public void HideCommandDescription ()
  {
    battleCommonOptionUiScript.attackDescriptionFrame.SetActive (false);
  }

  public bool EndBalloonRotation ()
  {
    // isBalloonRotatingの条件だけだと、回転がanimatorで止まっていても0度になっていないときがある
    bool isBalloonRotating = optionBalloonAnimator.GetBool ("isRotating");
    bool endRotation = false;
    if (!isBalloonRotating &&
      currentSelectOptionBalloon.transform.localEulerAngles.y == 0)
    {
      endRotation = true;
    }
    return endRotation;
  }

  public int SelectedOptionNum
  {
    get { return selectedOptionNum; }
  }
}