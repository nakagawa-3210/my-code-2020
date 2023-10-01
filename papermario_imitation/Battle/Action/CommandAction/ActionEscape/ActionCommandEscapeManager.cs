using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCommandEscapeManager : MonoBehaviour
{
  [SerializeField] float escapeCountdown;
  [SerializeField] GameObject escapeActionSign;

  private float countdown;

  private bool startCountdown;
  private bool endCountdown;
  private bool canEscape;

  public bool EndCountDown
  {
    get { return endCountdown; }
  }

  public bool CanEscape
  {
    get { return canEscape; }
  }

  private ActionCommandEscapeGaugeMotionManager actionCommandEscapeGaugeMotionManager;

  void Start ()
  {
    ResetEscapeCountdown ();
    actionCommandEscapeGaugeMotionManager = new ActionCommandEscapeGaugeMotionManager (
      escapeActionSign
    );
  }

  void Update ()
  {
    Countdown ();
    IncreaseEscapeGaugeValue ();
  }

  public void StartEscapeChance ()
  {
    ResetEscapeCountdown ();
    startCountdown = true;
    actionCommandEscapeGaugeMotionManager.StartEscapeCharange ();
  }

  void Countdown ()
  {
    if (startCountdown && !endCountdown)
    {
      countdown -= Time.deltaTime;
    }
    if (countdown < 0 && !endCountdown)
    {
      endCountdown = true;
      actionCommandEscapeGaugeMotionManager.KillEscapeArrowTween ();
      canEscape = GetCanEscape ();
    }
  }

  bool GetCanEscape ()
  {
    float escapeValue = actionCommandEscapeGaugeMotionManager.GetLightGreenGaugeValue ();
    float escapeLine = actionCommandEscapeGaugeMotionManager.GetEscapeArrowValue ();
    return escapeValue > escapeLine;
  }

  void IncreaseEscapeGaugeValue ()
  {
    if (startCountdown && !endCountdown && Input.GetKeyDown (KeyCode.Space))
    {
      actionCommandEscapeGaugeMotionManager.IncreaseLightGreenGaugeValue ();
    }
  }

  public void ResetEscapeCountdown ()
  {
    countdown = escapeCountdown;
    startCountdown = false;
    endCountdown = false;
    canEscape = false;
  }
}