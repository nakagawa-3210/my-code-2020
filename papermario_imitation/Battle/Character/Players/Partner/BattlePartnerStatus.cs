using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartnerStatus : MonoBehaviour
{
  private int hp = 0;
  private int maxHp = 0;
  private int fp = 0;
  private int maxFp = 0;
  private int at = 0;
  private int defaultAt = 0;
  private int df = 0;
  private int defaultDf = 0;
  private bool isParalysed = false;
  private bool isPoisoned = false;
  private bool isSleeping = false;
  public int Hp
  {
    set { hp = value; }
    get { return hp; }
  }
  public int MaxHp
  {
    set { maxHp = value; }
    get { return maxHp; }
  }
  public int At
  {
    set { at = value; }
    get { return at; }
  }
  public int DefaultAt
  {
    set { defaultAt = value; }
    get { return defaultAt; }
  }
  public int Df
  {
    set { df = value; }
    get { return df; }
  }
  public int DefaultDf
  {
    set { defaultDf = value; }
    get { return defaultDf; }
  }
  public bool IsParalysed
  {
    set { isParalysed = value; }
    get { return isParalysed; }
  }
  public bool IsPoisoned
  {
    set { isPoisoned = value; }
    get { return isPoisoned; }
  }
  public bool IsSleeping
  {
    set { isSleeping = value; }
    get { return isSleeping; }
  }
  public void SetupPartnerStatus (string partnerName, int partnerLevel)
  {
    MyGameData.MyData myData = SaveSystem.Instance.userData;
    PartnerDataArray partnerDataArray = new JsonReaderFromResourcesFolder ().GetPartnerDataArray ("JSON/GamePartnersData");
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    Hp = myData.sakuraCurrentHp; //サクラに依存中
    MaxHp = menuCommonFunctions.GetPartnerMaxHp (partnerName, partnerLevel, partnerDataArray);
    At = partnerLevel;
    Df = partnerLevel - 1; // 防御はデフォルトで0からスタートさせる
    DefaultAt = partnerLevel;
    DefaultDf = partnerLevel - 1;
  }
}