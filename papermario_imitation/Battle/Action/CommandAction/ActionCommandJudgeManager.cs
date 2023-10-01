using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

// アクションコマンドによるダメージの管理、エフェクトの表示を担う
public class ActionCommandJudgeManager : MonoBehaviour
{
  [SerializeField] BattleEnemyStatusManager battleEnemyStatusManager;
  [SerializeField] BattleDamageEffectUiManager battleDamageEffectManager;
  private SkillButtonInformationContainer skillButtonInformationContainer;
  private AttackedDetector attackedDetector;
  private ActionCommandState actionCommandState;
  private BattlePlayer battlePlayerForPartnerFpSubtraction;
  private GameObject currentPlayer;
  private GameObject currentEnemy;
  private GameObject currentTarget;
  private int currentPlayerHp;
  private int currentEnemyHp;
  private int skillDamage;
  private int skillUsingFp;
  private bool startCalculating;

  private enum ActionCommandState
  {
    noOne,
    player,
    enemy
  }
  public BattlePlayer BattlePlayerForPartnerFpSubtraction
  {
    set { battlePlayerForPartnerFpSubtraction = value; }
  }
  public int SkillDamage
  {
    get { return skillDamage; }
  }
  public int SkillUsingFp
  {
    get { return skillUsingFp; }
  }

  // バトルマネージャーで初期化する
  // ステートも初期化したい
  public bool StartCalculating
  {
    set { startCalculating = value; }
    get { return startCalculating; }
  }

  public void ResetManager ()
  {
    startCalculating = false;
    attackedDetector = null;
    actionCommandState = ActionCommandState.noOne;
  }

  // motionManagerを作ってUIの移動、ダメージの表示、被ダメージアニメーション再生を管理

  // ---- プレイヤーステートと選択中スキルのタイプをモーションマネージャーから参照して分岐 ----
  // プレイヤーの攻撃ターンでジャンプ攻撃する時
  //    プレイヤーと"攻撃対象の"敵のコライダーを見張る
  //      プレイヤーと"攻撃対象の"敵のコライダーが衝突したタイミングで特定のボタンが押されていればもう一度踏みつけ(複数回の踏みつけの動きが可能か要確認)

  // プレイヤーの攻撃ターンでハンマー攻撃する時
  //    カウントダウンを行い、決まったタイミングで特定のボタン処理があるかを見張る

  // プレイヤー攻撃ターンで逃げる時
  //    カウントダウンを行い、時間以内に一定回数のボタン連打があるかを見張る

  // ---- 仲間ステートと選択中のスキルのタイプをモーションマネージャーから参照して分岐 ----
  // 仲間の攻撃ターンでジャンプ攻撃する時
  //    仲間と"攻撃対象の"敵のコライダーを見張る

  // ---- バトルマネージャーが再生するTweenによる敵の攻撃モーションを見張る ----
  // 敵の攻撃ターンの時
  //    敵コライダーとプレイヤーメンバーのガードコライダー、ダメージコライダーを見張る

  void Start ()
  {
    ResetManager ();
  }

  // コライダー判定を用いる場合の処理
  void Update ()
  {
    if (actionCommandState == ActionCommandState.enemy)
    {
      ManageEnemyAttackCommand ();
    }
  }

  // ジャンプアクションコマンド管理
  public void ManagePlayerJumpAttackCommand (bool isLastJump)
  {
    BattleEnemyStatus enemyStatus = currentEnemy.GetComponent<BattleEnemyStatus> ();
    Enemy enemyData = enemyStatus.EnemyData;
    BattlePlayer battlePlayer = currentPlayer.GetComponent<BattlePlayer> ();
    int jumpAt = skillButtonInformationContainer.Attack;
    int usingFp = skillButtonInformationContainer.UsingFp;
    // 敵のDFを用いた計算に改修する
    battleEnemyStatusManager.EnemyHpGaugeValueReduction (currentEnemy, jumpAt);
    enemyData.enemyHp -= jumpAt;
    if (isLastJump)
    {
      battlePlayer.Fp -= usingFp;
      startCalculating = true;
    }
    battleEnemyStatusManager.SetEnemyHpText ();
    DamageAmountEffect (jumpAt);
    PlayNormalDamageParticleEffect (currentEnemy);
  }

