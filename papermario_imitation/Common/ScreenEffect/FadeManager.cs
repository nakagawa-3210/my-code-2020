using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
  [SerializeField] float fadeSpeed = default;
  [SerializeField] float dialogFadeSpeed = default;
  [SerializeField] bool startFadeInAtFirst = default;
  [SerializeField] Image fadeImage;

  private GameObject self;
  // private Image fadeImage;

  private float fadeIn;
  private float fadeOut;

  private bool startFadeTween;
  private bool endSetup;
  private bool endFadeIn;
  private bool endFadeOut;

  public bool StartFadeTween
  {
    get { return startFadeTween; }
  }

  public bool EndSetup
  {
    get { return endSetup; }
  }

  public bool EndFadeIn
  {
    get { return endFadeIn; }
  }

  public bool EndFadeOut
  {
    get { return endFadeOut; }
  }
  async UniTask Start ()
  {
    self = this.gameObject;

    RectTransform rectTransform = self.GetComponent<RectTransform> ();
    rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.currentResolution.width);
    rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.currentResolution.height);

    // fadeImage = self.transform.GetComponent<Image> ();

    fadeIn = 0.0f;
    fadeOut = 1.0f;

    endFadeIn = false;
    endFadeOut = false;

    await SetupFade ();
  }

  // ゲーム初回起動時のみフェードアウトが出来ないが、α値が変更されていることは確認出来た。
  // ゲームオーバー画面からタイトルに遷移したときにはフェードアウトは機能している
  // void Update ()
  // {
  //   Debug.Log ("fadeImage.color : " + fadeImage.color);
  //   Debug.Log ("fadeImage.GetComponent<Image>().enabled : " + fadeImage.GetComponent<Image> ().enabled);
  //   Debug.Log ("startFadeTween : " + startFadeTween);
  //   Debug.Log ("fadeImageのa値" + fadeImage.color.a);
  // }

  async UniTask SetupFade ()
  {
    endSetup = false;
    Debug.Log ("呼ばれてはいる");
    if (startFadeInAtFirst)
    {
      FadeIn ();
    }
    else
    {
      await FadeTween (fadeIn, 0.0f);
    }

    endSetup = true;
  }

  public async UniTask FadeIn ()
  {
    await FadeTween (fadeIn, fadeSpeed);
    endFadeIn = true;
  }

  public async UniTask FadeOut ()
  {
    await FadeTween (fadeOut, fadeSpeed);
    endFadeOut = true;
  }

  public async UniTask DialogFadeIn ()
  {
    await FadeTween (fadeIn, dialogFadeSpeed);
  }

  public async UniTask DialogFadeOut ()
  {
    float dialogFadeOut = fadeOut / 2.0f;
    await FadeTween (dialogFadeOut, dialogFadeSpeed);
  }

  async UniTask FadeTween (float fadeAlpha, float tweenSpeed)
  {
    Debug.Log ("画面遷移用のフェード関数がよばれてるよー");
    startFadeTween = true;
    await fadeImage.DOFade (fadeAlpha, tweenSpeed);
    startFadeTween = false;
  }

}