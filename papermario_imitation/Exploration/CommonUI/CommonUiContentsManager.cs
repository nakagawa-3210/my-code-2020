using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonUiContentsManager
{
  private GameObject previousFrameSelectedGameObject;
  private Text itemDescriptionText;
  public CommonUiContentsManager (GameObject commonItemDescriptionPanel)
  {
    previousFrameSelectedGameObject = null;

    Transform itemDescription = commonItemDescriptionPanel.transform.Find ("CommonItemDescriptionInfoText");
    itemDescriptionText = itemDescription.GetComponent<Text> ();
  }

  public void SetSelectingItemDescriptionText (GameObject currentSelectedGameButton)
  {
    if (currentSelectedGameButton == null || currentSelectedGameButton == previousFrameSelectedGameObject) return;

    if (currentSelectedGameButton.GetComponent<BelongingButtonInfoContainer> () != null)
    {
      previousFrameSelectedGameObject = currentSelectedGameButton;
      BelongingButtonInfoContainer currentButtonInfoContainer = currentSelectedGameButton.GetComponent<BelongingButtonInfoContainer> ();
      SetupItemDescription (currentButtonInfoContainer.Description);
      // itemDescriptionText.text = currentButtonInfoContainer.Description;
    }

  }

  public void SetupItemDescription (string itemDescription)
  {
    itemDescriptionText.text = itemDescription;
  }

}