using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopSellingActions
{
  private ConversationContentsManager conversationContentsManager;
  private ItemShopConversationActions itemShopConversationActions;
  private MenuCommonFunctions menuCommonFunctions;
  private int selectedItemSellingPrice;
  private int selectedItemButtonNum;
  public int SelectedItemSellingPrice
  {
    set { selectedItemSellingPrice = value; }
  }
  public int SelectedItemButtonNum
  {
    set { selectedItemButtonNum = value; }
  }
  public ItemShopSellingActions (ConversationContentsManager conversationContentsManager)
  {
    this.conversationContentsManager = conversationContentsManager;
    menuCommonFunctions = new MenuCommonFunctions ();
    itemShopConversationActions = new ItemShopConversationActions (conversationContentsManager);
  }

  public void TryToSelectSellingItem ()
  {
    int itemNum = SaveSystem.Instance.userData.havingItemsName.Length;
    if (itemNum > 0)
    {
      GoToSelectingSellingItemScene ();
    }
    else
    {
      itemShopConversationActions.GoToHavingNoItemScene ();
    }
  }

  // 売る
  public void GoToSelectingSellingItemScene ()
  {
    string selectingSellingItemScene = "005";
    conversationContentsManager.SetScene (selectingSellingItemScene);
  }

  public void GoToSellingConfirmationScene ()
  {
    string confirmationScene = "006";
    conversationContentsManager.SetScene (confirmationScene);
  }

  public void GoToSoldItemScene ()
  {
    string soldItemScene = "008";
    conversationContentsManager.SetScene (soldItemScene);
  }
  public void ChangeConversationStateToSellingItem ()
  {
    conversationContentsManager.GameConversationState =
      ConversationContentsManager.ConversationState.sellingItem;
  }

  // 売却によるデータ変更
  public void SellingMyItem ()
  {
    // 選ばれたアイテムを持っているアイテム一覧から削除
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    SaveSystem.Instance.userData.havingItemsName =
      menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (havingItemNameArr, selectedItemButtonNum);
    // アイテムの価格分だけコインを増やす
    SaveSystem.Instance.userData.havingCoin += selectedItemSellingPrice;
    // データの上書き
    // SaveSystem.Instance.Save ();
    GoToSoldItemScene ();
  }

}