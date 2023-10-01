using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
  [SerializeField] CurtainManager curtainManager = default;
  [SerializeField] GameOverCanvasUi gameOverCanvasUi;
  [SerializeField] SceneManageButtonEvents sceneManageButtonEvents;

  private GameOverMotionManager gameOverMotionManager;

  async UniTask Start ()
  {
    gameOverMotionManager = new GameOverMotionManager (gameOverCanvasUi);

    sceneManageButtonEvents.GameOverMotionManagerForButtonEvents = gameOverMotionManager;

    curtainManager.HideAllCurtains ();
    await UniTask.WaitUntil (() => curtainManager.EndTween);
    gameOverMotionManager.StartSelectingOption();
  }

  void Update ()
  {
    if (gameOverMotionManager != null)
    {
      gameOverMotionManager.ManageCursorPosition ();
    }
  }

}