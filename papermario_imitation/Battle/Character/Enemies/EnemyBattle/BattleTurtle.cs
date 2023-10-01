using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class BattleTurtle : MonoBehaviour
{
  [SerializeField] string stageEnemiesFileName = default;
  [SerializeField] string enemyName = default;

  private Animator turtleAnimator;
  private ActionCommandJudgeManager actionCommandJudgeManager;
  private BattleEnemyStatus battleEnemyStatus;
  private GameObject self;
  private GameObject currentTarget;
  private AttackedDetector targetAttackedDetector;
  private EnemySkill currentEnemySkill;
  private SkillState skillState;

  private ShockWave shockWaveMotion;
  private FireBall fireBallMotion;

  private int prevSelfHp;

  public enum SkillState
  {
    idle,
    fireBall,
    hitting
  }

  void Start ()
  {
    skillState = SkillState.idle;
    self = this.gameObject;

    turtleAnimator = self.GetComponent<Animator> ();

    battleEnemyStatus = self.GetComponent<BattleEnemyStatus> ();
    battleEnemyStatus.SetupBattleEnemyStatus (stageEnemiesFileName, enemyName);

    shockWaveMotion = self.GetComponent<ShockWave> ();
    fireBallMotion = self.GetComponent<FireBall> ();

    prevSelfHp = battleEnemyStatus.EnemyData.enemyHp;
  }

  void Update ()
  {
    PlayDamageAnimation ();
  }

  async UniTask PlayDamageAnimation ()
  {
    int currentBossHp = battleEnemyStatus.EnemyData.enemyHp;
    if (prevSelfHp != currentBossHp)
    {
      prevSelfHp = currentBossHp;
      turtleAnimator.SetTrigger ("PlayDamage");
      if (currentBossHp > 0)
      {
        await UniTask.WaitUntil (() =>
          turtleAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "BattleTurtleDamage" &&
          turtleAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f
        );
        turtleAnimator.SetTrigger ("PlayIdle");
      }
      // await UniTask.WaitUntil (() =>
      //   turtleAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "BattleTurtleDamage" &&
      //   turtleAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f
      // );
      // turtleAnimator.SetTrigger ("PlayIdle");
    }
  }

  public async UniTask ChooseAttackOptions (List<GameObject> battlePlayerPartyList, ActionCommandJudgeManager actionCommandJudgeManager)
  {
    this.actionCommandJudgeManager = actionCommandJudgeManager;
    if (battlePlayerPartyList.Count == 0) return;

    BattleEnemyStatus selfStatus = self.GetComponent<BattleEnemyStatus> ();

    int attackTargetNum = (int) (Random.value * battlePlayerPartyList.Count);
    int skillNum = (int) (Random.value * battleEnemyStatus.EnemySkillList.Count);

    // 技が完成するまでは固定
    // attackTargetNum = 0;
    // skillNum = 1;

    // 技と攻撃対象をジャッジマネージャーに渡す
    currentTarget = battlePlayerPartyList[attackTargetNum];
    battleEnemyStatus.CurrentTarget = currentTarget;
    currentEnemySkill = battleEnemyStatus.EnemySkillList[skillNum];
    battleEnemyStatus.CurrentEnemySkill = currentEnemySkill;
    actionCommandJudgeManager.SetupAttackedPlayerMemberDetector (currentTarget, self);

    await StartSkillAttack ();
  }

  async UniTask StartSkillAttack ()
  {
    if (currentEnemySkill.skillId == SkillState.hitting.ToString ())
    {
      await shockWaveMotion.SetUpTarget (currentTarget);
      skillState = SkillState.hitting;

      await shockWaveMotion.ShockWaveAttack ();
      skillState = SkillState.idle;
      battleEnemyStatus.EndEnemyTurn = true;
    }
    else if (currentEnemySkill.skillId == SkillState.fireBall.ToString ())
    {
      await fireBallMotion.SetUpTarget (currentTarget);
      skillState = SkillState.fireBall;

      await fireBallMotion.FireBallAttack ();
      skillState = SkillState.idle;
      battleEnemyStatus.EndEnemyTurn = true;
    }
  }
}