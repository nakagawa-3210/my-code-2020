using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
  [SerializeField] float doTweenDurationTime = default; // 0.8f
  [SerializeField] MenuCanvasGUI menuCanvasGui = default;
  [SerializeField] MenuButtonEventsManager menuButtonEventsManager = default;
  [SerializeField] HanaPlayerScript hanaPlayer = default;
  [SerializeField] GameObject firstSelectingTag = default;

  private MenuCanvasContentsManager menuCanvasContentsManager;
  private MenuCanvasMotionManager menuCanvasMotionManager;
  private MenuCanvasSeManager menuCanvasSeManager;
  private MenuAndPlayerManager menuAndPlayerManager;

  void Start ()
  {
    Setup ();
  }

  async UniTask Setup ()
  {
    menuCanvasContentsManager = new MenuCanvasContentsManager (menuCanvasGui);
    await UniTask.WaitUntil (() => menuCanvasContentsManager.EndSetup);
    menuCanvasMotionManager = new MenuCanvasMotionManager (menuCanvasGui, hanaPlayer, doTweenDurationTime);
    await UniTask.WaitUntil (() => menuCanvasMotionManager != null);
    menuAndPlayerManager = new MenuAndPlayerManager (menuCanvasMotionManager, hanaPlayer.GetComponent<HanaPlayerScript> ());
    menuButtonEventsManager.EventManagerMenuCanvasMotionManager = menuCanvasMotionManager;
    menuCanvasSeManager = new MenuCanvasSeManager (hanaPlayer, menuCanvasGui.eventSystem, firstSelectingTag);
  }

  void Update ()
  {
    // 回復対象選択後にメニューが閉じるが、その間にバックスペースキーを押しても処理はしないようにする
    if (menuCanvasContentsManager == null || menuCanvasMotionManager == null || menuAndPlayerManager == null) return;
    menuCanvasContentsManager.ContentsUpdate ();
    if (menuCanvasMotionManager.MenuState == "selectingRecoveringTarget")
    {
      menuCanvasContentsManager.ManageBackToItemListFromTargetList ();
    }
    menuCanvasMotionManager.MotionUpdate ();

    // 必要がないのに下記の処理を書いていた理由がわからないのでコメントアウト中
    // if (menuCanvasMotionManager.MenuState != "selectingRecoveringTarget" &&
    //   menuCanvasMotionManager.IsAnyItemButtonSelected ())
    // {
    menuCanvasSeManager.ManageMenuSe (menuCanvasMotionManager.MenuState);
    // Debug.Log ("menuCanvasMotionManager.MenuState : " + menuCanvasMotionManager.MenuState);
    //   // menuCanvasContentsManager.ResetIsSelectedInformation ();
    // }

    menuCanvasMotionManager.ManageCloseMenuAfterSelectRecoveringTarget ();
    menuAndPlayerManager.ManagePlayerStatus ();
  }

  void LateUpdate ()
  {
    // Debug.Log ("menuCanvasMotionManager : " + menuCanvasMotionManager);
    if (menuCanvasContentsManager == null || menuCanvasMotionManager == null) return;
    menuCanvasContentsManager.ContentsLateUpdate ();
    menuCanvasMotionManager.MotionLateUpdate ();
  }

}