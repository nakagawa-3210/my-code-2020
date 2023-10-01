using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformationContentsManager
{
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private ItemDataArray itemDataArray;
  private List<GameObject> shopItemList;
  private GameObject shopItemGroup;
  private GameObject selectingItem;
  private GameObject itemNameInfoText;
  private GameObject itemDescriptionInfoText;
  private Item selectingIteminformation;
  // private GameObject itemImage;

  public ItemInformationContentsManager (
    GameObject shopItemGroup,
    GameObject itemNameInfoText,
    GameObject itemDescriptionInfoText
  )
  {
    this.shopItemGroup = shopItemGroup;
    this.itemNameInfoText = itemNameInfoText;
    this.itemDescriptionInfoText = itemDescriptionInfoText;
    menuCommonFunctions = new MenuCommonFunctions ();
    shopItemList = new List<GameObject> ();
    shopItemList = menuCommonFunctions.GetChildList (shopItemGroup);
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameItemsData");
    // アイテムの価格設定
    SetupItemPriceText ();
  }

  // 目の前にプレイヤーがいるアイテムはいつも必ずひとつだけ
  // 前回のフレームでプレイヤーが前にいたアイテムが、今回のフレームでは別アイテムだったとき
  // 別アイテムだったばあい、書き換える
  public void SetItemInformation ()
  {
    foreach (var item in shopItemList)
    {
      ShopItemInfoContainer shopInfoConta = item.GetComponent<ShopItemInfoContainer> ();
      if (shopInfoConta.IsPlayerEnterering && item != selectingItem)
      {
        selectingItem = item;
        // shopInfoContaから情報を受け取るように変更する
        itemNameInfoText.GetComponent<Text> ().text = shopInfoConta.ShopItemName;
        itemDescriptionInfoText.GetComponent<Text> ().text = shopInfoConta.ShopItemDescription;
      }
    }
  }

  // アイテムの画像に応じて価格のテキストを変更
  // shopInfoContaから値を受け取るように変更予定
  void SetupItemPriceText ()
  {
    foreach (var item in shopItemList)
    {
      GameObject itemImg = item.transform.Find ("ItemImg").gameObject;
      string itemImgFileName = itemImg.GetComponent<SpriteRenderer> ().sprite.name;
      foreach (var itemData in itemDataArray.gameItems)
      {
        if (itemImgFileName == itemData.imgFileName)
        {
          GameObject itemPrice = item.transform.Find ("ItemPrice").gameObject;
          itemPrice.GetComponent<TextMeshPro> ().text = itemData.playersBuyingPrice.ToString ();
        }
      }
    }
  }

}