using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAndPlayerManager
{
  // [SerializeField] MenuCanvasManager menuCanvasManger;
  // [SerializeField] GameObject player;
  private MenuCanvasMotionManager menuCanvasMotionManagerScript;
  private HanaPlayerScript hanaPlayerScript;
  private bool resetStatus;

  // void Start ()
  // {
  //   resetStatus = true;
  //   // resetStatus = false;
  //   menuCanvasMotionManagerScript = menuCanvasManger.MenuCanvasMotionManagerScript;
  //   hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
  // }

  public MenuAndPlayerManager (MenuCanvasMotionManager menuCanvasMotionManagerScript, HanaPlayerScript hanaPlayerScript)
  {
    this.menuCanvasMotionManagerScript = menuCanvasMotionManagerScript;
    this.hanaPlayerScript = hanaPlayerScript;
  }

  public void ManagePlayerStatus ()
  {
    if (!menuCanvasMotionManagerScript.IsMenuClosedCompletely ())
    {
      hanaPlayerScript.PlayerState = HanaPlayerScript.State.OpenMenu;
      resetStatus = false;
    }
    else
    {
      //  HanaPlayerScript.State.WalkingNextSceneの箇所は要改修
      if (!resetStatus && hanaPlayerScript.PlayerState != HanaPlayerScript.State.WalkingNextScene)
      {
        // Debug.Log ("メニューが閉まったからnormalになったよ");
        resetStatus = true;
        hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
      }
    }
  }

  // void Update ()
  // {
  //   // tweenも済んでいるかの確認を含めた条件
  //   Debug.Log ("menuCanvasMotionManagerScript.IsMenuClosedCompletely () : " + menuCanvasMotionManagerScript.IsMenuClosedCompletely ());
  //   if (menuCanvasMotionManagerScript == null) return;
  //   if (!menuCanvasMotionManagerScript.IsMenuClosedCompletely ())
  //   {
  //     hanaPlayerScript.PlayerState = HanaPlayerScript.State.OpenMenu;
  //     resetStatus = false;
  //   }
  //   else
  //   {
  //     if (!resetStatus)
  //     {
  //       Debug.Log ("メニューが閉まったからnormalになったよ");
  //       resetStatus = true;
  //       hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
  //     }
  //   }
  // }
}