using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCommandDictionaryManager : MonoBehaviour
{
  [SerializeField] float cursorMotionDuration;
  [SerializeField] float successSliderValueFrom;
  [SerializeField] float successSliderValueTo;
  [SerializeField] Image enemyImage;
  [SerializeField] Text enemyNameText;
  [SerializeField] GameObject dictionaryActionSign;

  private ActionCommandDictionaryCursorMotionManager actionCommandDictionaryCursorMotionManager;
  private ActionCommandDictionaryContentsManager actionCommandDictionaryContentsManager;

  private bool hasActionChance;
  private bool isSucceeded;

  public bool HasActionChance
  {
    get { return hasActionChance; }
  }
  public bool IsSucceeded
  {
    get { return isSucceeded; }
  }
  void Start ()
  {
    actionCommandDictionaryCursorMotionManager = new ActionCommandDictionaryCursorMotionManager (
      dictionaryActionSign,
      cursorMotionDuration
    );
    actionCommandDictionaryContentsManager = new ActionCommandDictionaryContentsManager (
      enemyImage,
      enemyNameText
    );
    ResetBools ();
  }

  void Update ()
  {
    bool endCursorMotion = actionCommandDictionaryCursorMotionManager.EndCursorTween;
    // Debug.Log ("endCursorMotion : " + endCursorMotion);
    if (hasActionChance)
    {
      CheckIsEndCursorMotion ();
      if (Input.GetKeyDown (KeyCode.Space))
      {
        hasActionChance = false;
        actionCommandDictionaryCursorMotionManager.KillIncreasingSliderValue ();
        CheckIsSucceeded ();
      }
    }
  }

  public void StartDictionaryCursor ()
  {
    ResetBools ();
    actionCommandDictionaryCursorMotionManager.ResetCursorValue ();
    actionCommandDictionaryCursorMotionManager.StartIncreasingSliderValue ();
  }

  void CheckIsSucceeded ()
  {
    float cursorSliderValue = dictionaryActionSign.GetComponent<Slider> ().value;
    if (cursorSliderValue >= successSliderValueFrom && cursorSliderValue <= successSliderValueTo)
    {
      isSucceeded = true;
    }
    else
    {
      isSucceeded = false;
    }
  }

  void CheckIsEndCursorMotion ()
  {
    bool endCursorMotion = actionCommandDictionaryCursorMotionManager.EndCursorTween;
    if (endCursorMotion)
    {
      hasActionChance = false;
      isSucceeded = false;
      actionCommandDictionaryCursorMotionManager.KillIncreasingSliderValue ();
    }
  }

  public void SetupDictionaryContents (string enemyImageName, string enemyName)
  {
    actionCommandDictionaryContentsManager.SetupDictionaryContents (enemyImageName, enemyName);
  }

  void ResetBools ()
  {
    hasActionChance = true;
    isSucceeded = false;
  }
}