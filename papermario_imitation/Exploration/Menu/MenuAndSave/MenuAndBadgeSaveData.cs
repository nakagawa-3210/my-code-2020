using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuAndBadgeSaveData
{
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private MenuCommonFunctions menuCommonFunctions;

  public MenuAndBadgeSaveData ()
  {
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    menuCommonFunctions = new MenuCommonFunctions ();
  }

  public void ManageEquippingBadgeFromAll (GameObject badgeListContainer)
  {
    List<GameObject> badgeListViewButtonsList =
      menuCommonFunctions.GetChildList (badgeListContainer);
    foreach (var havingBadge in badgeListViewButtonsList)
    {
      if (havingBadge == null) return;
      BadgeButtonInfoContainer badgeButtonInfoContainer = havingBadge.GetComponent<BadgeButtonInfoContainer> ();
      // ボタンが押されて
      if (badgeButtonInfoContainer.IsSelected)
      {
        // 装備されていたら
        if (badgeButtonInfoContainer.IsEquipped)
        {
          badgeButtonInfoContainer.IsEquipped = false;
          // saveData
          int[] exceptionNums = SaveSystem.Instance.userData.puttingBadgeId;
          int selectedButtonNum =
            baseBadgeListFunctions.GetSelectedBadgeButtonNum (badgeListViewButtonsList, havingBadge);
          // selectedButtonNumを使い、puttingBadgeNumsから一致する数字を削除する
          RemoveSelectedButtonNumFromPuttingBadgeNums (selectedButtonNum);
          // 削除するタイミングは
          // puttingBadgeIdからバッジボタンの持つBadgeIdを探し、一致した数字を削除する
          int badgeId = badgeButtonInfoContainer.BadgeId;
          RemoveSelectedButtonBadgeIdFromPuttingBadgeId (badgeId);
        }
        else
        {
          // プレイヤーのバッジポイント上限を確認する
          int playerTotalBadgePoint = SaveSystem.Instance.userData.playerMaxBp;
          // プレイヤーの装備中のバッジの総ポイントを確認する
          // 装備中のバッジIdをもとに、バッジボタンリストから装備中のバッジを探す
          // 見つけた装備中すべてのバッジボタンのコストと、選択されたバッジボタンのコストの和を求める
          // プレイヤーのバッジポイント上限がコストの和以上の時にIsEquipped=true
          int totalCurrentBadgeCost = baseBadgeListFunctions.GetTotalEquippingBadgeCost (badgeListContainer);
          // Debug.Log ("totalCurrentBadgeCost : " + totalCurrentBadgeCost);
          bool canEquip = CanBadgeEquip (badgeButtonInfoContainer.Cost, totalCurrentBadgeCost);
          if (canEquip)
          {
            badgeButtonInfoContainer.IsEquipped = true;
            // Id取得時にデータにBadgeIdが加えられている
            badgeButtonInfoContainer.BadgeId = baseBadgeListFunctions.GetPseudoRandomNumId ();
            int selectedButtonNum =
              baseBadgeListFunctions.GetSelectedBadgeButtonNum (badgeListViewButtonsList, havingBadge);
            // puttingBadgeNumsはここでデータに書き加える
            AddSelectedButtonNumToPuttingBadgeNums (selectedButtonNum);
          }
        }
        badgeButtonInfoContainer.IsSelected = false;
      }
    }
  }

  // 選択中のバッジリストの場合はすべてのバッジリストと異なり、
  // puttingBadgeNumsに登録する数字は選択したボタンの数字ではなく、
  // 選択したボタンの持つIdと同じIdを持つボタンをすべてのバッジリストから検索して、
  // 該当するバッジの数字を登録する
  public void ManageEquippingBadgeFromEquipping (
    GameObject equippingBadgeListContainer,
    GameObject allHavingBadgeListContainer
  )
  {
    List<GameObject> badgeListViewButtonsList =
      menuCommonFunctions.GetChildList (equippingBadgeListContainer);
    foreach (var havingBadge in badgeListViewButtonsList)
    {
      if (havingBadge == null) return;
      BadgeButtonInfoContainer badgeButtonInfoContainer = havingBadge.GetComponent<BadgeButtonInfoContainer> ();
      if (badgeButtonInfoContainer.IsSelected)
      {
        // 装備を外す処理
        if (badgeButtonInfoContainer.IsEquipped)
        {
          badgeButtonInfoContainer.IsEquipped = false;
          int badgeId = badgeButtonInfoContainer.BadgeId;
          // 再度つけているバッジリスト内でつけられた時のためにIdを退避
          badgeButtonInfoContainer.PrevBadgeId = badgeId;
          int badgeNum = badgeButtonInfoContainer.PrevEquippingBadgeNum;
          RemoveSelectedButtonNumFromPuttingBadgeNums (badgeNum);
          RemoveSelectedButtonBadgeIdFromPuttingBadgeId (badgeId);
        }
        // 装備をつける(つけなおす)処理
        else
        {
          badgeButtonInfoContainer.IsEquipped = true;
          int prevBadgeId = badgeButtonInfoContainer.PrevBadgeId;
          int prevEquippingBadgeNum = badgeButtonInfoContainer.PrevEquippingBadgeNum;
          baseBadgeListFunctions.AddPrevBadgeId (prevBadgeId);
          AddSelectedButtonNumToPuttingBadgeNums (prevEquippingBadgeNum);
        }
        badgeButtonInfoContainer.IsSelected = false;
      }
    }
  }

  void RemoveEquippingBadgeNumBasedOnAllBadgeLists (int selectedBadgeId, GameObject allHavingBadgeListContainer)
  {
    int[] equippingBadgeNums = SaveSystem.Instance.userData.puttingBadgeNums;
    List<GameObject> allBadgeList = menuCommonFunctions.GetChildList (allHavingBadgeListContainer);
    for (var i = 0; i < allBadgeList.Count; i++)
    {
      int badgeIdFromAll = allBadgeList[i].GetComponent<BadgeButtonInfoContainer> ().BadgeId;
      if (selectedBadgeId == badgeIdFromAll)
      {
        // Debug.Log ("さくじょするbadgeId : " + selectedBadgeId);
        int badgeNum = i;
        foreach (var equippingBadgeNum in equippingBadgeNums)
        {
          {
            // 該当したBadgeNumを削除して新しくセーブする
            // Debug.Log ("さくじょするbadgeId : " + badgeNum);
            RemoveEquippingBadgeNums (badgeNum);
          }
        }
      }
    }
  }
  void RemoveEquippingBadgeNums (int removedNum)
  {
    int[] equippingBadgeNums = SaveSystem.Instance.userData.puttingBadgeNums;
    List<int> newEquippingBadgeNums = new List<int> ();
    foreach (var num in equippingBadgeNums)
    {
      if (removedNum != num)
      {
        newEquippingBadgeNums.Add (num);
      }
    }
    SaveSystem.Instance.userData.puttingBadgeNums = newEquippingBadgeNums.ToArray ();
    // SaveSystem.Instance.Save ();
  }

  bool CanBadgeEquip (int selectedBadgeCost, int totalCurrentEquippingBadgeCost)
  {
    bool canEquip = false;
    int badgeCostLimit = SaveSystem.Instance.userData.playerMaxBp;
    // Debug.Log ("selectedBadgeCost : " + selectedBadgeCost);
    // Debug.Log ("totalCurrentEquippingBadgeCost : " + totalCurrentEquippingBadgeCost);
    if (badgeCostLimit >= (selectedBadgeCost + totalCurrentEquippingBadgeCost))
    {
      canEquip = true;
    }
    return canEquip;
  }

  void AddSelectedButtonNumToPuttingBadgeNums (int selectedButtonNum)
  {
    int[] currentPuttingBadgeNums = SaveSystem.Instance.userData.puttingBadgeNums;
    int[] newerPuttingBadgeNums = currentPuttingBadgeNums.Concat (new int[] { selectedButtonNum }).ToArray ();
    List<int> newerPuttingBadgeNumsList = new List<int> ();
    newerPuttingBadgeNumsList.AddRange (newerPuttingBadgeNums);
    newerPuttingBadgeNumsList.Sort ();
    // Debug.Log ("newerPuttingBadgeNums,Len : " + newerPuttingBadgeNums.Length);
    // SaveSystem.Instance.userData.puttingBadgeNums = newerPuttingBadgeNums;
    SaveSystem.Instance.userData.puttingBadgeNums = newerPuttingBadgeNumsList.ToArray ();
    // SaveSystem.Instance.Save ();
  }

  // 使わなくなったけどまた使うかもなのでコメントアウト
  // void AddSelectedButtonNumFromPuttingBadgeId (int badgeId, GameObject allHavingBadgeListContainer)
  // {
  //   int badgeNum = 0;
  //   // 同じバッジIdのボタンがすべてのバッジリストで何番目にあるのか取得する
  //   List<GameObject> allhavingBadgeList = menuCommonFunctions.GetChildList (allHavingBadgeListContainer);
  //   foreach (var havingBadge in allhavingBadgeList)
  //   {
  //     BadgeButtonInfoContainer havingBadgeButtonInfoContainer = havingBadge.GetComponent<BadgeButtonInfoContainer> ();
  //     int havingBadgeId = havingBadgeButtonInfoContainer.BadgeId;
  //     if (badgeId == havingBadgeId)
  //     {
  //       badgeNum = havingBadgeButtonInfoContainer.PrevEquippingBadgeNum;
  //     }
  //   }
  //   AddSelectedButtonNumToPuttingBadgeNums (badgeNum);
  // }

  void RemoveSelectedButtonBadgeIdFromPuttingBadgeId (int badgeButtonId)
  {
    int[] currentPuttingBadgeIds = SaveSystem.Instance.userData.puttingBadgeId;
    List<int> editablePuttingBadgeIds = new List<int> ();
    editablePuttingBadgeIds.AddRange (currentPuttingBadgeIds);
    foreach (var badgeId in currentPuttingBadgeIds)
    {
      if (badgeButtonId == badgeId)
      {
        editablePuttingBadgeIds.Remove (badgeButtonId);
      }
    }
    int[] newerPuttingBadgeIds = editablePuttingBadgeIds.ToArray ();
    SaveSystem.Instance.userData.puttingBadgeId = newerPuttingBadgeIds;
    // SaveSystem.Instance.Save ();
  }

  void RemoveSelectedButtonNumFromPuttingBadgeNums (int selectedButtonNum)
  {
    // バッジボタン配列の何番目
    int[] equippingBadgeNums = SaveSystem.Instance.userData.puttingBadgeNums;
    List<int> editableBadgeNums = new List<int> ();
    editableBadgeNums.AddRange (equippingBadgeNums);
    foreach (var badgeNum in equippingBadgeNums)
    {
      if (selectedButtonNum == badgeNum)
      {
        editableBadgeNums.Remove (selectedButtonNum);
      }
    }
    int[] newerPuttingBadgeNums = editableBadgeNums.ToArray ();
    SaveSystem.Instance.userData.puttingBadgeNums = newerPuttingBadgeNums;
    // SaveSystem.Instance.Save ();
  }

}