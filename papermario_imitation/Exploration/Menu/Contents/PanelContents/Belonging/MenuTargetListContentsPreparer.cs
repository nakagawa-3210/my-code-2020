using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTargetListContentsPreparer
{
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private MenuCommonFunctions menuCommonFunctions;
  private GameObject buttonForRecoveringTargets;
  private Transform recoveringTargetListContainer;
  private PartnerDataArray partnerDataArray;
  private string charactorImgFolderName = "Explore/Menu/Partners";

  public enum PartnerNames
  {
    sakura
  }

  public MenuTargetListContentsPreparer (
    GameObject buttonForRecoveringTargets,
    Transform recoveringTargetListContainer
  )
  {
    this.buttonForRecoveringTargets = buttonForRecoveringTargets;
    this.recoveringTargetListContainer = recoveringTargetListContainer;
    menuCommonFunctions = new MenuCommonFunctions ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    SetupRecoveringTargetListViewButtons ();
  }

  void SetupRecoveringTargetListViewButtons ()
  {
    MyGameData.MyData userData = SaveSystem.Instance.userData;
    SetupTargetPlayerButton (userData);
    SetupTargetPartnerButton (userData);
  }

  public void SetupTargetPlayerButton (MyGameData.MyData userData)
  {
    // プレイヤーのボタンは常にあるのでGetChild (0)で指定する
    GameObject playerButton = recoveringTargetListContainer.transform.GetChild (0).gameObject;
    // Findを直打ちのままか、呼び出し元から受け取るか、迷い中
    Transform playerHp = playerButton.transform.Find ("Hp");
    Transform playerFp = playerButton.transform.Find ("Fp");
    playerHp.transform.Find ("CurrentHpText").GetComponent<Text> ().text = userData.playerCurrentHp.ToString ();
    playerHp.transform.Find ("MaxHpText").GetComponent<Text> ().text = userData.playerMaxHp.ToString ();
    playerFp.transform.Find ("CurrentFpText").GetComponent<Text> ().text = userData.playerCurrentFp.ToString ();
    playerFp.transform.Find ("MaxFpText").GetComponent<Text> ().text = userData.playerMaxFp.ToString ();
    // ボタンのコンポーネントに情報を持たせる
    SetupTargetButtonsIsSelectable (
      playerButton,
      userData.playerMaxHp,
      userData.playerCurrentHp,
      userData.playerMaxFp,
      userData.playerCurrentFp
    );
    // 名前は直打ち
    playerButton.GetComponent<TargetButtonInfoContainer> ().TargetName = "player";
  }

  void SetupTargetPartnerButton (MyGameData.MyData userData)
  {
    // 仲間の数だけボタンをつくる
    Sprite[] partnerSpriteArray = Resources.LoadAll<Sprite> (charactorImgFolderName);

    string[] partnerNameArr = userData.partnersName;
    for (var i = 0; i < partnerNameArr.Length; i++)
    {
      // ボタン作成
      GameObject newParterButton = GameObject.Instantiate<GameObject> (buttonForRecoveringTargets);
      newParterButton.transform.SetParent (recoveringTargetListContainer);
      float baseScale = 1.0f;
      newParterButton.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
      // ボタンの中身追加
      string partnerName = partnerNameArr[i];
      Transform partnerNameText = newParterButton.transform.Find ("NameText");
      partnerNameText.GetComponent<Text> ().text = partnerName;
      Transform partnerImg = newParterButton.transform.Find ("CharaImg");
      // Debug.Log ("メニュー側 partnerName : " + partnerName);
      partnerImg.GetComponent<Image> ().sprite = menuCommonFunctions.GetPartnerButtonSprite (partnerName, partnerDataArray, partnerSpriteArray);
      SetupPartnerButtonHpInformation (newParterButton, userData, partnerName);
      // imgの名前をボタンの名前として使用(アルファベットなので)
      newParterButton.GetComponent<TargetButtonInfoContainer> ().TargetName = GetPartnerImgName (partnerName, partnerDataArray, userData);
    }
  }

  void SetupPartnerButtonHpInformation (GameObject partnerButton, MyGameData.MyData userData, string partnerName)
  {
    Transform partnerHp = partnerButton.transform.Find ("Hp");
    int partnerLevel = GetPartnerLevel (partnerDataArray, userData);
    int partnerCurrentHp = GetPartnerCurrentHp (partnerDataArray, userData);
    int partnerMaxHp = menuCommonFunctions.GetPartnerMaxHp (partnerName, partnerLevel, partnerDataArray);
    partnerHp.transform.Find ("CurrentHpText").GetComponent<Text> ().text = partnerCurrentHp.ToString ();
    partnerHp.transform.Find ("MaxHpText").GetComponent<Text> ().text = partnerMaxHp.ToString ();
    SetupTargetButtonsIsSelectable (partnerButton, partnerMaxHp, partnerCurrentHp);
  }

  public void UpDateTargetPartnerButtonInformation (MyGameData.MyData userData)
  {
    string[] partnerNameArr = userData.partnersName;
    // プレイヤーボタンの1つ分
    for (var i = 0; i < partnerNameArr.Length; i++)
    {
      string partnerName = partnerNameArr[i];
      GameObject recoveringTargetButton = recoveringTargetListContainer.transform.GetChild (i + 1).gameObject;
      SetupPartnerButtonHpInformation (recoveringTargetButton, userData, partnerName);
    }
  }

  int GetPartnerLevel (PartnerDataArray partnerDataArray, MyGameData.MyData userData)
  {
    int level = 1;
    foreach (var partner in partnerDataArray.gamePartners)
    {
      if (partner.imgFileName == PartnerNames.sakura.ToString ())
      {
        level = userData.sakuraLevel;
      }
    }
    return level;
  }

  int GetPartnerCurrentHp (PartnerDataArray partnerDataArray, MyGameData.MyData userData)
  {
    int currentHp = 1;
    foreach (var partner in partnerDataArray.gamePartners)
    {
      if (partner.imgFileName == PartnerNames.sakura.ToString ())
      {
        currentHp = userData.sakuraCurrentHp;
      }
    }
    return currentHp;
  }

  string GetPartnerImgName (string partnerName, PartnerDataArray partnerDataArray, MyGameData.MyData userData)
  {
    string imgName = "";
    foreach (var partner in partnerDataArray.gamePartners)
    {
      if (partnerName == partner.name)
      {
        imgName = partner.imgFileName;
      }
    }
    return imgName;
  }

  void SetupTargetButtonsIsSelectable (GameObject targetButton, int maxHp, int currentHp, int maxFp = 0, int currentFp = 0)
  {
    // Debug.Log ("maxFp : " + maxFp);
    // Debug.Log ("currentFp : " + currentFp);
    TargetButtonInfoContainer targetButtonInfoContainer = targetButton.GetComponent<TargetButtonInfoContainer> ();
    if (maxHp == currentHp)
    {
      // Debug.Log ("HP回復できないよ");
      targetButtonInfoContainer.IsSelectableHp = false;
    }
    if (maxFp == currentFp)
    {
      // Debug.Log ("FP回復できないよ");
      targetButtonInfoContainer.IsSelectableFp = false;
    }
  }
}