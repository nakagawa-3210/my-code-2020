using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class ScaleWithRotationFunctions
{
  private Vector3 showingScale;
  private Vector3 hidingScale;
  private Vector3 showingDegree;
  private Vector3 hidingDegree;
  private float tweenDuration;
  public ScaleWithRotationFunctions ()
  {
    float noRotation = 0.0f;
    float tiltCounterClockwise = 60.0f;
    float defaultScale = 1.0f;
    float hideScale = 0.1f;
    tweenDuration = 0.1f;
    showingScale = new Vector3 (defaultScale, defaultScale, defaultScale);
    hidingScale = new Vector3 (hideScale, hideScale, hideScale);
    showingDegree = new Vector3 (noRotation, noRotation, noRotation);
    hidingDegree = new Vector3 (noRotation, noRotation, tiltCounterClockwise);
  }

  public async UniTask ShowingTweenUsingScaleAndRotate (GameObject target)
  {
    target.transform.DOScale (showingScale, tweenDuration);
    await target.transform.DORotate (showingDegree, tweenDuration);
  }

  // 消える時は逆時計周りで回転しながら縮小
  public async UniTask HidingTweenUsingScaleAndRotate (GameObject target)
  {
    target.transform.DOScale (hidingScale, tweenDuration);
    await target.transform.DORotate (hidingDegree, tweenDuration);
  }
}