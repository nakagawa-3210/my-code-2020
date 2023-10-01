using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class CommonUiManager : MonoBehaviour
{
  [SerializeField] CommonUiGUIManager commonUiGUIManager;

  private CommonUiMotionManager commonUiMotionManager;
  private CommonUiContentsManager commonUiContentsManager;

  void Start ()
  {
    commonUiMotionManager = new CommonUiMotionManager (
      commonUiGUIManager.commonHandCursor,
      commonUiGUIManager.commonItemListPanel,
      commonUiGUIManager.commonItemListNamePanel,
      commonUiGUIManager.commonItemDescriptionPanel,
      commonUiGUIManager.commonItemListWhichChoseTextFrame
    );

    commonUiContentsManager = new CommonUiContentsManager (
      commonUiGUIManager.commonItemDescriptionPanel
    );

    Setup ();
  }

  void Setup ()
  {
    commonUiGUIManager.commonHandCursor.SetActive (false);
    commonUiGUIManager.commonItemListPanel.SetActive (false);
    commonUiGUIManager.commonItemListNamePanel.SetActive (false);
    commonUiGUIManager.commonItemDescriptionPanel.SetActive (false);
    commonUiGUIManager.commonItemListWhichChoseTextFrame.SetActive (false);

    HideCommonHandCursor ();
    HideCommonUiForItemList ();
  }

  // モーション処理

  public void ShowCommonItemDescriptionPanel ()
  {
    commonUiMotionManager.ShowCommonItemDescriptionPanel ();
  }

  public async UniTask HideCommonItemDescriptionPanel ()
  {
    await commonUiMotionManager.HideCommonDescriptionPanel ();
  }

  public void ShowCommonHandCursor ()
  {
    commonUiMotionManager.ShowCommonHandCursor ();
  }

  public void HideCommonHandCursor ()
  {
    commonUiMotionManager.HideCommonHandCursor ();
  }

  public async UniTask ShowCommonUiForItemList ()
  {
    await commonUiMotionManager.ShowCommonUiForItemList ();
  }

  public async UniTask HideCommonUiForItemList ()
  {
    await commonUiMotionManager.HideCommonUiForItemList ();
  }

  // コンテンツ処理

  public void SetSelectingItemDescriptionText (GameObject currentSelectedGameButton)
  {
    commonUiContentsManager.SetSelectingItemDescriptionText (currentSelectedGameButton);
  }

  public void SetupItemDescription (string itemDescription)
  {
    commonUiContentsManager.SetupItemDescription (itemDescription);
  }

  public void RemoveAllItemButtonsFromListContainer ()
  {
    new MenuCommonFunctions ().DeleteButtonFromListView (commonUiGUIManager.commonItemListContainer.transform);
  }

}