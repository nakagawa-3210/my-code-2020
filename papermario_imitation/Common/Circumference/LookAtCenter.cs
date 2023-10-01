using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
  void Start ()
  {
    SelfChildrenLookAtCenter ();
  }

  void SelfChildrenLookAtCenter ()
  {
    GameObject self = this.gameObject;
    List<GameObject> childList = new MenuCommonFunctions ().GetChildList (self);
    for (var i = 0; i < childList.Count; i++)
    {
      GameObject childe = childList[i];
      Vector3 childeRotation = childe.transform.localEulerAngles;
      float degree = 360.0f / childList.Count;
      childeRotation.z -= degree * i;
      childe.transform.localEulerAngles = childeRotation;
    }
  }

}