using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenuListPreparer
{
  // イメージと名前をボタンに用意する
  public void SetupListButtonBaseInformation (
    GameObject newButton,
    Transform buttonListContainerTran,
    Sprite buttonSprite,
    string dataName,
    string imgGameObjectName,
    string textGameObjectName
  )
  {
    newButton.transform.SetParent (buttonListContainerTran);
    // ボタンは生成時にスケールが1になるように強制的に調整する
    float baseScale = 1.0f;
    newButton.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    // アイテムボタンの中身設定
    Transform buttonNameText = newButton.transform.Find (textGameObjectName);
    buttonNameText.GetComponent<Text> ().text = dataName;
    Transform buttonImg = newButton.transform.Find (imgGameObjectName);
    buttonImg.GetComponent<Image> ().sprite = buttonSprite;
  }

  public void SetItemButtonInformation (string itemName, GameObject itemButton, ItemDataArray itemDataArr)
  {
    // アイテムの情報をアイテムボタンに保持させる
    BelongingButtonInfoContainer buttonInfoContainer = itemButton.GetComponent<BelongingButtonInfoContainer> ();
    // 下記の代入をする場所改修予定
    buttonInfoContainer.BelongingName = itemName;
    foreach (var itemData in itemDataArr.gameItems)
    {
      if (itemName == itemData.name)
      {
        buttonInfoContainer.Description = itemData.description;
        buttonInfoContainer.Type = itemData.type;
        buttonInfoContainer.Amount = itemData.amount;
      }
    }
  }

  public void SetCanNotSelectColor (Text buttonNameText, Image buttonImage, Text pointNumText = null)
  {
    float gray = 140.0f;
    SetColor (gray, gray, buttonNameText, buttonImage, pointNumText);
  }

  public void SetCanSelectColor (Text buttonNameText, Image buttonImage, Text pointNumText = null)
  {
    float black = 50.0f;
    float white = 255.0f;
    SetColor (black, white, buttonNameText, buttonImage, pointNumText);
  }

  void SetColor (float textColor, float imgColor, Text buttonNameText, Image buttonImage, Text pointNumText = null)
  {
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    Text buttonName = buttonNameText.GetComponent<Text> ();
    Image buttonImg = buttonImage.GetComponent<Image> ();
    menuCommonFunctions.ChangeTextColor (buttonName, textColor);
    menuCommonFunctions.ChangeImageColor (buttonImg, imgColor);
    // 下記はバッジ用
    // 動的に作るボタンのうち、バッジボタンだけが名前とポイントの2つのテキストを持つ
    if (pointNumText == null) return;
    Text pointNum = pointNumText.GetComponent<Text> ();
    menuCommonFunctions.ChangeTextColor (pointNum, textColor);
  }

}