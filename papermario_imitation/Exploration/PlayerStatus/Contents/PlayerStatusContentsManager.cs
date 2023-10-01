using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// バトルシーンではシーンの立ち上げ時にセットアップする役割のみを担う
public class PlayerStatusContentsManager : MonoBehaviour
{
  [SerializeField] GameObject playerCurrentHp;
  [SerializeField] GameObject playerMaxHp;
  [SerializeField] GameObject playerCurrentFp;
  [SerializeField] GameObject playerMaxFp;
  [SerializeField] GameObject playerExperiencePoints;
  [SerializeField] GameObject playerCoin;
  [SerializeField] GameObject partnerCurrentHp;
  [SerializeField] GameObject partnerMaxHp;
  [SerializeField] GameObject partnerImg;
  // 別クラスに渡す予定もないので直打ちに変更
  // [SerializeField] string charactorImgFolderName;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private MenuCommonFunctions menuCommonFunctions;
  private PartnerDataArray partnerDataArray;
  private Sprite[] partnerSpriteArray;

  enum PartnerName
  {
    sakura
  }
  void Start ()
  {
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    menuCommonFunctions = new MenuCommonFunctions ();
    partnerSpriteArray = Resources.LoadAll<Sprite> ("Explore/Menu/Partners");
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    SetupStatusInformation ();
  }

  void Update ()
  {
    // ステータスの変更があった時のみに更新を行う
    bool isSaveDataChanged = menuCommonFunctions.IsSaveDataUpdated ();
    if (isSaveDataChanged)
    {
      SetupStatusInformation ();
    }
  }

  void SetupStatusInformation ()
  {
    var userData = SaveSystem.Instance.userData;
    SetupPlayerStatusInformation (userData);
    SetupPartnerStatusInformation (userData);
  }

  void SetupPlayerStatusInformation (MyGameData.MyData userData)
  {
    playerCurrentHp.GetComponent<Text> ().text = userData.playerCurrentHp.ToString ();
    playerMaxHp.GetComponent<Text> ().text = userData.playerMaxHp.ToString ();
    playerCurrentFp.GetComponent<Text> ().text = userData.playerCurrentFp.ToString ();
    playerMaxFp.GetComponent<Text> ().text = userData.playerMaxFp.ToString ();
    playerExperiencePoints.GetComponent<Text> ().text = userData.experience.ToString ();
    playerCoin.GetComponent<Text> ().text = userData.havingCoin.ToString ();
  }

  // 改修予定
  void SetupPartnerStatusInformation (MyGameData.MyData userData)
  {
    string currentSelectingPartnerName = userData.currentSelectedPartnerName;
    foreach (var partnerData in partnerDataArray.gamePartners)
    {
      if (currentSelectingPartnerName == partnerData.name)
      {
        SetupPartnerImageSprite (currentSelectingPartnerName);
        SetupPartnerCurrentHpText (partnerData.imgFileName, userData);
        SetupPartnerMaxHpText (partnerData.imgFileName, currentSelectingPartnerName, userData);
      }
    }
    // 現在選択中の仲間のステータスを表示する
    // partnerMaxHp.GetComponent<Text> ().text = userData.playerMaxHp.ToString ();
  }

  void SetupPartnerImageSprite (string partnerName)
  {
    partnerImg.GetComponent<Image> ().sprite =
      menuCommonFunctions.GetPartnerButtonSprite (partnerName, partnerDataArray, partnerSpriteArray);
  }
  void SetupPartnerCurrentHpText (string imgFileName, MyGameData.MyData userData)
  {
    if (imgFileName == PartnerName.sakura.ToString ())
    {
      partnerCurrentHp.GetComponent<Text> ().text = userData.sakuraCurrentHp.ToString ();
    }
  }

  void SetupPartnerMaxHpText (string imgFileName, string partnerName, MyGameData.MyData userData)
  {
    if (imgFileName == PartnerName.sakura.ToString ())
    {
      int currentLevel = userData.sakuraLevel;
      // Debug.Log ("currentLevel : " + currentLevel);
      int maxHp = menuCommonFunctions.GetPartnerMaxHp (partnerName, currentLevel, partnerDataArray);
      // Debug.Log ("maxHp : " + maxHp);
      partnerMaxHp.GetComponent<Text> ().text = maxHp.ToString ();
    }
  }
}