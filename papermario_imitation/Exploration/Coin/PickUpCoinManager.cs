using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoinManager : MonoBehaviour
{
  public void SaveGetCoinNum ()
  {
    SaveSystem.Instance.userData.havingCoin++;
    Debug.Log("havingCoin : " + SaveSystem.Instance.userData.havingCoin);
    // SaveSystem.Instance.Save ();
  }

}