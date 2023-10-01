using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CommonUiFunctions
{

  public Item GetItemInformationByUsingSpriteName (string itemSpriteName)
  {
    JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    ItemDataArray itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameItemsData");
    Item itemInformation = null;
    foreach (var itemData in itemDataArray.gameItems)
    {
      // アイテム名が一致したとき
      if (itemSpriteName == itemData.imgFileName)
      {
        itemInformation = itemData;
      }
    }
    return itemInformation;
  }

  // 持っているアイテム名一覧をもとにアイテムの情報を取り出すのに使える
  public Item GetItemInformationByUsingItemName (string itemName)
  {
    JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    ItemDataArray itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameItemsData");
    Item itemInformation = null;
    foreach (var itemData in itemDataArray.gameItems)
    {
      // アイテム名が一致したとき
      if (itemName == itemData.name)
      {
        itemInformation = itemData;
      }
    }
    return itemInformation;
  }

  public bool IsAnyItemButtonSelected (GameObject itemListContainer)
  {
    bool isSelected = false;
    // itemListContainerから子要素を全取得
    List<GameObject> itemListViewButtonsList = new List<GameObject> ();
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
    if (itemListViewButtonsList.Count == 0)
    {
      // Debug.Log ("ボタンが用意出来てない、またはアイテムがないよ");
      return isSelected;
    }
    foreach (var itemListViewButton in itemListViewButtonsList)
    {
      if (itemListViewButton == null) return isSelected;
      BelongingButtonInfoContainer buttonInfoContainer = itemListViewButton.GetComponent<BelongingButtonInfoContainer> ();
      // 選ばれたかつ、選べるボタンだった場合
      if (buttonInfoContainer.IsSelected && buttonInfoContainer.IsSelectable)
      {
        // Debug.Log ("アイテムは選ばれたよ");
        isSelected = true;
        return isSelected;
      }
      else
      {
        // Debug.Log ("アイテムはえらばれてないよ");
        isSelected = false;
      }
    }
    // 子要素であるボタンのどれかが選ばれていたら、isSelected = true;
    return isSelected;
  }

  public bool HasGameDataFile ()
  {
    string filePath = Application.persistentDataPath + "/paperHanadaGameData.json";
    bool hasDataFile = File.Exists (filePath);
    return hasDataFile;
  }
}