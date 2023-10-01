using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonEnableButtonFunction
{
  public void EnableListViewButtons (List<GameObject> listViewButtonsList)
  {
    foreach (var listViewButton in listViewButtonsList)
    {
      if (listViewButton == null) return;
      listViewButton.GetComponent<Button> ().enabled = true;
    }
  }
  public void DisableListViewButtons (List<GameObject> listViewButtonsList)
  {
    foreach (var listViewButton in listViewButtonsList)
    {
      if (listViewButton == null) return;
      listViewButton.GetComponent<Button> ().enabled = false;
    }
  }
}