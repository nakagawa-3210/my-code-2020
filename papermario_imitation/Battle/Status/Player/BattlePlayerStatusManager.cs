using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// バトルでのプレイヤーと仲間のコンポーネントが持つ値応じて変更する
public class BattlePlayerStatusManager : MonoBehaviour
{
  [SerializeField] GameObject partnerImg;
  [SerializeField] Text playerCurrentHpText;
  [SerializeField] Text playerMaxHpText;
  [SerializeField] Text playerCurrentFpText;
  [SerializeField] Text playerMaxFpText;
  [SerializeField] Text playerExperiencePointText;
  [SerializeField] Text playerCoinText;
  [SerializeField] Text partnerCurrentHpText;
  [SerializeField] Text partnerMaxHpText;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private MenuCommonFunctions menuCommonFunctions;
  private PartnerDataArray partnerDataArray;
  private Sprite[] partnerSpriteArray;
  // キャッシュ中バトルステータス情報
  private int playerCurrentHpNum;
  private int playerMaxHpNum;
  private int playerCurrentFpNum;
  private int playerMaxFpNum;
  private int playerExperiencePointsNum;
  private int playerCoinNum;
  private int partnerCurrentHpNum;
  private int partnerMaxHpNum;
  // アクション後のバトルステータス
  private int afterActionPlayerCurrentHpNum;
  private int afterActionPlayerMaxHpNum;
  private int afterActionPlayerCurrentFpNum;
  private int afterActionPlayerMaxFpNum;
  private int afterActionPlayerExperiencePointsNum;
  private int afterActionPlayerCoinNum;
  private int afterActionPartnerCurrentHpNum;
  private int afterActionPartnerMaxHpNum;

  // バトルマネージャーから変更する
  public int AfterActionPlayerCurrentHpNum
  {
    set { afterActionPlayerCurrentHpNum = value; }
  }
  public int AfterActionPlayerMaxHpNum
  {
    set { afterActionPlayerMaxHpNum = value; }
  }
  public int AfterActionPlayerCurrentFpNum
  {
    set { afterActionPlayerCurrentFpNum = value; }
  }
  public int AfterActionPlayerMaxFpNum
  {
    set { afterActionPlayerMaxFpNum = value; }
  }
  public int AfterActionPlayerExperiencePointsNum
  {
    set { afterActionPlayerExperiencePointsNum = value; }
  }
  public int AfterActionPlayerCoinNum
  {
    set { afterActionPlayerCoinNum = value; }
  }
  public int AfterActionPartnerCurrentHpNum
  {
    set { afterActionPartnerCurrentHpNum = value; }
  }
  public int AfterActionPartnerMaxHpNum
  {
    set { afterActionPartnerMaxHpNum = value; }
  }

  void Start ()
  {
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    menuCommonFunctions = new MenuCommonFunctions ();
    partnerSpriteArray = Resources.LoadAll<Sprite> ("Explore/Menu/Partners");
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    MyGameData.MyData myData = SaveSystem.Instance.userData;
    SetupPlayerInformationNum (myData);
    SetupPartnerInformationNum (myData);
  }

  void Update ()
  {
    ManagePlayerInformaiton ();
  }

  void SetupPlayerInformationNum (MyGameData.MyData myData)
  {
    playerCurrentHpNum = myData.playerCurrentHp;
    playerMaxHpNum = myData.playerMaxHp;
    playerCurrentFpNum = myData.playerCurrentFp;
    playerMaxFpNum = myData.playerMaxFp;
    playerExperiencePointsNum = myData.experience;
    playerCoinNum = myData.havingCoin;
    afterActionPlayerCurrentHpNum = playerCurrentHpNum;
    afterActionPlayerMaxHpNum = playerMaxHpNum;
    afterActionPlayerCurrentFpNum = playerCurrentFpNum;
    afterActionPlayerMaxFpNum = playerMaxFpNum;
  }

  void SetupPartnerInformationNum (MyGameData.MyData myData)
  {
    string currentSelectingPartnerName = myData.currentSelectedPartnerName;
    if (currentSelectingPartnerName == "サクちゃん")
    {
      int sakuraLevel = myData.sakuraLevel;
      partnerCurrentHpNum = myData.sakuraCurrentHp;
      partnerMaxHpNum = menuCommonFunctions.GetPartnerMaxHp (currentSelectingPartnerName, sakuraLevel, partnerDataArray);
      afterActionPartnerCurrentHpNum = partnerCurrentHpNum;
      afterActionPartnerMaxHpNum = partnerMaxHpNum;
    }
  }

  void ManagePlayerInformaiton ()
  {
    playerCurrentHpNum = ManageStatusInformation (afterActionPlayerCurrentHpNum, playerCurrentHpNum, playerCurrentHpText);
    playerCurrentFpNum = ManageStatusInformation (afterActionPlayerCurrentFpNum, playerCurrentFpNum, playerCurrentFpText);
    partnerCurrentHpNum = ManageStatusInformation (afterActionPartnerCurrentHpNum, partnerCurrentHpNum, partnerCurrentHpText);
  }

  // テキストの変更別処理にて行う予定
  int ManageStatusInformation (int afterActionNum, int currentShownNum, Text numText)
  {
    if (afterActionNum < currentShownNum)
    {
      int decreasingNum = currentShownNum;
      decreasingNum--;
      numText.text = decreasingNum.ToString ();
      currentShownNum = decreasingNum;
    }
    else if (afterActionNum > currentShownNum)
    {
      int increasingNum = currentShownNum;
      increasingNum++;
      numText.text = increasingNum.ToString ();
      currentShownNum = increasingNum;
    }
    return currentShownNum;
  }

}