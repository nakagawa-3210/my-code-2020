using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class ScreenEffectUi : MonoBehaviour
{
  public CurtainManager curtainManager;
  public FadeManager fadeManager;

  private bool endScreenEffectSetup;

  public bool EndScreenEffectSetup
  {
    get { return endScreenEffectSetup; }
  }

  async UniTask Start ()
  {
    endScreenEffectSetup = false;
    await UniTask.WaitUntil (() => fadeManager.EndSetup);
    endScreenEffectSetup = true;
  }

}