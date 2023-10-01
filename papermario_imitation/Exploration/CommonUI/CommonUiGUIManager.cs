using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommonUiGUIManager : MonoBehaviour
{
  public EventSystem commonEventSystem;
  public GameObject commonHandCursor;
  public GameObject commonItemListPanel;
  public GameObject commonItemListContainer;
  public GameObject commonItemListNamePanel;
  public GameObject commonItemDescriptionPanel;
  public GameObject commonItemListWhichChoseTextFrame;
  public BelongingButtonInfoContainer commonItemListButtonPrefab;
  public BelongingButtonInfoContainer commonNewItemListButtonPrefab;

  void Start ()
  {
    commonHandCursor.SetActive (false);
    commonItemListPanel.SetActive (false);
    commonItemListWhichChoseTextFrame.SetActive (false);
    commonItemListNamePanel.SetActive (false);
  }
}