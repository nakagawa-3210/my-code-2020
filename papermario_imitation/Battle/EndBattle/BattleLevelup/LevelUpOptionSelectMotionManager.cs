using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using TMPro;
using UniRx.Async;
using UnityEngine;

public class LevelUpOptionSelectMotionManager
{
  private GameObject badgeLevelOption;
  private GameObject heartLevelOption;
  private GameObject flowerLevelOption;
  private GameObject levelUpSpotLight;
  private GameObject levelUpOptionDescription;
  private List<GameObject> levelUpOptionList;

  private SEManager seManager;

  private int selectingOptionNum;

  private bool endSpotLightTween;
  private bool endSelecting;

  private string[] selectingOptionNameArray;

  public enum SelectingOption
  {
    badge,
    heart,
    flower
  }

  public int SelectingOptionNum
  {
    get { return selectingOptionNum; }
  }

  public bool EndSelecting
  {
    set { endSelecting = value; }
    get { return endSelecting; }
  }

  public GameObject GetSelectedLevelUpOption ()
  {
    return levelUpOptionList[selectingOptionNum];
  }

  public string GetSelectedLevelUpOptionName ()
  {
    return selectingOptionNameArray[selectingOptionNum];
  }

  public LevelUpOptionSelectMotionManager (
    GameObject badgeLevelOption,
    GameObject heartLevelOption,
    GameObject flowerLevelOption,
    GameObject levelUpSpotLight,
    GameObject levelUpOptionDescription
  )
  {
    this.badgeLevelOption = badgeLevelOption;
    this.heartLevelOption = heartLevelOption;
    this.flowerLevelOption = flowerLevelOption;
    this.levelUpSpotLight = levelUpSpotLight;
    this.levelUpOptionDescription = levelUpOptionDescription;

    seManager = SEManager.Instance;

    Setup ();
  }

  void Setup ()
  {
    levelUpOptionList = new List<GameObject> ();
    void SetupOption (GameObject option)
    {
      option.SetActive (false);
      levelUpOptionList.Add (option);
    }
    SetupOption (badgeLevelOption);
    SetupOption (heartLevelOption);
    SetupOption (flowerLevelOption);

    endSelecting = false;
    endSpotLightTween = true;

    levelUpSpotLight.SetActive (false);
    levelUpOptionDescription.SetActive (false);

    selectingOptionNameArray = new string[] { "badge", "heart", "flower" };
    int heart = 1;
    selectingOptionNum = heart;
  }

  public void ShowLevelUpOptions ()
  {
    foreach (var option in levelUpOptionList)
    {
      option.SetActive (true);
    }
    levelUpSpotLight.SetActive (true);
    levelUpOptionDescription.SetActive (true);
    ManageOptionsColor ();
    ManageSpotLightDegree ();
  }

  public async UniTask ManageSelectingOption ()
  {
    if (!endSpotLightTween) return;

    int lowerLimitNum = 0;
    int upperLimitNum = levelUpOptionList.Count - 1;

    if (Input.GetKeyDown (KeyCode.RightArrow) && selectingOptionNum != lowerLimitNum)
    {
      selectingOptionNum--;
      seManager.Play (SEPath.MENU_CURSOR);
      ChangeAllOptionsColorToNotSelected ();
      await ManageSpotLightDegree ();
      ManageOptionsColor ();
    }
    else if (Input.GetKeyDown (KeyCode.LeftArrow) && selectingOptionNum != upperLimitNum)
    {
      selectingOptionNum++;
      seManager.Play (SEPath.MENU_CURSOR);
      ChangeAllOptionsColorToNotSelected ();
      await ManageSpotLightDegree ();
      ManageOptionsColor ();
    }
  }

  public void InactivateSpotLight ()
  {
    levelUpSpotLight.SetActive (false);
  }

  public void InactivateLevelUpOptionDescription ()
  {
    levelUpOptionDescription.SetActive (false);
  }

