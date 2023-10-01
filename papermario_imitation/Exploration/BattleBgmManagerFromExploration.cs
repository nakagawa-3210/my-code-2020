using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleBgmManagerFromExploration
{
  // private string currentSceneName;
  public BattleBgmManagerFromExploration ()
  {
    // this.currentSceneName = currentSceneName;
  }

  public void PlayBattleBgm ()
  {
    BGMManager bgmManager = BGMManager.Instance;
    string currentSceneName = SceneManager.GetActiveScene ().name;
    if (currentSceneName == "StageOneExplorerOneScene")
    {
      bgmManager.Play (BGMPath.STEPPE_BATTLE);
    }
    else if (currentSceneName == "StageOneExplorerTwoScene")
    {
      bgmManager.Play (BGMPath.TURTLE_BOSS_BATTLE);
    }
  }
}