using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetButtonInfoContainer : BaseButtonInfoContainer
{
  private bool isSelectableHp = true;
  private bool isSelectableFp = true;
  private string targetName = "";

  // private void Update() {
  //   Debug.Log("targetName : " + targetName);
  // }

  public bool IsSelectableHp
  {
    set { isSelectableHp = value; }
    get { return isSelectableHp; }
  }
  public bool IsSelectableFp
  {
    set { isSelectableFp = value; }
    get { return isSelectableFp; }
  }
  public string TargetName
  {
    set { targetName = value; }
    get { return targetName; }
  }
}