  public void FadeOutNotSelectedOptions ()
  {
    foreach (var option in levelUpOptionList)
    {
      GameObject selectedOption = levelUpOptionList[selectingOptionNum];
      if (option != selectedOption)
      {
        float fadeOutDuration = 0.15f;
        FadeOutOption (option, fadeOutDuration);
      }
    }
  }

  void FadeOutOption (GameObject option, float fadeOutDuration)
  {
    Transform optionFrame = option.transform.Find ("OptionFrame");
    Transform nameText = option.transform.Find ("NameText");

    FadeOutSprite (optionFrame, fadeOutDuration);
    FadeOutMeshPro (nameText, fadeOutDuration);

    Transform valueInformation = option.transform.Find ("ValueInformation");
    Transform fromValueText = valueInformation.Find ("FromValueText");
    Transform arrowImage = valueInformation.Find ("ArrowImage");
    Transform toValueText = valueInformation.Find ("ToValueText");

    FadeOutMeshPro (fromValueText, fadeOutDuration);
    FadeOutSprite (arrowImage, fadeOutDuration);
    FadeOutMeshPro (toValueText, fadeOutDuration);
  }

  public async UniTask ApproachingSelectedOption ()
  {
    GameObject selectedOption = levelUpOptionList[selectingOptionNum];
    Vector3 playerOverheadPosition = GameObject.FindWithTag ("Player").transform.position;
    float approachingDuration = 0.6f;
    int rotationNum = 5;
    playerOverheadPosition.y = selectedOption.transform.position.y;
    await DOTween.Sequence ().Append (selectedOption.transform.DOMove (playerOverheadPosition, approachingDuration))
      .Join (selectedOption.transform.DOLocalRotate (new Vector3 (0, 360f, 0), approachingDuration / rotationNum, RotateMode.FastBeyond360)
        .SetEase (Ease.Linear)
        .SetLoops (rotationNum, LoopType.Restart));
  }

  async UniTask ManageSpotLightDegree ()
  {
    endSpotLightTween = false;
    Vector3 spotLightDegree = levelUpSpotLight.transform.localEulerAngles;
    spotLightDegree.z = GetSpotLightDegree ();
    float rotationDuration = 0.3f;
    await levelUpSpotLight.transform.DOLocalRotate (spotLightDegree, rotationDuration);
    // await
    endSpotLightTween = true;
  }

  float GetSpotLightDegree ()
  {
    // 角度を計算する方法が思いつくまで値を固定で作成中
    float spotLightDegree = 0;
    string selectingOptionName = selectingOptionNameArray[selectingOptionNum];
    if (selectingOptionName == SelectingOption.badge.ToString ())
    {
      spotLightDegree = 24.0f;
    }
    else if (selectingOptionName == SelectingOption.heart.ToString ())
    {
      spotLightDegree = 0;
    }
    else if (selectingOptionName == SelectingOption.flower.ToString ())
    {
      spotLightDegree = -24.0f;
    }
    return spotLightDegree;
  }

  // すべての選択肢の選ばれていない状態の色に変更
  void ChangeAllOptionsColorToNotSelected ()
  {
    foreach (var option in levelUpOptionList)
    {
      Transform optionFrame = option.transform.Find ("OptionFrame");
      Transform nameText = option.transform.Find ("NameText");

      ChangeSpriteColorToNotSelected (optionFrame);
      ChangeNameTextColorToNotSelected (nameText);

      Transform valueInformation = option.transform.Find ("ValueInformation");
      Transform fromValueText = valueInformation.Find ("FromValueText");
      Transform arrowImage = valueInformation.Find ("ArrowImage");
      Transform toValueText = valueInformation.Find ("ToValueText");

      ChangeValueInformationColorToNotSelected (fromValueText, arrowImage, toValueText);
    }
  }

