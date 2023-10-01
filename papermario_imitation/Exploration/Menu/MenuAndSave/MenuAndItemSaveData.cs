using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAndItemSaveData
{
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private List<GameObject> targetListViewButtonsList;
  private List<GameObject> itemListViewButtonsList;
  private PartnerDataArray partnerDataArray;
  private GameObject itemListContainer;
  private GameObject recoveringTargetListContainer;
  enum TargetButtonName
  {
    player,
    sakura
  }
  public MenuAndItemSaveData (
    GameObject itemListContainer,
    GameObject recoveringTargetListContainer
  )
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    targetListViewButtonsList = new List<GameObject> ();
    itemListViewButtonsList = new List<GameObject> ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    this.itemListContainer = itemListContainer;
    this.recoveringTargetListContainer = recoveringTargetListContainer;
    partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
  }
  public void ManageRecoveryByUsingItem ()
  {
    SetupButtonLists ();
    bool isSelectableTargetSelected =
      menuCommonFunctions.GetIsSelectableTargetButtonSelected (targetListViewButtonsList, itemListViewButtonsList);
    if (!isSelectableTargetSelected) return;
    // Debug.Log ("かいふくできたよー");
    int selectedItemButtonNum = menuCommonFunctions.GetSelectedItemButtonNum (itemListViewButtonsList);
    // Debug.Log ("selectedItemButtonNum : " + selectedItemButtonNum);
    GameObject selectedItemButton = itemListViewButtonsList[selectedItemButtonNum];
    // 回復対象のtype,amountを取得
    string selectedItemType = selectedItemButton.GetComponent<BelongingButtonInfoContainer> ().Type;
    int selectedItemAmount = selectedItemButton.GetComponent<BelongingButtonInfoContainer> ().Amount;
    GameObject selectedTargetButton = menuCommonFunctions.GetSelectedRecoveringTargetButton (targetListViewButtonsList);
    string targetName = selectedTargetButton.GetComponent<TargetButtonInfoContainer> ().TargetName;
    UpdateStatusData (targetName, selectedItemType, selectedItemAmount, selectedItemButtonNum);
  }

  void SetupButtonLists ()
  {
    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
    targetListViewButtonsList = menuCommonFunctions.GetChildList (recoveringTargetListContainer);
  }

  void UpdateStatusData (string targetName, string selectedItemType, int selectedItemAmount, int selectedItemButtonNum)
  {
    if (targetName == TargetButtonName.player.ToString ())
    {
      UpdatePlayerStatusData (selectedItemType, selectedItemAmount, selectedItemButtonNum);
    }
    else if (targetName == TargetButtonName.sakura.ToString ())
    {
      UpdateSakuraStatusData (targetName, selectedItemAmount, selectedItemButtonNum);
    }
    // saveDataにあるhavingItem配列からhoge番目のデータを削除する
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    SaveSystem.Instance.userData.havingItemsName =
      menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (havingItemNameArr, selectedItemButtonNum);
    // セーブ
    // SaveSystem.Instance.Save ();
  }

  void UpdatePlayerStatusData (string selectedItemType, int selectedItemAmount, int selectedItemButtonNum)
  {
    if (selectedItemType == BelongingButtonInfoContainer.State.recoverHp.ToString ())
    {
      RecoverPlayerCurretHp (selectedItemAmount);
    }
    else if (selectedItemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      RecoverPlayerCurretFp (selectedItemAmount);
    }
  }
  void RecoverPlayerCurretHp (int amount)
  {
    var userData = SaveSystem.Instance.userData;
    int currentHp = userData.playerCurrentHp;
    // amount分だけcurrent値に足して、最大でmaxHp,MaxFpの値にする
    int newerCurrentHp = System.Math.Min ((currentHp + amount), userData.playerMaxHp);
    userData.playerCurrentHp = newerCurrentHp;
  }
  void RecoverPlayerCurretFp (int amount)
  {
    var userData = SaveSystem.Instance.userData;
    int currentFp = userData.playerCurrentFp;
    int newerCurrentFp = System.Math.Min ((currentFp + amount), userData.playerMaxFp);
    userData.playerCurrentFp = newerCurrentFp;
  }

  void UpdateSakuraStatusData (string sakuraName, int selectedItemAmount, int selectedItemButtonNum)
  {
    var userData = SaveSystem.Instance.userData;
    int currentHp = userData.sakuraCurrentHp;
    // Debug.Log ("currentPartnerHp : " + currentHp);
    // amount分だけcurrent値に足して、最大でmaxHp,MaxFpの値にする
    int baseHp = menuCommonFunctions.GetPartnerBaseHp (sakuraName, partnerDataArray);
    // Debug.Log ("回復量 : " + selectedItemAmount);
    int newerCurrentHp = System.Math.Min ((currentHp + selectedItemAmount), userData.sakuraLevel * baseHp);
    // Debug.Log ("newerCurrentPartnerHp : " + newerCurrentHp);
    userData.sakuraCurrentHp = newerCurrentHp;
  }

}