using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeInformation
{
  private float defaultScreenWidth;
  private float defaultScreenHeight;
  private float screenSizeRatio;
  public ScreenSizeInformation ()
  {
    // ゲーム作成中の画面サイズ
    // Debug.Log ("Screen.width : " + Screen.width);  
    defaultScreenWidth = 640.0f;
    // Debug.Log ("Screen.height : " + Screen.height); 
    defaultScreenHeight = 360f;
    screenSizeRatio = Screen.width / defaultScreenWidth;
  }

  public float ScreenSizeRatio
  {
    get { return screenSizeRatio; }
  }
}