using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class BattleSakura : MonoBehaviour
{
  private GameObject currentTarget;
  private GameObject self;
  private List<GameObject> playerCharacterListForAll;
  private List<GameObject> enemyCharacterListForAll;
  private ReactionMotion sakuraReaction;
  private HammerRoute sakuraHammerRoute;
  private DictionaryMotion sakuraDictionaryMotion;
  private RecoveryMotion sakuraRecoveryMotion;
  private FireFlowerMotion sakuraFireFlowerMotion;
  private DefenceMotion sakuraDefenceMotion;
  private EscapeMotion sakuraEscapeMotion;
  private BattlePartnerStatus battlePartnerStatus;

  private ManagersForPlayerParty managersForPlayerParty;
  private BattleOptionMotionManager partnerBattleOptionMotionManager;

  private SkillState skillState;
  private bool startAction;
  private bool startCountdown;
  private bool choseMotion;
  private bool endSakuraTurn;
  public bool EndSakuraTurn
  {
    set { endSakuraTurn = value; }
    get { return endSakuraTurn; }
  }
  public SkillState SakuraState
  {
    get { return skillState; }
  }
  public enum SkillState
  {
    idle,
    search,
    hammer,
    item,
    strategy,
    escape
  }
  void Start ()
  {
    self = this.gameObject;
    SetupMotions ();
    SetupSakuraStatus ();
    ResetBools ();
  }

  // ハンマー攻撃はタックルやジャンプと違って、
  // アクションコマンドの成功はカウントダウンを監視するクラスに判定をしてもらう
  void Update ()
  {
    if (skillState == SkillState.hammer)
    {
      ManageHammerActions ();
    }
  }

  void SetupMotions ()
  {
    sakuraReaction = new ReactionMotion (self);
    sakuraHammerRoute = self.GetComponent<HammerRoute> ();
    sakuraDictionaryMotion = self.GetComponent<DictionaryMotion> ();
    sakuraRecoveryMotion = self.GetComponent<RecoveryMotion> ();
    sakuraFireFlowerMotion = self.GetComponent<FireFlowerMotion> ();
    sakuraDefenceMotion = self.GetComponent<DefenceMotion> ();
    sakuraEscapeMotion = self.GetComponent<EscapeMotion> ();
  }

  void SetupSakuraStatus ()
  {
    MyGameData.MyData myData = SaveSystem.Instance.userData;
    string sakuraName = myData.currentSelectedPartnerName;
    int sakuraLevel = myData.sakuraLevel;
    battlePartnerStatus = self.GetComponent<BattlePartnerStatus> ();
    battlePartnerStatus.SetupPartnerStatus (sakuraName, sakuraLevel);

    skillState = SkillState.idle;
  }
  void ResetBools ()
  {
    startAction = false;
    startCountdown = false;
    choseMotion = false;
    endSakuraTurn = true;
  }

  // UniTaskUntilを用いてupdateでよばないようにする改修予定
  async UniTask ManageHammerActions ()
  {
    // ハンマー攻撃
    if (startAction)
    {
      managersForPlayerParty.actionUiMotionManager.ShowHammerActionSign ();
      managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
      startAction = false;
      sakuraHammerRoute.StartHammerAction ();
    }
    // カウントダウン開始
    bool cameToHammerPosition = sakuraHammerRoute.CameToHammerPosition;
    if (cameToHammerPosition && !startCountdown)
    {
      // ボタンを押し続けるよう合図を出す
      if (Input.GetKeyDown (KeyCode.Space)) // 押すボタンはハンマーアクションの離すボタンと合わせる
      {
        startCountdown = true;
        managersForPlayerParty.actionCommandCountdownManager.StartCountdownAction ();
      }
    }
    bool hasActionCommandChance = managersForPlayerParty.actionCommandCountdownManager.HasActionCommandChance;
    if (startCountdown && !hasActionCommandChance && !choseMotion)
    {
      choseMotion = true;
      partnerBattleOptionMotionManager.HideCommandDescription ();
      managersForPlayerParty.battleEnemyStatusManager.ShowTargetEnemyHpGauge (currentTarget);
      sakuraHammerRoute.AfterSwingDownCamera (); // カメラの引きのタイミング管理

      bool succeededAction = managersForPlayerParty.actionCommandCountdownManager.SucceededAction;
      if (succeededAction)
      {
        managersForPlayerParty.actionCommandJudgeManager.PlayerActionCommandEffect ();
      };
      managersForPlayerParty.actionCommandJudgeManager.ManagePlayerHammerAttackCommand (succeededAction);
      managersForPlayerParty.actionCommandCountdownManager.ResetCountdown ();
      int waitMillSec = 1000;
      await UniTask.Delay (waitMillSec);
      // アクションコマンドを隠す
      await managersForPlayerParty.actionUiMotionManager.HideHammerActionSign ();
      // 体力ゲージを隠す
      managersForPlayerParty.battleEnemyStatusManager.HideTargetEnemyHpGauge (currentTarget);
      managersForPlayerParty.actionCommandCountdownManager.InactivateHammerSign ();
      await sakuraHammerRoute.EndHammerAction ();
    }
    if (sakuraHammerRoute.EndHammer)
    {
      skillState = SkillState.idle;
      sakuraHammerRoute.ResetBools ();
      endSakuraTurn = true;
    }
  }

  async UniTask ManageUsingRecoveringItemMotion (BelongingButtonInfoContainer itemInformation)
  {
    await sakuraRecoveryMotion.ShowUsingItem (currentTarget, itemInformation);
    skillState = SkillState.idle;
    endSakuraTurn = true;
  }

  async UniTask ManageUsingAttackItemMotion (BelongingButtonInfoContainer itemInformation)
  {
    int damageAmount = itemInformation.Amount;
    await sakuraFireFlowerMotion.ShowUsingItem (itemInformation);
    int getDamageTime = 1000;
    await UniTask.Delay (getDamageTime);

    // ダメージ量表示
    foreach (var enemy in enemyCharacterListForAll)
    {
      managersForPlayerParty.battleEnemyStatusManager.ShowTargetEnemyHpGauge (enemy);
      managersForPlayerParty.itemActionManager.DamageAmountEffect (enemy, damageAmount);
      int waitForNextEnemyAttack = 700;
      await UniTask.Delay (waitForNextEnemyAttack);
      managersForPlayerParty.battleEnemyStatusManager.HideTargetEnemyHpGauge (enemy);
    }

    await sakuraFireFlowerMotion.StopFire ();
    skillState = SkillState.idle;
    endSakuraTurn = true;
  }

  async UniTask ManageDefenceMotion (int upAmount)
  {
    // 違和感が出ないように少し間をおいて防御モーションを再生する
    int makeInterval = 100;
    await UniTask.Delay (makeInterval);

    await sakuraDefenceMotion.StartNormalDefenceMotion ();

    await managersForPlayerParty.battleInformationManager.ShowDefenceUpInformation (upAmount);

    battlePartnerStatus.Df += upAmount;

    managersForPlayerParty.battleInformationManager.HideBattleInformationFrame ();

    skillState = SkillState.idle;
    endSakuraTurn = true;
  }

  public async UniTask ResetShield ()
  {
    bool isUsingDefence = sakuraDefenceMotion.IsUsingDefence;
    if (isUsingDefence)
    {
      battlePartnerStatus.Df = battlePartnerStatus.DefaultDf;
      await sakuraDefenceMotion.HideDefenceShield ();
      await managersForPlayerParty.battleInformationManager.ShowDefenceResetInformation ();

      managersForPlayerParty.battleInformationManager.HideBattleInformationFrame ();
    }
  }

  async UniTask ManageEscapeMotion ()
  {
    sakuraEscapeMotion.StartEscape ();
    managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
    managersForPlayerParty.actionUiMotionManager.ShowEscapeActionSign ();

    managersForPlayerParty.actionCommandEscapeManager.StartEscapeChance ();
    await UniTask.WaitUntil (() => managersForPlayerParty.actionCommandEscapeManager.EndCountDown);
    sakuraEscapeMotion.KillPartyMemberMoveTween ();
    bool canEscape = managersForPlayerParty.actionCommandEscapeManager.CanEscape;
    if (canEscape)
    {
      await sakuraEscapeMotion.SucceededEscape ();
    }
    else
    {
      await sakuraEscapeMotion.FailedEscape ();
      skillState = SkillState.idle;
    }
    managersForPlayerParty.actionUiMotionManager.HideEscapeActionSign ();
    endSakuraTurn = true;
  }

  async UniTask ManageDictionaryMotion ()
  {
    managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
    await sakuraDictionaryMotion.ShowDictionary ();

    managersForPlayerParty.actionUiMotionManager.ShowDictionaryCursor (currentTarget);
    managersForPlayerParty.actionCommandDictionaryManager.StartDictionaryCursor ();

    await UniTask.WaitUntil (() => !managersForPlayerParty.actionCommandDictionaryManager.HasActionChance);

    managersForPlayerParty.actionUiMotionManager.HideDictionaryCursor ();
    partnerBattleOptionMotionManager.HideCommandDescription ();

    // 成功と失敗の分岐
    bool isSucceeded = managersForPlayerParty.actionCommandDictionaryManager.IsSucceeded;
    if (isSucceeded)
    {
      // 調べた敵としてリストに記録

      // 成功の様子を表示
      // ReactionMotion sakuraReaction = new ReactionMotion (self);
      sakuraReaction.ShowingReactionLightBulbMark ();
      await managersForPlayerParty.actionCommandJudgeManager.PlayerActionCommandEffect ();
      // 敵のイメージを辞書で表示
      BattleEnemyStatus enemyStatus = currentTarget.GetComponent<BattleEnemyStatus> ();
      string enemyImageName = enemyStatus.EnemyData.imageName;
      string enemyName = enemyStatus.EnemyData.enemyName;
      managersForPlayerParty.actionCommandDictionaryManager.SetupDictionaryContents (enemyImageName, enemyName);
      managersForPlayerParty.actionUiMotionManager.ShowDictionary ();
      // 会話再生
      string enemyDescriptionFileName = enemyStatus.EnemyData.enemyDescription;
      managersForPlayerParty.gameConversationManager.OpenConversationCanvas (enemyDescriptionFileName);
      // 会話終了を待つ
      await UniTask.WaitUntil (() => managersForPlayerParty.gameConversationManager.EndConversation);
      // 辞書非表示
      await managersForPlayerParty.actionUiMotionManager.HideDictionary ();
    }
    else
    {
      // 失敗モーション再生
      // Debug.Log ("失敗モーション再生");
      SEManager.Instance.Play (SEPath.FRUSTRATED);
      await sakuraDictionaryMotion.FailedSearch (sakuraReaction);
    }
    skillState = SkillState.idle;
    endSakuraTurn = true;
  }

  public async UniTask ChooseAttackOption (
    GameObject target,
    GameObject selectedOption,
    List<GameObject> playerCharacterListForAll,
    List<GameObject> enemyCharacterListForAll,
    ManagersForPlayerParty managersForPlayerParty,
    BattleOptionMotionManager partnerBattleOptionMotionManager
  )
  {
    this.managersForPlayerParty = managersForPlayerParty;
    this.partnerBattleOptionMotionManager = partnerBattleOptionMotionManager;
    if (skillState == SkillState.idle)
    {
      currentTarget = target;
      this.playerCharacterListForAll = playerCharacterListForAll;
      this.enemyCharacterListForAll = enemyCharacterListForAll;
      endSakuraTurn = false;

      // スキル選択
      if (selectedOption.GetComponent<SkillButtonInformationContainer> () != null)
      {
        partnerBattleOptionMotionManager.ShowCommandDescription ();
        SkillButtonInformationContainer skillInformation = selectedOption.GetComponent<SkillButtonInformationContainer> ();
        if (target.GetComponent<BattleEnemyStatus> () != null)
        {
          // わざ分岐
          if (skillInformation.SkillType == SkillButtonInformationContainer.State.hammer.ToString ())
          {
            skillState = SkillState.hammer;
            sakuraHammerRoute.SetupHammerRoute (target, managersForPlayerParty.battleVirtualCameras);
            managersForPlayerParty.actionCommandJudgeManager.SetupBeatenEnemyMemberDetector (target, self, skillInformation);
            startAction = true;
            startCountdown = false;
            choseMotion = false;
          }
          else if (skillInformation.SkillType == SkillButtonInformationContainer.State.search.ToString ())
          {
            skillState = SkillState.search;
            managersForPlayerParty.actionCommandJudgeManager.SetupTargetEnemy (currentTarget);
            await ManageDictionaryMotion ();
            managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
          }
        }
      }

      // アイテムボタン選択
      else if (selectedOption.GetComponent<BelongingButtonInfoContainer> () != null)
      {
        BelongingButtonInfoContainer itemInformation = selectedOption.GetComponent<BelongingButtonInfoContainer> ();
        skillState = SkillState.item;
        // 全体効果アイテム
        if (itemInformation.Type == BelongingButtonInfoContainer.State.flame.ToString ())
        {
          // Debug.Log ("全体ほのお攻撃アイテム使用開始！");
          await ManageUsingAttackItemMotion (itemInformation);
          managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
        }
        else if (itemInformation.Type == BelongingButtonInfoContainer.State.threat.ToString ())
        {
          // Debug.Log ("全体おどし攻撃アイテム使用開始！");
        }
        else if (itemInformation.Type == BelongingButtonInfoContainer.State.makeEnemySleep.ToString ())
        {
          // Debug.Log ("全体ねむらせ攻撃アイテム使用開始！");
        }
        // 単体効果アイテム
        else
        {
          if (target.GetComponent<BattleEnemyStatus> () != null)
          {
            // Debug.Log ("敵へのアイテムこうげき");
          }
          else if (target.GetComponent<BattlePlayer> () != null)
          {
            // プレイヤーの回復
            // Debug.Log ("プレイヤーの回復");
            BattlePlayer battlePlayerState = target.GetComponent<BattlePlayer> ();
            await ManageUsingRecoveringItemMotion (itemInformation);
            if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverHp.ToString ())
            {
              battlePlayerState.Hp += itemInformation.Amount;
              if (battlePlayerState.Hp > battlePlayerState.MaxHp)
              {
                battlePlayerState.Hp = battlePlayerState.MaxHp;
              }
            }
            else if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverFp.ToString ())
            {
              battlePlayerState.Fp += itemInformation.Amount;
              if (battlePlayerState.Fp > battlePlayerState.MaxFp)
              {
                battlePlayerState.Fp = battlePlayerState.MaxFp;
              }
            }
            managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
          }
          else if (target.GetComponent<BattlePartnerStatus> () != null)
          {
            if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverHp.ToString ())
            {
              await ManageUsingRecoveringItemMotion (itemInformation);
              battlePartnerStatus.Hp += itemInformation.Amount;
              if (battlePartnerStatus.Hp > battlePartnerStatus.MaxHp)
              {
                battlePartnerStatus.Hp = battlePartnerStatus.MaxHp;
              }
            }
            managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
          }
        }
      }

      // さくせんボタン選択
      else if (selectedOption.GetComponent<StrategyButtonInformationContainer> () != null)
      {
        StrategyButtonInformationContainer strategyOption = selectedOption.GetComponent<StrategyButtonInformationContainer> ();
        skillState = SkillState.strategy;
        if (strategyOption.Type == StrategyButtonInformationContainer.State.defenceUp.ToString ())
        {
          int upAmount = strategyOption.Amount;
          await ManageDefenceMotion (upAmount);
          managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
        }
        else if (strategyOption.Type == StrategyButtonInformationContainer.State.escape.ToString ())
        {
          // Debug.Log ("にげる開始");
          skillState = SkillState.escape;
          sakuraEscapeMotion.SetupEscapeRoute (playerCharacterListForAll, managersForPlayerParty.battleVirtualCameras);
          await ManageEscapeMotion ();
          managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
        }
      }
    }
  }
}