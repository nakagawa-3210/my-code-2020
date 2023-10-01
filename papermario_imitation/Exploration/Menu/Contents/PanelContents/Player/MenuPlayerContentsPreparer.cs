using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerContentsPreparer
{
  private GameObject playerLevel;
  private GameObject playerTitle;
  private GameObject playerCurrentHp;
  private GameObject playerMaxHp;
  private GameObject playerCurrentFp;
  private GameObject playerMaxFp;
  private GameObject playerMaxBp;
  private GameObject playerExperiencePoints;
  private GameObject playerCoin;
  private GameObject playerStarFragments;
  private GameObject playerSuperFertilizer;
  private GameObject playerTotalPlayingTimeHour;
  private GameObject playerTotalPlayingTimeMin;
  public MenuPlayerContentsPreparer (
    GameObject playerLevel,
    GameObject playerTitle,
    GameObject playerCurrentHp,
    GameObject playerMaxHp,
    GameObject playerCurrentFp,
    GameObject playerMaxFp,
    GameObject playerMaxBp,
    GameObject playerExperiencePoints,
    GameObject playerCoin,
    GameObject playerStarFragments,
    GameObject playerSuperFertilizer,
    GameObject playerTotalPlayingTimeHour,
    GameObject playerTotalPlayingTimeMin
  )
  {
    this.playerLevel = playerLevel;
    this.playerTitle = playerTitle;
    this.playerCurrentHp = playerCurrentHp;
    this.playerMaxHp = playerMaxHp;
    this.playerCurrentFp = playerCurrentFp;
    this.playerMaxFp = playerMaxFp;
    this.playerMaxBp = playerMaxBp;
    this.playerExperiencePoints = playerExperiencePoints;
    this.playerCoin = playerCoin;
    this.playerStarFragments = playerStarFragments;
    this.playerSuperFertilizer = playerSuperFertilizer;
    this.playerTotalPlayingTimeHour = playerTotalPlayingTimeHour;
    this.playerTotalPlayingTimeMin = playerTotalPlayingTimeMin;
    SetupMenuPanelsContentsText ();
  }
  public void SetupMenuPanelsContentsText ()
  {
    // データクラスの変数名を取りたかったがとれない。
    // リファクタリング時にまた試す
    // HanaPlayerScript test = new HanaPlayerScript();
    // var properties = test.GetType();
    // Debug.Log ("properties : " + properties);
    var userData = SaveSystem.Instance.userData;
    playerLevel.GetComponent<Text> ().text = userData.playerLevel.ToString ();
    playerTitle.GetComponent<Text> ().text = userData.playerTitle;
    playerCurrentHp.GetComponent<Text> ().text = userData.playerCurrentHp.ToString ();
    playerMaxHp.GetComponent<Text> ().text = userData.playerMaxHp.ToString ();
    playerCurrentFp.GetComponent<Text> ().text = userData.playerCurrentFp.ToString ();
    playerMaxFp.GetComponent<Text> ().text = userData.playerMaxFp.ToString ();
    playerMaxBp.GetComponent<Text> ().text = userData.playerMaxBp.ToString ();
    playerExperiencePoints.GetComponent<Text> ().text = userData.experience.ToString ();
    playerCoin.GetComponent<Text> ().text = userData.havingCoin.ToString ();
    playerStarFragments.GetComponent<Text> ().text = userData.havingStarFragments.ToString ();
    playerSuperFertilizer.GetComponent<Text> ().text = userData.havingSuperFertilizer.ToString ();
    playerTotalPlayingTimeHour.GetComponent<Text> ().text = GetPlayingHourTime (userData);
    playerTotalPlayingTimeMin.GetComponent<Text> ().text = GetPlayingMinTime (userData);
  }
  string GetPlayingHourTime (MyGameData.MyData userData)
  {
    int minutes = 60;
    int hours = userData.totalPlayingTimeMins / minutes;
    return hours.ToString ();
  }
  string GetPlayingMinTime (MyGameData.MyData userData)
  {
    int seconds = 60;
    int minutes = userData.totalPlayingTimeMins % seconds;
    string minsString = "";
    if (minutes < 10)
    {
      // 一桁分の時は、前に0をつける
      minsString = "0" + minutes.ToString ();
    }
    else
    {
      minsString = minutes.ToString ();
    }
    return minsString;
  }
}