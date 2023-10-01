using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShopGettingActions
{
  private ConversationContentsManager conversationContentsManager;
  private ItemShopConversationActions itemShopConversationActions;
  private MenuCommonFunctions menuCommonFunctions;
  private string selectedItemName;
  private int selectedItemButtonNum;
  public string SelectedItemName
  {
    set { selectedItemName = value; }
  }
  public int SelectedItemButtonNum
  {
    set { selectedItemButtonNum = value; }
  }
  public ItemShopGettingActions (
    ConversationContentsManager conversationContentsManager
  )
  {
    this.conversationContentsManager = conversationContentsManager;
    menuCommonFunctions = new MenuCommonFunctions ();
    itemShopConversationActions = new ItemShopConversationActions (conversationContentsManager);
  }

  public void TryToSelectGettingItem ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    int havingItemNum = saveData.havingItemsName.Length;
    int havingItemNumRestriction = saveData.havingItemsNumRestriction;
    int leavingItemNum = saveData.leavingItemsName.Length;
    if (havingItemNum < havingItemNumRestriction && leavingItemNum > 0)
    {
      // Debug.Log ("ひきとれますよ");
      GoToGettingItemScene ();
    }
    else if (havingItemNum >= havingItemNumRestriction)
    {
      // Debug.Log ("お客さん、お持ちのアイテムがいっぱいですよ");
      GoToCanNotGetItemScene ();
    }
    else if (leavingItemNum <= 0)
    {
      // Debug.Log ("お客さん、預かっているアイテムはありませんよ");
      GoToNoLeavingItemScene ();
    }
  }

  public void GoToGettingItemScene ()
  {
    string selectingGettingItemScene = "016";
    conversationContentsManager.SetScene (selectingGettingItemScene);
  }

  public void GoToGotItemScene ()
  {
    string gotItemScene = "017";
    conversationContentsManager.SetScene (gotItemScene);
  }

  public void GoToNoLeavingItemScene ()
  {
    string noLeavingItemScene = "018";
    conversationContentsManager.SetScene (noLeavingItemScene);
  }
  public void GoToCanNotGetItemScene ()
  {
    string canNotGetItemScene = "019";
    conversationContentsManager.SetScene (canNotGetItemScene);
  }

  public void ChangeConversationStateToGettingItem ()
  {
    conversationContentsManager.GameConversationState =
      ConversationContentsManager.ConversationState.gettingItem;
  }

  public void GettingMyItem ()
  {
    string[] leavingItemNameArr = SaveSystem.Instance.userData.leavingItemsName;
    SaveSystem.Instance.userData.leavingItemsName =
      menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (leavingItemNameArr, selectedItemButtonNum);
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    SaveSystem.Instance.userData.havingItemsName = havingItemNameArr.Concat (new string[] { selectedItemName }).ToArray ();
    // SaveSystem.Instance.Save ();
    GoToGotItemScene ();
  }

}