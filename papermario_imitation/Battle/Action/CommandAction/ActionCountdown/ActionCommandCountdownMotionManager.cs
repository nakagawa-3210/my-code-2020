using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class ActionCommandCountdownMotionManager
{
  private ActionCommandCountdownManager actionCommandCountdownManager;
  private GameObject hammerActionSign;
  private GameObject actionSigns;
  private int countdownTime;
  public ActionCommandCountdownMotionManager (ActionCommandCountdownManager actionCommandCountdownManager, GameObject hammerActionSign, float countdownTime)
  {
    this.actionCommandCountdownManager = actionCommandCountdownManager;
    this.hammerActionSign = hammerActionSign;
    this.countdownTime = (int) countdownTime;
    actionSigns = hammerActionSign.transform.Find ("ActionSigns").gameObject;
    InactivateFullSign ();
  }

  // ハンマー系の攻撃に用いるUIのサインの動き管理
  public async UniTask StartHammerCountdownMotion ()
  {
    List<GameObject> actionSignList;
    actionSignList = new MenuCommonFunctions ().GetChildList (actionSigns);
    int actionSignNum = actionSignList.Count;
    foreach (Transform actionSign in actionSigns.transform)
    {
      GameObject commandFullSign = actionSign.Find ("CommandFullSign").gameObject;
      int toMillSec = 1000;
      int waitMillSec = countdownTime * toMillSec / actionSignNum;
      await UniTask.Delay (waitMillSec);
      // ハンマーを振り下ろしたら光る処理はさせないのでハンマーアクションのコマンドに依存させる
      bool hasActionChance = actionCommandCountdownManager.HasActionCommandChance;
      if (hasActionChance)
      {
        commandFullSign.SetActive (true);
        int lastSign = actionSignNum - 1;
        if (actionSign.gameObject == actionSignList[lastSign])
        {
          SEManager.Instance.Play (SEPath.COUNT_END);
        }
        else
        {
          // 音の再生もここに書く
          SEManager.Instance.Play (SEPath.COUNT);
        }
      }
    }
  }

  public void InactivateFullSign ()
  {
    foreach (Transform actionSign in actionSigns.transform)
    {
      GameObject commandFullSign = actionSign.Find ("CommandFullSign").gameObject;
      commandFullSign.SetActive (false);
    }
  }

}