  // ハンマーアクションコマンド管理
  public void ManagePlayerHammerAttackCommand (bool succeededAction)
  {
    BattleEnemyStatus enemyStatus = currentEnemy.GetComponent<BattleEnemyStatus> ();
    Enemy enemyData = enemyStatus.EnemyData;
    int hammerAt = skillButtonInformationContainer.Attack;
    int usingFp = skillButtonInformationContainer.UsingFp;
    if (currentPlayer.GetComponent<BattlePlayer> () != null)
    {
      BattlePlayer battlePlayer = currentPlayer.GetComponent<BattlePlayer> ();
      // パートナーとかぶる処理は改修予定
      // ダメージ量変更
      if (succeededAction)
      {
        hammerAt += 1;
      }
      battleEnemyStatusManager.EnemyHpGaugeValueReduction (currentEnemy, hammerAt);
      enemyData.enemyHp -= hammerAt;
      battlePlayer.Fp -= usingFp;
    }
    else if (currentPlayer.GetComponent<BattleSakura> () != null)
    {
      BattleSakura battleSakura = currentPlayer.GetComponent<BattleSakura> ();
      // ダメージ量変更 処理関数化予定
      if (succeededAction)
      {
        hammerAt += 1;
      }
      battleEnemyStatusManager.EnemyHpGaugeValueReduction (currentEnemy, hammerAt);
      enemyData.enemyHp -= hammerAt;
      // プレイヤーのFpを消費する
      battlePlayerForPartnerFpSubtraction.Fp -= usingFp;
    }
    battleEnemyStatusManager.SetEnemyHpText ();
    DamageAmountEffect (hammerAt);
    PlayNormalDamageParticleEffect (currentEnemy);
    startCalculating = true;
  }

  // ガードコマンド管理
  void ManageEnemyAttackCommand ()
  {
    if (attackedDetector != null)
    {
      bool succeededAction = attackedDetector.SucceededAction;
      bool enteredDamageCollider = attackedDetector.GetEnteredDamageCollider ();
      // バトルマネージャーで計算の反映ができたらstartCalculatingをfalseにもどしてもらう
      if (!startCalculating)
      {
        // Debug.Log ("よばれてるで");
        // ダメージの計算と結果反映
        BattleEnemyStatus enemyStatus = currentEnemy.GetComponent<BattleEnemyStatus> ();
        Enemy enemyData = enemyStatus.EnemyData;
        // 攻撃される対象用意
        BattlePlayer battlePlayer;
        BattlePartnerStatus battlePartnerStatus;
        // 敵の攻撃情報
        int enemyAt = enemyData.enemyAt;
        // Debug.Log ("enemyStatus.CurrentEnemySkill.skillName : " + enemyStatus.CurrentEnemySkill.skillName);
        int skillDamage = enemyStatus.CurrentEnemySkill.amount;
        int skillFp = enemyStatus.CurrentEnemySkill.usingFp;
        void CommonDamageTask (int damage)
        {
          enemyStatus.EnemyData.enemyFp -= skillFp;
          DamageAmountEffect (damage);
          PlayNormalDamageParticleEffect (currentPlayer);
        }

        // ガード成功では受けるダメージがマイナス1
        if (succeededAction)
        {
          startCalculating = true;
          // Detectorの初期化
          attackedDetector.ResetDetector ();
          // 攻撃されるのが主人公だったとき
          if (currentPlayer.GetComponent<BattlePlayer> () != null)
          {
            battlePlayer = currentPlayer.GetComponent<BattlePlayer> ();
            int defence = battlePlayer.Df + 1;
            int damage = enemyAt + skillDamage - defence;
            // 主人公の体力減少
            battlePlayer.Hp -= damage;
            PlayerActionCommandEffect ();
            CommonDamageTask (damage);
          }
          // 仲間だったとき
          else if (currentPlayer.GetComponent<BattlePartnerStatus> () != null)
          {
            battlePartnerStatus = currentPlayer.GetComponent<BattlePartnerStatus> ();
            int defence = battlePartnerStatus.Df + 1;
            int damage = enemyAt + skillDamage - defence;
            battlePartnerStatus.Hp -= damage;
            PlayerActionCommandEffect ();
            CommonDamageTask (damage);
          }
        }
        // ガード失敗ではダメージそのまま
        else if (enteredDamageCollider)
        {
          startCalculating = true;
          attackedDetector.ResetDetector ();
          if (currentPlayer.GetComponent<BattlePlayer> () != null)
          {
            battlePlayer = currentPlayer.GetComponent<BattlePlayer> ();
            int defence = battlePlayer.Df;
            int damage = enemyAt + skillDamage - defence;
            battlePlayer.Hp -= damage;
            CommonDamageTask (damage);
          }
          else if (currentPlayer.GetComponent<BattlePartnerStatus> () != null)
          {
            battlePartnerStatus = currentPlayer.GetComponent<BattlePartnerStatus> ();
            int defence = battlePartnerStatus.Df;
            int damage = enemyAt + skillDamage - defence;
            battlePartnerStatus.Hp -= damage;
            CommonDamageTask (damage);
          }
        }
      }
    }
  }

