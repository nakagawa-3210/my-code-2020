using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;

public class ActionCommandDictionaryCursorMotionManager
{
  private Slider dictionaryActionSlider;
  private Tween dictionaryActionSliderTween;
  private float sliderDurartion;
  private bool endCursorTween;

  public bool EndCursorTween
  {
    get { return endCursorTween; }
  }

  public ActionCommandDictionaryCursorMotionManager (GameObject dictionaryActionSign, float sliderDurartion)
  {
    this.sliderDurartion = sliderDurartion;
    dictionaryActionSlider = dictionaryActionSign.GetComponent<Slider> ();
    endCursorTween = false;
  }

  public void StartIncreasingSliderValue ()
  {
    // 音再生
    SEManager.Instance.Play (SEPath.DICTIONARY_CURSOR, 1, 0, 1, true, null);

    float sliderMinValue = 0.0f;
    float sliderMaxValue = 1.0f;
    dictionaryActionSliderTween = DOTween.To (
      () => sliderMinValue,
      x =>
      {
        dictionaryActionSlider.value = x;
      },
      sliderMaxValue,
      sliderDurartion
    ).OnComplete (() =>
    {
      endCursorTween = true;
    });
  }

  public void KillIncreasingSliderValue ()
  {
    endCursorTween = false;
    dictionaryActionSliderTween.Kill ();
    SEManager.Instance.Stop (SEPath.DICTIONARY_CURSOR);
  }

  public void ResetCursorValue ()
  {
    dictionaryActionSlider.value = 0.0f;
  }

}