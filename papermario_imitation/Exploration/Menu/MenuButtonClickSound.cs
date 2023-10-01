using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class MenuButtonClickSound : MonoBehaviour
{
  public void OnClickButton ()
  {
    SEManager.Instance.Play (SEPath.MENU_DECISION);
  }
}