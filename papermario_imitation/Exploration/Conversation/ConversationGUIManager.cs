using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 下記のやり方を他のUiManagerにも用いて、
// ContentsとMotionのマネージャー共通の[SerializeField]を省略する
public class ConversationGUIManager : MonoBehaviour
{
  public Camera MainCamera;
  public EventSystem eventSystem;
  public Transform buttonSmallPanel;
  public Transform buttonBigPanel;
  public Button OptionButton;
  public Text conversationText;
  public GameObject conversationCanvas;
  public GameObject conversationPanel;
  public GameObject nextTalkSign;

  void Start ()
  {
    buttonSmallPanel.gameObject.SetActive (false);
    buttonBigPanel.gameObject.SetActive (false);
    nextTalkSign.SetActive (false);
  }

  // void Update ()
  // {
  //   Debug.Log ("buttonSmallPanel.transform.localScale : " + buttonSmallPanel.transform.localScale);
  // }
}