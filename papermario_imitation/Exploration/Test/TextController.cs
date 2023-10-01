using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
  [SerializeField]
  private Text _uiText;
  [SerializeField] float intervalForCharacterDisplay = 0.05f;
  [SerializeField][Range (0.001f, 0.3f)]
  private string currentText = string.Empty;
  private float timeUntilDisplay = 0;
  private float timeElapsed = 1;
  private int lastUpdateCharacter = -1;
  

  public bool IsCompleteDisplayText
  {
    get { return Time.time > timeElapsed + timeUntilDisplay; }
  }

  public void ForceCompleteDisplayText ()
  {
    timeUntilDisplay = 0;
  }

  public void SetNextLine (string text)
  {
    currentText = text;
    timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
    timeElapsed = Time.time;
    lastUpdateCharacter = -1;
  }

  #region UNITY_CALLBACK
  void Update ()
  {
    int displayCharacterCount = (int) (Mathf.Clamp01 ((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
    if (displayCharacterCount != lastUpdateCharacter)
    {
      _uiText.text = currentText.Substring (0, displayCharacterCount);
      lastUpdateCharacter = displayCharacterCount;
    }
  }
  #endregion
}