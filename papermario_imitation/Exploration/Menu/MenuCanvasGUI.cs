using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCanvasGUI : MonoBehaviour
{
  // 大枠
  public GameObject[] tagButtonArray;
  public GameObject[] contentsPanelArray;
  public GameObject[] belongingOptionsArray;
  public GameObject[] badgeOptionsArray;
  public GameObject defaultSelectedTagButton;
  public GameObject defaultContentsPanel;
  public GameObject descriptionPanel;
  public GameObject descriptionSelectImage;
  public GameObject descriptionText;
  public GameObject cursor;
  public EventSystem eventSystem;

  // プレイヤーパネル
  public GameObject playerLevel;
  public GameObject playerTitle;
  public GameObject playerCurrentHp;
  public GameObject playerMaxHp;
  public GameObject playerCurrentFp;
  public GameObject playerMaxFp;
  public GameObject playerMaxBp;
  public GameObject playerExperiencePoints;
  public GameObject playerCoin;
  public GameObject playerStarFragments;
  public GameObject playerSuperFertilizer;
  public GameObject playerTotalPlayingTimeHour;
  public GameObject playerTotalPlayingTimeMin;

  // 仲間パネル
  public GameObject skillButton;
  public GameObject currentPartnerCurrentHp;
  public GameObject currentPartnerMaxHp;
  public GameObject currentPartnerNameText;
  public GameObject partnerCursorTarget;
  public GameObject skillListContainer;

  // アイテムパネル
  public GameObject itemListButton;
  public GameObject recoveringTargetListButton;
  public GameObject whoPanel;
  public GameObject itemScrollViewPanel;
  public GameObject improtantThingScrollViewPanel;
  public GameObject recoveringTargetScrollViewPanel;
  public GameObject itemListContainer;
  public GameObject improtantThingListContainer;
  public GameObject recoveringTargetListContainer;

  // バッジパネル
  public GameObject badgeListButton;
  public GameObject badgeEmptyCostImage;
  public GameObject badgeFullCostImage;
  public GameObject playerTotalBadgePointNumText;
  public GameObject playerRestOfTheBadgePoint;
  public GameObject allHavingBadgeScrollViewPanel;
  public GameObject equippingBadgeScrollViewPanel;
  public GameObject allHavingBadgeListContainer;
  public GameObject equippingBadgeListContainer;
  public GameObject playerUsingBadgePointInformationPanel;
  public GameObject badgeEmptyCostImgContainer;
  public GameObject badgeFullCostImgContainer;

  void Start ()
  {

  }

}