using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemGUIManager : MonoBehaviour
{
  public GameObject pickUpItemName;
  public GameObject pickUpItemCaution;

  void Start ()
  {
    pickUpItemName.SetActive (false);
    pickUpItemCaution.SetActive (false);
  }

  
}