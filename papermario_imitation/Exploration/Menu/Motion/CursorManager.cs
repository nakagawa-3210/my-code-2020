using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager
{
  private ScreenSizeInformation screenSizeInformation;
  private GameObject defaultSelectedButton;
  private EventSystem eventSystem;
  private Vector3 selectedTagPosition;
  private GameObject self;
  private GameObject previousSelectedButton;
  private Tween tween;

  private float tweenSpeed;
  private float littleLeft;
  private float defaultScreenWidth = 640.0f;

  public CursorManager (
    GameObject self,
    EventSystem eventSystem
  )
  {
    this.self = self;
    this.eventSystem = eventSystem;
    screenSizeInformation = new ScreenSizeInformation ();
  }
  public void InitCursorPosition (GameObject defaultSelectedButton)
  {

    this.defaultSelectedButton = defaultSelectedButton;
    Vector3 initialPosition = defaultSelectedButton.transform.position;
    initialPosition.x -= littleLeft;
    self.transform.position = initialPosition;
  }
  GameObject GetSelectedButton ()
  {
    GameObject selectedButton = defaultSelectedButton;
    if (eventSystem.currentSelectedGameObject == null)
    {
      return null;
    }
    selectedButton = eventSystem.currentSelectedGameObject.gameObject;
    return selectedButton;
  }
  public void ManageTweenSpeed (bool isOpen)
  {
    if (isOpen)
    {
      tweenSpeed = 0.6f;
    }
    else
    {
      tweenSpeed = 0.1f;
    }
  }
  public void SetMyTweenSpeed (float speed)
  {
    tweenSpeed = speed;
  }
  public void MoveCursorTween ()
  {
    if (defaultSelectedButton == null) return;
    // ボタンが前回と別で選択されているか、ボタンがメニューの開け閉めによって位置が変わっている時にカーソルが移動する
    GameObject selectedButton = GetSelectedButton ();
    if (selectedButton != null && (previousSelectedButton != selectedButton || self.transform.position != selectedButton.transform.position))
    {
      previousSelectedButton = selectedButton;
      selectedTagPosition = selectedButton.transform.position;
      float ratio = screenSizeInformation.ScreenSizeRatio;
      // 画面サイズと選択中のボタンに合わせて指さし位置の調整
      littleLeft = (selectedButton.GetComponent<RectTransform> ().sizeDelta.x / 2) * ratio;
      // 指さし対象よりも少し左の位置になるようにする
      selectedTagPosition.x -= littleLeft;
    }
    self.transform.DOMove (selectedTagPosition, tweenSpeed);
  }
}