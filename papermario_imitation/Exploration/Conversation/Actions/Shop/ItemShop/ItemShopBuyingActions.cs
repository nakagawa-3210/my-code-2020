using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShopBuyingActions
{
  private ConversationContentsManager conversationContentsManager;
  private HanaPlayerTalkScript hanaPlayerTalkScript;
  private MyGameData.MyData saveData;
  public ItemShopBuyingActions (ConversationContentsManager conversationContentsManager, HanaPlayerTalkScript hanaPlayerTalkScript = null)
  {
    this.conversationContentsManager = conversationContentsManager;
    this.hanaPlayerTalkScript = hanaPlayerTalkScript;
    saveData = SaveSystem.Instance.userData;
  }

  public void TryToBuyShopItem ()
  {
    int itemPrice = hanaPlayerTalkScript.ShopItemPrice;
    int havingCoin = saveData.havingCoin;
    int havingItemsNum = saveData.havingItemsName.Length;
    int itemNumRestriction = saveData.havingItemsNumRestriction;
    if (havingCoin >= itemPrice && itemNumRestriction > havingItemsNum)
    {
      // プレイヤーの所持金がアイテムの値段よりも高い、かつ持ち物が20未満の場合
      BuyingShopItem ();
    }
    else if (itemNumRestriction <= havingItemsNum)
    {
      // プレイヤーの持ち物が20以上の場合
      HavingTooManyItem ();
    }
    else if (havingCoin < itemPrice)
    {
      // プレイヤーの所持金がアイテムの値段よりも低い場合
      HavingLessCoin ();
    }
  }
  // アイテム購入時の会話の分岐数とシーン指定はショップ共通なので直打ち
  // ショップでの会話のうち、アイテムの購入のみは別ファイルにて会話を保存してある
  // なので3ケタのシーン番号は別のショップシーンと重複しているが、影響はない
  void BuyingShopItem ()
  {
    string itemName = hanaPlayerTalkScript.ShopItemName;
    string[] newerHavingItemsName = new MenuCommonFunctions ().GetNewStringItemNameArrayAddedNewItem (itemName);
    SaveSystem.Instance.userData.havingItemsName = newerHavingItemsName;
    int itemPrice = hanaPlayerTalkScript.ShopItemPrice;
    SaveSystem.Instance.userData.havingCoin -= itemPrice;
    // SaveSystem.Instance.Save ();
    string boughtItemScene = "003";
    conversationContentsManager.SetScene (boughtItemScene);
  }
  void HavingTooManyItem ()
  {
    string boughtItemScene = "004";
    conversationContentsManager.SetScene (boughtItemScene);
  }
  void HavingLessCoin ()
  {
    string boughtItemScene = "005";
    conversationContentsManager.SetScene (boughtItemScene);
  }
}