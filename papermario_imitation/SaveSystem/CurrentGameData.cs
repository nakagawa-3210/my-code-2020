using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameData : MonoBehaviour
{
  public static string[] havingItemsName = { };
  public static string[] leavingItemsName = { };
  public static string[] havingImportantThing = { };
  public static string[] havingBadgesName = { };
  public static int[] puttingBadgeNums = { };
  public static int[] puttingBadgeId = { };
  public static string[] partnersName = { };
  public static string savePointSceneName = "";
  public static Vector3 savedPlayerPosition = new Vector3 ();
  public static Vector3 savedPartnerPosition = new Vector3 ();
  public static int playerLevel = 0;
  public static string playerTitle = "";
  public static int playerMaxHp = 0;
  public static int playerCurrentHp = 0;
  public static int playerHpGrowthPoint = 0;
  public static int playerMaxFp = 0;
  public static int playerCurrentFp = 0;
  public static int playerFpGrowthPoint = 0;
  public static int playerMaxBp = 0;
  public static int playerBpGrowthPoint = 0;
  public static int playerUsingBp = 0;
  public static int shoesLevel = 0;
  public static int hammerLevel = 0;
  public static int havingCoin = 0;
  public static int experience = 0;
  public static int havingStars = 0;
  public static int havingStarFragments = 0;
  public static int havingSuperFertilizer = 0;
  public static int totalPlayingTimeMins = 0;
  public static int havingItemsNumRestriction = 0;
  public static int leavingItemsNumRestriction = 0;
  public static string currentSelectedPartnerName = "";
  public static int sakuraLevel = 0;
  public static int sakuraCurrentHp = 0;

  public static bool EndReadingData ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    bool endReadingData = false;

    if (
      havingItemsName == saveData.havingItemsName &&
      leavingItemsName == saveData.leavingItemsName &&
      havingImportantThing == saveData.havingImportantThing &&
      havingBadgesName == saveData.havingBadgesName &&
      puttingBadgeNums.Length == saveData.puttingBadgeNums.Length &&
      puttingBadgeId.Length == saveData.puttingBadgeId.Length &&
      partnersName == saveData.partnersName &&
      savePointSceneName == saveData.savePointSceneName &&
      savedPlayerPosition == saveData.savedPlayerPosition &&
      savedPartnerPosition == saveData.savedPartnerPosition &&
      playerLevel == saveData.playerLevel &&
      playerTitle == saveData.playerTitle &&
      playerMaxHp == saveData.playerMaxHp &&
      playerCurrentHp == saveData.playerCurrentHp &&
      playerHpGrowthPoint == saveData.playerHpGrowthPoint &&
      playerMaxFp == saveData.playerMaxFp &&
      playerCurrentFp == saveData.playerCurrentFp &&
      playerFpGrowthPoint == saveData.playerFpGrowthPoint &&
      playerMaxBp == saveData.playerMaxBp &&
      playerBpGrowthPoint == saveData.playerBpGrowthPoint &&
      playerUsingBp == saveData.playerUsingBp &&
      shoesLevel == saveData.shoesLevel &&
      hammerLevel == saveData.hammerLevel &&
      havingCoin == saveData.havingCoin &&
      experience == saveData.experience &&
      havingStars == saveData.havingStars &&
      havingStarFragments == saveData.havingStarFragments &&
      havingSuperFertilizer == saveData.havingSuperFertilizer &&
      totalPlayingTimeMins == saveData.totalPlayingTimeMins &&
      havingItemsNumRestriction == saveData.havingItemsNumRestriction &&
      leavingItemsNumRestriction == saveData.leavingItemsNumRestriction &&
      currentSelectedPartnerName == saveData.currentSelectedPartnerName &&
      sakuraLevel == saveData.sakuraLevel &&
      sakuraCurrentHp == saveData.sakuraCurrentHp
    )
    {
      endReadingData = true;
    }
    return endReadingData;
  }

  void Start ()
  {
    ReadingSaveData ();
    DontDestroyOnLoad (this);
  }

  void ReadingSaveData ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;
    havingItemsName = saveData.havingItemsName;
    leavingItemsName = saveData.leavingItemsName;
    havingImportantThing = saveData.havingImportantThing;
    havingBadgesName = saveData.havingBadgesName;
    puttingBadgeNums = saveData.puttingBadgeNums;
    puttingBadgeId = saveData.puttingBadgeId;
    partnersName = saveData.partnersName;
    savePointSceneName = saveData.savePointSceneName;
    savedPlayerPosition = saveData.savedPlayerPosition;
    savedPartnerPosition = saveData.savedPartnerPosition;
    playerLevel = saveData.playerLevel;
    playerTitle = saveData.playerTitle;
    playerMaxHp = saveData.playerMaxHp;
    playerCurrentHp = saveData.playerCurrentHp;
    playerHpGrowthPoint = saveData.playerHpGrowthPoint;
    playerMaxFp = saveData.playerMaxFp;
    playerCurrentFp = saveData.playerCurrentFp;
    playerFpGrowthPoint = saveData.playerFpGrowthPoint;
    playerMaxBp = saveData.playerMaxBp;
    playerBpGrowthPoint = saveData.playerBpGrowthPoint;
    playerUsingBp = saveData.playerUsingBp;
    shoesLevel = saveData.shoesLevel;
    hammerLevel = saveData.hammerLevel;
    havingCoin = saveData.havingCoin;
    experience = saveData.experience;
    havingStars = saveData.havingStars;
    havingStarFragments = saveData.havingStarFragments;
    havingSuperFertilizer = saveData.havingSuperFertilizer;
    totalPlayingTimeMins = saveData.totalPlayingTimeMins;
    havingItemsNumRestriction = saveData.havingItemsNumRestriction;
    leavingItemsNumRestriction = saveData.leavingItemsNumRestriction;
    currentSelectedPartnerName = saveData.currentSelectedPartnerName;
    sakuraLevel = saveData.sakuraLevel;
    sakuraCurrentHp = saveData.sakuraCurrentHp;
  }

  // void Update ()
  // {
  //   if (Input.GetKeyDown (KeyCode.S))
  //   {
  //     havingItemsName = new string[]
  //     {
  //       "きせきのしゅくはくけん"
  //     };
  //     leavingItemsName = new string[]
  //     {
  //       "きせきのしゅくはくけん"
  //     };
  //     SaveAllData ();
  //   }
  // }

  public static void SaveAllData ()
  {
    MyGameData.MyData saveData = SaveSystem.Instance.userData;

    saveData.havingItemsName = havingItemsName;
    saveData.leavingItemsName = leavingItemsName;
    saveData.leavingItemsName = leavingItemsName;
    saveData.havingImportantThing = havingImportantThing;
    saveData.havingBadgesName = havingBadgesName;
    saveData.puttingBadgeNums = puttingBadgeNums;
    saveData.puttingBadgeId = puttingBadgeId;
    saveData.partnersName = partnersName;
    saveData.savePointSceneName = savePointSceneName;
    saveData.savedPlayerPosition = savedPlayerPosition;
    saveData.savedPartnerPosition = savedPartnerPosition;
    saveData.playerLevel = playerLevel;
    saveData.playerTitle = playerTitle;
    saveData.playerMaxHp = playerMaxHp;
    saveData.playerCurrentHp = playerCurrentHp;
    saveData.playerHpGrowthPoint = playerHpGrowthPoint;
    saveData.playerMaxFp = playerMaxFp;
    saveData.playerCurrentFp = playerCurrentFp;
    saveData.playerFpGrowthPoint = playerFpGrowthPoint;
    saveData.playerMaxBp = playerMaxBp;
    saveData.playerBpGrowthPoint = playerBpGrowthPoint;
    saveData.playerUsingBp = playerUsingBp;
    saveData.shoesLevel = shoesLevel;
    saveData.hammerLevel = hammerLevel;
    saveData.havingCoin = havingCoin;
    saveData.experience = experience;
    saveData.havingStars = havingStars;
    saveData.havingStarFragments = havingStarFragments;
    saveData.havingSuperFertilizer = havingSuperFertilizer;
    saveData.totalPlayingTimeMins = totalPlayingTimeMins;
    saveData.havingItemsNumRestriction = havingItemsNumRestriction;
    saveData.leavingItemsNumRestriction = leavingItemsNumRestriction;
    saveData.currentSelectedPartnerName = currentSelectedPartnerName;
    saveData.sakuraLevel = sakuraLevel;
    saveData.sakuraCurrentHp = sakuraCurrentHp;

    // SaveSystem.Instance.Save ();

  }

}