using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemBackground : MonoBehaviour
{
  void Start ()
  {
    Rotate ();
  }

  void Rotate ()
  {
    GameObject self = this.gameObject;
    float rotationDuration = 5.0f;
    self.transform.DOLocalRotate (new Vector3 (0, 0, 360.0f), rotationDuration, RotateMode.FastBeyond360)
      .SetEase (Ease.Linear)
      .SetLoops (-1, LoopType.Restart);
  }

}