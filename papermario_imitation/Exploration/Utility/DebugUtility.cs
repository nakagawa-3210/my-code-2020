using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtility
{
  public static void HandleErrorIfNullFindObject<TO, TS> (Object obj, Component source)
  {
    if (Application.isEditor && obj == null)
    {
      Debug.LogError ("Error: Component of type " + typeof (TS) + " on GameObject " + source.gameObject.name +
        " expected to find an object of type " + typeof (TO) + " in the scene, but none were found.");
    }
  }
}