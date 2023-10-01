using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommonSkillListContentsPreparer
{
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private GetGameSprite getGameSprite;
  private List<GameObject> badgeSkillButtonList;
  private GameObject buttonForSkill;
  private Transform skillListContainerTransform;
  string skillImageGameObjectName;
  string skillNameTextGameObjectName;
  string skillFpCostTextGameObjectName;
  string skillFpGameObjectName;

  public List<GameObject> BadgeSkillButtonList
  {
    get { return badgeSkillButtonList; }
  }

  public PlayerCommonSkillListContentsPreparer ()
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    getGameSprite = new GetGameSprite ();
    badgeSkillButtonList = new List<GameObject> ();
    skillImageGameObjectName = "SkillImage";
    skillNameTextGameObjectName = "SkillNameText";
    skillFpCostTextGameObjectName = "SkillFpPointNum";
    skillFpGameObjectName = "Fp";
  }

  public void SetupSkillListViewButtons (GameObject buttonForSkill, Transform skillListContainerTransform, string skillType, string normalSkillName)
  {
    // バッジデータのリスト取得
    BadgeDataArray badgeDataArray = jsonReaderFromResourcesFolder.GetBadgeDataArray ("JSON/GameBadgesData");
    // つけているバッジデータの内容に関わらず、基本攻撃は必ずリストに追加する
    SetupNormalSkill (badgeDataArray, buttonForSkill, skillListContainerTransform, normalSkillName);
    // つけているバッジに応じてスキル追加
    string[] equippingBadgeNameArr = menuCommonFunctions.GetEquippingBadgeNameArray ();
    foreach (var badgeData in badgeDataArray.gameBadges)
    {
      foreach (var equippingBadgeName in equippingBadgeNameArr)
      {
        if (badgeData.name == equippingBadgeName && badgeData.type == skillType)
        {
          GameObject skillButton = GameObject.Instantiate<GameObject> (buttonForSkill);
          string badgeName = badgeData.name;
          Sprite buttonSprite = getGameSprite.GetSameNameBadgeSprite (badgeName);
          SetupSkillButton (skillListContainerTransform, badgeDataArray, skillButton, buttonSprite, badgeName);
          SetupBadgeInformation (skillButton, badgeDataArray, badgeName);
          SetupSkillCommandDescriptionInformation (skillButton, badgeName);
          badgeSkillButtonList.Add (skillButton);
        }
      }
    }
  }

  void SetupNormalSkill (BadgeDataArray badgeDataArray, GameObject buttonForSkill, Transform skillListContainerTransform, string normalSkillName)
  {
    GameObject normalNormalSkillButton = GameObject.Instantiate<GameObject> (buttonForSkill);
    Sprite buttonSprite = getGameSprite.GetSameNameBadgeSprite (normalSkillName);
    SetupSkillButton (skillListContainerTransform, badgeDataArray, normalNormalSkillButton, buttonSprite, normalSkillName);
    // 基本攻撃はFpを消費しないのでFp情報は非表示にする
    Transform skillFpCostText = normalNormalSkillButton.transform.Find (skillFpCostTextGameObjectName);
    skillFpCostText.gameObject.SetActive (false);
    Transform skillFp = normalNormalSkillButton.transform.Find (skillFpGameObjectName);
    skillFp.gameObject.SetActive (false);
    // ボタンに固有の情報を持たせる
    SetupBadgeInformation (normalNormalSkillButton, badgeDataArray, normalSkillName);
    SetupSkillCommandDescriptionInformation (normalNormalSkillButton, normalSkillName);
    // 
  }

  void SetupSkillButton (Transform skillListContainerTransform, BadgeDataArray badgeDataArray, GameObject newSkillButton, Sprite skillSprite, string skillName)
  {
    // 親セット
    newSkillButton.transform.SetParent (skillListContainerTransform);
    // サイズセット
    float baseScale = 1.0f;
    newSkillButton.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    // 名前、画像セット
    Text skillNameText = newSkillButton.transform.Find (skillNameTextGameObjectName).GetComponent<Text> ();
    skillNameText.text = skillName;
    Image skillImg = newSkillButton.transform.Find (skillImageGameObjectName).GetComponent<Image> ();
    skillImg.sprite = skillSprite;
    // ボタン色変更に使用
    Text skillFpCostText = newSkillButton.transform.Find (skillFpCostTextGameObjectName).GetComponent<Text> ();
    Text skillFp = newSkillButton.transform.Find (skillFpGameObjectName).GetComponent<Text> ();
    // ボタンの色情報セット
    SetCanNotSelectColor (badgeDataArray, newSkillButton, skillImg, skillNameText, skillFpCostText, skillFp, skillName);
  }

  void SetupBadgeInformation (GameObject skillButton, BadgeDataArray badgeDataArray, string skillName)
  {
    foreach (var badgeData in badgeDataArray.gameBadges)
    {
      if (skillName == badgeData.name)
      {
        // 表示するFP数値変更
        Transform skillFpCostText = skillButton.transform.Find (skillFpCostTextGameObjectName);
        skillFpCostText.GetComponent<Text> ().text = badgeData.usingFp.ToString ();
        // informationContainerにデータ保存
        SkillButtonInformationContainer skillButtonInfoContainer = skillButton.GetComponent<SkillButtonInformationContainer> ();
        skillButtonInfoContainer.SkillName = skillName;
        skillButtonInfoContainer.Description = badgeData.description;
        skillButtonInfoContainer.SkillType = badgeData.type;
        skillButtonInfoContainer.Attack = badgeData.amount;
        skillButtonInfoContainer.UsingFp = badgeData.usingFp;
        int currentFp = SaveSystem.Instance.userData.playerCurrentFp;
        // FPが足りないボタンは選べないようにする
        if (currentFp < badgeData.usingFp)
        {
          skillButtonInfoContainer.IsSelectable = false;
        }
      }
    }
  }

  public void SetupSkillCommandDescriptionInformation (GameObject skillButton, string skillName)
  {
    HowToCommandDescriptionDataArray commandDescriptionDataArray = new JsonReaderFromResourcesFolder ().GetCommandDescriptionDataArray ("JSON/GameHowToCommandDescriptionData");
    foreach (var commandDescriptionData in commandDescriptionDataArray.gameHowToCommandDescriptions)
    {
      if (skillName == commandDescriptionData.name)
      {
        SkillButtonInformationContainer skillButtonInfoContainer = skillButton.GetComponent<SkillButtonInformationContainer> ();
        skillButtonInfoContainer.HowToCommand = commandDescriptionData.description;
      }
    }
  }

  void SetCanNotSelectColor (
    BadgeDataArray badgeDataArray,
    GameObject skillButton,
    Image skillImg,
    Text skillNameText,
    Text skillFpCostText,
    Text skillFp,
    string skillName
  )
  {
    foreach (var badgeData in badgeDataArray.gameBadges)
    {
      if (skillName == badgeData.name)
      {
        int skillFpCost = badgeData.usingFp;
        int playerCurrentFp = SaveSystem.Instance.userData.playerCurrentFp;
        if (skillFpCost > playerCurrentFp)
        {
          menuCommonFunctions.SetBattleSkillButtonGrayColor (skillButton, skillImg, skillNameText, skillFpCostText, skillFp);
        }
      }
    }
  }

  public void ResetAvailableBadgeSkillButtonInformation (int currentBattlePlayerFp)
  {
    foreach (var skillButton in badgeSkillButtonList)
    {
      SkillButtonInformationContainer skillButtonInfo = skillButton.GetComponent<SkillButtonInformationContainer> ();

      Text skillNameText = skillButton.transform.Find (skillNameTextGameObjectName).GetComponent<Text> ();
      Image skillImg = skillButton.transform.Find (skillImageGameObjectName).GetComponent<Image> ();
      Text skillFpCostText = skillButton.transform.Find (skillFpCostTextGameObjectName).GetComponent<Text> ();
      Text skillFp = skillButton.transform.Find (skillFpGameObjectName).GetComponent<Text> ();
      int skillFpCost = skillButtonInfo.UsingFp;
      if (skillFpCost > currentBattlePlayerFp)
      {
        menuCommonFunctions.SetBattleSkillButtonGrayColor (skillButton, skillImg, skillNameText, skillFpCostText, skillFp);
        skillButtonInfo.IsSelectable = false;
        // Debug.Log (skillButtonInfo.SkillName + " は使えない");
      }
      else
      {
        menuCommonFunctions.SetBattleSkillButtonDefaultColor (skillButton, skillImg, skillNameText, skillFpCostText, skillFp);
        skillButtonInfo.IsSelectable = true;
        // Debug.Log (skillButtonInfo.SkillName + " は使える");
      }
    }
  }
}