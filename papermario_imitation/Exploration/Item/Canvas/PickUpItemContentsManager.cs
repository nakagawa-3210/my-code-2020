using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpItemContentsManager
{
  private MenuCommonFunctions menuCommonFunctions;
  private ItemListCommonFunction itemListCommonFunction;
  private CursorManager cursorManager;
  private CommonUiContentsManager commonUiContentsManager;
  private GameObject itemListButton;
  private GameObject newItemListButton;
  private List<GameObject> itemButtonListAddedPickUpItem;
  // private List<BelongingButtonInfoContainer> itemInfoContainerList;
  private Transform itemListContainerTransform;

  private string discardedItemName;

  private string[] itemNameArrAddedPickUpItem;
  private string[] newItemArrayRemovedRedundantItem;

  public string DiscardedItemName
  {
    get { return discardedItemName; }
  }

  public string[] NewItemArrayRemovedRedundantItem
  {
    get { return newItemArrayRemovedRedundantItem; }
  }

  public PickUpItemContentsManager (
    CursorManager cursorManager,
    GameObject itemListButton,
    GameObject newItemListButton,
    GameObject commonItemDescriptionPanel,
    Transform itemListContainerTransform
  )
  {

    commonUiContentsManager = new CommonUiContentsManager (
      commonItemDescriptionPanel
    );

    menuCommonFunctions = new MenuCommonFunctions ();
    itemListCommonFunction = new ItemListCommonFunction ();

    this.cursorManager = cursorManager;
    this.itemListButton = itemListButton;
    this.newItemListButton = newItemListButton;
    this.itemListContainerTransform = itemListContainerTransform;
  }

  public async UniTask SetupItemList (string pickUpItemName)
  {
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    itemNameArrAddedPickUpItem = new string[] { pickUpItemName };
    // havingItemNameArrの先頭に拾ったアイテム名を追加
    itemNameArrAddedPickUpItem = itemNameArrAddedPickUpItem.Concat (havingItemNameArr).ToArray ();
    for (var i = 0; i < itemNameArrAddedPickUpItem.Length; i++)
    {
      GameObject newButton = null;
      string itemName = itemNameArrAddedPickUpItem[i];
      if (i == 0)
      {
        newButton = GameObject.Instantiate<GameObject> (newItemListButton);
      }
      else
      {
        newButton = GameObject.Instantiate<GameObject> (itemListButton);
      }
      itemListCommonFunction.SetupBaseButton (newButton, itemListContainerTransform, itemName);
      itemListCommonFunction.InactivateItemPriceText (newButton);
    }
    itemButtonListAddedPickUpItem = menuCommonFunctions.GetChildList (itemListContainerTransform.gameObject);
    await UniTask.WaitUntil (() => itemButtonListAddedPickUpItem.Count == itemNameArrAddedPickUpItem.Length);
  }

  public void SetupItemDescriptionText ()
  {
    GameObject currentSelectedItemButton = EventSystem.current.currentSelectedGameObject;
    commonUiContentsManager.SetSelectingItemDescriptionText (currentSelectedItemButton);
  }

  public async UniTask GetNewItemArrayRemovedRedundantItem ()
  {
    // アイテムが選ばれるか、バックスペースキーが押されるかで捨てるアイテムを選択する
    await UniTask.WaitUntil (() =>
      menuCommonFunctions.GetSelectedItemButton (itemButtonListAddedPickUpItem) != null ||
      Input.GetKeyDown (KeyCode.Backspace)
    );

    // 拾ったアイテム名キャッシュ
    string pickUpItemName = itemNameArrAddedPickUpItem[0];
    string[] pickUpItemNameArr = new string[] { pickUpItemName };

    // バックスペースキーで拾ったアイテムを捨てたとき
    if (Input.GetKeyDown (KeyCode.Backspace))
    {
      // 先頭のアイテム（ひろったアイテム）をリストから削除
      discardedItemName = itemNameArrAddedPickUpItem[0];
      newItemArrayRemovedRedundantItem = menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (itemNameArrAddedPickUpItem, 0);
    }
    // アイテムを選んで捨てた時
    else
    {
      int selectedItemButtonNum = menuCommonFunctions.GetSelectedItemButtonNum (itemButtonListAddedPickUpItem);
      discardedItemName = itemNameArrAddedPickUpItem[selectedItemButtonNum];
      if (selectedItemButtonNum == 0)
      {
        // 選んだ（拾った）アイテムの削除
        newItemArrayRemovedRedundantItem = menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (itemNameArrAddedPickUpItem, selectedItemButtonNum);
      }
      else
      {
        // 選んだアイテムの削除
        string[] excludingSelectItemName = null;
        excludingSelectItemName = menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (itemNameArrAddedPickUpItem, selectedItemButtonNum);
        await UniTask.WaitUntil (() => excludingSelectItemName != null);;

        // リストの先頭にある今回拾ったアイテムを削除
        string[] excludingPickUpItemName = null;
        excludingPickUpItemName = menuCommonFunctions.GetNewStringArrayExcludingSelectedElement (excludingSelectItemName, 0);
        await UniTask.WaitUntil (() => excludingPickUpItemName != null);

        // 拾ったアイテムを配列の最後尾に再度追加
        newItemArrayRemovedRedundantItem = null;
        newItemArrayRemovedRedundantItem = excludingPickUpItemName.Concat (pickUpItemNameArr).ToArray ();
        await UniTask.WaitUntil (() => newItemArrayRemovedRedundantItem != null);
      }
    }
  }

  public void ResetItemListContents ()
  {
    menuCommonFunctions.DeleteButtonFromListView (itemListContainerTransform);
  }

}