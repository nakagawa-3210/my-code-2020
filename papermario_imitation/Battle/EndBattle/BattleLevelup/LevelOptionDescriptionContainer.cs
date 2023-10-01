using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOptionDescriptionContainer : MonoBehaviour
{
  private string description;

  public string Description
  {
    get { return description; }
  }

  void Start ()
  {
    Transform optionFrame = this.gameObject.transform.Find ("OptionFrame");
    string imgFileName = optionFrame.GetComponent<SpriteRenderer> ().sprite.name;

    LevelUpOptionDataArray levelUpOptionDataArray = new JsonReaderFromResourcesFolder ().GetLevelUpOptionDataArray ();
    
    foreach (var levelUpOptionData in levelUpOptionDataArray.gameLevelUp)
    {
      string levelUpImageFileName = levelUpOptionData.imgFileName;
      if (levelUpImageFileName == imgFileName)
      {
        description = levelUpOptionData.description;
      }
    }
  }

}