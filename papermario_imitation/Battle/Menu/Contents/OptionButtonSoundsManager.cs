using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButtonSoundsManager : MonoBehaviour, ISelectHandler
{
  // private SEManager seManager;
  // void Start ()
  // {
  //   seManager = SEManager.Instance;
  // }

  public void OnSelect (BaseEventData eventData)
  {
    SEManager.Instance.Play (SEPath.MENU_CURSOR);
  }

  public void OnClickButton ()
  {
    SEManager.Instance.Play (SEPath.MENU_DECISION);
  }

}