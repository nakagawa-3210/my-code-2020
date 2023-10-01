using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class Test_Rotation : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] GameObject rotationCube;
  float test = 0.0f;
  void Start ()
  {

  }

  // Update is called once per frame
  void Update ()
  {
    // if (Input.GetKeyDown (KeyCode.Z))
    // {
    //   transform.DOLocalRotate (new Vector3 (0, 360f, 0), 0.8f, RotateMode.FastBeyond360)
    //     .SetEase (Ease.Linear)
    //     .SetLoops (2, LoopType.Restart);
    // }
    // if (test <= 720.0f)
    // {
    //   test++;
    // }

    // float testX = rotationCube.transform.localEulerAngles.x;
    // testX = test;
    // rotationCube.transform.localEulerAngles = new Vector3 (test, rotationCube.transform.localEulerAngles.y, rotationCube.transform.localEulerAngles.z);
  }
}