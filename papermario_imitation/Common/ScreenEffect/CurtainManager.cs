using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class CurtainManager : MonoBehaviour
{
  [SerializeField] GameObject curtainGroup;
  [SerializeField] float doTweenDurationTime;
  [SerializeField] bool wantCurtainOpenAtStart;
  [SerializeField] List<GameObject> curtainList;
  private List<Vector3> curtainShowingPositionList;
  private float curtainOutOfScreenPositionY;
  private bool endTween;
  private bool endSetupCurtain;

  public bool EndTween
  {
    get { return endTween; }
  }
  public bool EndSetupCurtain
  {
    get { return endSetupCurtain; }
  }

  void Start ()
  {
    endTween = false;
    endSetupCurtain = false;
    curtainOutOfScreenPositionY = Screen.height * 1.1f;
    SetupCurtain ();
  }

  void SetupCurtain ()
  {
    CacheCurtainPosition ();
    SetupCurtainInitialPosition ();
    endSetupCurtain = true;
  }

  void CacheCurtainPosition ()
  {
    List<Vector3> positionList = new List<Vector3> ();
    foreach (var curtain in curtainList)
    {
      Vector3 curtainPosition = curtain.transform.position;
      positionList.Add (curtainPosition);
    }
    curtainShowingPositionList = positionList;
  }

  void SetupCurtainInitialPosition ()
  {
    if (wantCurtainOpenAtStart)
    {
      foreach (var curtain in curtainList)
      {
        Vector3 curtainPosition = curtain.transform.position;
        curtainPosition.y = curtainOutOfScreenPositionY;
        curtain.transform.position = curtainPosition;
      }
    }
  }

  public async UniTask ShowAllCurtains ()
  {
    for (var curtainNum = 0; curtainNum < curtainList.Count; curtainNum++)
    {
      for (var shownCurtain = 0; shownCurtain < curtainList.Count - curtainNum; shownCurtain++)
      {
        var curtain = curtainList[curtainNum];
        await ShowCurtainTween (curtain, curtainNum);
      }
    }
    int toMilSec = 1000;
    await UniTask.Delay (Mathf.CeilToInt (doTweenDurationTime) * toMilSec);
    Debug.Log ("おわり");
  }

  async UniTask ShowCurtainTween (GameObject curtain, int curtainNum)
  {
    Vector3 setCurtainPosition = curtainShowingPositionList[curtainNum];
    curtain.transform.DOMove (setCurtainPosition, doTweenDurationTime).OnComplete (() =>
    {
      endTween = true;
    });
  }

  public async UniTask HideAllCurtains ()
  {
    await UniTask.WaitUntil (() => endSetupCurtain);
    for (var curtainNum = curtainList.Count - 1; curtainNum >= 0; curtainNum--)
    {
      int count = curtainList.Count - curtainNum;
      for (var hiddenCurtain = curtainList.Count - 1; hiddenCurtain >= count - 1; hiddenCurtain--)
      {
        var curtain = curtainList[curtainNum];
        await HideCurtainTween (curtain, curtainNum);
      }
    }
  }

  async UniTask HideCurtainTween (GameObject curtain, int curtainNum)
  {
    Vector3 setCurtainPosition = curtainShowingPositionList[curtainNum];
    setCurtainPosition.y = curtainOutOfScreenPositionY;
    curtain.transform.DOMove (setCurtainPosition, doTweenDurationTime).OnComplete (() =>
    {
      endTween = true;
    });
  }
}