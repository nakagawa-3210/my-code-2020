using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionManager : MonoBehaviour
{
  [SerializeField] BattleEnemyStatusManager enemyStatusUiManager;
  [SerializeField] BattleDamageEffectUiManager battleDamageEffectUiManager;

  public void DamageAmountEffect (GameObject currentTarget, int damageAmount)
  {
    // ダメージ量表示
    new CommonActionManager ().DamageAmountEffect (
      currentTarget,
      battleDamageEffectUiManager,
      damageAmount
    );
    // ダメージをエネミーステータスUIに反映
    enemyStatusUiManager.EnemyHpGaugeValueReduction (currentTarget, damageAmount);
    // ダメージをステータスデータに反映
    BattleEnemyStatus enemyStatus = currentTarget.GetComponent<BattleEnemyStatus> ();
    enemyStatus.EnemyData.enemyHp -= damageAmount;
    enemyStatusUiManager.SetEnemyHpText ();
  }

}