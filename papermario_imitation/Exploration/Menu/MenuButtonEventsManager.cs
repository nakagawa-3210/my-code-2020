using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonEventsManager : MonoBehaviour
{
  private MenuCanvasMotionManager menuCanvasMotionManager;
  public MenuCanvasMotionManager EventManagerMenuCanvasMotionManager
  {
    set { menuCanvasMotionManager = value; }
  }

  public void SelectingMenuTags ()
  {
    menuCanvasMotionManager.SelectingMenuTags ();
  }
  public void SelectingPartnerOptions ()
  {
    menuCanvasMotionManager.SelectingPartnerOptions ();
  }
  public void SelectingSkill ()
  {
    menuCanvasMotionManager.SelectingSkill ();
  }
  public void SelectingBelongingOptions ()
  {
    menuCanvasMotionManager.SelectingBelongingOptions ();
  }
  public void SelectingItem ()
  {
    menuCanvasMotionManager.SelectingItem ();
  }
  public void SelectingImportantThing ()
  {
    menuCanvasMotionManager.SelectingImportantThing ();
  }
  public void SelectingBadgeOptions ()
  {
    menuCanvasMotionManager.SelectingBadgeOptions ();
  }
  public void SelectingBadgesFromAll ()
  {
    menuCanvasMotionManager.SelectingBadgesFromAll ();
  }
  public void SelectingBadgeFromEquipping ()
  {
    menuCanvasMotionManager.SelectingBadgeFromEquipping ();
  }
}