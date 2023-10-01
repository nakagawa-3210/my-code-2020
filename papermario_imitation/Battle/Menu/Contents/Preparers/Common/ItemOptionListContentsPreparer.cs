using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 探索シーンのアイテムリストの作成と、ボタン以外はほとんど同じなので共通関数を作成して用いる
// アイテムのリストはアイテムが使用された場合中身を削除して新しく作り直す必要がある

public class ItemOptionListContentsPreparer
{
  // private BaseMenuListPreparer baseMenuListPreparer;
  private MenuCommonFunctions menuCommonFunctions;
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private GameObject buttonForItemOption;
  private Transform itemOptionListContainerTransform;
  private MyGameData.MyData gameData;
  string buttonImgGameObjectName;
  string buttonTextGameObjectName;
  private int currentBattlePlayerHp;
  private int currentBattlePlayerFp;
  private int currentBattlePartnerHp;
  private int battlePlayerMaxHp;
  private int battlePlayerMaxFp;
  private int currentBattlePartnerMaxHp;

  // リストの中身を用意しない仲間のためにdefault値をnullにした引数にしてある
  public ItemOptionListContentsPreparer (
    GameObject buttonForItemOption = null,
    Transform itemOptionListContainerTransform = null
  )
  {
    // baseMenuListPreparer = new BaseMenuListPreparer ();
    menuCommonFunctions = new MenuCommonFunctions ();
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();

    gameData = SaveSystem.Instance.userData;
    string currentSelectedPartnerName = gameData.currentSelectedPartnerName;
    buttonImgGameObjectName = "ItemImage";
    buttonTextGameObjectName = "ItemNameText";

    SetupMaxInformation (currentSelectedPartnerName);
    currentBattlePlayerHp = gameData.playerCurrentHp;
    currentBattlePlayerFp = gameData.playerCurrentFp;
    currentBattlePartnerHp = GetCurrentPartnerCurrentHp (currentSelectedPartnerName);

    if (buttonForItemOption != null && itemOptionListContainerTransform != null)
    {
      this.buttonForItemOption = buttonForItemOption;
      this.itemOptionListContainerTransform = itemOptionListContainerTransform;
      SetupItemOptionButtons (currentBattlePlayerHp, currentBattlePartnerHp);
    };
  }

  public void SetupMaxInformation (string currentPartnerName)
  {
    PartnerDataArray partnerDataArray = jsonReaderFromResourcesFolder.GetPartnerDataArray ("JSON/GamePartnersData");
    battlePlayerMaxHp = gameData.playerMaxHp;
    battlePlayerMaxFp = gameData.playerMaxFp;
    int currentPartnerLevel = 0;
    if (currentPartnerName == "サクちゃん") //他クラスを含め、直打ちの方法をなおしたい
    {
      currentPartnerLevel = gameData.sakuraLevel;
    }
    currentBattlePartnerMaxHp = menuCommonFunctions.GetPartnerMaxHp (currentPartnerName, currentPartnerLevel, partnerDataArray);
  }

  // 最初の設定にしか用いていないので
  int GetCurrentPartnerCurrentHp (string currentPartnerName)
  {
    int currentPartnerHp = 0;
    if (currentPartnerName == "サクちゃん")
    {
      currentPartnerHp = gameData.sakuraCurrentHp;
    }
    return currentPartnerHp;
  }

  // ターン獲得時に毎回アイテムリストを作成しなおすが、使用したアイテムデータが削除されたitemDataArrayをもとにボタンリストを作成する

  public void SetupItemOptionButtons (int currentBattlePlayerHp, int currentBattlePartnerHp)
  {
    string[] havingItemNameArr = SaveSystem.Instance.userData.havingItemsName;
    foreach (var itemName in havingItemNameArr)
    {
      GameObject newItemButton = GameObject.Instantiate<GameObject> (buttonForItemOption);
      Sprite itemButtonSprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
      // 下記の関数内での処理が探索シーンのアイテムリストと異なる
      SetupItemOptionButton (newItemButton, itemButtonSprite, itemName);
      SetupItemOptionSelectable (newItemButton, itemName);
    }
  }

