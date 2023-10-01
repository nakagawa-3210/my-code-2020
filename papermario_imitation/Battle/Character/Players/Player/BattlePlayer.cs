using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

// playerは敵や仲間と違い一種類しかいないので、
// Statusファイルを用いないで体力等のステータスを持つ 
public class BattlePlayer : BaseBattleCharacter
{
  private GameObject currentTarget;
  private GameObject self;
  private List<GameObject> playerCharacterListForAll;
  private List<GameObject> enemyCharacterListForAll;
  private PlayerJumpRoute playerJumpRoute;
  private HammerRoute playerHammerRoute;
  private RecoveryMotion playerRecoveryMotion;
  private FireFlowerMotion playerFireFlowerMotion;
  private DefenceMotion playerDefenceMotion;
  private EscapeMotion playerEscapeMotion;
  private AttackedDetector targetAttackedDetector;

  private ManagersForPlayerParty managersForPlayerParty;
  private BattleOptionMotionManager playerBattleOptionMotionManager;
  private SkillState skillState;
  private int playerLevel;
  private int playerExperience;
  private bool startAction;
  private bool waitingAction;
  private bool startCountdown;
  private bool choseMotion;
  private bool endLastJump;
  private bool endPlayerTurn;
  public int PlayerLevel
  {
    get { return playerLevel; }
  }
  public int PlayerExperience
  {
    get { return playerExperience; }
  }

  // リセット関数でbool値を管理できるように変更する予定
  public bool EndPlayerTurn
  {
    set { endPlayerTurn = value; }
    get { return endPlayerTurn; }
  }
  public GameObject CurrentTarget
  {
    get { return currentTarget; }
  }
  public SkillState PlayerSkillState
  {
    get { return skillState; }
  }
  public enum SkillState
  {
    idle,
    jump,
    hammer,
    item,
    strategy,
    escape
  }
  void Start ()
  {
    self = this.gameObject;
    SetupMotions ();
    SetupStatus ();
    ResetBools ();
  }

  async UniTask Update ()
  {
    // 連続2回まで踏めるジャンプ
    if (skillState == SkillState.jump)
    {
      ManageJumpActions ();
    }
    else if (skillState == SkillState.hammer)
    {
      ManageHammerActions ();
    }
  }

  void SetupMotions ()
  {
    playerJumpRoute = self.GetComponent<PlayerJumpRoute> ();
    playerHammerRoute = self.GetComponent<HammerRoute> ();
    playerRecoveryMotion = self.GetComponent<RecoveryMotion> ();
    playerFireFlowerMotion = self.GetComponent<FireFlowerMotion> ();
    playerDefenceMotion = self.GetComponent<DefenceMotion> ();
    playerEscapeMotion = self.GetComponent<EscapeMotion> ();
  }

  void SetupStatus ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    Hp = saveData.playerCurrentHp;
    MaxHp = saveData.playerMaxHp;
    Fp = saveData.playerCurrentFp;
    MaxFp = saveData.playerMaxFp;
    playerLevel = saveData.playerLevel;
    playerExperience = saveData.experience;

    skillState = SkillState.idle;

