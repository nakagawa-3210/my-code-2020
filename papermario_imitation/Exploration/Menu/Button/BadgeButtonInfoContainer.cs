using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeButtonInfoContainer : BaseButtonInfoContainer
{

  // アイテムとは違い、
  // 選べるかどうかが動的に変化する。isSelectableが変化する

  // 選べない時に選択された際には、trueにされたselectedをすぐにfalseにする

  // saveDataのputtingBadgeNameArrayに変更があるたびにisSelectableを更新する

  public enum State
  {
    upHp,
    upFp,
    upAtDownDf
  }

  private bool isEquipped = false;
  private string badgeName = "";
  private string description = "";
  private string type = "";
  private int badgeId = 0;
  private int prevBadgeId = 0;
  private int prevEquippingBadgeNum = 0;
  private int amount = 0;
  private int cost = 0;

  void Update ()
  {
    // 中身確認用
    // Debug.Log ("badgeName : " + badgeName);
    // Debug.Log ("description : " + description);
    // Debug.Log ("type : " + type);
    // Debug.Log ("amount : " + amount);
    // Debug.Log ("IsEquipped : " + badgeName + IsEquipped + prevEquippingBadgeNum);
    // Debug.Log (" badgeName  badgeId : " + badgeName + badgeId);
    // Debug.Log ("badgeId : " + badgeId);
  }
  public override void Selected ()
  {
    if (IsSelected == true)
    {
      IsSelected = false;
    }
    else
    {
      IsSelected = true;
    }
  }

  public bool IsEquipped
  {
    set { isEquipped = value; }
    get { return isEquipped; }
  }
  public string BadgeName
  {
    set { badgeName = value; }
    get { return badgeName; }
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
  public int BadgeId
  {
    set { badgeId = value; }
    get { return badgeId; }
  }
  public int PrevBadgeId
  {
    set { prevBadgeId = value; }
    get { return prevBadgeId; }
  }
  public int PrevEquippingBadgeNum
  {
    set { prevEquippingBadgeNum = value; }
    get { return prevEquippingBadgeNum; }
  }
  public int Amount
  {
    set { amount = value; }
    get { return amount; }
  }
  public int Cost
  {
    set { cost = value; }
    get { return cost; }
  }
}