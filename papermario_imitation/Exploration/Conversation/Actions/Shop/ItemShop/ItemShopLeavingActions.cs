using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShopLeavingActions
{
  private ConversationContentsManager conversationContentsManager;
  private ItemShopConversationActions itemShopConversationActions;
  private MenuCommonFunctions menuCommonFunctions;
  private string selectedItemName;
  private int selectedItemButtonNum;
  private int canLeavingItemNum;

  public string SelectedItemName
  {
    set { selectedItemName = value; }
  }
  public int SelectedItemButtonNum
  {
    set { selectedItemButtonNum = value; }
  }
  public int CanLeavingItemNum
  {
    get { return canLeavingItemNum; }
  }

  public ItemShopLeavingActions (
    ConversationContentsManager conversationContentsManager
  )
  {
    this.conversationContentsManager = conversationContentsManager;
    menuCommonFunctions = new MenuCommonFunctions ();
    itemShopConversationActions = new ItemShopConversationActions (conversationContentsManager);
  }
  public void TryToSelectLeavingItem ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    int havingItemNum = saveData.havingItemsName.Length;
    int leavingItemNum = saveData.leavingItemsName.Length;
    int leavingItemsNumRestriction = saveData.leavingItemsNumRestriction;
    if (havingItemNum > 0 && leavingItemNum < leavingItemsNumRestriction)
    {
      // のこり預けられるアイテム数表示に必要
      canLeavingItemNum = leavingItemsNumRestriction - leavingItemNum;
      GoToLeavingItemScene ();
    }
    else if (havingItemNum <= 0)
    {
      itemShopConversationActions.GoToHavingNoItemScene ();
    }
    else if (leavingItemNum >= leavingItemsNumRestriction)
    {
      GoToCanNotLeavingItemScene ();
    }
  }

  public void GoToLeavingItemScene ()
  {
    string selectingLeavingItemScene = "012";
    conversationContentsManager.SetScene (selectingLeavingItemScene);
  }
  public void GoToLeftItemScene ()
  {
    string leftItemScene = "013";
    conversationContentsManager.SetScene (leftItemScene);
  }
  public void GoToCanNotLeavingItemScene ()
  {
    string canNotLeavingItemScene = "014";
    conversationContentsManager.SetScene (canNotLeavingItemScene);
  }
  public void ChangeConversationStateToLeavingItem ()
  {
    conversationContentsManager.GameConversationState =
      ConversationContentsManager.ConversationState.leavingItem;
  }

  public void LeavingMyItem ()
  {
    // 選ばれたアイテムを持っているアイテム一覧から削除
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    SaveSystem.Instance.userData.havingItemsName =
      menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (havingItemNameArr, selectedItemButtonNum);
    // 保存アイテム一覧にアイテムを追加
    string[] leavingItemNameArr = SaveSystem.Instance.userData.leavingItemsName;
    SaveSystem.Instance.userData.leavingItemsName = leavingItemNameArr.Concat (new string[] { selectedItemName }).ToArray ();
    // データの上書き
    // SaveSystem.Instance.Save ();
    GoToLeftItemScene ();
  }

}