using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DefenceShieldRotation : MonoBehaviour
{
  void Start ()
  {
    GameObject self = this.gameObject;
    float defaultRotationZ = self.transform.localEulerAngles.z;
    float rotationDuration = 1.5f;
    self.transform.DOLocalRotate (new Vector3 (0, 360f, defaultRotationZ), rotationDuration, RotateMode.FastBeyond360)
      .SetLoops (-1, LoopType.Incremental);
  }

}