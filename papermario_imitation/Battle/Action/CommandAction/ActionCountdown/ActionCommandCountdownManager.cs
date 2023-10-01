using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class ActionCommandCountdownManager : MonoBehaviour
{
  [SerializeField] int hammerChanceFrame; //調整用 30~50くらい？
  [SerializeField] GameObject hammerActionSign;
  private ActionCommandCountdownMotionManager actionCommandCountdownMotionManager;
  private float countdown;
  private bool startCountdown;
  private bool endCountdown;
  private bool hasActionCommandChance;
  private bool startActionTime;
  private bool succeededAction;

  public bool EndCountDown
  {
    get { return endCountdown; }
  }
  public bool HasActionCommandChance
  {
    get { return hasActionCommandChance; }
  }
  public bool SucceededAction
  {
    get { return succeededAction; }
  }

  void Start ()
  {
    ResetCountdown ();
    actionCommandCountdownMotionManager = new ActionCommandCountdownMotionManager (
      this,
      hammerActionSign,
      countdown
    );
  }

  void Update ()
  {
    Countdown ();
    ManageActionChance ();
    ObserveCommandResult ();
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
    }
  }

  async UniTask ManageActionChance ()
  {
    if (!startActionTime && endCountdown)
    {
      startActionTime = true;
      // 待ち時間をバッジの有無で変更予定
      // int actionCommandFrame = 10;
      int actionCommandFrame = hammerChanceFrame;
      await UniTask.DelayFrame (actionCommandFrame);
      hasActionCommandChance = false;
    }
  }

  void ObserveCommandResult ()
  {
    if (hasActionCommandChance && Input.GetKeyUp (KeyCode.Space))
    {
      if (endCountdown)
      {
        // Debug.Log ("せいこう");
        succeededAction = true;
      }
      // カウントダウン終了前にもハンマーをおろせる
      else if (!endCountdown)
      {
        // Debug.Log ("はやすぎー");
        succeededAction = false;
      }
      // 一度離せばおしまい
      hasActionCommandChance = false;
    }
  }

  public void StartCountdownAction ()
  {
    startCountdown = true;
    hasActionCommandChance = true;
    // Debug.Log ("カウントダウン開始!!");
    actionCommandCountdownMotionManager.StartHammerCountdownMotion ();
  }

  public void InactivateHammerSign ()
  {
    actionCommandCountdownMotionManager.InactivateFullSign ();
  }

  public void ResetCountdown ()
  {
    // 原作ではカウントダウンは2秒くらい
    countdown = 2.0f;
    startCountdown = false;
    endCountdown = false;
    hasActionCommandChance = false;
    startActionTime = false;
    succeededAction = false;
  }

  void SetPoweredAttackCountdown (bool isPoweredAttack)
  {
    // 重い攻撃の時はカウントダウンが倍の長さになる
    countdown *= 2.0f;
  }
}