using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCanvasSeManager
{
  private GameObject firstSelectingTag;
  private GameObject prevSelectedGameObject;
  private SEManager seManager;
  private HanaPlayerScript hanaPlayer;
  private HanaPlayerScript.State prevHanaPlayerState;
  private EventSystem eventSystem;
  private string prevMenuState;

  public MenuCanvasSeManager (HanaPlayerScript hanaPlayer, EventSystem eventSystem, GameObject firstSelectingTag)
  {
    this.hanaPlayer = hanaPlayer;
    this.eventSystem = eventSystem;
    this.firstSelectingTag = firstSelectingTag;
    prevSelectedGameObject = firstSelectingTag;
    prevMenuState = MenuCanvasEnableButtonsManager.State.close.ToString ();
    seManager = SEManager.Instance;
  }

  public void ManageMenuSe (string currentMenuState)
  {
    // 開けたときの音
    HanaPlayerScript.State currentHanaPlayerState = hanaPlayer.PlayerState;
    if (prevHanaPlayerState != HanaPlayerScript.State.OpenMenu && currentHanaPlayerState == HanaPlayerScript.State.OpenMenu && currentMenuState != MenuCanvasEnableButtonsManager.State.close.ToString ())
    {
      prevHanaPlayerState = currentHanaPlayerState;
      prevMenuState = currentMenuState;
      seManager.Play (SEPath.OPEN_MENU);
    }
    // 閉めたときの音
    else if (prevMenuState != MenuCanvasEnableButtonsManager.State.close.ToString () && currentMenuState == MenuCanvasEnableButtonsManager.State.close.ToString ())
    {
      prevMenuState = currentMenuState;
      prevHanaPlayerState = HanaPlayerScript.State.Normal;
      prevSelectedGameObject = firstSelectingTag;
      seManager.Play (SEPath.CLOSE_MENU);
    }
    // カーソル移動音
    void CheckAndPlaySe (string state, string sePath)
    {
      GameObject currentGameObject = eventSystem.currentSelectedGameObject;
      if (currentMenuState == state && prevMenuState == state && prevSelectedGameObject != currentGameObject)
      {
        seManager.Play (sePath);
      }
    }
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingTag.ToString (), SEPath.MENU_SELECT_TAG);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingSkill.ToString (), SEPath.MENU_CURSOR);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingBelonging.ToString (), SEPath.MENU_CURSOR);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingItem.ToString (), SEPath.MENU_CURSOR);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingImportantThingButton.ToString (), SEPath.MENU_CURSOR);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingBadge.ToString (), SEPath.MENU_CURSOR);
    CheckAndPlaySe (MenuCanvasEnableButtonsManager.State.selectingBadgeFromAll.ToString (), SEPath.MENU_CURSOR);
    // 戻るボタン
    if (currentHanaPlayerState == HanaPlayerScript.State.OpenMenu &&
      prevMenuState != MenuCanvasEnableButtonsManager.State.selectingTag.ToString () &&
      Input.GetKeyDown (KeyCode.Backspace)
    )
    {
      seManager.Play (SEPath.MENU_BACK);
    }
    // リセット
    GameObject currentSelectedGameObject = eventSystem.currentSelectedGameObject;
    prevMenuState = currentMenuState;
    prevSelectedGameObject = currentSelectedGameObject;
  }
}