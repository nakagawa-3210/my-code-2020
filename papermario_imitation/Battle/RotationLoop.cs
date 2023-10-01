using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotationLoop : MonoBehaviour
{
  [SerializeField] bool rotationX;
  [SerializeField] bool rotationY;
  [SerializeField] bool rotationZ;
  [SerializeField] bool useSerializeFieldDuration = default;
  [SerializeField] float duration = default;

  private Tween rotation;

  private GameObject self;

  void Start ()
  {
    self = this.gameObject;
    self.transform.localEulerAngles = new Vector3 (0, 0, 0);

    float rotationDuration = 0.8f;
    if (useSerializeFieldDuration)
    {
      rotationDuration = duration;
    }
    RotationLoopTween (rotationDuration);
  }

  void RotationLoopTween (float rotationDuration)
  {
    Vector3 rotationVector3 = new Vector3 ();
    if (rotationX == true)
    {
      rotationVector3 = new Vector3 (360f, 0, 0);
    }
    if (rotationY == true)
    {
      rotationVector3 = new Vector3 (0, 360f, 0);
    }
    if (rotationZ == true)
    {
      rotationVector3 = new Vector3 (0, 0, 360f);
    }
    rotation = gameObject.transform.DOLocalRotate (rotationVector3, rotationDuration, RotateMode.FastBeyond360).SetEase (Ease.Linear)
      .SetLoops (-1, LoopType.Restart);
  }

  private void OnDestroy ()
  {
    // Debug.Log ("回転しているオブジェクトの削除");
    if (DOTween.instance != null)
    {
      // Debug.Log ("Tweenの削除");
      rotation?.Kill ();
    }
  }

}