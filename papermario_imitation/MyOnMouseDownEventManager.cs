using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOnMouseDownEventManager : MonoBehaviour
{
  // マウスのイベントを管理する
  // もしかしたらゲームマネージャー内に移植するかもしれない処理
  void Start ()
  {
    // マウスを固定できるがイベントを拾ってしまう
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update ()
  {

  }
}