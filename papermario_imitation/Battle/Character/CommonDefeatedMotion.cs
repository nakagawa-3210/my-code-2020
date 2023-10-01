using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class CommonDefeatedMotion
{
  
  public async UniTask DefeatedMotion (GameObject rotateRoot, GameObject fallDownRoot, GameObject self,float rotationDuration = 1.2f)
  {
    int rotationNum = 2;
    await DOTween.Sequence ()
      .Append (rotateRoot.transform.DOLocalRotate (new Vector3 (0, 360f, 0), rotationDuration / rotationNum, RotateMode.FastBeyond360)
        .SetEase (Ease.Linear)
        .SetLoops (rotationNum, LoopType.Restart))
      .Append (fallDownRoot.transform.DOLocalRotate (new Vector3 (90, 0, 0), rotationDuration / rotationNum))
      .OnComplete (() =>
      {
        self.SetActive (false);
      });
  }

}