  // 選択されている選択肢に応じて色の変更
  void ManageOptionsColor ()
  {
    int lastOptionNum = levelUpOptionList.Count - 1;
    for (var i = 0; i < levelUpOptionList.Count; i++)
    {
      int optionNum = i;
      Transform option = levelUpOptionList[optionNum].transform;
      Transform optionFrame = option.Find ("OptionFrame");
      Transform nameText = option.Find ("NameText");
      Transform valueInformation = option.transform.Find ("ValueInformation");
      Transform fromValueText = valueInformation.Find ("FromValueText");
      Transform arrowImage = valueInformation.Find ("ArrowImage");
      Transform toValueText = valueInformation.Find ("ToValueText");
      if (optionNum == selectingOptionNum)
      {
        ChangeSpriteColorToSelected (optionFrame);
        ChangeNameTextColorToSelected (nameText);
        ChangeValueInformationColorToSelected (fromValueText, arrowImage, toValueText);
      }
      else
      {
        ChangeSpriteColorToNotSelected (optionFrame);
        ChangeNameTextColorToNotSelected (nameText);
        ChangeValueInformationColorToNotSelected (fromValueText, arrowImage, toValueText);
      }
    }
  }

  void ChangeValueInformationColorToSelected (
    Transform fromValueText,
    Transform arrowImage,
    Transform toValueText
  )
  {
    float max = 255.0f;
    float selectedColor = max;
    float r = selectedColor / max;
    float g = selectedColor / max;
    float b = selectedColor / max;
    float a = max / max;
    Color selected = new Color (r, g, b, a);
    ChangeSpriteColor (arrowImage, selectedColor);
    ManageMeshProTextColor (fromValueText, selected);
    ManageMeshProTextColor (toValueText, selected);
  }

  void ChangeValueInformationColorToNotSelected (
    Transform fromValueText,
    Transform arrowImage,
    Transform toValueText
  )
  {
    float max = 255.0f;
    float notSelectedColor = 111.0f;
    float r = notSelectedColor / max;
    float g = notSelectedColor / max;
    float b = notSelectedColor / max;
    float a = max / max;
    Color notSelected = new Color (r, g, b, a);
    ChangeSpriteColor (arrowImage, notSelectedColor);
    ManageMeshProTextColor (fromValueText, notSelected);
    ManageMeshProTextColor (toValueText, notSelected);
  }

  // MeshPro
  void ChangeNameTextColorToSelected (Transform nameText)
  {
    float max = 255.0f;
    float r = max / max;
    float g = 92.0f / max;
    float b = 0.0f / max;
    float a = max / max;
    Color selected = new Color (r, g, b, a);
    ManageMeshProTextColor (nameText, selected);
  }
  void ChangeNameTextColorToNotSelected (Transform nameText)
  {
    float max = 255.0f;
    float r = 133.0f / max;
    float g = 48.0f / max;
    float b = 0.0f / max;
    float a = max / max;
    Color notSelected = new Color (r, g, b, a);
    ManageMeshProTextColor (nameText, notSelected);
  }

  void ManageMeshProTextColor (Transform text, Color nameTextColor)
  {
    text.GetComponent<TextMeshPro> ().color = nameTextColor;
  }

  void FadeOutMeshPro (Transform text, float hideDuration)
  {
    float hide = 0.0f;
    text.GetComponent<TextMeshPro> ().DOFade (hide, hideDuration);
  }

  // Sprite
  void ChangeSpriteColorToSelected (Transform sprite)
  {
    float selectedColor = 255.0f;
    ChangeSpriteColor (sprite, selectedColor);
  }

  void ChangeSpriteColorToNotSelected (Transform sprite)
  {
    float notSelectedColor = 111.0f;
    ChangeSpriteColor (sprite, notSelectedColor);
  }

  void ChangeSpriteColor (Transform sprite, float color)
  {
    float max = 255.0f;
    float notSelectedFrameColor = color / max;
    sprite.GetComponent<SpriteRenderer> ().color = new Color (notSelectedFrameColor, notSelectedFrameColor, notSelectedFrameColor, max / max);
  }

  void FadeOutSprite (Transform sprite, float hideDuration)
  {
    float hide = 0.0f;
    sprite.GetComponent<SpriteRenderer> ().DOFade (hide, hideDuration);
  }

}