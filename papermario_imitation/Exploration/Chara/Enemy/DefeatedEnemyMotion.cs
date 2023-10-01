using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class DefeatedEnemyMotion : MonoBehaviour
{
  [SerializeField] GameObject selfSigns;
  [SerializeField] GameObject selfFallDownRoot = default;
  [SerializeField] GameObject selfSprite = default;
  [SerializeField] SpawnByWinningBattle enemyItemSpawnArea = default;

  public async UniTask PlayDefeatedEnemy ()
  {
    GameObject self = this.gameObject;

    // バトル発生エリアの非アクティブ化
    self.transform.Find ("BattleOpponentSearchArea").gameObject.SetActive (false);
    // 追いかけ機能非アクティブ化
    self.transform.GetComponent<EnemyAttackMotion> ().enabled = false;
    await UniTask.WaitUntil (() => !self.transform.GetComponent<EnemyAttackMotion> ().isActiveAndEnabled);
    selfSigns.SetActive (false);

    // 回転しながら煙を上げて後ろに倒れていく
    await new CommonDefeatedMotion ().DefeatedMotion (selfSprite, selfFallDownRoot, selfSprite);
    EnemySmokeManager enemySmokeManager = self.GetComponent<EnemySmokeManager> ();
    enemySmokeManager.PlayDefeatedSmoke ();
    await UniTask.WaitUntil (() => enemySmokeManager.EndPlayingParticle ());
    // コイン、HP、FP、アイテムをランダムに敵のデータに合わせてばらまく
    // HP,FPは今後追加予定
    await enemyItemSpawnArea.SpawnSomething ();
    // 倒れ終えたら自身を削除
    // Destroy (self);
    self.SetActive (false);
  }
}