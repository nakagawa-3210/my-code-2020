using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStageOneActiveCameraManager : MonoBehaviour
{
  [SerializeField] GameObject chasingPlayerCamera;
  [SerializeField] GameObject itemShopCamera;

  public void ActivateItemShopCamera ()
  {
    itemShopCamera.SetActive (true);
    chasingPlayerCamera.SetActive (false);
  }

  public void ActivateChasingPlayerCamera ()
  {
    chasingPlayerCamera.SetActive (true);
    itemShopCamera.SetActive (false);
  }

}