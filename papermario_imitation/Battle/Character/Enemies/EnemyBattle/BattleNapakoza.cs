using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の攻撃の分岐を書いて、敵の体力等のステータスはBattleEnemyStratusに任せる
// Player側で敵の体力等を確認する際にBattleEnemyStatusを見れば済む
public class BattleNapakoza : MonoBehaviour
{
  [SerializeField] string stageEnemiesFileName;
  [SerializeField] string enemyName;
  private ActionCommandJudgeManager actionCommandJudgeManager;
  private BattleEnemyStatus battleEnemyStatus;
  private TackleRoute tackleRoute;
  private GameObject self;
  private GameObject currentTarget;
  private AttackedDetector targetAttackedDetector;
  private EnemySkill currentEnemySkill;
  private SkillState skillState;

  private bool startAction;
  // private bool endEnemyTurn;

  // public bool EndEnemyTurn
  // {
  //   get { return endEnemyTurn; }
  // }

  public enum SkillState
  {
    idle,
    tackle,
    escape
  }
  void Start ()
  {
    startAction = false;
    skillState = SkillState.idle;
    self = this.gameObject;
    tackleRoute = self.GetComponent<TackleRoute> ();
    // // ステータス保存用クラス
    battleEnemyStatus = self.GetComponent<BattleEnemyStatus> ();
    battleEnemyStatus.SetupBattleEnemyStatus (stageEnemiesFileName, enemyName);
    // Debug.Log ("バトルエネミー用意完了");
  }

  void Update ()
  {
    // BattleScirptではあくまでもキャラクターの動きを決めるだけにする
    // 動ける状態(startAction=true)の時に、SkillStateに従って行動の動きを決める
    if (skillState == SkillState.tackle)
    {
      if (startAction)
      {
        startAction = false;
        tackleRoute.StartTackle ();
      }
      // タックルし終えるか、ガードされたら元の位置に戻る
      bool enteredDamageCollider = targetAttackedDetector.GetEnteredDamageCollider ();
      bool isGuarded = targetAttackedDetector.SucceededAction;
      // Debug.Log ("targetAttackedDetector.HasActionChance : " + targetAttackedDetector.HasActionChance);
      // Debug.Log ("enteredDamageCollider : " + enteredDamageCollider);
      if (enteredDamageCollider || isGuarded)
      {
        // Debug.Log ("isGuarded : " + isGuarded);
        tackleRoute.BackToInitialPosition ();
      }
      // 元の位置にもどったら終了したことのフラグを建てる
      if (tackleRoute.EndTackle)
      {
        skillState = SkillState.idle;
        tackleRoute.EndTackle = false;
        battleEnemyStatus.EndEnemyTurn = true;
        // Debug.Log ("タックルアニメ終了");
      }
    }

    // メモ
    // バトルマネージャーの役割
    // 敵の攻撃結果をバトルに反映
    //    ダメージ量をダメージを受けたキャラの頭上に表示
    //    敵の体力回復を反映して保存
    //    プレイヤーパーティーへ与えたダメージをプレイヤーパーティーの体力に反映して保存
    //       防御の成功、失敗を計算に用いる

    // 敵のアニメーション再生
    //   ○○のときだけ処理の実行に移る

    //    敵自身の回復の場合
    //      敵の頭上に回復のメッセージ表示

    // 敵の攻撃結果をバトルに反映
    //    敵の体力回復を反映して保存
    //    プレイヤーパーティーへ与えたダメージをプレイヤーパーティーの体力に反映して保存

  }

  // 選択肢から選んだモードを実行
  public void ChooseAttackOptions (List<GameObject> battlePlayerPartyList, ActionCommandJudgeManager actionCommandJudgeManager)
  {
    this.actionCommandJudgeManager = actionCommandJudgeManager;
    // 使うスキル、スキルを使用する対象をここできめる
    // 理想はもう少しかしこい敵にしたい
    //    プレイヤーパーティーの状態を確認する
    //    敵パーティーの状態を確認する
    //    それぞれのパーティーの状態に応じた攻撃と攻撃対象を選択する(弱っている仲間を回復、弱っているプレイヤーメンバーを攻撃するなど)
    if (battleEnemyStatus.EnemySkillList.Count <= 0) return;
    int attackTargetNum = (int) (Random.value * battlePlayerPartyList.Count); //今だけ乱数できめる
    int skillNum = (int) (Random.value * battleEnemyStatus.EnemySkillList.Count);
    Debug.Log ("attackTargetNum : " + attackTargetNum);
    currentTarget = battlePlayerPartyList[attackTargetNum];
    // いまだけプレイヤーしかターゲットにしない
    // currentTarget = battlePlayerPartyList[1];
    
    // ステータスにターゲットを渡す
    battleEnemyStatus.CurrentTarget = currentTarget;
    // タックルの移動位置設定
    tackleRoute.SetupTargetPosition (currentTarget);
    targetAttackedDetector = currentTarget.GetComponent<AttackedDetector> ();
    targetAttackedDetector.ResetDetector ();
    currentEnemySkill = battleEnemyStatus.EnemySkillList[skillNum];
    // スキルの設定
    battleEnemyStatus.CurrentEnemySkill = currentEnemySkill;
    if (currentEnemySkill.skillId == BaseBattleCharacter.BattleState.tackle.ToString ())
    {
      skillState = SkillState.tackle;
      actionCommandJudgeManager.SetupAttackedPlayerMemberDetector (currentTarget, self);
      startAction = true;
    }
  }

}