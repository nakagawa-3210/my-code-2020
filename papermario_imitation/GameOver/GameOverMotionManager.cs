using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverMotionManager
{
  private CommonEnableButtonFunction commonEnableButtonFunction;
  private CursorManager cursorManager;
  private GameOverCanvasUi gameOverCanvasUi;

  private bool endSelecting;

  private GameOverState gameOverState;

  public enum GameOverState
  {
    gameOver,
    selected
  }

  public GameOverMotionManager (
    GameOverCanvasUi gameOverCanvasUi
  )
  {
    this.gameOverCanvasUi = gameOverCanvasUi;

    gameOverState = GameOverState.selected;

    commonEnableButtonFunction = new CommonEnableButtonFunction ();

    cursorManager = new CursorManager (gameOverCanvasUi.gameOverCursor, gameOverCanvasUi.eventSystem);

    commonEnableButtonFunction.DisableListViewButtons (gameOverCanvasUi.gameOverButtonList);

    gameOverCanvasUi.gameOverOptions.SetActive (false);
  }

  public void ManageCursorPosition ()
  {
    if (gameOverState == GameOverState.gameOver)
    {
      cursorManager.MoveCursorTween ();
    }
  }

  public void StartSelectingOption ()
  {
    gameOverCanvasUi.gameOverOptions.SetActive (true);

    commonEnableButtonFunction.EnableListViewButtons (gameOverCanvasUi.gameOverButtonList);

    cursorManager.InitCursorPosition (gameOverCanvasUi.firstSelectedButton);
    cursorManager.SetMyTweenSpeed (0.6f);
    EventSystem.current.SetSelectedGameObject (gameOverCanvasUi.firstSelectedButton);

    gameOverState = GameOverState.gameOver;
  }

  public void EndSelectingOption ()
  {
    gameOverState = GameOverState.selected;
    commonEnableButtonFunction.DisableListViewButtons (gameOverCanvasUi.gameOverButtonList);
    EventSystem.current.SetSelectedGameObject (null);
  }
}