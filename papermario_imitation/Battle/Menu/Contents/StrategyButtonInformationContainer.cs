using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyButtonInformationContainer : BaseButtonInfoContainer
{
  public enum State
  {
    defenceUp,
    escape
  }
  private bool isSelectable = true; // 逃げられないバトル用
  private string strategyName = "";
  private string description = "";
  private string type = "";
  private int amount = 0;

  public bool IsSelectable
  {
    set { isSelectable = value; }
    get { return isSelectable; }
  }
  public string StrategyName
  {
    set { strategyName = value; }
    get { return strategyName; }
  }
  public string Description
  {
    set { description = value; }
    get { return description; }
  }
  public string Type
  {
    set { type = value; }
    get { return type; }
  }
  public int Amount
  {
    set { amount = value; }
    get { return amount; }
  }
}