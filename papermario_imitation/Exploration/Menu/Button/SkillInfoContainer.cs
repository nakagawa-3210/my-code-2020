using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoContainer : MonoBehaviour
{
  private string description = "";

  public string Description
  {
    set { description = value; }
    get { return description; }
  }
}