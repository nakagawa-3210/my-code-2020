using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemListContentsPreparer
{
  private CommonUiFunctions commonUiFunctions;
  private ItemListCommonFunction itemListCommonFunction;
  private ConversationContentsManager conversationContentsManager;
  private GameObject buttonForitemList;
  private Transform itemListContainerTransform;
  public ShopItemListContentsPreparer (
    ConversationContentsManager conversationContentsManager,
    GameObject buttonForitemList,
    Transform itemListContainerTransform
  )
  {
    commonUiFunctions = new CommonUiFunctions ();
    itemListCommonFunction = new ItemListCommonFunction ();
    this.conversationContentsManager = conversationContentsManager;
    this.buttonForitemList = buttonForitemList;
    this.itemListContainerTransform = itemListContainerTransform;
  }

  public async UniTask SetupItemListButtons (ConversationContentsManager.ConversationState conversationState)
  {
    int itemNum = SaveSystem.Instance.userData.havingItemsName.Length;
    if (itemNum <= 0)
    {
      Debug.LogError ("アイテムがないのに呼んでるよ！");
      return;
    };
    // conversationStateに合わせて用意するリストを決める
    if (conversationState == ConversationContentsManager.ConversationState.sellingItem)
    {
      await SetupHavingItemListViewButtons ();
    }
    else if (conversationState == ConversationContentsManager.ConversationState.leavingItem)
    {
      await SetupLeavingItemListViewButtons ();
    }
    else if (conversationState == ConversationContentsManager.ConversationState.gettingItem)
    {
      await SetupGettingItemListViewButtons ();
    }
  }

  // 売る場合
  public async UniTask SetupHavingItemListViewButtons ()
  {
    List<GameObject> buttonList = new List<GameObject> ();
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    foreach (var havingItemName in havingItemNameArr)
    {
      GameObject newButton = GameObject.Instantiate<GameObject> (buttonForitemList);
      itemListCommonFunction.SetupBaseButton (newButton, itemListContainerTransform, havingItemName);
      // 売却価格表示
      Item itemInformation = commonUiFunctions.GetItemInformationByUsingItemName (havingItemName);
      Transform buttonPriceText = newButton.transform.Find ("ShopItemListButtonCoinText");
      buttonPriceText.GetComponent<Text> ().text = itemInformation.playersSellingPrice.ToString ();
      buttonList.Add (newButton);
    }
    await UniTask.WaitUntil (() => buttonList.Count == havingItemNameArr.Length);
  }

  // 預ける場合
  public async UniTask SetupLeavingItemListViewButtons ()
  {
    List<GameObject> buttonList = new List<GameObject> ();
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    foreach (var havingItemName in havingItemNameArr)
    {
      GameObject newButton = GameObject.Instantiate<GameObject> (buttonForitemList);
      itemListCommonFunction.SetupBaseButton (newButton, itemListContainerTransform, havingItemName);
      // 売却価格非表示
      itemListCommonFunction.InactivateItemPriceText (newButton);
      buttonList.Add (newButton);
    }
    await UniTask.WaitUntil (() => buttonList.Count == havingItemNameArr.Length);
  }

  // 引き取る場合
  public async UniTask SetupGettingItemListViewButtons ()
  {
    List<GameObject> buttonList = new List<GameObject> ();
    string[] leavingItemNameArr = SaveSystem.Instance.userData.leavingItemsName;
    foreach (var leavingItemName in leavingItemNameArr)
    {
      GameObject newButton = GameObject.Instantiate<GameObject> (buttonForitemList);
      itemListCommonFunction.SetupBaseButton (newButton, itemListContainerTransform, leavingItemName);
      // 売却価格非表示
      itemListCommonFunction.InactivateItemPriceText (newButton);
      buttonList.Add (newButton);
    }
    await UniTask.WaitUntil (() => buttonList.Count == leavingItemNameArr.Length);
  }

}