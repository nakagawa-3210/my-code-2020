using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonInformationContainer : BaseButtonInfoContainer
{
  public enum State
  {
    jump,
    hammer,
    search
  }
  private bool isSelectable = true;
  private string skillName = "";
  private string description = "";
  private string howToCommand = "";
  // jumpかhammerか仲間固有のスキルか
  private string skillType = "";
  // スキルの攻撃力
  private int attack = 0;
  private int usingFp = 0;
  public bool IsSelectable
  {
    set { isSelectable = value; }
    get { return isSelectable; }
  }
  public string SkillName
  {
    set { skillName = value; }
    get { return skillName; }
  }
  public string Description
  {
    set { description = value; }
    get { return description; }
  }
  public string HowToCommand
  {
    set { howToCommand = value; }
    get { return howToCommand; }
  }
  public string SkillType
  {
    set { skillType = value; }
    get { return skillType; }
  }
  public int Attack
  {
    set { attack = value; }
    get { return attack; }
  }
  public int UsingFp
  {
    set { usingFp = value; }
    get { return usingFp; }
  }
}