  // 攻撃のコマンドの成功ではBattlePlayer等のクラスから呼んでもらう
  // 防御の場合はこのクラス内で呼ぶ
  public async UniTask PlayerActionCommandEffect ()
  {
    Transform actionCommandPosition = currentEnemy.transform.Find ("ActionCommandPosition");
    Transform fontsTransform = actionCommandPosition.Find ("ActionFontsEffectPosition");
    // 引数にアクションコマンドの成功数をとって、数に合わせて分岐をさせて文字を変更できる
    await battleDamageEffectManager.PlaySucceededActionEffect (fontsTransform);
  }

  // ダメージ量を星と数字で表示
  void DamageAmountEffect (int damageAmount)
  {
    new CommonActionManager ().DamageAmountEffect (
      currentTarget,
      battleDamageEffectManager,
      damageAmount
    );
  }

  // ダメージエフェクトだけどUGUIではないので一旦は攻撃対象がすぐにとれるここにて作成
  void PlayNormalDamageParticleEffect (GameObject target)
  {
    GameObject normalDamageParticle = target.transform.Find ("NormalDamageParticleSystem").gameObject;
    GameObject normalDamageDustParticle = target.transform.Find ("WhiteDustParticleSystem").gameObject;

    normalDamageParticle.GetComponent<ParticleSystem> ().Play ();
    normalDamageDustParticle.GetComponent<ParticleSystem> ().Play ();
  }

  // playerのジャンプコマンドに必要になる
  // playerが敵を踏みつけるジャンプコマンド結果の取得に必要になる
  // 敵のコライダーとの接触に合わせてタイミングよくボタンを押す
  public void SetupTrampledEnemyMemberDetector (GameObject beAttackedCharactor, GameObject attackCharactor, SkillButtonInformationContainer skillButtonInformationContainer)
  {
    currentPlayer = attackCharactor;
    currentEnemy = beAttackedCharactor;
    currentTarget = beAttackedCharactor;
    attackedDetector = beAttackedCharactor.GetComponent<AttackedDetector> ();
    this.skillButtonInformationContainer = skillButtonInformationContainer;
    actionCommandState = ActionCommandState.player;
  }

  // playerのハンマーコマンドに必要になる
  // タイミングゲーだからattackedDetectorはいらない。
  public void SetupBeatenEnemyMemberDetector (GameObject beAttackedCharactor, GameObject attackCharactor, SkillButtonInformationContainer skillButtonInformationContainer)
  {
    currentPlayer = attackCharactor;
    currentEnemy = beAttackedCharactor;
    currentTarget = beAttackedCharactor;
    this.skillButtonInformationContainer = skillButtonInformationContainer;
    actionCommandState = ActionCommandState.player;
  }

  public void SetupTargetEnemy (GameObject target)
  {
    currentEnemy = target;
  }

  // プレイヤー側のキャラが攻撃されるときにセットする
  // ガードアクションコマンド結果の取得に必要になる
  public void SetupAttackedPlayerMemberDetector (GameObject beAttackedCharactor, GameObject attackCharactor)
  {
    // 攻撃されるのが仲間であっても主人公であってもプレイヤーが操作するのでcurrentPlayerで一括りにする
    currentPlayer = beAttackedCharactor;
    currentEnemy = attackCharactor;
    currentTarget = beAttackedCharactor;
    // 攻撃されるキャラのコライダー判定監視の用意
    attackedDetector = beAttackedCharactor.GetComponent<AttackedDetector> ();
    actionCommandState = ActionCommandState.enemy;
  }
}