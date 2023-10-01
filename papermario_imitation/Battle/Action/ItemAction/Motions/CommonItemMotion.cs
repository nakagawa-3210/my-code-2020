using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class CommonItemMotion
{
  private CircumferenceArrangement itemFocusCircumferenceArrangement;

  public async UniTask ShowingUsingItemImage (GameObject self, GameObject itemImage, GameObject itemFocus, Sprite usingItemSprite)
  {
    // 表示に少し間を作る
    int showDelay = 500;
    await UniTask.Delay (showDelay);
    // アイテム表示音
    SEManager.Instance.Play (SEPath.SHOW_ITEM);

    // 使用回復アイテム画像表示
    itemImage.GetComponent<Image> ().sprite = usingItemSprite;

    // アイテム画像表示位置調整
    Vector3 itemPosition = self.transform.position;
    float modifier = 0.6f;
    itemPosition.y += modifier;
    itemImage.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, itemPosition);
    itemImage.SetActive (true);
    // 強調位置調整
    itemFocus.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, itemPosition);

    // 強調再生
    itemFocusCircumferenceArrangement = itemFocus.GetComponent<CircumferenceArrangement> ();
    itemFocusCircumferenceArrangement.ResetDeployXY ();
    itemFocus.SetActive (true);
    itemFocus.GetComponent<CanvasGroup> ().alpha = 0.4f;
    // フェード
    float fadeDuration = 0.5f;
    await ItemFocusTweenFadeIn (itemFocus, fadeDuration);
    await ItemFocusTweenFadeOut (itemFocus, fadeDuration);
    // リセット
    itemFocus.SetActive (false);
    itemImage.SetActive (false);
  }

  async UniTask ItemFocusTweenFadeIn (GameObject itemFocus, float fadeDuration)
  {
    // 強調イメージの円の拡大
    float showFade = 0.6f;
    float endRadius = Screen.width / 24; // 画面サイズに合わせて一定の比率の大きさの円周になるようにする
    List<GameObject> itemFocusList = new MenuCommonFunctions ().GetChildList (itemFocus);
    for (var i = 0; i < itemFocusList.Count; i++)
    {
      GameObject focusChilde = itemFocusList[i];
      int childeNum = i;
      Vector3 movePosition = itemFocusCircumferenceArrangement.GetExpandedCircumferencePositionXY (focusChilde, childeNum, itemFocusList.Count, endRadius);
      focusChilde.transform.DOMove (movePosition, fadeDuration);
    }

    // フェードイン
    await itemFocus.GetComponent<CanvasGroup> ().DOFade (showFade, fadeDuration);
  }

  async UniTask ItemFocusTweenFadeOut (GameObject itemFocus, float fadeDuration)
  {
    float showFade = 0.0f;
    await itemFocus.GetComponent<CanvasGroup> ().DOFade (showFade, fadeDuration);
  }

}