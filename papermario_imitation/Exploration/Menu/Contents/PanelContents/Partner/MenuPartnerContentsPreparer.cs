using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPartnerContentsPreparer
{
  enum PartnerName
  {
    sakura,
    laiju,
  }
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private MenuCommonFunctions menuCommonFunctions;
  private PartnerSkillDataArray partnerSkillDataArray;
  private PartnerDataArray partnerDataArray;
  private GameObject partnerCursorTarget;
  private GameObject skillButton;
  private GameObject currentPartnerCurrentHp;
  private GameObject currentPartnerMaxHp;
  private GameObject currentPartnerNameText;
  private Transform skillListContainerTra;
  private EventSystem eventSystem;
  private string[] partnerNameArray;
  private string showingPartnerName;
  private string partnerDescription;
  private string partnerImgFileName;
  private string skillDataName;
  public MenuPartnerContentsPreparer (
    GameObject partnerCursorTarget,
    GameObject skillButton,
    GameObject currentPartnerCurrentHp,
    GameObject currentPartnerMaxHp,
    GameObject currentPartnerNameText,
    Transform skillListContainerTra,
    EventSystem eventSystem
  )
  {
    this.partnerCursorTarget = partnerCursorTarget;
    this.skillButton = skillButton;
    this.currentPartnerCurrentHp = currentPartnerCurrentHp;
    this.currentPartnerMaxHp = currentPartnerMaxHp;
    this.currentPartnerNameText = currentPartnerNameText;
    this.skillListContainerTra = skillListContainerTra;
    this.eventSystem = eventSystem;
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    menuCommonFunctions = new MenuCommonFunctions ();
    partnerDataArray = new PartnerDataArray ();
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    // 表示中の仲間の名前に応じて体力等の情報を表示する
    partnerNameArray = SaveSystem.Instance.userData.partnersName;
    SetDefaultPartnerNameAndSkillDataName ();
    SetupPartnerInformation ();
  }

  void SetupPartnerInformation ()
  {
    SetPartnerData ();
    SetupPartnerName ();
    SetupPartnerHp ();
    SetupPartnerSkills ();
    SetupPartnerInfoContainer ();
  }

  void SetupPartnerName ()
  {
    currentPartnerNameText.GetComponent<Text> ().text = showingPartnerName;
  }

  public void SetupPartnerHp ()
  {
    int currentHp = 0;
    int maxHp = 0;
    int partnerLevel = GetPartnerLevel ();
    MyGameData.MyData data = SaveSystem.Instance.userData;
    if (partnerImgFileName == PartnerName.sakura.ToString ())
    {
      currentHp = data.sakuraCurrentHp;
      maxHp = menuCommonFunctions.GetPartnerMaxHp (showingPartnerName, partnerLevel, partnerDataArray);
    }
    else if (partnerImgFileName == PartnerName.laiju.ToString ())
    {

    }
    currentPartnerCurrentHp.GetComponent<Text> ().text = currentHp.ToString ();
    currentPartnerMaxHp.GetComponent<Text> ().text = maxHp.ToString ();
  }

  void SetupPartnerSkills ()
  {
    MyGameData.MyData data = SaveSystem.Instance.userData;
    int partnerLevel = GetPartnerLevel ();
    PartnerSkill[] partnerSkills = partnerSkillDataArray.gamePartnerSkills;
    // 技はレベル+1の数だけもっている
    for (var i = 0; i < partnerLevel + 1; i++)
    {
      GameObject newSkill = GameObject.Instantiate<GameObject> (skillButton);
      newSkill.transform.SetParent (skillListContainerTra);
      float baseScale = 1.0f;
      newSkill.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
      // スキル名
      Transform skillNameText = newSkill.transform.Find ("SkillName");
      skillNameText.GetComponent<Text> ().text = partnerSkills[i].skillName;
      // スキルコスト
      Transform skillCost = newSkill.transform.Find ("SkillPointNum");
      skillCost.GetComponent<Text> ().text = partnerSkills[i].usingFp.ToString ();
      // 技に説明情報を持たせる
      SkillInfoContainer skillInfoContainer = newSkill.GetComponent<SkillInfoContainer> ();
      skillInfoContainer.Description = partnerSkills[i].description;
    }
  }

  int GetPartnerLevel ()
  {
    int level = 0;
    MyGameData.MyData data = SaveSystem.Instance.userData;
    if (partnerImgFileName == PartnerName.sakura.ToString ())
    {
      level = data.sakuraLevel;
    }
    else if (partnerImgFileName == PartnerName.laiju.ToString ())
    {

    }
    return level;
  }

  void SetPartnerData ()
  {
    // showingPartnerNameを用いて
    // gamePartners.nameと一致するデータのtypeを読み取る
    partnerSkillDataArray =
      new JsonReaderFromResourcesFolder ().GetPartnerSkillDataArray ("JSON/PartnerSkill/" + skillDataName);
  }

  void SetDefaultPartnerNameAndSkillDataName ()
  {
    int selectedPartnerNum = GetSelectedCurrentPartnerName ();
    // partnerDataArrからselectedPartnerNum目のデータを取得
    Partner partnerData = partnerDataArray.gamePartners[selectedPartnerNum];
    showingPartnerName = partnerData.name;
    partnerDescription = partnerData.description;
    skillDataName = partnerData.type;
    partnerImgFileName = partnerData.imgFileName;
  }

  void ManageShowingPartnerNameAndSkillDataName ()
  {
    // 選択中のゲームオブジェクトの名前がhogeのとき、
    string selectingPartnerTagName = "PartnerCursorTargetPosition";
    string tagName = eventSystem.currentSelectedGameObject.tag;
    int nameArrayLength = partnerNameArray.Length;
    int selectedPartnerNum = GetSelectedCurrentPartnerName ();
    // 選択中の仲間の名前がnameArrayLengthの何番目のものなのかを取得する
    if (tagName == selectingPartnerTagName && nameArrayLength != 1)
    {
      // 矢印キーの左右で表示中の名前を変更出来る
      if (Input.GetKeyDown (KeyCode.LeftArrow))
      {
        // selectedPartnerNumの変更
      }
      if (Input.GetKeyDown (KeyCode.RightArrow))
      {
        // selectedPartnerNumの変更
      }
      // partnerNameArray
      // 仲間の名前とスキルデータ名が欲しい
    }
  }

  // NameではなくNumに名前を直す
  int GetSelectedCurrentPartnerName ()
  {
    int partnerNum = 0;
    string selectedPartnerName = SaveSystem.Instance.userData.currentSelectedPartnerName;
    for (var i = 0; i < partnerNameArray.Length; i++)
    {
      if (selectedPartnerName == partnerNameArray[i])
      {
        partnerNum = i;
      }
    }
    return partnerNum;
  }

  void SetupPartnerInfoContainer ()
  {
    PartnerInfoContainer partnerInfoContainer =
      partnerCursorTarget.gameObject.GetComponent<PartnerInfoContainer> ();
    partnerInfoContainer.PartnerName = showingPartnerName;
    partnerInfoContainer.Description = partnerDescription;
  }

}