  // アイテムボタンの情報用意
  void SetupItemOptionButton (GameObject newButton, Sprite buttonSprite, string itemName)
  {
    BaseMenuListPreparer baseMenuListPreparer = new BaseMenuListPreparer ();
    
    baseMenuListPreparer.SetupListButtonBaseInformation (
      newButton,
      itemOptionListContainerTransform,
      buttonSprite,
      itemName,
      buttonImgGameObjectName,
      buttonTextGameObjectName
    );

    ItemDataArray itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameItemsData");
    baseMenuListPreparer.SetItemButtonInformation (itemName, newButton, itemDataArray);
  }

  // 選べないなら先に分かる方がストレスがなさそう
  void SetupItemOptionSelectable (GameObject newButton, string itemName)
  {
    BelongingButtonInfoContainer belongingButtonInfoContainer = newButton.GetComponent<BelongingButtonInfoContainer> ();
    Text buttonNameText = newButton.transform.Find (buttonTextGameObjectName).GetComponent<Text> ();
    Image buttonImg = newButton.transform.Find (buttonImgGameObjectName).GetComponent<Image> ();
    string itemType = belongingButtonInfoContainer.Type;
    if (itemType == BelongingButtonInfoContainer.State.recoverHp.ToString ())
    {
      // HPがプレイヤー、パートナーともに最大の時には選択不可
      if (currentBattlePlayerHp == battlePlayerMaxHp && currentBattlePartnerHp == currentBattlePartnerMaxHp)
      {
        belongingButtonInfoContainer.IsSelectable = false;
        menuCommonFunctions.SetBattleOptionButtonGrayColor (newButton, buttonImg, buttonNameText);
      }
      else
      {
        belongingButtonInfoContainer.IsSelectable = true;
        menuCommonFunctions.SetBattleOptionButtonDefaultColor (newButton, buttonImg, buttonNameText);
      }
    }
    // FP最大の時には選択不可
    else if (itemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      if (currentBattlePlayerFp == battlePlayerMaxFp)
      {
        belongingButtonInfoContainer.IsSelectable = false;
        menuCommonFunctions.SetBattleOptionButtonGrayColor (newButton, buttonImg, buttonNameText);
      }
      else
      {
        belongingButtonInfoContainer.IsSelectable = true;
        menuCommonFunctions.SetBattleOptionButtonDefaultColor (newButton, buttonImg, buttonNameText);
      }
    }
    // 宿泊券選択不可
    else if (
      itemType == BelongingButtonInfoContainer.State.hotelVoucher.ToString () ||
      itemType == BelongingButtonInfoContainer.State.superHotelVoucher.ToString () ||
      itemType == BelongingButtonInfoContainer.State.miracleHotelVoucher.ToString ()
    )
    {
      belongingButtonInfoContainer.IsSelectable = false;
      menuCommonFunctions.SetBattleOptionButtonGrayColor (newButton, buttonImg, buttonNameText);
    }
  }

  // 使用したボタンは削除するので要らないかも
  // プレイヤー、なかまがターンを獲得したときに呼ぶ
  // HPとFPの減少を考慮するため、アイテムリストのSelectable情報を更新する
  public void ResetAvailableItemButtonInformation (List<GameObject> currentItemOptionList, int currentPlayerHp, int currentPlayerFp, int currentPartnerHp, int currentPartnerMaxHp)
  {
    // 体力等の情報更新
    currentBattlePlayerHp = currentPlayerHp;
    currentBattlePlayerFp = currentPlayerFp;
    currentBattlePartnerHp = currentPartnerHp;
    currentBattlePartnerMaxHp = currentPartnerMaxHp;
    // 更新情報をもとにボタンの情報を更新
    foreach (var itemOption in currentItemOptionList)
    {
      string itemName = itemOption.GetComponent<BelongingButtonInfoContainer> ().BelongingName;
      SetupItemOptionSelectable (itemOption, itemName);
    }
  }

}