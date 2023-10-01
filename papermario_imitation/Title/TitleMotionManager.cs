using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleMotionManager
{
  private TitleCanvasUi titleCanvasUi;
  private TitleButtonEnableManager titleButtonEnableManager;
  private CursorManager titleOptionCursorManager;
  private CursorManager dialogOptionCursorManager;
  private GameObject titleStartText;
  private GameObject titleOptions;
  private GameObject titleFirstSelectedOption;
  private GameObject dialogFirstSelectedOption;
  private GameObject titleContinueButton;
  private bool pushedSpaceKey;
  private bool endSelected;

  private TitleState titleState;

  public enum TitleState
  {
    title,
    dialog,
    selected
  }

  private TitleState MotionTitleState
  {
    set { titleState = value; }
    get { return titleState; }
  }

  public TitleMotionManager (
    TitleCanvasUi titleCanvasUi
  )
  {
    pushedSpaceKey = false;
    endSelected = false;

    titleState = TitleState.title;

    this.titleCanvasUi = titleCanvasUi;
    this.titleStartText = titleCanvasUi.startText.gameObject;
    this.titleOptions = titleCanvasUi.titleOptions;
    this.titleFirstSelectedOption = titleCanvasUi.titleFirstSelectedOption;
    this.dialogFirstSelectedOption = titleCanvasUi.dialogFirstSelectedOption;
    this.titleContinueButton = titleCanvasUi.titleContinueButton;

    SetupCursors (titleCanvasUi);

    titleButtonEnableManager = new TitleButtonEnableManager (
      this,
      titleCanvasUi.titleOptionButtonList,
      titleCanvasUi.dialogOptionButtonList
    );

    titleStartText.SetActive (true);
    titleOptions.SetActive (false);

    titleStartText.GetComponent<BlinkingByAlpha> ().BlinkingSelfByAlpha ();

    ManageFirstSelectableButton ();
  }

  void SetupCursors (TitleCanvasUi titleCanvasUi)
  {
    titleOptionCursorManager = new CursorManager (titleCanvasUi.titleCursor, titleCanvasUi.eventSystem);
    titleOptionCursorManager.InitCursorPosition (titleFirstSelectedOption);
    titleOptionCursorManager.SetMyTweenSpeed (0.6f);

    dialogOptionCursorManager = new CursorManager (titleCanvasUi.dialogCursor, titleCanvasUi.eventSystem);
    dialogOptionCursorManager.InitCursorPosition (dialogFirstSelectedOption);
    dialogOptionCursorManager.SetMyTweenSpeed (0.6f);
  }

  void ManageFirstSelectableButton ()
  {
    bool hasDataFile = new CommonUiFunctions ().HasGameDataFile ();
    if (hasDataFile)
    {
      titleContinueButton.SetActive (true);
    }
    else
    {
      // titleCanvasUi.dialogManager.HideDialog ();
      titleContinueButton.SetActive (false);
    }
  }

  public void ManageTitleMotion ()
  {
    if (titleState == TitleState.title)
    {
      StartSelectingOption ();
      titleOptionCursorManager.MoveCursorTween ();
    }
    else if (titleState == TitleState.dialog)
    {
      dialogOptionCursorManager.MoveCursorTween ();
      if (Input.GetKeyDown (KeyCode.Backspace))
      {
        SelectingFromTitleOptions ();
        // Debug.Log ("ダイアログを閉じる関数をタイトルモーションマネージャーから呼んでるよ");
        titleCanvasUi.dialogManager.CloseDialog ();
      }
    }
  }

  void StartSelectingOption ()
  {
    if (!pushedSpaceKey && Input.GetKeyDown (KeyCode.Space))
    {
      pushedSpaceKey = true;
      titleStartText.SetActive (false);
      titleOptions.SetActive (true);

      EventSystem.current.SetSelectedGameObject (titleFirstSelectedOption);
    }
  }

  public void SelectingFromTitleOptions ()
  {
    titleButtonEnableManager.SelectingFromTitleOptions ();
    titleState = TitleState.title;
  }

  public void SelectingFromNewGameDialogOptions ()
  {
    titleButtonEnableManager.SelectingFromNewGameDialogOptions ();
    titleState = TitleState.dialog;
  }

  public void CanNotSelectAnyButton ()
  {
    titleButtonEnableManager.CanNotSelectAnyButton ();
  }

  public void EndSelectingOption ()
  {
    titleState = TitleState.selected;
    CanNotSelectAnyButton ();
  }

}