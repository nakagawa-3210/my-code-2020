using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GaugeValueTween
{
  public void ChangeGaugeValueTween (Image gaugeImage, float valueFrom, float valueTo, float tweenDuration)
  {
    Tween gaugeValueReduction = DOTween.To (
      () => valueFrom, // 対象
      x =>
      {
        gaugeImage.fillAmount = x; // 値の更新
      },
      valueTo,
      tweenDuration
    );
  }
}