using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderConfirmationConversationScopeScript : MonoBehaviour
{
  private GameObject shopItem;
  private ShopItemConversationScript shopItemConversationScript;

  // 表示されている商品情報どおりの注文確認会話が始まるかを確認。コライダー調整が必要になる

  void Start ()
  {
    shopItem = this.gameObject;
    shopItemConversationScript = shopItem.GetComponent<ShopItemConversationScript> ();
  }

  void OnTriggerStay (Collider other)
  {
    if (other.tag == "Player")
    {
      GameObject player = other.gameObject;
      HanaPlayerScript hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      HanaPlayerTalkScript hanaPlayerTalkScript = player.GetComponent<HanaPlayerTalkScript> ();
      // Debug.Log ("アイテム購入確認のための会話ができる範囲にいるよ");
      if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
      {
        // SetHogeHogeをConversationTarget=hogeの形に変える
        shopItemConversationScript.TalkingTarget = player;
        hanaPlayerTalkScript.ConversationTarget = shopItem;
        hanaPlayerTalkScript.ConversationFileName = shopItemConversationScript.TalkTextFileName;
      }
      else if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.OpenMenu)
      {
        hanaPlayerTalkScript.ResetConversationTarget (shopItem);
      }
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      GameObject player = other.gameObject;
      HanaPlayerScript hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
      {
        player.GetComponent<HanaPlayerTalkScript> ().ResetConversationTarget (shopItem);
      }
    }
  }
}