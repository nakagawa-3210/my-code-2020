using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class BgmPathGetterWithTransitionSceneName
{
  public string GetBgmPathWithTransitionSceneName (string transitionSceneName)
  {
    string bgmPath = "";
    if (transitionSceneName == "TitleScene")
    {
      bgmPath = BGMPath.TITLE_SCENE;
    }
    else if (transitionSceneName == "GameOverScene")
    {
      bgmPath = BGMPath.GAME_OVER_SCENE;
    }
    else if (transitionSceneName == "GameClearScene")
    {
      bgmPath = BGMPath.GAME_CLEAR_SCENE;
    }
    else if (transitionSceneName == "StageOneExplorerOneScene")
    {
      bgmPath = BGMPath.STEPPE_EXPLORER;
    }
    else if (transitionSceneName == "StageOneExplorerTwoScene")
    {
      bgmPath = BGMPath.TURTLE_EXPLORER_SCENE;
    }
    else if (transitionSceneName == "StartScene")
    {
      bgmPath = BGMPath.HOME_SCENE;
    }
    else if (transitionSceneName == "TownStageOneScene")
    {
      bgmPath = BGMPath.SHOP_SCENE;
    }
    return bgmPath;
  }
}