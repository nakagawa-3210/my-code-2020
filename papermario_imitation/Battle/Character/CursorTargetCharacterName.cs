using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTargetCharacterName : MonoBehaviour
{
  [SerializeField] string characterName;
  public string CharacterName
  {
    get { return characterName; }
  }
}