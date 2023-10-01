using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConversationHolder
{
  public List<ConversationScene> ConversationScenes = new List<ConversationScene> ();
  private ConversationContentsManager conversationContentsManager;

  public ConversationHolder (ConversationContentsManager conversationContentsManager, string conversationTextFileName)
  {
    this.conversationContentsManager = conversationContentsManager;
    Load (conversationTextFileName);
  }

  public void Load (string conversationTextFileName)
  {
    TextAsset[] scenarioFiles = Resources.LoadAll<TextAsset> ("Conversation");
    TextAsset selectedScenarioFileFile = null;
    foreach (var scenarioFile in scenarioFiles)
    {
      if (conversationTextFileName == scenarioFile.name)
      {
        selectedScenarioFileFile = scenarioFile;
      }
    }
    // 上のやり方でおそかったら、下記の方法で引数で読み込み先を指定するのでコメントアウト中
    // TextAsset itemFile = Resources.Load ("Scenario/" + conversationTextFileName, typeof (TextAsset)) as TextAsset;
    // List<string> csvData = LoadCSV (itemFile);
    List<string> csvData = LoadCSV (selectedScenarioFileFile);
    ConversationScenes = Parse (csvData);
  }

  public List<string> LoadCSV (TextAsset file)
  {
    StringReader reader = new StringReader (file.text);
    List<string> list = new List<string> ();
    //ストリームの末端まで繰り返す
    while (reader.Peek () > -1)
    {
      string line = reader.ReadLine ();
      list.Add (line);
    }
    return list;
  }

  public List<ConversationScene> Parse (List<string> list)
  {
    List<ConversationScene> conversationScenes = new List<ConversationScene> ();
    ConversationScene conversationScene = new ConversationScene ();
    foreach (string line in list)
    {
      // メモ帳の記載方法に合わせる
      if (line.Contains ("#scene"))
      {
        string ID = line.Replace ("#scene=", "");
        conversationScene = new ConversationScene (ID);
        conversationScenes.Add (conversationScene);
      }
      else
      {
        conversationScene.Lines.Add (line);
      }
    }
    return conversationScenes;
  }

}