using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BelongingButtonInfoContainer : BaseButtonInfoContainer
{
  // private string imgFilebelongingName;
  // enumでタイプの種類を統一する
  public enum State
  {
    recoverHp,
    recoverFp,
    cure,
    flame,
    threat,
    makeEnemySleep,
    hotelVoucher,
    superHotelVoucher,
    miracleHotelVoucher,
    mystery,
    importantThing
  }

  // メニュー画面では回復系アイテムしか選択出来ないようにする
  private bool isSelectable = true;
  private string belongingName = "";
  private string description = "";
  private string type = "";
  private int amount = 0;
  private int playersSellingPrice = 0;

  public bool IsSelectable
  {
    set { isSelectable = value; }
    get { return isSelectable; }
  }
  public string BelongingName
  {
    set { belongingName = value; }
    get { return belongingName; }
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
  public int PlayersSellingPrice
  {
    set { playersSellingPrice = value; }
    get { return playersSellingPrice; }
  }
}