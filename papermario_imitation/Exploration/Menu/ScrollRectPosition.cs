// 下記サイトよりコードを持ってきて自分のリストビューに合わせて変更
// 子要素の持つGridLayoutGroupのCellSizeYの値に依存性があるので、GridLayoutGroupで決めるボタンの大きさはリストビューの大きさに合わせて要調整
// https://forum.unity.com/threads/scroll-rect-and-scroll-bar-arrow-keys-control.339661/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectPosition : MonoBehaviour
{

  RectTransform scrollRectTransform;
  RectTransform contentPanel;
  RectTransform selectedRectTransform;
  GameObject lastSelected;

  void Start ()
  {
    scrollRectTransform = GetComponent<RectTransform> ();
    contentPanel = GetComponent<ScrollRect> ().content;
  }

  void Update ()
  {
    // Get the currently selected UI element from the event system.
    GameObject selected = EventSystem.current.currentSelectedGameObject;

    // Return if there are none.
    if (selected == null)
    {
      return;
    }
    // Return if the selected game object is not inside the scroll rect.
    if (selected.transform.parent != contentPanel.transform)
    {
      return;
    }
    // Return if the selected game object is the same as it was last frame,
    // meaning we haven't moved.
    if (selected == lastSelected)
    {
      return;
    }

    // Get the rect tranform for the selected game object.
    selectedRectTransform = selected.GetComponent<RectTransform> ();
    // Debug.Log(selectedRectTransform.rect.height);
    // The position of the selected UI element is the absolute anchor position,
    // ie. the local position within the scroll rect + its height if we're
    // scrolling down. If we're scrolling up it's just the absolute anchor position.
    float selectedPositionY = Mathf.Abs (selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;

    // The upper bound of the scroll view is the anchor position of the content we're scrolling.
    float scrollViewMinY = contentPanel.anchoredPosition.y;
    // The lower bound is the anchor position + the height of the scroll rect.
    float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

    // If the selected position is below the current lower bound of the scroll view we scroll down.
    if (selectedPositionY > scrollViewMaxY)
    {
      // Debug.Log ("selectedPositionY : " + selectedPositionY);
      // Debug.Log ("scrollViewMaxY : " + scrollViewMaxY);
      // 選ぶボタンのアンカーの位置はGridLayoutGroupによって中心にはなく左上にあるので、その分selectedRectTransform.rect.height/2で高さを修正
      float newY = selectedPositionY - scrollRectTransform.rect.height - selectedRectTransform.rect.height / 2;
      contentPanel.anchoredPosition = new Vector2 (contentPanel.anchoredPosition.x, newY);
    }
    // If the selected position is above the current upper bound of the scroll view we scroll up.
    else if (Mathf.Abs (selectedRectTransform.anchoredPosition.y) < scrollViewMinY)
    {
      // selectedRectTransform.rect.height/2で修正
      contentPanel.anchoredPosition = new Vector2 (contentPanel.anchoredPosition.x, Mathf.Abs (selectedRectTransform.anchoredPosition.y + selectedRectTransform.rect.height / 2));
    }

    lastSelected = selected;
  }
}