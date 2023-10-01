using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCoinInformationContentsManager
{
  private GameObject playerHavingCoinText;
  public PlayerCoinInformationContentsManager (
    GameObject playerHavingCoinText
  )
  {
    this.playerHavingCoinText = playerHavingCoinText;
    SetupCoinText ();
  }

  void SetupCoinText ()
  {
    int playerHavingCoin = SaveSystem.Instance.userData.havingCoin;
    playerHavingCoinText.GetComponent<TextMeshProUGUI> ().text = playerHavingCoin.ToString ();
  }
}