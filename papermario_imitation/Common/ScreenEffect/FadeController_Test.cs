using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeController_Test : MonoBehaviour
{
  [SerializeField] bool isDialogFade;
  [SerializeField] float fadeSpeed = 0.03f;
  private float red, green, blue, alpha;
  private Image fadeImg;

  //外部のクラスからも変更可能
  // SerializeFieldに変更しても使えるかを必ず確認して改修
  public bool isFadeOut = false;
  public bool isDialogFadeOut = false;
  public bool isFadeIn = false;
  public bool isAnimated = false;
  private bool endDialogFade = false;

  void Start ()
  {
    // ダイアログ用フェードならfadeSpeedを上げる
    if (isDialogFade)
    {
      fadeSpeed *= 3;
    }
    //画面いっぱいに配置
    RectTransform rtf = this.GetComponent<RectTransform> ();
    // 横方向のサイズ
    rtf.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.currentResolution.width);
    // 縦方向のサイズ
    rtf.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.currentResolution.height);
    fadeImg = GetComponent<Image> ();
    red = fadeImg.color.r;
    green = fadeImg.color.g;
    blue = fadeImg.color.b;
    alpha = fadeImg.color.a;
    SetupFade ();
  }

  void Update ()
  {
    ManageFadeInOut ();
  }

  public bool IsFadeOut
  {
    set { isFadeOut = value; }
    get { return isFadeOut; }
  }

  public bool IsFadeIn
  {
    set { isFadeIn = value; }
    get { return isFadeIn; }
  }

  public float FadeAlpha
  {
    get { return fadeImg.color.a; }
  }

  public bool IsAnimated
  {
    set { isAnimated = value; }
    get { return isAnimated; }
  }

  public bool EndDialogFade
  {
    get { return endDialogFade; }
  }
  void SetupFade ()
  {
    if (isFadeIn)
    {
      alpha = 1;
      ApplyAlphaValue ();
    }
    if (isFadeOut)
    {
      alpha = 0;
      ApplyAlphaValue ();
    }
  }

  public void StartFadeIn ()
  {
    isFadeIn = true;
  }
  public void StartFadeOut ()
  {
    isFadeOut = true;
  }
  public void StartDialogFadeOut ()
  {
    isDialogFadeOut = true;
  }

  void ManageFadeInOut ()
  {
    if (isFadeIn)
    {
      FadeIn ();
    }
    if (isFadeOut)
    {
      FadeOut ();
    }
    if (isDialogFadeOut)
    {
      DialogFadeOut ();
    }
  }

  void FadeIn ()
  {
    alpha -= fadeSpeed * Time.deltaTime;
    ApplyAlphaValue ();
    //フェードイン終了のタイミング
    if (alpha <= 0)
    {
      isFadeIn = false;
      fadeImg.enabled = false;
      isAnimated = true;
    }
  }

  void FadeOut ()
  {
    // フェードイン中にフェードアウトはできない
    if (!isFadeIn)
    {
      fadeImg.enabled = true;
      alpha += fadeSpeed * Time.deltaTime;
      ApplyAlphaValue ();
      //フェードアウト終了のタイミング
      if (alpha >= 1)
      {
        isFadeOut = false;
        isAnimated = true;
      }
    }
  }

  void DialogFadeOut ()
  {
    if (!isFadeIn)
    {
      //フェードアウト終了のタイミング
      if (alpha >= 0.7f)
      {
        isDialogFadeOut = false;
        isAnimated = true;
        endDialogFade = true;
      }
      else
      {
        fadeImg.enabled = true;
        alpha += fadeSpeed * Time.deltaTime;
        ApplyAlphaValue ();
      }
    }
  }

  void ApplyAlphaValue ()
  {
    fadeImg.color = new Color (red, green, blue, alpha);
  }
}