using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
  [SerializeField] SceneManageButtonEvents sceneManageButtonEvents;
  [SerializeField] TitleCanvasUi titleCanvasUi;
  [SerializeField] ScreenEffectUi screenEffectUi;
  // [SerializeField] GameObject titleStartText;

  private TitleMotionManager titleMotionManager;

  async UniTask Start ()
  {

    titleMotionManager = new TitleMotionManager (
      titleCanvasUi
    );

    sceneManageButtonEvents.TitleMotionManagerForButtonEvents = titleMotionManager;

    await UniTask.WaitUntil (() => screenEffectUi.EndScreenEffectSetup && titleCanvasUi.dialogManager.EndSetup);
  }

  void Update ()
  {
    TitleMotionTask ();
  }

  void TitleMotionTask ()
  {
    if (screenEffectUi.EndScreenEffectSetup && titleCanvasUi.dialogManager.EndSetup)
    {
      titleMotionManager.ManageTitleMotion ();
    }
  }
}