﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// RequireComponentを調べる
[RequireComponent (typeof (TextController))]
// SingletonMonoBehaviourFastではない方法で試してみる
public class ScenarioManager : MonoBehaviour
{
  [SerializeField] string loadFileName;
  private string[] m_scenarios;
  private int m_currentLine = 0;
  private bool m_isCallPreload = false;

  private TextController m_textController;
  private CommandController m_commandController;

  void RequestNextLine ()
  {
    var currentText = m_scenarios[m_currentLine];

    m_textController.SetNextLine (CommandProcess (currentText));
  }

  public void UpdateLines (string fileName)
  {
    var scenarioText = Resources.Load<TextAsset> ("Scenario/" + fileName);

    if (scenarioText == null)
    {
      Debug.LogError ("シナリオファイルが見つかりませんでした");
      Debug.LogError ("ScenarioManagerを無効化します");
      // なんのenabledかわからない
      enabled = false;
      return;
    }

    // 空文字の行に置き換え？
    m_scenarios = scenarioText.text.Split (new string[] { "@br" }, System.StringSplitOptions.None);
    m_currentLine = 0;
  }

  private string CommandProcess (string line)
  {
    var lineReader = new StringReader (line);
    var lineBuilder = new StringBuilder ();
    var text = string.Empty;
    while ((text = lineReader.ReadLine ()) != null)
    {
      var commentCharacterCount = text.IndexOf ("//");
      if (commentCharacterCount != -1)
      {
        text = text.Substring (0, commentCharacterCount);
      }
      if (!string.IsNullOrEmpty (text))
      {
        if (text[0] == '@' && m_commandController.LoadCommand (text))
          continue;
        lineBuilder.AppendLine (text);
      }
    }
    return lineBuilder.ToString ();
  }
  #region UNITY_CALLBACK
  void Start ()
  {
    m_textController = GetComponent<TextController> ();
    m_commandController = GetComponent<CommandController> ();

    UpdateLines (loadFileName);
    RequestNextLine ();
  }

  void Update ()
  {
    if (m_textController.IsCompleteDisplayText)
    {
      if (m_currentLine < m_scenarios.Length)
      {
        if (!m_isCallPreload)
        {
          m_commandController.PreloadCommand (m_scenarios[m_currentLine]);
          m_isCallPreload = true;
        }

        if (Input.GetMouseButtonDown (0))
        {
          RequestNextLine ();
        }
      }
    }
    else
    {
      if (Input.GetMouseButtonDown (0))
      {
        m_textController.ForceCompleteDisplayText ();
      }
    }
  }
  #endregion
}