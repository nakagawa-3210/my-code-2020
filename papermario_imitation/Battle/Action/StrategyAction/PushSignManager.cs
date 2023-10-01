using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushSignManager : MonoBehaviour
{
  [SerializeField] Image spacekeyImage;
  [SerializeField] Image pressedSpacekeyImage;

  private float timeElapsed;
  private float changeTime;

  private bool showNormalSign;

  void Start ()
  {
    timeElapsed = 0.0f;
    changeTime = 0.2f;
    showNormalSign = true;
    ShowNormalSign ();
  }

  void Update ()
  {
    timeElapsed += Time.deltaTime;
    if (timeElapsed >= changeTime)
    {
      ChangeEnabledSprite ();
      timeElapsed = 0.0f;
    }
  }

  void ChangeEnabledSprite ()
  {
    if (showNormalSign)
    {
      showNormalSign = false;
      ShowPressedSign ();
    }
    else
    {
      showNormalSign = true;
      ShowNormalSign ();
    }
  }

  void ShowNormalSign ()
  {
    spacekeyImage.enabled = true;
    pressedSpacekeyImage.enabled = false;
  }

  void ShowPressedSign ()
  {
    spacekeyImage.enabled = false;
    pressedSpacekeyImage.enabled = true;
  }
}