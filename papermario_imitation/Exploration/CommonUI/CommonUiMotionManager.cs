using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class CommonUiMotionManager
{
  private GameObject commonHandCursor;
  private GameObject commonItemListPanel;
  private GameObject commonItemListNamePanel;
  private GameObject commonItemDescriptionPanel;
  private GameObject commonItemListWhichChoseTextFrame;

  private Image handCursorImg;

  private float showingAlpha;
  private float hidingAlpha;

  public CommonUiMotionManager (
    GameObject commonHandCursor,
    GameObject commonItemListPanel,
    GameObject commonItemListNamePanel,
    GameObject commonItemDescriptionPanel,
    GameObject commonItemListWhichChoseTextFrame
  )
  {
    this.commonHandCursor = commonHandCursor;
    this.commonItemListPanel = commonItemListPanel;
    this.commonItemListNamePanel = commonItemListNamePanel;
    this.commonItemDescriptionPanel = commonItemDescriptionPanel;
    this.commonItemListWhichChoseTextFrame = commonItemListWhichChoseTextFrame;

    handCursorImg = commonHandCursor.transform.Find ("CommonHandCursorImg").gameObject.GetComponent<Image> ();

    showingAlpha = 1.0f;
    hidingAlpha = 0.0f;
  }

  public void ShowCommonItemDescriptionPanel ()
  {
    commonItemDescriptionPanel.SetActive (true);

    // 複数回呼んでいるのを直す
    ScaleWithRotationFunctions scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (commonItemDescriptionPanel);
  }

  public async UniTask HideCommonDescriptionPanel ()
  {
    ScaleWithRotationFunctions scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    await scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (commonItemDescriptionPanel);
    commonItemDescriptionPanel.SetActive (false);
  }

  public void ShowCommonHandCursor (float duration = 0.2f)
  {
    commonHandCursor.SetActive (true);
    handCursorImg.DOFade (showingAlpha, duration);
  }

  public async UniTask HideCommonHandCursor (float duration = 0.2f)
  {
    await handCursorImg.DOFade (hidingAlpha, duration);
    commonHandCursor.SetActive (false);
  }

  public async UniTask ShowCommonUiForItemList ()
  {
    ShowCommonHandCursor ();

    commonItemListPanel.SetActive (true);
    commonItemListNamePanel.SetActive (true);
    commonItemDescriptionPanel.SetActive (true);
    commonItemListWhichChoseTextFrame.SetActive (true);

    ScaleWithRotationFunctions scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (commonItemListPanel);
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (commonItemListNamePanel);
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (commonItemDescriptionPanel);
    await scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (commonItemListWhichChoseTextFrame);
  }

  public async UniTask HideCommonUiForItemList ()
  {
    HideCommonHandCursor ();
    ScaleWithRotationFunctions scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (commonItemListPanel);
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (commonItemListNamePanel);
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (commonItemDescriptionPanel);
    await scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (commonItemListWhichChoseTextFrame);
    commonItemListPanel.SetActive (false);
    commonItemListNamePanel.SetActive (false);
    commonItemDescriptionPanel.SetActive (false);
    commonItemListWhichChoseTextFrame.SetActive (false);
  }

}