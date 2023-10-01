using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
  [SerializeField] ManagersForPlayerParty managersForPlayerParty = default;
  // 前シーンが探索だったときを想定したテスト
  [SerializeField] List<GameObject> testEnemyList = default;
  [SerializeField] BattlePlayerStatusManager playerStatusUiManager = default;
  // プレイヤーパーティーとエネミーパーティーのリストをBattleOptionMotionManagerに渡すために呼ぶ
  [SerializeField] BattleOptionMotionManager playerBattleOptionMotionManager = default;
  [SerializeField] BattleOptionContentsManager playerBattleOptionContentsManager = default;
  [SerializeField] BattleOptionMotionManager partnerBattleOptionMotionManager = default;
  [SerializeField] BattleOptionContentsManager partnerBattleOptionContentsManager = default;
  [SerializeField] ScreenEffectUi screenEffectUi = default;
  [SerializeField] GameObject playerPartyGroup = default;
  [SerializeField] BattlePlayer player = default;
  [SerializeField] Transform playerInitialPosition = default;
  [SerializeField] BattlePartnerStatus[] partners = default;
  [SerializeField] Transform partnerInitialPosition = default;
  [SerializeField] GameObject enemyPartyGroup = default;
  [SerializeField] Transform[] enemyInitialPositionList = default;
  private GameObject battlePlayer;
  private BattlePlayer battlePlayerStatus;
  private BattlePartnerStatus currentBattlePartnerStatus;
  private GameObject battleCurrentSelectedPartner;
  private GameObject currentSelectedOptionButton;
  private List<GameObject> battleAllCharactersList = new List<GameObject> ();
  private List<GameObject> battlePlayersCharacterList = new List<GameObject> ();
  private List<GameObject> battleEnemyCharacterList = new List<GameObject> ();
  private List<GameObject> battleEnemyCharacterListForInstantiate;
  private ActionCharacter actionCharacter;

  private ActionCommandJudgeManager actionCommandJudgeManager;
  private BattleEnemyStatusManager battleEnemyStatusManager;
  private BattleExperienceManager battleExperienceManager;
  private BattleLevelUpManager battleLevelUpManager;
  private BattleResultSaveManager battleResultSaveManager;
  private RemoveDefeatedEnemyManager removeDefeatedEnemyManager;

  // private AfterOpeningTheaterMotion afterOpeningTheaterMotion;

  private Vector3 explorationScenePlayerPosition;
  private Vector3 explorationScenePartnerPosition;
  private List<string> explorationDefeatedEnemyList;
  private List<Vector3> explorationEnemyPositionList;

  private BGMManager bgmManager;

  private string encounteredEnemyName;

  // バトル終了後に遷移するシーン名
  private string nextSceneName = "";
  private bool startBattleAction;

  // private GameObject _test;

  private enum ActionCharacter
  {
    player,
    partner,
    enemy
  }
  public GameObject CurrentSelectedOptionButton
  {
    set { currentSelectedOptionButton = value; }
    get { return currentSelectedOptionButton; }
  }

  public Vector3 ExplorationScenePlayerPosition
  {
    set { explorationScenePlayerPosition = value; }
  }
  public Vector3 ExplorationScenePartnerPosition
  {
    set { explorationScenePartnerPosition = value; }
  }
  public List<string> ExplorationDefeatedEnemyList
  {
    set { explorationDefeatedEnemyList = value; }
  }
  public List<Vector3> ExplorationEnemyPositionList
  {
    set { explorationEnemyPositionList = value; }
  }
  public List<GameObject> BattleEnemyCharacterListForInstantiating
  {
    set { battleEnemyCharacterListForInstantiate = value; }
    get { return battleEnemyCharacterListForInstantiate; }
  }
  public List<GameObject> BattleAllCharactersList
  {
    get { return battleAllCharactersList; }
  }
  public List<GameObject> BattleEnemyCharacterList
  {
    get { return battleEnemyCharacterList; }
  }

  public string EncounteredEnemyName
  {
    set { encounteredEnemyName = value; }
    get { return encounteredEnemyName; }
  }

  public string NextSceneName
  {
    set { nextSceneName = value; }
  }

  private int currentActingCharacterOrder;
  async UniTask Start ()
  {
    // BGM再生
    bgmManager = BGMManager.Instance;
    // BGMManager.Instance.Play (BGMPath.STEPPE_BATTLE);
    // 引き渡し確認
    // Debug.Log ("nextSceneName : " + nextSceneName);
    // Debug.Log ("explorationScenePlayerPosition : " + explorationScenePlayerPosition);
    // Debug.Log ("explorationScenePlayerPosition : " + explorationScenePlayerPosition);
    // Debug.Log ("explorationScenePartnerPosition : " + explorationScenePartnerPosition);
    SceneManager.sceneLoaded += OnSceneLoaded;
    startBattleAction = false;
    currentSelectedOptionButton = null;

    currentActingCharacterOrder = 0;
    // currentActingCharacterOrder = 2;

    // 共通
    actionCommandJudgeManager = managersForPlayerParty.actionCommandJudgeManager.GetComponent<ActionCommandJudgeManager> ();
    battleEnemyStatusManager = managersForPlayerParty.battleEnemyStatusManager.GetComponent<BattleEnemyStatusManager> ();
    battleExperienceManager = managersForPlayerParty.battleExperienceManager;
    battleLevelUpManager = managersForPlayerParty.battleLevelUpManager;
    battleResultSaveManager = new BattleResultSaveManager ();
    removeDefeatedEnemyManager = new RemoveDefeatedEnemyManager (this, battleExperienceManager, battleEnemyStatusManager);
    // afterOpeningTheaterMotion = new AfterOpeningTheaterMotion ();
    // Debug.Log("")
    await UniTask.WaitUntil (() => screenEffectUi.curtainManager.EndSetupCurtain);
    screenEffectUi.curtainManager.HideAllCurtains ();
    await SetupPlayerParty ();
    actionCommandJudgeManager.BattlePlayerForPartnerFpSubtraction = battlePlayersCharacterList[0].GetComponent<BattlePlayer> ();

    // 前シーンから再生するのを省くためにテストリストを用いている(コメントアウトしておけば、探索シーンからの遷移時勝手に代入される)
    // battleEnemyCharacterListForInstantiate = testEnemyList;
    // Debug.Log ("battleEnemyCharacterListForInstantiate : " + battleEnemyCharacterListForInstantiate.Count);
    // 敵設定
    SetupEnemyParty ();
    // ターン獲得キャラ指定
    actionCharacter = ActionCharacter.player;
    // リスト更新
    SetManagersBattlePlayerCharacterLists ();
    SetManagersBattleEnemyCharacterLists ();
    // アイテムコンテナ更新

    // 敵リストを敵体力Uiに渡す
    battleEnemyStatusManager.SetupEnemyHpGauge (battleEnemyCharacterList);
  }

  async UniTask SetupPlayerParty ()
  {
    battlePlayer = GameObject.Instantiate<GameObject> (player.gameObject);
    battleAllCharactersList.Add (battlePlayer);
    battlePlayersCharacterList.Add (battlePlayer);
    SetupFighterPosition (battlePlayer, playerPartyGroup.transform, playerInitialPosition);
    battlePlayerStatus = battlePlayer.GetComponent<BattlePlayer> ();
    await UniTask.WaitUntil (() => battlePlayerStatus != null);

    GameObject selectedPartner = GetSelectedPartner ();
    GameObject battlePartner = GameObject.Instantiate<GameObject> (selectedPartner);
    battleAllCharactersList.Add (battlePartner);
    battlePlayersCharacterList.Add (battlePartner);
    SetupFighterPosition (battlePartner, playerPartyGroup.transform, partnerInitialPosition);
    currentBattlePartnerStatus = battlePartner.GetComponent<BattlePartnerStatus> ();
    await UniTask.WaitUntil (() => currentBattlePartnerStatus != null);

    battleCurrentSelectedPartner = battlePartner;
  }

  void SetManagersBattlePlayerCharacterLists ()
  {
    // playerScript
    playerBattleOptionMotionManager.BattlePlayersCharacterList = battlePlayersCharacterList;
    playerBattleOptionContentsManager.BattlePlayersCharacterList = battlePlayersCharacterList;
    // partnerScript
    partnerBattleOptionMotionManager.BattlePlayersCharacterList = battlePlayersCharacterList;
    partnerBattleOptionContentsManager.BattlePlayersCharacterList = battlePlayersCharacterList;
  }
  void SetManagersBattleEnemyCharacterLists ()
  {
    // playerScript
    playerBattleOptionMotionManager.BattleEnemyCharacterList = battleEnemyCharacterList;
    playerBattleOptionContentsManager.BattleEnemyCharacterList = battleEnemyCharacterList;
    // partnerScript
    partnerBattleOptionMotionManager.BattleEnemyCharacterList = battleEnemyCharacterList;
    partnerBattleOptionContentsManager.BattleEnemyCharacterList = battleEnemyCharacterList;
  }

  GameObject GetSelectedPartner ()
  {
    string currentPartnerName = SaveSystem.Instance.userData.currentSelectedPartnerName;
    GameObject selectedPartner = null;
    if (currentPartnerName == "サクちゃん")
    {
      // 数字直打ちではない方法に改修が必要
      selectedPartner = partners[0].gameObject;
    }
    return selectedPartner;
  }

  void SetupEnemyParty ()
  {
    for (var enemyNum = 0; enemyNum < battleEnemyCharacterListForInstantiate.Count; enemyNum++)
    {
      GameObject enemy = battleEnemyCharacterListForInstantiate[enemyNum];
      Transform enemyPosition = enemyInitialPositionList[enemyNum];
      GameObject battleEnemy = GameObject.Instantiate<GameObject> (enemy);
      battleAllCharactersList.Add (battleEnemy);
      battleEnemyCharacterList.Add (battleEnemy);
      SetupFighterPosition (battleEnemy, enemyPartyGroup.transform, enemyPosition);
    }
  }

  void SetupFighterPosition (GameObject fighter, Transform fighterParent, Transform fighterInitialPosition)
  {
    fighter.transform.SetParent (fighterParent);
    fighter.transform.position = fighterInitialPosition.position;
  }

  void Update ()
  {
    // プレイヤーの体力監視
    // Debug.Log ("プレイヤーのFp監視 : " + battlePlayersCharacterList[0].GetComponent<BattlePlayer> ().Fp);

    bool endCurtainTween = screenEffectUi.curtainManager.EndTween;
    if (endCurtainTween)
    {
      if (!startBattleAction)
      {
        startBattleAction = true;
        // 敵の動きが完成するまではキャラオーダー固定
        // currentActingCharacterOrder = 2; //
        SelectBattleOptions (battleAllCharactersList[currentActingCharacterOrder]);
      }
      ManagePlayerAttack (battleAllCharactersList[currentActingCharacterOrder]);
      ManagePartnerAttack (battleAllCharactersList[currentActingCharacterOrder]);
      ManageBattleOptionResult (battleAllCharactersList[currentActingCharacterOrder]);
      ManagePlayerPartyStatusUiInformation ();
    }
  }

  // キャラクターの攻撃の選択処理
  async UniTask SelectBattleOptions (GameObject character)
  {
    await SetupItemList ();
    BattlePlayer currentBattlePlayerStatus = battlePlayer.GetComponent<BattlePlayer> ();
    BattlePartnerStatus currentBattlePartnerStatus = battleCurrentSelectedPartner.GetComponent<BattlePartnerStatus> ();
    int currentPlayerHp = currentBattlePlayerStatus.Hp;
    int currentPlayerFp = currentBattlePlayerStatus.Fp;
    int currentPartnerHp = currentBattlePartnerStatus.Hp;
    int currentPartnerMaxHp = currentBattlePartnerStatus.MaxHp;
    if (character.GetComponent<BattlePlayer> () != null) //キャラの判別に一度呼ぶだけなのでGetComponentで使う
    {
      // 防御でのDFアップ解除
      await currentBattlePlayerStatus.ResetShield ();

      // 敵の体力表示
      battleEnemyStatusManager.ShowAllEnemyGauges ();
      // コンテンツマネージャーのStartPlayerTurnも用意してスキルリストのボタンを更新するようにする
      playerBattleOptionContentsManager.StartPlayerTurn (currentPlayerHp, currentPlayerFp, currentPartnerHp, currentPartnerMaxHp);
      playerBattleOptionMotionManager.StartPlayerTurn ();
      actionCharacter = ActionCharacter.player;
    }
    else if (character.GetComponent<BattleSakura> () != null)
    {
      await character.GetComponent<BattleSakura> ().ResetShield ();

      // 敵の体力表示
      battleEnemyStatusManager.ShowAllEnemyGauges ();
      // コンテンツマネージャーのStartPlayerTurnも用意してスキルリストのボタンを更新するようにする
      partnerBattleOptionContentsManager.StartPlayerTurn (currentPlayerHp, currentPlayerFp, currentPartnerHp, currentPartnerMaxHp);
      partnerBattleOptionMotionManager.StartPlayerTurn ();
      actionCharacter = ActionCharacter.partner;
    }
    else
    {
      // 敵の体力非表示
      battleEnemyStatusManager.HideAllEnemyGauges ();
      EnemyAttack (character);
      actionCharacter = ActionCharacter.enemy;
    }
  }

  async UniTask ManagePlayerAttack (GameObject character)
  {
    if (actionCharacter == ActionCharacter.player)
    {
      if (battleEnemyCharacterList.Count == 0 || playerBattleOptionMotionManager.CurrentBattleMenuState != BattleOptionMotionManager.BattleMenuState.actionTime) return;
      // stateの変更
      playerBattleOptionMotionManager.CurrentBattleMenuState = BattleOptionMotionManager.BattleMenuState.waitingTime;

      // 敵の体力非表示
      battleEnemyStatusManager.HideAllEnemyGauges ();

      // プレイヤーの最初の選択肢位置リセット
      await playerBattleOptionMotionManager.ResetPlayerFirstOption ();

      // 選択したスキルをモーションマネージャーから受け取る
      BattlePlayer battlePlayer = character.GetComponent<BattlePlayer> ();
      GameObject selectedTarget = playerBattleOptionMotionManager.SelectedTarget;

      // スキルと敵を引数に渡し、スキルに合わせたモーションを再生
      // プレイヤーからFind関数を用いて用意する方法にいくつかの引数は変更するかもしれない
      battlePlayer.ChooseAttackOptions (
        selectedTarget,
        currentSelectedOptionButton,
        battlePlayersCharacterList,
        battleEnemyCharacterList,
        managersForPlayerParty,
        playerBattleOptionMotionManager
      );

      playerBattleOptionMotionManager.ResetSelectedTargetNum ();
    }
  }

  async UniTask ManagePartnerAttack (GameObject character)
  {
    if (actionCharacter == ActionCharacter.partner)
    {
      if (battleEnemyCharacterList.Count == 0 || partnerBattleOptionMotionManager.CurrentBattleMenuState != BattleOptionMotionManager.BattleMenuState.actionTime) return;
      // なかまで分岐(原作で仲間は七人)
      if (character.GetComponent<BattleSakura> () != null)
      {
        partnerBattleOptionMotionManager.CurrentBattleMenuState = BattleOptionMotionManager.BattleMenuState.waitingTime;

        // 敵の体力非表示
        battleEnemyStatusManager.HideAllEnemyGauges ();

        await partnerBattleOptionMotionManager.ResetPlayerFirstOption ();

        BattleSakura battleSakura = character.GetComponent<BattleSakura> ();
        GameObject selectedTarget = partnerBattleOptionMotionManager.SelectedTarget;

        battleSakura.ChooseAttackOption (
          selectedTarget,
          currentSelectedOptionButton,
          battlePlayersCharacterList,
          battleEnemyCharacterList,
          managersForPlayerParty,
          partnerBattleOptionMotionManager
        );

        partnerBattleOptionMotionManager.ResetSelectedTargetNum ();
      }
    }
  }

  // 敵の攻撃処理
  // バトルマネージャー側で、どの敵なのかを判定する
  // => battleEnemyStatusを持っているかの判断に変更して、別クラスにてどの敵なのかを判断する処理を作るように変更したい
  void EnemyAttack (GameObject character)
  {
    // まだプレイヤー達のバトルクラスを作成出来てないので一時的にreturnを用意
    // if (character.GetComponent<BattleNapakoza> () == null)
    // {
    //   startBattleAction = false;
    //   return;
    // }
    // 敵に合わせて戦術の分岐を作成していく
    if (character.GetComponent<BattleNapakoza> () != null)
    {
      BattleNapakoza battleEnemy = character.GetComponent<BattleNapakoza> ();
      battleEnemy.ChooseAttackOptions (battlePlayersCharacterList, actionCommandJudgeManager);
    }
    // 別敵の例
    else if (character.GetComponent<BattleTurtle> () != null)
    {
      // Debug.Log ("かめのばん");
      BattleTurtle battleEnemy = character.GetComponent<BattleTurtle> ();
      battleEnemy.ChooseAttackOptions (battlePlayersCharacterList, actionCommandJudgeManager);
    }
  }

  async UniTask ManageBattleOptionResult (GameObject character)
  {

    async UniTask SaveEscapingBattleResult ()
    {
      int playerHp = battlePlayerStatus.Hp;
      int playerFp = battlePlayerStatus.Fp;
      int partnerHp = currentBattlePartnerStatus.Hp;
      int acquiredExperienceNum = battleExperienceManager.AcquiredExperienceNum;
      string[] havingItemNameArray = playerBattleOptionContentsManager.GetHavingItemNameList ().ToArray ();
      await UniTask.WaitUntil (() => havingItemNameArray != null);
      battleResultSaveManager.SaveBattleResultAfterEscapingBattle (
        playerHp,
        playerFp,
        partnerHp,
        havingItemNameArray
      );
    }

    async UniTask ManageNotEscapeResult ()
    {
      await ManageDefeatedBattleEnemy (character);
      ManageNextTurn ();
    }

    if (actionCharacter == ActionCharacter.player)
    {
      // Debug.Log ("プレイヤーのターンを終わらせようとしてる");
      bool endPlayerTurn = battlePlayerStatus.EndPlayerTurn;
      bool startCalculating = actionCommandJudgeManager.StartCalculating;
      if (endPlayerTurn && startCalculating)
      {
        actionCommandJudgeManager.StartCalculating = false;
        await playerBattleOptionContentsManager.RemoveUsedItem ();
        await SetupItemList ();
        // プレイヤーのリスト選択肢のIsSelected情報のリセット
        playerBattleOptionContentsManager.ResetIsSelectedInformation ();
        battlePlayerStatus.EndPlayerTurn = false;
        if (battlePlayerStatus.PlayerSkillState == BattlePlayer.SkillState.escape)
        {
          SaveEscapingBattleResult ();
          GoToExplorationScene ();
        }
        else
        {
          ManageNotEscapeResult ();
        }
      }
    }
    else if (actionCharacter == ActionCharacter.partner)
    {
      bool startCalculating = actionCommandJudgeManager.StartCalculating;
      //  BattlePartnerからパートナーのターンが終わったかを取れるように変更する
      if (character.GetComponent<BattleSakura> () != null)
      {
        bool endSakuraTurn = character.GetComponent<BattleSakura> ().EndSakuraTurn;
        if (endSakuraTurn && startCalculating)
        {
          actionCommandJudgeManager.StartCalculating = false;
          await partnerBattleOptionContentsManager.RemoveUsedItem ();
          await SetupItemList ();
          partnerBattleOptionContentsManager.ResetIsSelectedInformation ();
          character.GetComponent<BattleSakura> ().EndSakuraTurn = false;
          if (character.GetComponent<BattleSakura> ().SakuraState == BattleSakura.SkillState.escape)
          {
            string[] havingItemNameArray = playerBattleOptionContentsManager.GetHavingItemNameList ().ToArray ();
            SaveEscapingBattleResult ();
            GoToExplorationScene ();
          }
          else
          {
            ManageNotEscapeResult ();
          }
        }
      }
    }
    else if (actionCharacter == ActionCharacter.enemy)
    {
      // ダメージの計算と敵の攻撃アニメーションが完全に終わったタイミングでstartBattleAction = false;にする
      bool startCalculating = actionCommandJudgeManager.StartCalculating;
      // 条件式に計算に用いる数字について触れさせる
      if (character.GetComponent<BattleEnemyStatus> () != null)
      {
        bool endEnemyTurn = character.GetComponent<BattleEnemyStatus> ().EndEnemyTurn;
        if (endEnemyTurn && startCalculating)
        {
          actionCommandJudgeManager.StartCalculating = false;
          character.GetComponent<BattleEnemyStatus> ().EndEnemyTurn = false;
          // 敵が自らプレイヤーのコライダーに入った情報を削除
          character.GetComponent<AttackedDetector> ().ResetEnemyAttackedInformation ();
          ManageNotEscapeResult ();
        }
      }
    }
  }

  async UniTask SetupItemList ()
  {
    await playerBattleOptionMotionManager.SetupItemList ();
    await playerBattleOptionContentsManager.SetupItemList ();
    await partnerBattleOptionMotionManager.SetupItemList ();
    await partnerBattleOptionContentsManager.SetupItemList ();
  }

  // 必要なときしか呼ばないように直す予定
  void ManagePlayerPartyStatusUiInformation ()
  {
    ManagePlayerStatusUiInformation ();
    ManagePartnerStatusUiInformation ();
  }

  void ManagePlayerStatusUiInformation ()
  {
    // 必要なときしか呼ばないように直す予定
    BattlePlayer battlePlayerScript = battlePlayer.GetComponent<BattlePlayer> ();
    playerStatusUiManager.AfterActionPlayerCurrentHpNum = battlePlayerScript.Hp;
    playerStatusUiManager.AfterActionPlayerCurrentFpNum = battlePlayerScript.Fp;
  }
  void ManagePartnerStatusUiInformation ()
  {
    foreach (var playerPartyMember in battlePlayersCharacterList)
    {
      if (playerPartyMember.GetComponent<BattlePartnerStatus> () != null)
      {
        BattlePartnerStatus battlePartnerStatus = playerPartyMember.GetComponent<BattlePartnerStatus> ();
        playerStatusUiManager.AfterActionPartnerCurrentHpNum = battlePartnerStatus.Hp;
      }
    }
  }

  async UniTask ManageDefeatedBattleEnemy (GameObject character)
  {
    if (actionCharacter == ActionCharacter.player || actionCharacter == ActionCharacter.partner)
    {
      await removeDefeatedEnemyManager.RemoveDefeatedEnemy ();
      SetManagersBattleEnemyCharacterLists ();
      if (battleEnemyCharacterList.Count > 0)
      {
        managersForPlayerParty.actionUiMotionManager.ShowWholePlayerStatus ();
      }
    }
    else if (actionCharacter == ActionCharacter.enemy)
    {
      // BattleEnemyStatus battleEnemyStatus = character.GetComponent<BattleEnemyStatus> ();
      // GameObject currentTarget = character.GetComponent<BattleEnemyStatus> ().CurrentTarget;
      // BattleEnemyStatus otherBattleEnemyStatus;
      // if (currentTarget.GetComponent<BattleEnemyStatus> () != null)
      // {
      //   // 回復、またはバフ
      //   otherBattleEnemyStatus = currentTarget.GetComponent<BattleEnemyStatus> ();
      // }
    }
  }

  async UniTask ManageNextTurn ()
  {
    if (battleEnemyCharacterList.Count > 0)
    {
      int playerHp = battlePlayerStatus.Hp;
      int partnerHp = currentBattlePartnerStatus.Hp;
      if (playerHp > 0 && partnerHp > 0)
      {
        GoToNextTurn ();
      }
      else
      {
        // プレイヤーパーティーから倒れた者を探し、プレイヤーならゲームオーバー(復活なし)
        // Debug.Log ("プレイヤーかなかまが倒れた");
        await RemovePartnerOrGoToGameOver ();
      }
    }
    else
    {
      // バトル終了
      // プレイヤーの残りHP,FP,パートナーの残りHP,獲得経験値をデータに上書き
      int playerHp = battlePlayerStatus.Hp;
      int playerFp = battlePlayerStatus.Fp;
      int partnerHp = currentBattlePartnerStatus.Hp;
      int acquiredExperienceNum = battleExperienceManager.AcquiredExperienceNum;

      // レベルアップ演出用にキャッシュ
      int totalExperience = acquiredExperienceNum + SaveSystem.Instance.userData.experience;
      // Debug.Log ("totalExperience : " + totalExperience);
      string[] havingItemNameArray = playerBattleOptionContentsManager.GetHavingItemNameList ().ToArray ();
      await UniTask.WaitUntil (() => havingItemNameArray != null);
      battleResultSaveManager.SaveBattleResultAfterWinningBattle (
        playerHp,
        playerFp,
        partnerHp,
        acquiredExperienceNum,
        havingItemNameArray
      );

      bgmManager.Stop (BGMPath.STEPPE_BATTLE);
      bgmManager.Play (BGMPath.STAGE_CLEAR, 1, 0.2f, 1, false, false);
      // battleExperienceManager.ZoomInPlayerParty (); // リファクタリング途中のためコメントアウト中
      battleExperienceManager.ShowTotalExperiencePoint ();
      // 総獲得経験値を表示
      int showTotalExperience = 1000;
      await UniTask.Delay (showTotalExperience);

      battleExperienceManager.StartDecreaseTotalExperienceNum = true;
      await UniTask.WaitUntil (() => battleExperienceManager.EndDecreasingAllTotalExperienceBalls ());

      await battleExperienceManager.ManageExperienceResultMotion ();
      // Debug.Log ("totalExperience : " + totalExperience);
      await LevelUpPerformance (totalExperience);

      // 改修予定
      if (SceneManager.GetActiveScene ().name == "TurtleBossBattleScene")
      {
        GoToGameClearScene ();
      }
      else
      {
        GoToExplorationScene ();
      }
    }
  }

  async UniTask RemovePartnerOrGoToGameOver ()
  {
    int playerHp = battlePlayersCharacterList[0].GetComponent<BattlePlayer> ().Hp;
    if (playerHp <= 0)
    {
      PlayerMemberDefeatedMotion playerDefeatedMotion = battlePlayer.GetComponent<PlayerMemberDefeatedMotion> ();
      await playerDefeatedMotion.DefeatedPlayerPartyMember ();

      // BGMManager.Instance.FadeOut (BGMPath.STEPPE_BATTLE, 1.0f, () =>
      // {
      //   BGMManager.Instance.Stop (BGMPath.STEPPE_BATTLE);
      // });
      float fadeDuration = 1.0f;
      bgmManager.FadeOut (fadeDuration);

      await screenEffectUi.curtainManager.ShowAllCurtains ();
      SceneManager.LoadScene ("GameOverScene");
    }
    if (battlePlayersCharacterList.Count > 1)
    {
      int partnerHp = battlePlayersCharacterList[1].GetComponent<BattlePartnerStatus> ().Hp;
      if (partnerHp <= 0)
      {
        GameObject defeatedPartner = battlePlayersCharacterList[1];
        PlayerMemberDefeatedMotion partnerDefeatedMotion = defeatedPartner.GetComponent<PlayerMemberDefeatedMotion> ();
        // なかまが抜けた分配列が少なくなり、GoToNextTurnで配列が合わなくなるので調整
        currentActingCharacterOrder--;
        // なかまをバトルから退場
        battleAllCharactersList.Remove (defeatedPartner);
        battlePlayersCharacterList.Remove (defeatedPartner);
        await partnerDefeatedMotion.DefeatedPlayerPartyMember ();
        partnerDefeatedMotion.ActivateWholeStageCamera ();
      }
    }
    GoToNextTurn ();
  }

  void GoToNextTurn ()
  {
    // リセット
    actionCommandJudgeManager.ResetManager ();
    // 次のキャラにターンを渡す(体力判定がつくれたらコメントアウト)
    startBattleAction = false;
    currentActingCharacterOrder++;
    // Debug.Log ("currentActingCharacterOrder : " + currentActingCharacterOrder);
    if (currentActingCharacterOrder >= battleAllCharactersList.Count)
    {
      currentActingCharacterOrder = 0;
    }
  }

  async UniTask LevelUpPerformance (int totalExperience)
  {
    if (totalExperience >= 100)
    {
      bgmManager.Stop (BGMPath.STAGE_CLEAR);
      managersForPlayerParty.actionUiMotionManager.ShowWholePlayerStatus ();
      // レベルアップの演出再生
      await battleLevelUpManager.ShowLevelUpPerformance (bgmManager);
    }
  }

  // 改修予定
  async UniTask GoToExplorationScene ()
  {
    // Debug.Log ("レベルアップ終了後のここのしょり来てる?");
    bgmManager.FadeOut (1.0f, () =>
    {
      // Debug.Log ("レベルアップ終了後音消し処理は来てる?");
      // test用
      // nextSceneName = "GameClearScene";
      // nextSceneName = "GameOverScene";
      GoToNextScene (nextSceneName);
    });
  }

  async UniTask GoToGameClearScene ()
  {
    nextSceneName = "GameClearScene";
    bgmManager.FadeOut (1.0f, () =>
    {
      GoToNextScene (nextSceneName);
    });
  }

  async UniTask GoToNextScene (string nextSceneName)
  {
    // var loadAsync = SceneManager.LoadSceneAsync (nextSceneName);
    // loadAsync.allowSceneActivation = false;
    // Debug.Log ("レベルアップ終了後のここのしょり来てる?");
    await screenEffectUi.fadeManager.FadeOut ();
    DOTween.Clear (true);

    // フェードアウト再生
    string bgmPath = new BgmPathGetterWithTransitionSceneName ().GetBgmPathWithTransitionSceneName (nextSceneName);
    bgmManager.Play (bgmPath);
    // loadAsync.allowSceneActivation = true;
    SceneManager.LoadScene (nextSceneName);
  }

  void OnSceneLoaded (Scene scene, LoadSceneMode mode)
  {
    string exceptionScene = "GameClearScene";
    if (SceneManager.GetActiveScene ().name == nextSceneName && nextSceneName != exceptionScene)
    {
      // 画面遷移時に前の探索シーンで受け取ったプレイヤーと仲間の位置を渡す
      ExploreManager exploreManager = GameObject.FindWithTag ("ExploreManager").GetComponent<ExploreManager> ();
      // プレイヤーの位置を渡す
      exploreManager.PlayerPosition = explorationScenePlayerPosition;
      exploreManager.PartnerPosition = explorationScenePartnerPosition;
      // Debug.Log ("explorationScenePlayerPosition : " + explorationScenePlayerPosition);
      // Debug.Log ("explorationScenePartnerPosition : " + explorationScenePartnerPosition);
      // 敵情報を渡す
      exploreManager.EnemyPositionList = explorationEnemyPositionList;
      exploreManager.DefeatedEnemyList = explorationDefeatedEnemyList;
      exploreManager.EncounteredEnemyName = encounteredEnemyName;
      // 探索シーンにどこから来たかを明示
      if (battleEnemyCharacterList.Count > 0)
      {
        exploreManager.CurrentExploreState = ExploreManager.ExploreState.fromEscapeTheBattle;
        PlayerPrefs.SetString ("previousSceneName", ExploreManager.ExploreState.fromEscapeTheBattle.ToString ());
      }
      else
      {
        exploreManager.CurrentExploreState = ExploreManager.ExploreState.fromWonTheBattle;
        PlayerPrefs.SetString ("previousSceneName", ExploreManager.ExploreState.fromWonTheBattle.ToString ());
      }
    }
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

}