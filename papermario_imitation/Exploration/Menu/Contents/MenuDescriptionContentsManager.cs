using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 動的に作るボタンはボタンの持つデータを読む

// 静的なボタンはボタンのゲームオブジェクトがもつタグを参照して条件式にてtext変更
// 静的なボタンの表示内容はJSONで管理する
// タグで5, アイテムオプションで2, バッジオプションで2, 

public class MenuDescriptionContentsManager
{
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private GameMenuButtonDataArray menuButtonDataArray;
  private ItemDataArray itemDataArray;
  private GameObject descriptionText;
  private GameObject descriptionSelectImage;
  private GameObject previousFrameSelectedGameObject;
  private EventSystem eventSystem;
  private string exceptionTagName = "RecoveringTargetButton";

  public MenuDescriptionContentsManager (
    GameObject descriptionText,
    GameObject descriptionSelectImage,
    EventSystem eventSystem
  )
  {
    this.descriptionText = descriptionText;
    this.descriptionSelectImage = descriptionSelectImage;
    this.eventSystem = eventSystem;
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    menuButtonDataArray = jsonReaderFromResourcesFolder.GetMenuButtonDataArray ("JSON/GameMenuDescriptionData");
    itemDataArray = jsonReaderFromResourcesFolder.GetItemDataArray ("JSON/GameMenuDescriptionData");
    previousFrameSelectedGameObject = null;
  }

  public void SetDescriptionContents (GameObject currentSelectedGameButton, GameObject previousFrameSelectedGameObject)
  {
    if (currentSelectedGameButton == null) return;
    if (currentSelectedGameButton != previousFrameSelectedGameObject)
    {
      // 回復対象選択中の場合はアイテムの情報を保持して表示する
      SetStaticButtonInformation (currentSelectedGameButton);
      SetDynamicallyMadeButtonInformation (currentSelectedGameButton);
    }
  }

  // 動的に作られない固定のボタンの情報をDescriptionTextに表示する
  void SetStaticButtonInformation (GameObject currentSelectedGameButton)
  {
    // ゲームオブジェクトのタグ名を取得
    if (currentSelectedGameButton.tag == "Untagged" || currentSelectedGameButton.tag == exceptionTagName) return;
    string tagName = currentSelectedGameButton.tag;
    string tagDescription = GetDescriptionInfoBasedOnTagName (tagName);
    SetDescriptionText (tagDescription);
    // Debug.Log ("中身かえてるよ");
  }
  string GetDescriptionInfoBasedOnTagName (string tagName)
  {
    string description = null;
    foreach (var menuData in menuButtonDataArray.gameMenuDescriptions)
    {
      if (tagName == menuData.tagName)
      {
        description = menuData.description;
      }
    }
    return description;
  }

  // BelongingButtonInfoContainerにしか対応していない
  void SetDynamicallyMadeButtonInformation (GameObject selectedGameButton)
  {
    SetPartnerInformation (selectedGameButton);
    SetSkillInformation (selectedGameButton);
    // 下記二つの関数の共通の処理をまとめる
    SetBelongingButtonInformation (selectedGameButton);
    SetBadgeButtonInformation (selectedGameButton);
  }

  void SetSkillInformation (GameObject selectedGameButton)
  {
    if (selectedGameButton.GetComponent<SkillInfoContainer> () == null) return;
    SkillInfoContainer skillInfoContainer = selectedGameButton.GetComponent<SkillInfoContainer> ();
    SetDescriptionText (skillInfoContainer.Description);
  }

  void SetPartnerInformation (GameObject selectedGameButton)
  {
    if (selectedGameButton.GetComponent<PartnerInfoContainer> () == null) return;
    PartnerInfoContainer partnerInfoContainer = selectedGameButton.GetComponent<PartnerInfoContainer> ();
    SetDescriptionText (partnerInfoContainer.Description);
  }

  void SetBelongingButtonInformation (GameObject selectedGameButton)
  {
    if (selectedGameButton.GetComponent<BelongingButtonInfoContainer> () == null)
    {
      // 特定のタグを持ったボタンを選択している場合はImageを表示し続ける
      if (descriptionSelectImage != null && selectedGameButton.tag != exceptionTagName)
      {
        descriptionSelectImage.SetActive (false);
      }
      return;
    }
    BelongingButtonInfoContainer itemButtonInfoContainer = selectedGameButton.GetComponent<BelongingButtonInfoContainer> ();
    // 説明の変更
    SetDescriptionText (itemButtonInfoContainer.Description);
    // イメージの変更
    Sprite imgSprite = GetSprite (itemButtonInfoContainer.BelongingName);
    SetDescriptionImage (imgSprite);
  }

  void SetBadgeButtonInformation (GameObject selectedGameButton)
  {
    if (selectedGameButton.GetComponent<BadgeButtonInfoContainer> () == null) return;
    BadgeButtonInfoContainer badgeButtonInfoContainer = selectedGameButton.GetComponent<BadgeButtonInfoContainer> ();
    SetDescriptionText (badgeButtonInfoContainer.Description);
    Sprite imgSprite = GetSprite (badgeButtonInfoContainer.BadgeName);
    SetDescriptionImage (imgSprite);
  }

  void SetDescriptionText (string text)
  {
    descriptionText.GetComponent<Text> ().text = text;
  }

  void SetDescriptionImage (Sprite imgSprite)
  {
    // Findの指定はserializeFieldにするか考える
    Transform descriptionImg = descriptionSelectImage.transform.Find ("DescriptionImage");
    descriptionImg.GetComponent<Image> ().sprite = imgSprite;
    descriptionSelectImage.SetActive (true);
  }

  Sprite GetSprite (string belongingOrBadgeName)
  {
    Sprite imgSprite = null;
    GetGameSprite getSprite = new GetGameSprite ();
    if (getSprite.GetSameNameItemSprite (belongingOrBadgeName) != null)
    {
      imgSprite = getSprite.GetSameNameItemSprite (belongingOrBadgeName);
    }
    else if (getSprite.GetSameNameImportantThingSprite (belongingOrBadgeName) != null)
    {
      imgSprite = getSprite.GetSameNameImportantThingSprite (belongingOrBadgeName);
    }
    else if (getSprite.GetSameNameBadgeSprite (belongingOrBadgeName) != null)
    {
      imgSprite = getSprite.GetSameNameBadgeSprite (belongingOrBadgeName);
    }
    return imgSprite;
  }
}