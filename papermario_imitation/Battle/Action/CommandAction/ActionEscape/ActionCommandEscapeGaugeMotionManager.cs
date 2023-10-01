using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class ActionCommandEscapeGaugeMotionManager
{
  private GameObject escapeActionSign;
  private GameObject escapeArrow;

  private Sequence escapeArrowSequence;
  private Image escapeLightGreenGaugeImg;
  public ActionCommandEscapeGaugeMotionManager (
    GameObject escapeActionSign
  )
  {
    this.escapeActionSign = escapeActionSign;
    Setup ();
  }

  void Setup ()
  {
    Transform escapeGauge = escapeActionSign.transform.Find ("EscapeGauge");
    escapeLightGreenGaugeImg = escapeGauge.transform.Find ("EscapeLightGreenGauge").GetComponent<Image> ();
    // float valueFrom = Random.Range (0.4f, 0.7f); // 乱数で決まるようにしたい(0.4~0.7くらい？)
    // escapeLightGreenGaugeImg.fillAmount = valueFrom;

    escapeArrow = escapeGauge.transform.Find ("EscapeArrow").gameObject;
  }

  public void IncreaseLightGreenGaugeValue ()
  {
    float increaseValue = 0.02f;
    float valueFrom = escapeLightGreenGaugeImg.fillAmount;
    float valueTo = (escapeLightGreenGaugeImg.fillAmount + increaseValue) / 1.0f; //0~1がfillAmountの値の幅なので1.0を分母にしている
    float duration = 0.2f;
    new GaugeValueTween ().ChangeGaugeValueTween (
      escapeLightGreenGaugeImg,
      valueFrom,
      valueTo,
      duration
    );
  }

  public async UniTask MovingEscapeArrow ()
  {
    float startValue = Random.Range (0.0f, 1.0f);

    float repetitionDuration = 2.0f;
    float maxValue = 1.0f;
    float minValue = 0.0f;

    // 初動
    await DOTween.Sequence ().Append (DOTween.To (
        () => startValue,
        x =>
        {
          escapeArrow.GetComponent<Slider> ().value = x;
        },
        maxValue,
        repetitionDuration / 2 * (1.0f - startValue) //スタート位置の値に比例してduration変更
      ).SetEase (Ease.Linear))
      .Append (DOTween.To (
        () => maxValue,
        x =>
        {
          escapeArrow.GetComponent<Slider> ().value = x;
        },
        minValue,
        repetitionDuration / 2
      ).SetEase (Ease.Linear));

    // 往復
    escapeArrowSequence = DOTween.Sequence ().Append (DOTween.To (
        () => minValue,
        x =>
        {
          escapeArrow.GetComponent<Slider> ().value = x;
        },
        maxValue,
        repetitionDuration / 2
      ).SetEase (Ease.Linear))
      .Append (DOTween.To (
        () => maxValue,
        x =>
        {
          escapeArrow.GetComponent<Slider> ().value = x;
        },
        minValue,
        repetitionDuration / 2
      ).SetEase (Ease.Linear)).SetLoops (-1);
  }

  public void StartEscapeCharange ()
  {
    float valueFrom = Random.Range (0.4f, 0.7f); // 乱数で決まるようにしたい(0.4~0.7くらい？)
    escapeLightGreenGaugeImg.fillAmount = valueFrom;
    PlayEscapeArrowTween ();
  }

  void PlayEscapeArrowTween ()
  {
    float startValue = Random.Range (0.0f, 1.0f);
    escapeArrow.GetComponent<Slider> ().value = startValue;
    MovingEscapeArrow ();
  }

  public void KillEscapeArrowTween ()
  {
    escapeArrowSequence.Kill ();
  }

  public float GetLightGreenGaugeValue ()
  {
    return escapeLightGreenGaugeImg.fillAmount;
  }

  public float GetEscapeArrowValue ()
  {
    return escapeArrow.GetComponent<Slider> ().value;
  }

}