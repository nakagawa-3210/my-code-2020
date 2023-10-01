using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinInformationMotionManager
{
  private GameObject playerHavingCoinPlate;
  private GameObject playerHavingCoinText;
  private int previousCoinNum;

  public PlayerCoinInformationMotionManager (
    GameObject playerHavingCoinPlate
  )
  {
    this.playerHavingCoinPlate = playerHavingCoinPlate;
    playerHavingCoinText = playerHavingCoinPlate.transform.Find ("PlayerCoinNumText").gameObject;
    // previousCoinNum = SaveSystem.Instance.userData.havingCoin;
    Setup ();
    HidePlayerHavingCoinPlate ();
  }

  public void Setup ()
  {
    previousCoinNum = SaveSystem.Instance.userData.havingCoin;
    playerHavingCoinText.GetComponent<TextMeshProUGUI> ().text = previousCoinNum.ToString ();
  }

  public void ShowPlayerHavingCoinPlate ()
  {
    playerHavingCoinPlate.SetActive (true);
  }
  public void HidePlayerHavingCoinPlate ()
  {
    playerHavingCoinPlate.SetActive (false);
  }

  public void DecreaseCoinNumTextMotion ()
  {
    int currentCoinNum = SaveSystem.Instance.userData.havingCoin;
    if (previousCoinNum > currentCoinNum)
    {
      previousCoinNum--;
      playerHavingCoinText.GetComponent<TextMeshProUGUI> ().text = previousCoinNum.ToString ();
    }
  }
}