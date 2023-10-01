using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerSkillListContentsPreparer
{
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private PartnerDataArray partnerDataArray;
  private PartnerSkillDataArray partnerSkillDataArray;
  private GetGameSprite getGameSprite;
  private GameObject buttonForPartnerSkill;
  private Transform partnerSkillListContainerTransform;
  string skillImageGameObjectName;
  string skillNameTextGameObjectName;
  string skillFpCostTextGameObjectName;
  string skillFpGameObjectName;

  public PartnerSkillListContentsPreparer (
    GameObject buttonForPartnerSkill,
    Transform partnerSkillListContainerTransform
  )
  {
    this.buttonForPartnerSkill = buttonForPartnerSkill;
    this.partnerSkillListContainerTransform = partnerSkillListContainerTransform;
    // イメージは原作だと色のついた玉？
    skillImageGameObjectName = "SkillImage";
    skillNameTextGameObjectName = "SkillNameText";
    skillFpCostTextGameObjectName = "SkillFpPointNum";
    skillFpGameObjectName = "Fp";
    menuCommonFunctions = new MenuCommonFunctions ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    // ここで確認
    SetupPartnerSkillListViewButtons ();
  }

  void SetupPartnerSkillListViewButtons ()
  {
    string currentSelectedPartnerName = SaveSystem.Instance.userData.currentSelectedPartnerName;
    int partnerLevel = GetSelectedPartnerLevel (currentSelectedPartnerName);
    string partnerType = GetSelectedPartnerType (currentSelectedPartnerName);
    partnerSkillDataArray = jsonReaderFromResourcesFolder.GetPartnerSkillDataArray ("JSON/PartnerSkill/" + partnerType);
    // 技はレベル+1の数だけもっている
    for (var i = 0; i < partnerLevel + 1; i++)
    {
      GameObject partnerSkillButton = GameObject.Instantiate<GameObject> (buttonForPartnerSkill);
      // 1から始まるようにしておく
      int partnerSkillNumber = i + 1;
      PartnerSkill partnerSkillData = partnerSkillDataArray.gamePartnerSkills[i];
      // int partnerSkillFpCost = 30; // ボタンの色変更テスト用
      int partnerSkillFpCost = partnerSkillData.usingFp;
      string partnerSkillName = partnerSkillData.skillName;
      string partnerSkillImgName = "skill" + partnerSkillNumber.ToString ();
      Sprite partnerSkillSprite = new GetGameSprite ().GetSameNamePartnerSkillSprite (partnerSkillImgName);
      SetupPartnerSkillButton (partnerSkillData, partnerSkillButton, partnerSkillSprite, partnerSkillName, partnerSkillFpCost);
      // 攻撃方法説明のみプレイヤーと共通処理
      new PlayerCommonSkillListContentsPreparer ().SetupSkillCommandDescriptionInformation (partnerSkillButton, partnerSkillName);
    }
  }

  // 改修時にMenuTargetListContentsPreparerのGetPartnerLevelを下記の関数に変更する
  int GetSelectedPartnerLevel (string partnerName)
  {
    MyGameData.MyData myData = SaveSystem.Instance.userData;
    int level = 0;
    if (partnerName == "サクちゃん")
    {
      level = myData.sakuraLevel;
    }
    return level;
  }

  string GetSelectedPartnerType (string partnerName)
  {
    // skillデータの取得にはSetPartnerDataを用いる
    string type = "";
    // 仲間のjsonデータ配列を読み込む
    foreach (var partnerData in partnerDataArray.gamePartners)
    {
      if (partnerName == partnerData.name)
      {
        type = partnerData.type;
      }
    }
    return type;
  }

  void SetupPartnerSkillButton (PartnerSkill partnerSkillData, GameObject newPartnerSkillButton, Sprite partnerSkillSprite, string partnerSkillName, int partnerSkillFpCost)
  {
    newPartnerSkillButton.transform.SetParent (partnerSkillListContainerTransform);
    float baseScale = 1.0f;
    newPartnerSkillButton.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    Text skillNameText = newPartnerSkillButton.transform.Find (skillNameTextGameObjectName).GetComponent<Text> ();
    skillNameText.text = partnerSkillName;
    Image skillImg = newPartnerSkillButton.transform.Find (skillImageGameObjectName).GetComponent<Image> ();
    skillImg.sprite = partnerSkillSprite;
    // スキルイメージの調整（今後画像のサイズをあらかじめ統一して不要になるかも）
    // 位置調整
    Vector3 skillImgPosition = skillImg.transform.localPosition;
    skillImgPosition.x = -115.0f;
    skillImg.transform.localPosition = skillImgPosition;
    // サイズ調整
    Vector3 skillImgScale = skillImg.transform.localScale;
    skillImgScale.x = 0.20f;
    skillImgScale.y = 0.20f;
    skillImg.transform.localScale = skillImgScale;
    Text skillFpCostText = newPartnerSkillButton.transform.Find (skillFpCostTextGameObjectName).GetComponent<Text> ();
    skillFpCostText.text = partnerSkillFpCost.ToString ();
    Text skillFp = newPartnerSkillButton.transform.Find (skillFpGameObjectName).GetComponent<Text> ();
    HideFpInformation (skillFpCostText, skillFp, partnerSkillFpCost);
    // fpの足りないボタンは色変更
    int playerCurrentFp = SaveSystem.Instance.userData.playerCurrentFp;
    if (playerCurrentFp < partnerSkillFpCost)
    {
      menuCommonFunctions.SetBattleSkillButtonGrayColor (
        newPartnerSkillButton,
        skillImg,
        skillNameText,
        skillFpCostText,
        skillFp
      );
    }
    // コンポーネントに情報保存
    SetupSkillInformation (partnerSkillData, newPartnerSkillButton);
  }

  void SetupSkillInformation (PartnerSkill partnerSkillData, GameObject newPartnerSkillButton)
  {

    // ボタンコンポーネントにボタン情報を渡す
    SkillButtonInformationContainer skillButtonInfoContainer = newPartnerSkillButton.GetComponent<SkillButtonInformationContainer> ();
    skillButtonInfoContainer.SkillName = partnerSkillData.skillName;
    skillButtonInfoContainer.Description = partnerSkillData.description;
    skillButtonInfoContainer.SkillType = partnerSkillData.type;
    skillButtonInfoContainer.Attack = partnerSkillData.amount;
    skillButtonInfoContainer.UsingFp = partnerSkillData.usingFp;
  }

  void HideFpInformation (Text skillFpCostText, Text skillFp, int partnerSkillFpCost)
  {
    if (partnerSkillFpCost == 0)
    {
      skillFpCostText.gameObject.SetActive (false);
      skillFp.gameObject.SetActive (false);
    }
  }

}