using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class RemoveDefeatedEnemyManager
{
  private BattleManager battleManager;
  private BattleExperienceManager battleExperienceManager;
  private BattleEnemyStatusManager battleEnemyStatusManager;

  private float mobEnemyHideDuration;

  public RemoveDefeatedEnemyManager (
    BattleManager battleManager,
    BattleExperienceManager battleExperienceManager,
    BattleEnemyStatusManager battleEnemyStatusManager
  )
  {
    this.battleManager = battleManager;
    this.battleExperienceManager = battleExperienceManager;
    this.battleEnemyStatusManager = battleEnemyStatusManager;

    mobEnemyHideDuration = 1.0f;
  }

  public async UniTask RemoveDefeatedEnemy ()
  {
    // 敵の体力を確認
    // 敵リストから体力0の敵だけを一時的なリストに保存
    // バトルキャラリストと敵キャラリストの中から、一時的なリストと一致する要素を削除する
    bool isThereDefeatedEnemy = false;
    List<GameObject> defeatedEnemyList = new List<GameObject> ();

    // float hideDuration = 1.0f;
    // 倒れるモーション再生
    foreach (var enemy in battleManager.BattleEnemyCharacterList)
    {
      Enemy enemyData = enemy.GetComponent<BattleEnemyStatus> ().EnemyData;
      if (enemyData.enemyHp <= 0)
      {
        isThereDefeatedEnemy = true;
        defeatedEnemyList.Add (enemy);
        // ザコテキの場合
        if (enemy.GetComponent<EnemyDefeatedMotion> () != null)
        {
          enemy.GetComponent<EnemyDefeatedMotion> ().HideDefeatedEnemySprite (mobEnemyHideDuration);
        }
        // ボステキの場合
        else if (enemy.GetComponent<BossDefeatedMotion> () != null)
        {
          await enemy.GetComponent<BossDefeatedMotion> ().PlayDefeatedBossMotion ();
        }
      }
    }

    if (isThereDefeatedEnemy)
    {
      // 倒された音再生
      SEManager.Instance.Play (SEPath.DEFEATED);
      int hideEnemyListDelay = (int) (mobEnemyHideDuration * 1000);
      await UniTask.Delay (hideEnemyListDelay);
      // 倒された敵の経験値モーション再生
      // Debug.Log ("defeatedEnemyList.Count : " + defeatedEnemyList.Count);
      foreach (var defeatedEnemy in defeatedEnemyList)
      {
        // 煙パーティクル再生
        defeatedEnemy.GetComponent<EnemySmokeManager> ().PlayDefeatedSmoke ();
        // 経験値をUIに反映
        int enemyExperienceNum = defeatedEnemy.GetComponent<BattleEnemyStatus> ().EnemyData.enemyExperience;
        // enemyExperienceNum = 30;
        battleExperienceManager.AcquiredExperienceNum += enemyExperienceNum;
        // 経験値を落とす音
        SEManager.Instance.Play (SEPath.DROP_EXPERIENCE);
        // 経験値を落とすモーション
        battleExperienceManager.ProduceExperiencePoint (defeatedEnemy);
        // Debug.Log ("なんかいよばれた");
      }
      // Debug.Log ("managersForPlayerParty.battleExperienceManager.AcquiredExperienceNum : " + managersForPlayerParty.battleExperienceManager.AcquiredExperienceNum);

      // すぐに次のターンに移るとゲームの流れに違和感があるので、少しだけ間を作る
      int waitForRemove = 500;
      await UniTask.Delay (waitForRemove);
    }

    // 倒れた敵をバトルリストからはずす
    foreach (var defeatedEnemy in defeatedEnemyList)
    {
      battleManager.BattleAllCharactersList.Remove (defeatedEnemy);
      battleManager.BattleEnemyCharacterList.Remove (defeatedEnemy);
      battleEnemyStatusManager.ResetEnemyHpGauge (battleManager.BattleEnemyCharacterList);
    }
  }

}