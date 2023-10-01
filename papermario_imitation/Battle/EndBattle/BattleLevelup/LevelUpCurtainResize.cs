using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCurtainResize : MonoBehaviour
{
  // private Vector3 selfInitialScale;
  void Start ()
  {
    // selfInitialScale = this.gameObject.transform.localScale;
    ResizeSelf ();
  }

  public void ResizeSelf ()
  {
    Vector3 selfScale = this.gameObject.transform.localScale;
    float selfScaleX = selfScale.x;
    float resized = selfScaleX / 6 * 5;
    Vector3 resizedSelfScale = selfScale;
    resizedSelfScale.x = resized;
    this.gameObject.transform.localScale = resizedSelfScale;
  }

}