using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScene
{
  // 下記の書き方、要理解
  public string ID { get; private set; }
  public List<string> Lines { get; private set; } = new List<string> ();
  public int Index { get; private set; } = 0;

  public ConversationScene (string ID = "")
  {
    this.ID = ID;
  }

  //  なんのためにクローンがあるかを理解する
  public ConversationScene Clone ()
  {
    return new ConversationScene ()
    {
      Index = 0,
        Lines = new List<string> (Lines)
    };
  }

  public bool IsFinished ()
  {
    return Index >= Lines.Count;
  }

  public string GetCurrentLines ()
  {
    return Lines[Index];
  }
  
  public void GoNextLine ()
  {
    Index++;
  }
}