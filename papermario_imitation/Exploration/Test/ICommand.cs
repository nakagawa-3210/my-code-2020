using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{

  // なにをしているかわからない
  string Tag { get; }
  void Command (Dictionary<string, string> command);
}

public interface IPreCommand
{
  string Tag { get; }
  void PreCommand (Dictionary<string, string> command);
}