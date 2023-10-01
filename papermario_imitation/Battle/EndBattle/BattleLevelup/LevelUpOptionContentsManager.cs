using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOptionContentsManager
{
  private GameObject badgeLevelOption;
  private GameObject heartLevelOption;
  private GameObject flowerLevelOption;
  private Text levelUpOptionDescription;
  private List<GameObject> levelUpOptionList;
  private List<LevelOptionDescriptionContainer> levelOptionDescriptionContainerList;
  public LevelUpOptionContentsManager (
    GameObject badgeLevelOption,
    GameObject heartLevelOption,
    GameObject flowerLevelOption,
    GameObject levelUpOptionDescriptionFrame
  )
  {
    this.badgeLevelOption = badgeLevelOption;
    this.heartLevelOption = heartLevelOption;
    this.flowerLevelOption = flowerLevelOption;

    levelUpOptionDescription = levelUpOptionDescriptionFrame.transform.Find ("LevelUpOptionDescriptionText").GetComponent<Text> ();

    SetupLists ();

    SetupOptionsText ();
  }

  void SetupLists ()
  {
    levelUpOptionList = new List<GameObject> ();
    levelUpOptionList.Add (badgeLevelOption);
    levelUpOptionList.Add (heartLevelOption);
    levelUpOptionList.Add (flowerLevelOption);

    levelOptionDescriptionContainerList = new List<LevelOptionDescriptionContainer> ();
    levelOptionDescriptionContainerList.Add (badgeLevelOption.GetComponent<LevelOptionDescriptionContainer> ());
    levelOptionDescriptionContainerList.Add (heartLevelOption.GetComponent<LevelOptionDescriptionContainer> ());
    levelOptionDescriptionContainerList.Add (flowerLevelOption.GetComponent<LevelOptionDescriptionContainer> ());
  }

  void SetupOptionsText ()
  {
    MyGameData.MyData data = SaveSystem.Instance.userData;

    int hpGrowth = data.playerHpGrowthPoint;
    int fpGrowth = data.playerFpGrowthPoint;
    int bpGrowth = data.playerBpGrowthPoint;

    int currentMaxHp = data.playerMaxHp;
    int currentMaxFp = data.playerMaxFp;
    int currentMaxBp = data.playerMaxBp;

    SetupText (heartLevelOption, currentMaxHp, hpGrowth);
    SetupText (flowerLevelOption, currentMaxFp, fpGrowth);
    SetupText (badgeLevelOption, currentMaxBp, bpGrowth);
  }

  void SetupText (GameObject option, int currentPoint, int growthPoint)
  {
    Transform valueInformation = option.transform.Find ("ValueInformation");
    Transform fromValueText = valueInformation.Find ("FromValueText");
    Transform toValueText = valueInformation.Find ("ToValueText");

    fromValueText.GetComponent<TextMeshPro> ().text = currentPoint.ToString ();
    toValueText.GetComponent<TextMeshPro> ().text = (currentPoint + growthPoint).ToString ();
  }

  public void ManageLevelUpOptionDescription (GameObject selectingOption)
  {
    for (var i = 0; i < levelUpOptionList.Count; i++)
    {
      int levelOptionNum = i;
      GameObject levelUpOption = levelUpOptionList[levelOptionNum];
      if (levelUpOption == selectingOption)
      {
        levelUpOptionDescription.text = levelOptionDescriptionContainerList[levelOptionNum].Description;
      }
    }
  }
}