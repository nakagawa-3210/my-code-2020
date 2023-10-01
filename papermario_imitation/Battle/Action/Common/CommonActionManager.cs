using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class CommonActionManager
{
  public void DamageAmountEffect (
    GameObject currentTarget,
    BattleDamageEffectUiManager battleDamageEffectUiManager,
    int damageAmount
  )
  {
    Transform actionCommandPosition = currentTarget.transform.Find ("ActionCommandPosition");
    Transform starTransform = actionCommandPosition.Find ("ActionDamageAmountEffectPosition");
    if (damageAmount != 0)
    {
      battleDamageEffectUiManager.PlayDamageEffectStar (starTransform, damageAmount);
      battleDamageEffectUiManager.ShowSmallDamageStars (starTransform, damageAmount);
    }
    // 共通ダメージ音再生
    SEManager.Instance.Play (SEPath.DAMAGE);
  }

}