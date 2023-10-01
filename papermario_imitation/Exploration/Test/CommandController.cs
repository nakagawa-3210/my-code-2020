using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CommandController : MonoBehaviour
{
  private readonly List<ICommand> m_commandList = new List<ICommand> ()
  {
    // new CommandUpdateImage (),
    new CommandJumpNextScenario (),
  };

  private List<IPreCommand> m_preCommandList = new List<IPreCommand> ();

  public void PreloadCommand (string line)
  {
    var dic = CommandAnalytics (line);
    // foreach (var command in m_commandList)
      // if (command.Tag == dic["tag"])
        // command.PreCommand (dic);
  }

  public bool LoadCommand (string line)
  {
    var dic = CommandAnalytics (line);
    foreach (var command in m_commandList)
    {
      if (command.Tag == dic["tag"])
      {
        command.Command (dic);
        return true;
      }
    }
    return false;
  }

  // コマンドの解析
  private Dictionary<string, string> CommandAnalytics (string line)
  {
    Dictionary<string, string> command = new Dictionary<string, string> ();
    // コマンド名の取得
    var tag = Regex.Match (line, "@(\\S+)\\s");
    command.Add ("tag", tag.Groups[1].ToString ());

    // コマンドのパラメータを取得
    Regex regex = new Regex ("(\\S+)=(\\S+)");
    var matches = regex.Matches (line);
    foreach (Match match in matches)
    {
      command.Add (match.Groups[1].ToString (), match.Groups[2].ToString ());
    }
    return command;
  }

  #region UNITY_CALLBACK
  void Awake ()
  {
    // base.Awake ();
    foreach (var command in m_commandList)
    {
      if (command is IPreCommand)
      {
        m_preCommandList.Add ((IPreCommand) command);
      }
    }

  }
  #endregion
}