using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedOptionButtonDescriptionContentsPreparer
{
  private GameObject selectedOptionButtonDescriptionFrame;
  private GameObject selectedOptionButtonDescriptionText;
  private EventSystem eventSystem;

  public SelectedOptionButtonDescriptionContentsPreparer (
    GameObject selectedOptionButtonDescriptionFrame,
    EventSystem eventSystem
  )
  {
    this.selectedOptionButtonDescriptionFrame = selectedOptionButtonDescriptionFrame;
    this.eventSystem = eventSystem;
    selectedOptionButtonDescriptionText = selectedOptionButtonDescriptionFrame.transform.Find ("OptionDescriptionText").gameObject;
  }

  public void ManageSelectedOptionText ()
  {
    if (eventSystem.currentSelectedGameObject == null)
    {
      selectedOptionButtonDescriptionText.GetComponent<Text> ().text = "";
    }
    else if (eventSystem.currentSelectedGameObject.GetComponent<SkillButtonInformationContainer> () != null)
    {
      SkillButtonInformationContainer skillInformationContainer = eventSystem.currentSelectedGameObject.GetComponent<SkillButtonInformationContainer> ();
      selectedOptionButtonDescriptionText.GetComponent<Text> ().text = skillInformationContainer.Description;
      // Debug.Log("skillInformationContainer.HowToCommand : " + skillInformationContainer.HowToCommand);
    }
    else if (eventSystem.currentSelectedGameObject.GetComponent<BelongingButtonInfoContainer> () != null)
    {
      BelongingButtonInfoContainer itemInformationContainer = eventSystem.currentSelectedGameObject.GetComponent<BelongingButtonInfoContainer> ();
      selectedOptionButtonDescriptionText.GetComponent<Text> ().text = itemInformationContainer.Description;
    }
    else if (eventSystem.currentSelectedGameObject.GetComponent<StrategyButtonInformationContainer> () != null)
    {
      StrategyButtonInformationContainer strategyButtonInformationContainer = eventSystem.currentSelectedGameObject.GetComponent<StrategyButtonInformationContainer> ();
      selectedOptionButtonDescriptionText.GetComponent<Text> ().text = strategyButtonInformationContainer.Description;
    }
  }
}