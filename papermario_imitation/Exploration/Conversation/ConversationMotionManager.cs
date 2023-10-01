using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class ConversationMotionManager
{
  private CommonUiManager commonUiManager;
  private ConversationGUIManager conversationGUIManager;
  private ScaleWithRotationFunctions scaleWithRotationFunctions;
  // private Image conversationPanelImage;
  private Tween tween;
  private Vector3 showingScale;
  private Vector3 hidingScale;
  private Vector3 showingPosition;
  private Vector3 hidingPosition;
  private Vector3 initialItemListScale;
  float showingAlpha = 1.0f;
  float hidingAlpha = 0.0f;
  private float showAndHideTweenDuration;
  private bool closedConversationPanel;

  public bool ClosedConversationPanel
  {
    get { return closedConversationPanel; }
  }

  public ConversationMotionManager (
    CommonUiManager commonUiManager,
    ConversationGUIManager conversationGUIManager
  )
  {
    scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    this.commonUiManager = commonUiManager;
    this.conversationGUIManager = conversationGUIManager;
    // conversationPanelImage = conversationGUIManager.conversationPanel.GetComponent<Image> ();
    float defaultScale = 1.0f;
    float hideScale = 0.3f;
    showingScale = new Vector3 (defaultScale, defaultScale, defaultScale);
    hidingScale = new Vector3 (hideScale, hideScale, hideScale);
    showingPosition = conversationGUIManager.conversationPanel.transform.position;
    hidingPosition = showingPosition;
    hidingPosition.y -= 30.0f;
    showAndHideTweenDuration = 0.2f;
    // showAndHideTweenDuration = 1.0f;
    SetupNextTalkSignMotion ();
    HidingConversationPanel ();
    conversationGUIManager.conversationPanel.SetActive (false);
  }

  public async UniTask ShowingConversationPanel ()
  {
    closedConversationPanel = false;
    conversationGUIManager.conversationPanel.SetActive (true);
    // 3つのtween、durationは共通
    // 拡大
    conversationGUIManager.conversationPanel.transform.DOScale (showingScale, showAndHideTweenDuration);
    // 移動
    conversationGUIManager.conversationPanel.transform.DOMoveY (showingPosition.y, showAndHideTweenDuration);
    // フェードイン
    await conversationGUIManager.conversationPanel.GetComponent<CanvasGroup> ().DOFade (showingAlpha, showAndHideTweenDuration);
  }

  public async UniTask HidingConversationPanel ()
  {
    conversationGUIManager.nextTalkSign.SetActive (false);
    conversationGUIManager.conversationPanel.transform.DOScale (hidingScale, showAndHideTweenDuration);
    conversationGUIManager.conversationPanel.transform.DOMoveY (hidingPosition.y, showAndHideTweenDuration);
    await conversationGUIManager.conversationPanel.GetComponent<CanvasGroup> ().DOFade (hidingAlpha, showAndHideTweenDuration);
    conversationGUIManager.conversationPanel.SetActive (false);
    closedConversationPanel = true;
  }

  public async UniTask FadeInOptionPanel (GameObject optionPanel)
  {
    commonUiManager.ShowCommonHandCursor ();
    await optionPanel.GetComponent<CanvasGroup> ().DOFade (showingAlpha, showAndHideTweenDuration);
  }

  public async UniTask FadeoutOptionPanel (GameObject optionPanel)
  {
    commonUiManager.HideCommonHandCursor ();
    await optionPanel.GetComponent<CanvasGroup> ().DOFade (hidingAlpha, showAndHideTweenDuration);
  }

  public async UniTask ShowingItemList ()
  {
    await commonUiManager.ShowCommonUiForItemList ();
  }

  public async UniTask HidingItemList ()
  {
    await commonUiManager.HideCommonUiForItemList ();
  }

  void SetupNextTalkSignMotion ()
  {
    GameObject nextTalkSign = conversationGUIManager.nextTalkSign;
    float signPositionY = nextTalkSign.transform.localPosition.y;
    float noRotation = 0.0f;
    float rotationDegree = 60.0f;
    float durationTime = 0.5f;
    // 回転処理
    this.tween = nextTalkSign.transform.DORotate (new Vector3 (noRotation, noRotation, -rotationDegree), durationTime * 3)
      .SetEase (Ease.InOutQuad).OnComplete (() =>
      {
        var sequence = DOTween.Sequence ();
        sequence.Append (
            nextTalkSign.transform.DORotate (new Vector3 (noRotation, noRotation, rotationDegree), durationTime)
          )
          .SetEase (Ease.InOutQuad)
          .SetLoops (-1, LoopType.Yoyo);
        this.tween = sequence;
      });
  }
}