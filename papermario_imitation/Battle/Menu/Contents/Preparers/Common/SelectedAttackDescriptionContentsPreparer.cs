using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedAttackDescriptionContentsPreparer
{
  private GameObject selectedAttackDescriptionFrame;
  private GameObject selectedAttackDescriptionText;

  public SelectedAttackDescriptionContentsPreparer (
    GameObject selectedAttackDescriptionFrame
  )
  {
    this.selectedAttackDescriptionFrame = selectedAttackDescriptionFrame;
    selectedAttackDescriptionText = selectedAttackDescriptionFrame.transform.Find ("AttackDescriptionText").gameObject;
  }

  public void ManageSelectedAttackText (GameObject currentSelectedButton)
  {
    if (currentSelectedButton == null)
    {
      selectedAttackDescriptionText.GetComponent<Text> ().text = "";
    }
    else if (currentSelectedButton.GetComponent<SkillButtonInformationContainer> () != null)
    {
      SkillButtonInformationContainer skillInformationContainer = currentSelectedButton.GetComponent<SkillButtonInformationContainer> ();
      selectedAttackDescriptionText.GetComponent<Text> ().text = skillInformationContainer.HowToCommand;
    }
  }
}