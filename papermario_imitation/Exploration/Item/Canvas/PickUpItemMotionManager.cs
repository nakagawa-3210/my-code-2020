using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItemMotionManager
{
  private GetGameSprite getGameSprite;
  private PickUpItemGUIManager pickUpItemGUI;
  private CommonUiManager commonUiManager;
  private CursorManager cursorManager;
  private ScaleWithRotationFunctions scaleWithRotationFunctions;
  private SpriteRenderer pickUpItemImageSpriteRenderer;
  private GameObject commonItemListContainer;
  private GameObject pickUpItemImageBackground;
  private Transform pickUpItemImage;

  public PickUpItemMotionManager (
    PickUpItemGUIManager pickUpItemGUI,
    CommonUiManager commonUiManager,
    CursorManager cursorManager,
    GameObject commonItemListContainer,
    Transform pickUpItemImage
  )
  {
    this.pickUpItemGUI = pickUpItemGUI;
    this.commonUiManager = commonUiManager;
    this.cursorManager = cursorManager;
    this.commonItemListContainer = commonItemListContainer;
    this.pickUpItemImage = pickUpItemImage;

    pickUpItemImageBackground = pickUpItemImage.Find ("Background").gameObject;
    pickUpItemImageSpriteRenderer = pickUpItemImage.Find ("ItemImage").GetComponent<SpriteRenderer> ();

    getGameSprite = new GetGameSprite ();
    scaleWithRotationFunctions = new ScaleWithRotationFunctions ();

    Setup ();
  }

  void Setup ()
  {
    pickUpItemGUI.pickUpItemName.SetActive (false);
    pickUpItemGUI.pickUpItemCaution.SetActive (false);
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemName);
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemCaution);
  }

  public void ShowPickUpItemCaution ()
  {
    pickUpItemGUI.pickUpItemCaution.SetActive (true);
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemCaution);
  }

  public void HidePickUpItemInformation ()
  {
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemName);
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemCaution);
    pickUpItemGUI.pickUpItemName.SetActive (false);
    pickUpItemGUI.pickUpItemCaution.SetActive (false);
  }

  public void ShowPickUpItemInformation (Item itemData)
  {
    string itemName = itemData.name;
    string itemDescription = itemData.description;

    ShowItemDescription (itemDescription);
    ShowPickUpItemName (itemName);

    pickUpItemImageSpriteRenderer.sprite = getGameSprite.GetSameNameItemSprite (itemName);
    pickUpItemImageBackground.SetActive (true);
    pickUpItemImage.gameObject.SetActive (true);
  }

  void ShowItemDescription (string itemDescription)
  {
    commonUiManager.SetupItemDescription (itemDescription);
    commonUiManager.ShowCommonItemDescriptionPanel ();
  }

  void ShowPickUpItemName (string itemName)
  {
    string additionalWords = " をゲットだ！";
    ShowItemName (itemName, additionalWords);
  }

  public async UniTask ShowDiscardedItemInformation (string itemName)
  {
    pickUpItemImageBackground.SetActive (false);
    pickUpItemImageSpriteRenderer.sprite = getGameSprite.GetSameNameItemSprite (itemName);
    pickUpItemImage.gameObject.SetActive (true);

    string additionalWords = " をすてた！";
    await ShowItemName (itemName, additionalWords);
  }

  public async UniTask HideDiscardedItemInformation ()
  {
    pickUpItemImage.gameObject.SetActive (false);
    
    await scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemName);
    pickUpItemGUI.pickUpItemName.SetActive (false);
  }

  async UniTask ShowItemName (string itemName, string additionalWords)
  {
    Transform pickItemNameFrame = pickUpItemGUI.pickUpItemName.transform.Find ("PickItemNameText");
    // アイテム名の文字のみ色変更
    pickItemNameFrame.GetComponent<Text> ().text = "<color=#CC0000>" + itemName + "</color>" + additionalWords;

    pickUpItemGUI.pickUpItemName.SetActive (true);
    await scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (pickUpItemGUI.pickUpItemName);
  }

  public async UniTask HideItemInformation ()
  {
    // ボタンを押したら隠す
    await UniTask.WaitUntil (() => Input.GetKeyDown (KeyCode.Space));
    // Input.ResetInputAxes (); //ジャンプしないようにキー情報削除

    HidePickUpItemInformation ();
    pickUpItemImage.gameObject.SetActive (false);
    await commonUiManager.HideCommonItemDescriptionPanel ();
  }

  public void SetupHandCursor ()
  {
    List<GameObject> buttonList = new MenuCommonFunctions ().GetChildList (commonItemListContainer);
    Button firstButton = buttonList[0].GetComponent<Button> ();
    cursorManager.InitCursorPosition (firstButton.gameObject);
    firstButton.Select ();
    cursorManager.SetMyTweenSpeed (0.6f);
  }

}