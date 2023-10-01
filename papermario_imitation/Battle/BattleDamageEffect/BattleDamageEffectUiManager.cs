using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

// コマンド成功時のフォント表示とダメージ量表示は呼ぶ場所が異なる
public class BattleDamageEffectUiManager : MonoBehaviour
{
  [SerializeField] Transform battleDamageEffectCanvas;
  [SerializeField] GameObject battleDamageEffectFonts;
  [SerializeField] GameObject battleDamageEffectStarPrefab;
  [SerializeField] GameObject battleSmallStarPrefab;
  [SerializeField] GameObject battleSmallStarGroupPrefab;
  private List<GameObject> battleDamageEffectFontList;
  private MenuCommonFunctions menuCommonFunctions;
  void Start ()
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    battleDamageEffectFontList = menuCommonFunctions.GetChildList (battleDamageEffectFonts);
    ResetActionEffectsFonts ();
    ResetActionEffectStar ();
  }

  void Update ()
  {
    // if (Input.GetKeyDown (KeyCode.Z))
    // {
    //   // エフェクト文字の表示テスト
    //   // PlaySucceededActionEffect ();
    //   // PlayDamageEffectStar (1);
    // }

    // if (Input.GetKeyDown (KeyCode.R))
    // {
    //   // ResetActionEffectStar ();
    //   HideDamageStar ();
    // }
  }

  // アクションコマンド成功の際に表示する処理をまとめた関数を用意する
  public async UniTask PlaySucceededActionEffect (Transform fontsTran)
  {
    SEManager.Instance.Play (SEPath.SUCCESS_COMMAND);
    await ShowSucceededActionEffectFonts (fontsTran);
    HideSucceededActionEffectFonts ();
  }

  // 一旦はアクションコマンドの成功が最高で一回までのフォントのみ表示
  public async UniTask ShowSucceededActionEffectFonts (Transform fontsTran)
  {
    float showScale = 1.0f;
    float scaleDuration = 0.08f;
    battleDamageEffectFonts.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, fontsTran.position);
    // スケール拡大が終わった直後はリコイル終了を待たずに次のフォントをスケール拡大を始めたい
    // RecoilMotionをawaitすると原作と動きが若干異なる、でもawaitなしの呼び方はfontPositionが途中で変更されてしまってリコイルの位置がずれてしまう
    foreach (var battleEffectFont in battleDamageEffectFontList)
    {
      int waitMillSec = (int) scaleDuration;
      await battleEffectFont.transform.DOScale (showScale, scaleDuration);
      Vector3 fontPosition = battleEffectFont.transform.position;
      // await RecoilMotion (battleEffectFont);
    }
    // 原作通りの反動のつけ方が思いつかないのでコメントアウト中
    // async UniTask RecoilMotion (GameObject font)
    // {
    //   float recoil = 5.0f;
    //   float recoilDuration = 0.04f;
    //   Vector3 fontPosition = font.transform.position;
    //   float initialPositionX = fontPosition.x;
    //   float initialPositionY = fontPosition.y;
    //   float recoilPlusX = initialPositionX + recoil;
    //   float recoilPlusY = initialPositionY + recoil;
    //   float recoilMinusX = initialPositionX - recoil;
    //   float recoilMinusY = initialPositionY - recoil;
    //   await DOTween.Sequence ()
    //     .Append (font.transform.DOMove (new Vector3 (recoilPlusX, recoilPlusY, fontPosition.z), recoilDuration).SetEase (Ease.InBounce))
    //     .Append (font.transform.DOMove (new Vector3 (recoilMinusX, recoilMinusY, fontPosition.z), recoilDuration).SetEase (Ease.InBounce))
    //     .Append (font.transform.DOMove (new Vector3 (initialPositionX, initialPositionY, fontPosition.z), recoilDuration).SetEase (Ease.InBounce));
    // }
  }

  public async UniTask HideSucceededActionEffectFonts ()
  {
    await CommonHideTween (battleDamageEffectFonts);
    ResetActionEffectsFonts ();
  }

  void ResetActionEffectsFonts ()
  {
    float baseScale = 1.0f;
    float hideScale = 0.0f;
    float initAlpha = 1.0f;
    foreach (var battleEffectFont in battleDamageEffectFontList)
    {
      battleEffectFont.transform.localScale = new Vector3 (hideScale, hideScale, hideScale);
    }
    battleDamageEffectFonts.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    battleDamageEffectFonts.GetComponent<CanvasGroup> ().alpha = initAlpha;
  }

  // ダメージ量を示すサインは2つある
  //   ダメージ量の数だけ星を表示するパーティクルのような星サイン
  //   ダメージ量を文字で示したテキスト付きの1つの星サイン
  // 2つのサイン共にInstantiateで複製して表示する
  public async UniTask PlayDamageEffectStar (Transform starTran, int damageAmount)
  {
    GameObject effectStar = GameObject.Instantiate<GameObject> (battleDamageEffectStarPrefab);
    effectStar.transform.SetParent (battleDamageEffectCanvas);
    effectStar.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, starTran.position);
    await ShowDamageTextStar (effectStar, damageAmount);
    HideDamageStar (effectStar);
  }

  //   ダメージ量を文字で示したテキスト付きの1つの星サイン
  async UniTask ShowDamageTextStar (GameObject effectStar, int damageAmount)
  {
    Transform starFrameRotationRoot = effectStar.transform.Find ("StarFrameRotationRoot");
    Transform effectStarImg = starFrameRotationRoot.transform.Find ("StarFrameImg");
    Transform starDamageText = effectStarImg.Find ("DamageText");
    // ダメージ量テキスト更新
    starDamageText.GetComponent<Text> ().text = damageAmount.ToString ();
    // 星の表示
    float showScale = 1.0f;
    float showingScaleDuration = 0.08f;
    await effectStar.transform.DOScale (showScale, showingScaleDuration);
    // 星の伸び縮み
    float extendedScale = 1.2f;
    float contractedScale = 0.4f;
    float baseScale = 1.0f;
    float elasticScaleDuration = 0.1f;
    await DOTween.Sequence ().Append (effectStarImg.DOScaleY (extendedScale, elasticScaleDuration))
      .Append (effectStarImg.DOScaleY (contractedScale, elasticScaleDuration))
      .Append (effectStarImg.DOScaleY (baseScale, elasticScaleDuration));
    // 星の回転
    float firstRotationDuration = 0.1f;
    Vector3 starFirstRotation = starFrameRotationRoot.transform.localEulerAngles;
    starFirstRotation.z = 30.0f;
    Vector3 starTextFirstRotation = starFrameRotationRoot.transform.localEulerAngles;
    starTextFirstRotation.z = 0.0f;
    // 外見が指定された角度まで回転する
    // フレームが30度回転している場合、子要素のテキストは-30度指定で親回転を相殺するのではなく、自身が0度であることを指定する
    await DOTween.Sequence ().Append (starFrameRotationRoot.DORotate (starFirstRotation, firstRotationDuration))
      .Join (starDamageText.DORotate (starTextFirstRotation, firstRotationDuration));
    float secondRotationDuration = 0.1f;
    starFirstRotation.z = -180.0f;
    // 移動を加えた二度目の回転
    Vector3 battleDamageEffectStarPrefabPosition = effectStar.transform.position;
    battleDamageEffectStarPrefabPosition.x += 15.0f;
    await DOTween.Sequence ().Append (starFrameRotationRoot.DOLocalRotate (starFirstRotation, secondRotationDuration, RotateMode.FastBeyond360))
      .Join (starDamageText.DORotate (starTextFirstRotation, secondRotationDuration))
      .Join (effectStar.transform.DOMove (battleDamageEffectStarPrefabPosition, secondRotationDuration));
  }
  async UniTask CommonHideTween (GameObject hideTarget)
  {
    float expandedScale = 1.2f;
    float hideScale = 0.7f;
    float scaleDuration = 0.1f;
    float hideAlpha = 0.0f;
    float fadeDuration = 0.2f;
    await hideTarget.transform.DOScale (expandedScale, scaleDuration);
    hideTarget.transform.DOScale (hideScale, scaleDuration);
    await hideTarget.GetComponent<CanvasGroup> ().DOFade (hideAlpha, fadeDuration);
  }
  async void HideDamageStar (GameObject effectStar)
  {
    await CommonHideTween (effectStar);
    Destroy (effectStar);
  }

  void ResetActionEffectStar ()
  {
    float hideScale = 0.0f;
    battleDamageEffectStarPrefab.transform.localScale = new Vector3 (hideScale, hideScale, hideScale);
  }

  public async UniTask ShowSmallDamageStars (Transform starTran, int damageAmount)
  {
    // スモールスターグループの作成と初期設定
    GameObject damageSmallStarGroup = GameObject.Instantiate (battleSmallStarGroupPrefab);
    damageSmallStarGroup.transform.SetParent (battleDamageEffectCanvas);
    damageSmallStarGroup.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, starTran.position);
    float baseScale = 1.0f;
    damageSmallStarGroup.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);

    List<GameObject> smallStarList = new List<GameObject> ();
    for (var i = 0; i < damageAmount; i++)
    {
      GameObject smallStar = GameObject.Instantiate<GameObject> (battleSmallStarPrefab);
      smallStar.transform.SetParent (damageSmallStarGroup.transform);
      // 初期非表示サイズ変更
      smallStar.transform.localScale = new Vector3 (0, 0, 0);
      // 初期位置リセット
      smallStar.transform.localPosition = new Vector3 (0, 0, 0);
      smallStarList.Add (smallStar);
    }

    // 最初の表示
    CircumferenceArrangement smallStarGroupCircumferenceArrangement = damageSmallStarGroup.GetComponent<CircumferenceArrangement> ();
    float showDuration = 0.2f;
    float endRadius = Screen.width / 72 * 5;
    for (var i = 0; i < smallStarList.Count; i++)
    {
      GameObject smallStar = smallStarList[i];
      int childeNum = i;
      Vector3 movePosition = smallStarGroupCircumferenceArrangement.GetExpandedCircumferencePositionXY (smallStar, childeNum, smallStarList.Count, endRadius);
      float showScale = 0.4f;
      DOTween.Sequence ().Append (smallStar.transform.DOMove (movePosition, showDuration))
        .Join (smallStar.transform.DOScale (showScale, showDuration));
    }
    // 最初の表示を待つ
    await WaitForEachDoTween (showDuration);

    // 表示後にすこし縮む
    float shrinkDuration = 0.1f;
    float shrinkScale = 0.2f;
    ChangeSmallStarsScale (smallStarList, shrinkScale, shrinkDuration);
    await WaitForEachDoTween (shrinkDuration);

    // 再度少し大きくなる
    float expandDuration = 0.15f;
    float expandScale = 0.25f;
    ChangeSmallStarsScale (smallStarList, expandScale, expandDuration);
    await WaitForEachDoTween (expandDuration);

    // 2回転する
    float rotationDuration = 0.2f;
    foreach (var smallStar in smallStarList)
    {
      smallStar.transform.DOLocalRotate (new Vector3 (0, 360f, 0), rotationDuration, RotateMode.FastBeyond360)
        .SetLoops (2, LoopType.Restart);
    }
    await WaitForEachDoTween (rotationDuration);

    // y軸に一度縮み、上に移動しながら、y軸に伸びつつ、x軸に縮む
    float totalHideDuration = 0.0f;
    foreach (var smallStar in smallStarList)
    {
      float shrinkXY = 0.05f;
      float shrinkYDuration = 0.1f;
      float hideScaleY = 0.3f;
      float hideDuration = 0.1f;
      Vector3 hidePosition = smallStar.transform.position;
      hidePosition.y += 60.0f;
      totalHideDuration = shrinkYDuration + hideDuration;
      DOTween.Sequence ().Append (smallStar.transform.DOScaleY (shrinkXY, shrinkYDuration))
        .Append (smallStar.transform.DOScaleX (shrinkXY, hideDuration))
        .Join (smallStar.transform.DOScaleY (hideScaleY, hideDuration))
        .Join (smallStar.transform.DOMoveY (hidePosition.y, hideDuration));
    }
    await WaitForEachDoTween (totalHideDuration);

    // 星の削除
    Destroy (damageSmallStarGroup);
  }

  void ChangeSmallStarsScale (List<GameObject> smallStarList, float shrinkScale, float shrinkDuration)
  {
    foreach (var smallStar in smallStarList)
    {
      smallStar.transform.DOScale (shrinkScale, shrinkDuration);
    }
  }

  async UniTask WaitForEachDoTween (float duration)
  {
    int durationWait = (int) (duration * 1000);
    await UniTask.Delay (durationWait);
  }

}