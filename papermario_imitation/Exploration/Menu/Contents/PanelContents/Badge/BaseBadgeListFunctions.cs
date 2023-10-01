using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class BaseBadgeListFunctions
{
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private MenuCommonFunctions menuCommonFunctions;
  private BaseMenuListPreparer baseMenuListPreparer;
  private BadgeDataArray badgeDataArray;
  public BaseBadgeListFunctions ()
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    baseMenuListPreparer = new BaseMenuListPreparer ();
  }

  public void SetupHavingBadgeListButton (
    GameObject newButton,
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    Transform buttonListContainerTra,
    Sprite buttonSprite,
    string imgGameObjectName,
    string nameTextGameObjectName,
    string havingBadgeName,
    string badgePointInfoGameObjectName,
    string badgePointCostTextInfoGameObjectName,
    string badgeCostImgContainersName,
    string badgePointEmptyCostImgContainerName,
    string badgePointFullCostImgContainerName
  )
  {
    // TransformからのFind()は孫を探せない。でもGameObject.Findは遅いので使わない
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    badgeDataArray = jsonReaderFromResourcesFolder.GetBadgeDataArray ("JSON/GameBadgesData");
    Transform buttonPointInfo = newButton.transform.Find (badgePointInfoGameObjectName);
    Transform buttonCostImgContainers = buttonPointInfo.Find (badgeCostImgContainersName);
    Transform buttonEmptyCostImgContainer = buttonCostImgContainers.transform.Find (badgePointEmptyCostImgContainerName);
    Transform buttonFullCostImgContainer = buttonCostImgContainers.transform.Find (badgePointFullCostImgContainerName);
    baseMenuListPreparer.SetupListButtonBaseInformation (
      newButton,
      buttonListContainerTra,
      buttonSprite,
      havingBadgeName,
      imgGameObjectName,
      nameTextGameObjectName
    );
    // バッジ名からデータの取得
    SetupBadgeListButtonCostText (
      newButton,
      havingBadgeName,
      badgePointInfoGameObjectName,
      badgePointCostTextInfoGameObjectName
    );
    // コスト数を〇で表示
    SetupBadgeListButtonCostImg (
      newButton,
      badgeEmptyCostImage,
      badgeFullCostImage,
      buttonEmptyCostImgContainer,
      buttonFullCostImgContainer,
      havingBadgeName
    );
    // バッジのインフォコンテナに個々の情報を渡す
    SetupBadgeInfoContainer (newButton, badgeDataArray, havingBadgeName);
  }

  // バッジのコストを示す数字のゲームオブジェクトの名前
  public void SetupBadgeListButtonCostText (
    GameObject newButton,
    string badgeName,
    string badgePointInfoGameObjectName,
    string badgePointCostInfoGameObjectName
  )
  {
    JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    BadgeDataArray badgeDataArray = jsonReaderFromResourcesFolder.GetBadgeDataArray ("JSON/GameBadgesData");
    Transform buttonPointInfo = newButton.transform.Find (badgePointInfoGameObjectName);
    Transform buttonCostText = buttonPointInfo.transform.Find (badgePointCostInfoGameObjectName);
    // バッジ名がデータを一致した場合にテキスト変更
    foreach (var badgeData in badgeDataArray.gameBadges)
    {
      if (badgeName == badgeData.name)
      {
        int badgeCost = badgeData.cost;
        buttonCostText.GetComponent<Text> ().text = badgeCost.ToString ();
      }
    }
  }

  public void SetupBadgeListButtonCostImg (
    GameObject newButton,
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    Transform buttonEmptyCostImgContainer,
    Transform buttonFullCostImgContainer,
    string badgeName
  )
  {
    ScreenSizeInformation screenSizeInformation = new ScreenSizeInformation ();
    JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    BadgeDataArray badgeDataArray = jsonReaderFromResourcesFolder.GetBadgeDataArray ("JSON/GameBadgesData");
    foreach (var badgeData in badgeDataArray.gameBadges)
    {
      if (badgeName == badgeData.name)
      {
        int badgeCost = badgeData.cost;
        // BadgeCostImgsゲームオブジェクトの子要素としてfor文で生成
        for (var i = 0; i < badgeCost; i++)
        {
          GameObject newEmptyCostImg = GameObject.Instantiate<GameObject> (badgeEmptyCostImage);
          newEmptyCostImg.transform.SetParent (buttonEmptyCostImgContainer);
          GameObject newFullCostImg = GameObject.Instantiate<GameObject> (badgeFullCostImage);
          newFullCostImg.transform.SetParent (buttonFullCostImgContainer);
          float baseScale = 1.0f;
          newEmptyCostImg.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
          newFullCostImg.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
          Vector3 costPosi = newFullCostImg.transform.localPosition;
          float littleRight = 60.0f;
          float costFirstPosiX = (costPosi.x + littleRight);
          // 等間隔で並べる処理は画面サイズに合わせた変更を自分でする必要があるかも？ 考え直すかもなのでコメントアウト
          // Debug.Log ("screenSizeInformation.ScreenSizeRatio : " + screenSizeInformation.ScreenSizeRatio);
          // Debug.Log ("newFullCostImg" + newFullCostImg.GetComponent<RectTransform> ().sizeDelta.x);
          // float gap = newFullCostImg.GetComponent<RectTransform> ().sizeDelta.x * screenSizeInformation.ScreenSizeRatio
          float gap = newFullCostImg.GetComponent<RectTransform> ().sizeDelta.x;
          newEmptyCostImg.transform.localPosition = new Vector3 (costFirstPosiX + gap * i, costPosi.y, costPosi.z);
          newFullCostImg.transform.localPosition = new Vector3 (costFirstPosiX + gap * i, costPosi.y, costPosi.z);
        }
      }
    }
  }

  public void SetupBadgeEquippedBadgeButtonInformation (
    Transform allBadgeListContainerTra,
    string puttingBadgeSignGameObjectName,
    string badgePointInfoGameObjectName,
    string badgeCostImgContainersName,
    string badgePointFullCostImgContainerName
  )
  {
    List<GameObject> allHavingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (allBadgeListContainerTra.gameObject);
    // Debug.Log ("せっていする側での長さ確認 : " + allHavingBadgeListViewButtonsList.Count);

    // puttingBadgeIdの初期化
    InitPuttingBadgeIds ();
    // equippingBadgeでIdのズレがでないようにソートする
    SortPuttingBadgeNums ();
    int[] puttingBadgeNumArr = SaveSystem.Instance.userData.puttingBadgeNums;
    // puttingBadgeNumArrは持っているバッジの名前リストのうち、何番目のバッジをつけているかを配列で保存している : [0番目, 3番目]
    for (var i = 0; i < allHavingBadgeListViewButtonsList.Count; i++)
    {
      GameObject equippedBadgeButton = allHavingBadgeListViewButtonsList[i];
      // puttingBadgeSignとbuttonFullCostImgContainerが必要なのでFind()
      Transform puttingBadgeSign = equippedBadgeButton.transform.Find (puttingBadgeSignGameObjectName);
      Transform buttonPointInfo = equippedBadgeButton.transform.Find (badgePointInfoGameObjectName);
      Transform buttonCostImgContainers = buttonPointInfo.Find (badgeCostImgContainersName);
      Transform buttonFullCostImgContainer = buttonCostImgContainers.transform.Find (badgePointFullCostImgContainerName);
      // すべてのバッジボタンの装着サインを無効にする
      puttingBadgeSign.gameObject.SetActive (false);
      buttonFullCostImgContainer.gameObject.SetActive (false);
      equippedBadgeButton.GetComponent<BadgeButtonInfoContainer> ().IsEquipped = false;
      for (var j = 0; j < puttingBadgeNumArr.Length; j++)
      {
        // つけているバッジの番号と一致した場合
        int havingButtonNum = puttingBadgeNumArr[j];
        if (i == havingButtonNum)
        {
          // Debug.Log ("havingButtonNum : " + havingButtonNum);
          // 装着しているバッジボタンの装着サインのみ有効にする
          // つけているバッジボタンがもつ特定のGameObjectをActiveにする
          puttingBadgeSign.gameObject.SetActive (true);
          buttonFullCostImgContainer.gameObject.SetActive (true);
          // つけているバッジボタンのIsEquippedをtrueにする
          equippedBadgeButton.GetComponent<BadgeButtonInfoContainer> ().IsEquipped = true;
          // Debug.Log ("かくにんさせてー : " + equippedBadgeButton.GetComponent<BadgeButtonInfoContainer> ().BadgeName + equippedBadgeButton.GetComponent<BadgeButtonInfoContainer> ().IsEquipped);
          // つけているバッジのbadgeButtonにBadgeIdをもたせる
          // セーブデータには新たにBadgeIdがputtingBadgeIdに追加されている
          equippedBadgeButton.GetComponent<BadgeButtonInfoContainer> ().BadgeId = GetPseudoRandomNumId ();
        }
      }
    }

    // テスト バッジリストの状態確認
    // Debug.Log ("allHavingBadgeListViewButtonsList.Count : " + allHavingBadgeListViewButtonsList.Count);
    // foreach (var havingBadge in allHavingBadgeListViewButtonsList)
    // {
    //   BadgeButtonInfoContainer badgeButtonInfoContainer = havingBadge.GetComponent<BadgeButtonInfoContainer> ();
    //   // Debug.Log ("badgeButtonInfoContainer.BadgeName " + badgeButtonInfoContainer.BadgeName + badgeButtonInfoContainer.IsEquipped);
    // }
  }

  public Transform DeleteButtonFromListViewForHavingBadge (Transform buttonListContainerTra)
  {
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    List<GameObject> badgeListViewButtonsList = menuCommonFunctions.GetChildList (buttonListContainerTra.gameObject);
    if (badgeListViewButtonsList.Count == 0) return buttonListContainerTra;
    for (var i = 0; i < badgeListViewButtonsList.Count; i++)
    {
      GameObject badgeButton = buttonListContainerTra.GetChild (i).gameObject;
      MonoBehaviour.Destroy (badgeButton);
    }
    // Debug.Log ("ケシタアトの数 : " + menuCommonFunctions.GetChildList (buttonListContainerTra.gameObject).Count);
    return buttonListContainerTra;
  }

  void InitPuttingBadgeIds ()
  {
    int[] empty = { };
    SaveSystem.Instance.userData.puttingBadgeId = empty;
    // SaveSystem.Instance.Save ();
  }

  void SortPuttingBadgeNums ()
  {
    int[] puttingBadgeNumArr = SaveSystem.Instance.userData.puttingBadgeNums;
    List<int> sortedNumsList = new List<int> ();
    sortedNumsList.AddRange (puttingBadgeNumArr);
    sortedNumsList.Sort ();
    SaveSystem.Instance.userData.puttingBadgeNums = sortedNumsList.ToArray ();
  }

  public void SetupBadgeInfoContainer (GameObject newButton, BadgeDataArray badgeDataArray, string badgeName)
  {
    // バッジの情報をコンテナに保持させる
    BadgeButtonInfoContainer badgeButtonInfoContainer = newButton.GetComponent<BadgeButtonInfoContainer> ();
    badgeButtonInfoContainer.BadgeName = badgeName;
    foreach (var badge in badgeDataArray.gameBadges)
    {
      if (badgeName == badge.name)
      {
        badgeButtonInfoContainer.Description = badge.description;
        badgeButtonInfoContainer.Type = badge.type;
        badgeButtonInfoContainer.Amount = badge.amount;
        badgeButtonInfoContainer.Cost = badge.cost;
      }
    }
  }

  public void SetupBadgeFullConstImgActivity (Transform costImgParentTra, int usingBadgeCost)
  {
    List<GameObject> fullCostImgList =
      menuCommonFunctions.GetChildList (costImgParentTra.gameObject);
    for (var i = 0; i < fullCostImgList.Count; i++)
    {
      GameObject fllCostImg = fullCostImgList[i];
      if (usingBadgeCost >= i + 1)
      {
        fllCostImg.SetActive (true);
      }
      else
      {
        fllCostImg.SetActive (false);
      }
    }
  }

  public int GetSelectedBadgeButtonNum (List<GameObject> listViewButtonsList, GameObject selectedButton)
  {
    int selectedButtonNum = 0;
    for (var i = 0; i < listViewButtonsList.Count; i++)
    {
      int number = i;
      GameObject listButton = listViewButtonsList[i];
      if (selectedButton == listButton)
      {
        selectedButtonNum = number;
      }
    }
    return selectedButtonNum;
  }

  public int GetTotalEquippingBadgeCost (GameObject allHavingBadgeListContainer)
  {
    int badgeCost = 0;
    int[] puttingBadgeNums = SaveSystem.Instance.userData.puttingBadgeNums;
    if (puttingBadgeNums.Length == 0) return badgeCost;
    List<GameObject> allHavingBadgeListViewButtonsList = menuCommonFunctions.GetChildList (allHavingBadgeListContainer);
    foreach (var puttingBadgeNum in puttingBadgeNums)
    {
      for (var i = 0; i < allHavingBadgeListViewButtonsList.Count; i++)
      {
        int badgeNum = i;
        if (puttingBadgeNum == badgeNum)
        {
          // Debug.Log ("badgeNum : " + badgeNum);
          GameObject equippingBadge = allHavingBadgeListViewButtonsList[badgeNum];
          BadgeButtonInfoContainer badgeButtonInfoContainer = equippingBadge.GetComponent<BadgeButtonInfoContainer> ();
          // Debug.Log ("badgeButtonInfoContainer.BadgeName : " + badgeButtonInfoContainer.BadgeName);
          badgeCost += badgeButtonInfoContainer.Cost;
        }
      }
    }
    // Debug.Log ("badgeCost : " + badgeCost);
    return badgeCost;
  }

  public void AddPrevBadgeId (int prevBadgeId)
  {
    int[] currentUsedIds = SaveSystem.Instance.userData.puttingBadgeId;
    int[] newerUsedIds = currentUsedIds.Concat (new int[] { prevBadgeId }).ToArray ();
    SaveSystem.Instance.userData.puttingBadgeId = newerUsedIds;
    // SaveSystem.Instance.Save ();
  }

  public int GetPseudoRandomNumId ()
  {
    int[] exceptionNums = SaveSystem.Instance.userData.puttingBadgeId;
    int randomNumId;
    // 登録するIDの候補リスト
    int randomNumRange = 200;
    List<int> rangeNums = menuCommonFunctions.GetRandomIntNumList (randomNumRange);
    // 削除候補リスト
    List<int> save = new List<int> ();
    // セーブデータに登録されている数字を探す
    foreach (var rangeNum in rangeNums)
    {
      foreach (var exceptionNum in exceptionNums)
      {
        if (rangeNum == exceptionNum)
        {
          save.Add (exceptionNum);
        }
      }
    }
    // 既に登録された数字を候補から削除
    foreach (var removeItem in save)
    {
      rangeNums.Remove (removeItem);
    }
    if (rangeNums.Count == 0)
    {
      // Debug.LogError ("バッジの数がIdの候補数を上回っています");
      return randomNumId = 0;
    }
    // 候補の数からランダムに選択
    // Idを持たないバッジはデフォルトでId=0となるので、randomNumIdの候補に0は入れずに1から始める
    randomNumId = rangeNums[Random.Range (1, rangeNums.Count)];
    // 今回選ばれたrandomNumIdは次のGetPseudoRandomNumIdで選ばれないようにデータを上書きしておく
    UpdateUsedBadgeIdArray (randomNumId);
    return randomNumId;
  }

  void UpdateUsedBadgeIdArray (int newId)
  {
    int[] currentUsedIds = SaveSystem.Instance.userData.puttingBadgeId;
    int[] newerUsedIds = currentUsedIds.Concat (new int[] { newId }).ToArray ();
    // List<int> newerUsedIdsList = new List<int> ();
    // newerUsedIdsList.AddRange (newerUsedIds);
    // newerUsedIdsList.Sort ();
    SaveSystem.Instance.userData.puttingBadgeId = newerUsedIds;
    // SaveSystem.Instance.Save ();
  }

}