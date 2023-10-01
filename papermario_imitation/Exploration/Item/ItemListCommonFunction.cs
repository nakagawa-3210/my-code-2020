using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListCommonFunction
{
  private CommonUiFunctions commonUiFunctions;
  public ItemListCommonFunction ()
  {
    commonUiFunctions = new CommonUiFunctions ();
  }

  public void SetupBaseButton (GameObject newButton, Transform itemListContainerTransform, string itemName)
  {
    SetupBaseButtonInfo (newButton, itemName);
    SetupButtonTransform (newButton, itemListContainerTransform);
  }

  void SetupBaseButtonInfo (GameObject newButton, string itemName)
  {
    BelongingButtonInfoContainer belongingButtonInfoContainer = newButton.GetComponent<BelongingButtonInfoContainer> ();
    // アイテム情報
    Item itemInformation = commonUiFunctions.GetItemInformationByUsingItemName (itemName);
    belongingButtonInfoContainer.Description = itemInformation.description;
    belongingButtonInfoContainer.PlayersSellingPrice = itemInformation.playersSellingPrice;
    belongingButtonInfoContainer.BelongingName = itemName;
    belongingButtonInfoContainer.IsSelectable = true;
    // 画像
    Transform buttonImg = newButton.transform.Find ("ShopItemListButtonImg");
    buttonImg.GetComponent<Image> ().sprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
    // 名前
    Transform buttonNameText = newButton.transform.Find ("ShopItemListButtonText");
    buttonNameText.GetComponent<Text> ().text = itemName;
  }

  void SetupButtonTransform (GameObject newButton, Transform itemListContainerTransform)
  {
    newButton.transform.SetParent (itemListContainerTransform);
    float defaultSize = 1.0f;
    newButton.transform.localScale = new Vector3 (defaultSize, defaultSize, defaultSize);
    float noRotation = 0.0f;
    newButton.transform.localRotation = Quaternion.Euler (noRotation, noRotation, noRotation);
  }

  public void InactivateItemPriceText (GameObject newButton)
  {
    Transform buttonPriceText = newButton.transform.Find ("ShopItemListButtonCoinText");
    buttonPriceText.gameObject.SetActive (false);
  }

}