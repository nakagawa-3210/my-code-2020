using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverCanvasUi : MonoBehaviour
{
  public GameObject gameOverOptions = default;
  public GameObject gameOverCursor = default;
  public GameObject firstSelectedButton = default;
  public List<GameObject> gameOverButtonList = default;
  public EventSystem eventSystem = default;

}