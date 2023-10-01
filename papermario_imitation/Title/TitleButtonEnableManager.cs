using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButtonEnableManager
{
  private CommonEnableButtonFunction commonEnableButtonFunction;
  private TitleMotionManager titleMotionManager;
  private List<GameObject> titleOptionButtonList;
  private List<GameObject> dialogOptionButtonList;

  public TitleButtonEnableManager (
    TitleMotionManager titleMotionManager,
    List<GameObject> titleOptionButtonList,
    List<GameObject> dialogOptionButtonList
  )
  {
    commonEnableButtonFunction = new CommonEnableButtonFunction ();

    this.titleMotionManager = titleMotionManager;
    this.titleOptionButtonList = titleOptionButtonList;
    this.dialogOptionButtonList = dialogOptionButtonList;

    SelectingFromTitleOptions ();
  }

  public void SelectingFromTitleOptions ()
  {
    commonEnableButtonFunction.EnableListViewButtons (titleOptionButtonList);
    commonEnableButtonFunction.DisableListViewButtons (dialogOptionButtonList);
    EventSystem.current.SetSelectedGameObject (titleOptionButtonList[0]);
  }

  public void SelectingFromNewGameDialogOptions ()
  {
    commonEnableButtonFunction.DisableListViewButtons (titleOptionButtonList);
    commonEnableButtonFunction.EnableListViewButtons (dialogOptionButtonList);
    EventSystem.current.SetSelectedGameObject (dialogOptionButtonList[0]);
  }

  public void CanNotSelectAnyButton ()
  {
    commonEnableButtonFunction.DisableListViewButtons (titleOptionButtonList);
    commonEnableButtonFunction.DisableListViewButtons (dialogOptionButtonList);
    EventSystem.current.SetSelectedGameObject (null); 
  }

}