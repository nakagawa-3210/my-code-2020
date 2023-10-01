using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlinkingByAlpha : MonoBehaviour
{
  [SerializeField] float blinkingTime = default;
  [SerializeField] float blinkDuration = default;

  private int loopTimes;

  public void BlinkingSelfByAlpha ()
  {
    if (blinkingTime == default)
    {
      loopTimes = -1;
    }
    else
    {
      loopTimes = (int) (blinkingTime / blinkDuration);
    }

    float hideAlpha = 0.0f;
    float duration = blinkDuration;
    this.gameObject.transform.GetComponent<CanvasGroup> ().DOFade (hideAlpha, duration).SetLoops (loopTimes, LoopType.Yoyo);
  }

}