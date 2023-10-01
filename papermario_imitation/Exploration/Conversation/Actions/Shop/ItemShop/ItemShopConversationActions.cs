using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopConversationActions
{
  private ConversationContentsManager conversationContentsManager;
  public ItemShopConversationActions (
    ConversationContentsManager conversationContentsManager
  )
  {
    this.conversationContentsManager = conversationContentsManager;
  }

  public void ShowItemList ()
  {
    conversationContentsManager.IsItemListShown = true;
  }

  // ショップの会話フローが共通で、フローが行ったり来たりする会話が
  // ショップくらいしかないので直打ち中
  // フローが何度も戻るような会話が増えるようなら、パーサーを変更する

  // 共通
  public void GoToSelectingOptionScene ()
  {
    string selectingOptionScene = "002";
    conversationContentsManager.SetScene (selectingOptionScene);
  }
  public void GoToEndItemShopConversationScene ()
  {
    string endConversation = "009";
    conversationContentsManager.SetScene (endConversation);
  }
  public void GoToHavingNoItemScene ()
  {
    string havingNoItemScene = "010";
    conversationContentsManager.SetScene (havingNoItemScene);
  }
  public void RemoveListAllButtons ()
  {
    conversationContentsManager.DeleteAllItemButtons ();
  }
  public void ChangeConversationStateToItemShopOptions ()
  {
    conversationContentsManager.GameConversationState =
      ConversationContentsManager.ConversationState.selectingItemShopOptions;
  }

}