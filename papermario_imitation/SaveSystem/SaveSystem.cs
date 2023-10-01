using System.Collections;
using System.Collections.Generic;
using System.IO;
using MyGameData;
using UnityEngine;

public class SaveSystem
{
  #region Singleton
  private static SaveSystem instance = new SaveSystem ();
  public static SaveSystem Instance => instance;
  #endregion

  private SaveSystem ()
  {
    Load ();
  }
  public string userJsonPath => Application.persistentDataPath + "/paperHanadaGameData.json";
  public MyData userData { get; private set; }
  public void Save ()
  {
    string jsonData = JsonUtility.ToJson (userData);
    StreamWriter writer = new StreamWriter (userJsonPath, false);
    writer.Write (jsonData);
    writer.Flush ();
    writer.Close ();
  }
  public void Load ()
  {
    if (!File.Exists (userJsonPath))
    {
      userData = new MyData ();
      Save ();
      return;
    }
    StreamReader reader = new StreamReader (userJsonPath);
    string jsonData = reader.ReadToEnd ();
    userData = JsonUtility.FromJson<MyData> (jsonData);
    reader.Close ();
  }
}