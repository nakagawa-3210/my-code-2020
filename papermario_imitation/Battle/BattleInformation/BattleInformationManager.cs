using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class BattleInformationManager : MonoBehaviour
{
  [SerializeField] GameObject battleInformationFrame;
  [SerializeField] GameObject battleInformationText;
  
  private Text informationText;

  private int showingDelay;
  

  void Start ()
  {
    informationText = battleInformationText.GetComponent<Text> ();
    showingDelay = 800;
    HideBattleInformationFrame ();
  }

  public async UniTask ShowDefenceUpInformation (int defenceUpNum = 0)
  {
    string upAmount = defenceUpNum.ToString ();
    string description = "ぼうぎょりょくが　" + upAmount + "アップしたぞ！";
    informationText.text = description;

    battleInformationFrame.SetActive (true);

    // 表示の間を作る
    await UniTask.Delay (showingDelay);
  }

  public async UniTask ShowDefenceResetInformation ()
  {
    string description = "ぼうぎょりょくが　もとにもどったぞ！";
    informationText.text = description;

    battleInformationFrame.SetActive (true);

    // 表示の間を作る
    await UniTask.Delay (showingDelay);
  }

  public void HideBattleInformationFrame ()
  {
    string description = "";
    informationText.text = description;
    battleInformationFrame.SetActive (false);
  }

}