    // デフォルトのATとDFを設定する処理を出来たら作成したい
    DefaultAt = 0;
    DefaultDf = 0;
  }

  void ResetBools ()
  {
    startAction = false;
    waitingAction = false;
    startCountdown = false;
    choseMotion = false;
    endLastJump = false;
    endPlayerTurn = true;
    // Debug.Log ("おわったよ endPlayerTurn : " + endPlayerTurn);
  }

  // ジャンプとハンマーで共通の改修あり
  // await UniTask.WaitUntil (() => ); を用いてUpdateで関数を呼ばないように改修予定
  async UniTask ManageJumpActions ()
  {
    if (startAction)
    {
      startAction = false;
      // ジャンプ開始アニメーション再生
      playerJumpRoute.StartJump ();
      managersForPlayerParty.actionUiMotionManager.ShowJumpActionSign ();
      // 自分のターンが来た時に隠すだけで、表示するのは次のパートナーのターンが来た時
      managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
    }
    bool enteredActionCollider = targetAttackedDetector.GetEnteredActionCollider ();
    if (enteredActionCollider && !waitingAction)
    {
      waitingAction = true;
      managersForPlayerParty.actionUiMotionManager.ShowPressedJumpActionSign ();
      await playerJumpRoute.WaitingJump ();
      managersForPlayerParty.actionUiMotionManager.ShowNormalJumpActionSign ();
    }
    bool canChooseAction = playerJumpRoute.CanChooseAction;
    // Debug.Log ("canChooseAction : " + canChooseAction);
    if (!choseMotion && canChooseAction)
    {
      choseMotion = true;
      playerBattleOptionMotionManager.HideCommandDescription ();
      managersForPlayerParty.battleEnemyStatusManager.ShowTargetEnemyHpGauge (currentTarget);
      bool succeededAction = targetAttackedDetector.SucceededAction;
      if (succeededAction)
      {
        managersForPlayerParty.actionCommandJudgeManager.PlayerActionCommandEffect ();
        managersForPlayerParty.actionCommandJudgeManager.ManagePlayerJumpAttackCommand (false);
        await playerJumpRoute.JumpAgain ();
        // ジャンプ攻撃終了後にbool値変更
        choseMotion = false;
        waitingAction = false;
        targetAttackedDetector.SucceededAction = false;
      }
      else
      {
        playerJumpRoute.ApproachToTarget ();
      }
    }
    bool enteredDamageCollider = targetAttackedDetector.GetEnteredDamageCollider ();
    // Debug.Log ("enteredDamageCollider : " + enteredDamageCollider);
    if (enteredDamageCollider && !endLastJump)
    {
      endLastJump = true;
      managersForPlayerParty.actionCommandJudgeManager.ManagePlayerJumpAttackCommand (true);
      await playerJumpRoute.LastJump ();
      managersForPlayerParty.battleEnemyStatusManager.HideTargetEnemyHpGauge (currentTarget);
      managersForPlayerParty.actionUiMotionManager.HideJumpActionSign ();
      await playerJumpRoute.BackToPlayerInitialPosition ();
      // 下記のタイミングだと敵を倒した場合に都合が悪くなる
      // actionUiMotionManager.ShowWholePlayerStatus ();
    }
    if (playerJumpRoute.EndJump)
    {
      // skillState = SkillState.idle;
      // endPlayerTurn = true;　の処理は場所を変える予定
      skillState = SkillState.idle;
      playerJumpRoute.ResetBools ();
      endPlayerTurn = true;
    }
  }

  // ハンマー攻撃の基本的な移動だけはなかまと共通なのでまとめられるようにしたい
  // await UniTask.WaitUntil (() => ); を用いてUpdateで関数を呼ばないように改修予定
  async UniTask ManageHammerActions ()
  {
    if (startAction)
    {
      managersForPlayerParty.actionUiMotionManager.ShowHammerActionSign ();
      managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
      startAction = false;
      playerHammerRoute.StartHammerAction ();
    }
    bool cameToHammerPosition = playerHammerRoute.CameToHammerPosition;
    if (cameToHammerPosition && !startCountdown)
    {
      if (Input.GetKeyDown (KeyCode.Space))
      {
        startCountdown = true;
        managersForPlayerParty.actionCommandCountdownManager.StartCountdownAction ();
      }
    }
    bool hasActionCommandChance = managersForPlayerParty.actionCommandCountdownManager.HasActionCommandChance;
    if (startCountdown && !hasActionCommandChance && !choseMotion)
    {
      choseMotion = true;
      playerBattleOptionMotionManager.HideCommandDescription ();
      managersForPlayerParty.battleEnemyStatusManager.ShowTargetEnemyHpGauge (currentTarget);
      playerHammerRoute.AfterSwingDownCamera (); // カメラの引きのタイミング管理

      bool succeededAction = managersForPlayerParty.actionCommandCountdownManager.SucceededAction;
      if (succeededAction)
      {
        managersForPlayerParty.actionCommandJudgeManager.PlayerActionCommandEffect ();
      }
      managersForPlayerParty.actionCommandJudgeManager.ManagePlayerHammerAttackCommand (succeededAction);
      managersForPlayerParty.actionCommandCountdownManager.ResetCountdown ();
      int waitMillSec = 1000;
      await UniTask.Delay (waitMillSec);
      await managersForPlayerParty.actionUiMotionManager.HideHammerActionSign ();
      // 体力ゲージを隠す
      managersForPlayerParty.battleEnemyStatusManager.HideTargetEnemyHpGauge (currentTarget);
      managersForPlayerParty.actionCommandCountdownManager.InactivateHammerSign ();
      await playerHammerRoute.EndHammerAction ();
    }
    if (playerHammerRoute.EndHammer)
    {
      skillState = SkillState.idle;
      playerHammerRoute.ResetBools ();
      endPlayerTurn = true;
    }
  }

  async UniTask ManageUsingRecoveringItemMotion (BelongingButtonInfoContainer itemInformation)
  {
    await playerRecoveryMotion.ShowUsingItem (currentTarget, itemInformation);
    skillState = SkillState.idle;
    endPlayerTurn = true;
  }

  // アイテム攻撃 
  // 仲間のアイテム攻撃と共通処理をまとめられるように改修予定
  async UniTask ManageUsingAttackItemMotion (BelongingButtonInfoContainer itemInformation)
  {
    int damageAmount = itemInformation.Amount;
    await playerFireFlowerMotion.ShowUsingItem (itemInformation);

    // すぐにダメージ量表示だと違和感があるので調整
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

    await playerFireFlowerMotion.StopFire ();
    skillState = SkillState.idle;
    endPlayerTurn = true;
  }

  async UniTask ManageDefenceMotion (int upAmount)
  {
    // 違和感が出ないように少し間をおいて防御モーションを再生する
    int makeInterval = 100;
    await UniTask.Delay (makeInterval);

    // ディフェンスモーション再生
    await playerDefenceMotion.StartNormalDefenceMotion ();

    // 効果説明表示
    await managersForPlayerParty.battleInformationManager.ShowDefenceUpInformation (upAmount);
    // 防御アップ
    Df += upAmount;

    //効果非表示 
    managersForPlayerParty.battleInformationManager.HideBattleInformationFrame ();

    skillState = SkillState.idle;
    endPlayerTurn = true;
  }

  public async UniTask ResetShield ()
  {
    bool isUsingDefence = playerDefenceMotion.IsUsingDefence;
    if (isUsingDefence)
    {
      Df = DefaultDf;
      await playerDefenceMotion.HideDefenceShield ();
      await managersForPlayerParty.battleInformationManager.ShowDefenceResetInformation ();

      managersForPlayerParty.battleInformationManager.HideBattleInformationFrame ();
    }
  }

  async UniTask ManageEscapeMotion ()
  {
    // カメラがプレイヤーに寄る
    playerEscapeMotion.StartEscape ();
    // カメラの寄り終えたタイミングで連打ゲー開始
    managersForPlayerParty.actionUiMotionManager.HideWholePlayerStatus ();
    managersForPlayerParty.actionUiMotionManager.ShowEscapeActionSign ();
    managersForPlayerParty.actionCommandEscapeManager.StartEscapeChance ();

    await UniTask.WaitUntil (() => managersForPlayerParty.actionCommandEscapeManager.EndCountDown);
    playerEscapeMotion.KillPartyMemberMoveTween ();
    bool canEscape = managersForPlayerParty.actionCommandEscapeManager.CanEscape;
    if (canEscape)
    {
      // 逃げる成功モーション再生
      await playerEscapeMotion.SucceededEscape ();
      // Debug.Log ("にげれた！");
    }
    else
    {
      // 逃げる失敗モーション再生
      await playerEscapeMotion.FailedEscape ();
      // Debug.Log ("にげれなかった！");
      skillState = SkillState.idle;
    }
    managersForPlayerParty.actionUiMotionManager.HideEscapeActionSign ();
    endPlayerTurn = true;
    // Debug.Log ("にげる連打おわり");
  }

  // UIGameObjectにUGUIをまとめたように必要なマネージャーをまとめてFindで呼び、transform.Findで代入にするかも
  public async UniTask ChooseAttackOptions (
    GameObject target,
    GameObject selectedOption,
    List<GameObject> playerCharacterListForAll,
    List<GameObject> enemyCharacterListForAll,
    ManagersForPlayerParty managersForPlayerParty,
    BattleOptionMotionManager playerBattleOptionMotionManager
  )
  {
    // Managerをセット
    this.managersForPlayerParty = managersForPlayerParty;
    // アクションコマンドチャンスが無くなった時にコマンド説明を非表示にするのに必要
    this.playerBattleOptionMotionManager = playerBattleOptionMotionManager;

    if (skillState == SkillState.idle)
    {
      currentTarget = target;
      this.playerCharacterListForAll = playerCharacterListForAll;
      this.enemyCharacterListForAll = enemyCharacterListForAll;
      endPlayerTurn = false;

      // スキルボタンを選択した際の行動
      if (selectedOption.GetComponent<SkillButtonInformationContainer> () != null)
      {
        playerBattleOptionMotionManager.ShowCommandDescription ();
        SkillButtonInformationContainer skillInformation = selectedOption.GetComponent<SkillButtonInformationContainer> ();
        if (target.GetComponent<BattleEnemyStatus> () != null)
        {
          if (skillInformation.SkillType == SkillButtonInformationContainer.State.jump.ToString ())
          {
            skillState = SkillState.jump;
            playerJumpRoute.SetupJumpRoute (currentTarget, managersForPlayerParty.battleVirtualCameras);
            targetAttackedDetector = target.GetComponent<AttackedDetector> ();
            targetAttackedDetector.HasActionChance = true;
            managersForPlayerParty.actionCommandJudgeManager.SetupTrampledEnemyMemberDetector (currentTarget, self, skillInformation);
            startAction = true;
            waitingAction = false;
            choseMotion = false;
            endLastJump = false;
          }
          else if (skillInformation.SkillType == SkillButtonInformationContainer.State.hammer.ToString ())
          {
            skillState = SkillState.hammer;
            managersForPlayerParty.actionCommandJudgeManager.SetupBeatenEnemyMemberDetector (currentTarget, self, skillInformation);
            playerHammerRoute.SetupHammerRoute (currentTarget, managersForPlayerParty.battleVirtualCameras);
            startAction = true;
            startCountdown = false;
            choseMotion = false;
          }
        }
      }

      // アイテムボタンを選択した際の行動
      else if (selectedOption.GetComponent<BelongingButtonInfoContainer> () != null)
      {
        BelongingButtonInfoContainer itemInformation = selectedOption.GetComponent<BelongingButtonInfoContainer> ();
        skillState = SkillState.item;
        // 敵全体効果アイテム
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
            // Debug.Log ("プレイヤー自身の回復処理");
            await ManageUsingRecoveringItemMotion (itemInformation);
            // 体力等回復のデータ書き換えをおこなう 
            if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverHp.ToString ())
            {
              Hp += itemInformation.Amount;
              if (Hp > MaxHp)
              {
                Hp = MaxHp;
              }
            }
            else if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverFp.ToString ())
            {
              Fp += itemInformation.Amount;
              if (Fp > MaxFp)
              {
                Fp = MaxFp;
              }
            }
            managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
          }
          else if (target.GetComponent<BattlePartnerStatus> () != null)
          {
            // Debug.Log ("なかまの回復処理");
            BattlePartnerStatus battlePartnerStatus = target.GetComponent<BattlePartnerStatus> ();
            await ManageUsingRecoveringItemMotion (itemInformation);
            if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverHp.ToString ())
            {
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

      // さくせんボタンを選択した際の行動
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
          skillState = SkillState.escape;
          playerEscapeMotion.SetupEscapeRoute (playerCharacterListForAll, managersForPlayerParty.battleVirtualCameras);
          await ManageEscapeMotion ();
          managersForPlayerParty.actionCommandJudgeManager.StartCalculating = true;
        }
      }
    }
  }
}