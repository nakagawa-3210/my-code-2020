using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 使わないかも…。
public class ConversationBackManager
{
  private ConversationContentsManager conversationContentsManager;
  public ConversationBackManager (ConversationContentsManager conversationContentsManager)
  {
    this.conversationContentsManager = conversationContentsManager;
  }
  // アイテムショップ、バッジショップでの会話中(さっき話していた内容にもどる、または終了する処理が必要な会話)
  // 会話が表示中の場合はバックスペースキーを押しても反応しないようにする
  void CloseItemListAndBackToSelectOption ()
  {
    conversationContentsManager.IsItemListShown = false;
    conversationContentsManager.SetScene("");
  }
  
  // アイテムショップでの会話選択時にバックスペースキーで会話が終了してほしい

  // 売却アイテムの選択中にバックスペースキーで会話選択に戻ってほしい
  // ＝＞アイテム選択中にバックスペースキーで会話に戻ってほしい。
}