using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCanvasPanelsManager
{
  private GameObject[] menuTagsList;
  private GameObject[] menuPanelsList;
  private EventSystem eventSystem;
  private GameObject defaultSelectedTag;
  private GameObject previousSelectedTag;
  private MenuCanvasOpenCloseManager menuCanvasOpenCloseManager;
  public MenuCanvasPanelsManager (
    GameObject[] menuTagsList,
    GameObject[] menuPanelsList,
    GameObject defaultSelectedTag,
    MenuCanvasOpenCloseManager menuCanvasOpenCloseManager,
    EventSystem eventSystem
  )
  {
    this.menuTagsList = menuTagsList;
    this.menuPanelsList = menuPanelsList;
    this.defaultSelectedTag = defaultSelectedTag;
    this.menuCanvasOpenCloseManager = menuCanvasOpenCloseManager;
    this.eventSystem = eventSystem;
  }
  public void ShowSelectedPanel ()
  {
    GameObject selectedButton = eventSystem.currentSelectedGameObject.gameObject;
    if (selectedButton != previousSelectedTag && menuCanvasOpenCloseManager.IsMenuOpen ())
    {
      previousSelectedTag = selectedButton;
      int tagNumber = GetSelectedTagButtonNum (selectedButton);
      ChangeShowingContentsPanel (tagNumber);
    }
  }

  int GetSelectedTagButtonNum (GameObject selectedButton)
  {
    int tagNumber = 0;
    for (var tagNum = 0; tagNum < menuTagsList.Length; tagNum++)
    {
      GameObject tagButton = menuTagsList[tagNum];
      if (tagButton == selectedButton)
      {
        tagNumber = tagNum;
      }
    }
    return tagNumber;
  }

  void ChangeShowingContentsPanel (int tagNumber)
  {
    if (tagNumber == 0)
    {
      ShowPanelsTween (1);
      HidePanelsTween (4);
    }
    else if (tagNumber == 1)
    {
      ShowPanelsTween (2);
      HidePanelsTween (3);
    }
    else if (tagNumber == 2)
    {
      ShowPanelsTween (3);
      HidePanelsTween (2);
    }
    else if (tagNumber == 3)
    {
      ShowPanelsTween (4);
      HidePanelsTween (1);
    }
    else if (tagNumber == 4)
    {
      ShowPanelsTween (5);
      HidePanelsTween (0);
    }
  }
  void ShowPanelsTween (int showPanelNum)
  {
    for (var i = 0; i < showPanelNum; i++)
    {
      menuCanvasOpenCloseManager.ShowContentsPanelTween (menuPanelsList[i]);
    }
  }
  void HidePanelsTween (int hidePanelNum)
  {
    for (var i = menuPanelsList.Length; i > menuPanelsList.Length - hidePanelNum; i--)
    {
      menuCanvasOpenCloseManager.HideContentsPanelTween (menuPanelsList[i - 1]);
    }
  }
}