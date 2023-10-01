using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonSelectedTargetNameContentsPreparer
{
  private GameObject prevSelectedTarget;
  public CommonSelectedTargetNameContentsPreparer ()
  {
    prevSelectedTarget = null;
  }

  public void ManageSelectedTargetInformation (GameObject selectedTarget, GameObject selectedTargetNameText, GameObject selectedTargetNameFrame)
  {
    if (selectedTarget != prevSelectedTarget)
    {
      ManageSelectedTargetNameText (selectedTarget, selectedTargetNameText);
      ManageSelectedTargetNameFrameSize (selectedTargetNameFrame, selectedTargetNameText);
    }
  }

  public void ManageSelectedTargetNameText (GameObject selectedTarget, GameObject selectedTargetNameText)
  {
    // if (selectedTarget != prevSelectedTarget)
    // {
    prevSelectedTarget = selectedTarget;
    CursorTargetCharacterName cursorTargetCharacterName = selectedTarget.transform.Find ("CursorTarget").GetComponent<CursorTargetCharacterName> ();
    // Debug.Log ("cursorTargetCharacterName : " + cursorTargetCharacterName.CharacterName);
    selectedTargetNameText.GetComponent<Text> ().text = cursorTargetCharacterName.CharacterName;
    // }
  }

  public void ManageSelectedTargetNameFrameSize (GameObject selectedTargetNameFrame, GameObject selectedTargetNameText)
  {
    float textFontSize = selectedTargetNameText.GetComponent<Text> ().fontSize;
    float textLength = selectedTargetNameText.GetComponent<Text> ().text.Length;
    Vector2 size = selectedTargetNameFrame.GetComponent<RectTransform> ().sizeDelta;
    // すこしフレームに余白を持たせたサイズにするために+1
    size.x = textFontSize * (textLength + 1);
    selectedTargetNameFrame.GetComponent<RectTransform> ().sizeDelta = size;
  }
}