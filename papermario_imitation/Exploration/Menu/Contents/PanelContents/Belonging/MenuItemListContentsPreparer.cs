using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemListContentsPreparer
{
  private BaseMenuListPreparer baseMenuListPreparer;
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private GameObject buttonForItems;
  private Transform itemListContainerTra;
  private ItemDataArray itemDataArray;
  private string nameTextGameObjectName;
  private string imgGameObjectName;
  public MenuItemListContentsPreparer (
    GameObject buttonForItems,
    Transform itemListContainerTra,
    string imgGameObjectName,
    string nameTextGameObjectName
  )
  {
    baseMenuListPreparer = new BaseMenuListPreparer ();
    menuCommonFunctions = new MenuCommonFunctions ();
    this.buttonForItems = buttonForItems;
    this.itemListContainerTra = itemListContainerTra;
    this.imgGameObjectName = imgGameObjectName;
    this.nameTextGameObjectName = nameTextGameObjectName;
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameItemsData");
    SetupItemListViewButtons ();
  }

  void SetupItemListViewButtons ()
  {
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    // iを一つの目的でしか使用していないのでforeach文に変更予定
    for (var i = 0; i < havingItemNameArr.Length; i++)
    {
      string itemName = havingItemNameArr[i];
      GameObject newItemButton = GameObject.Instantiate<GameObject> (buttonForItems);
      Sprite buttonSprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
      SetupItemListButton (newItemButton, buttonSprite, itemName);
    }
  }

  void SetupItemListButton (GameObject newButton, Sprite buttonSprite, string itemName)
  {
    // アイテムボタンの中身設定
    baseMenuListPreparer.SetupListButtonBaseInformation (
      newButton,
      itemListContainerTra,
      buttonSprite,
      itemName,
      imgGameObjectName,
      nameTextGameObjectName
    );
    // アイテムボタン固有の要素
    Text itemNameText = newButton.transform.Find (nameTextGameObjectName).GetComponent<Text> ();
    Image itemImg = newButton.transform.Find (imgGameObjectName).GetComponent<Image> ();
    baseMenuListPreparer.SetItemButtonInformation (itemName, newButton, itemDataArray);
    SetupItemButtonsIsSelectable (newButton);
    ChangeNotRecoveringTypeItemColorToDisableColor (newButton, itemNameText, itemImg);
    // アイテムボタンのボタンコンポーネント非アクティブ化
    newButton.GetComponent<Button> ().enabled = false;
  }

  void SetupItemButtonsIsSelectable (GameObject newItemButton)
  {
    BelongingButtonInfoContainer buttonInfoContainer = newItemButton.GetComponent<BelongingButtonInfoContainer> ();
    string itemType = buttonInfoContainer.Type;
    if (itemType == BelongingButtonInfoContainer.State.recoverHp.ToString () || itemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      buttonInfoContainer.IsSelectable = true;
    }
    else
    {
      buttonInfoContainer.IsSelectable = false;
    }
  }

  void ChangeNotRecoveringTypeItemColorToDisableColor (GameObject newItemButton, Text itemNameText, Image itemImage)
  {
    string itemType = newItemButton.GetComponent<BelongingButtonInfoContainer> ().Type;
    if (itemType == BelongingButtonInfoContainer.State.recoverHp.ToString () || itemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      return;
    }
    baseMenuListPreparer.SetCanNotSelectColor (itemNameText, itemImage);
  }

  public void ResetItemListViewButtons ()
  {
    menuCommonFunctions.DeleteButtonFromListView (itemListContainerTra);
    SetupItemListViewButtons ();
  }

}