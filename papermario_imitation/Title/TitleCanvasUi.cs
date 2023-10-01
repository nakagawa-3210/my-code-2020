using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleCanvasUi : MonoBehaviour
{
  public DialogManager dialogManager = default;
  public GameObject startText = default;
  public GameObject titleOptions = default;
  public List<GameObject> titleOptionButtonList = default;
  public List<GameObject> dialogOptionButtonList = default;
  public GameObject titleFirstSelectedOption = default;
  public GameObject dialogFirstSelectedOption = default;
  public GameObject titleContinueButton = default;
  public GameObject titleCursor = default;
  public GameObject dialogCursor = default;
  public EventSystem eventSystem = default;
}