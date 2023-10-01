using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class BattleExperienceMotionManager
{
  private MenuCommonFunctions menuCommonFunctions;
  private BattleVirtualCameras battleVirtualCameras;

  // private GameObject bigExperienceBallPrefab;
  // private GameObject smallExperienceBallPrefab;

  // private GameObject totalAcquiredExperiencePointNumText;

  // private Transform totalAcquiredExperiencePoint;
  // private Transform currentAcquiredExperiencePoint;

  private Transform totalBigExperienceBallContainer;
  private Transform totalSmallExperienceBallContainer;

  private Vector3 acquiredExperiencePointNumTextShownPosition;
  private Vector3 acquiredExperiencePointNumTextHidePosition;

  private int shownAcquiredTotalExperienceNum;

  private float timeElapsed;
  private float changeTime;
  float baseSmallScale;

  public BattleExperienceMotionManager (
    BattleVirtualCameras battleVirtualCameras
    // Transform currentAcquiredExperiencePoint
  )
  {
    menuCommonFunctions = new MenuCommonFunctions ();

    this.battleVirtualCameras = battleVirtualCameras;
    // this.currentAcquiredExperiencePoint = currentAcquiredExperiencePoint;

    shownAcquiredTotalExperienceNum = 0;
    timeElapsed = 0.0f;
    changeTime = 0.01f;
    baseSmallScale = 0.25f;

    // SetupAcquiredExperiencePointNumText ();
  }

  public void SetupAcquiredExperiencePointNumText (GameObject totalAcquiredExperiencePointNumText)
  {
    // 表示位置キャッシュ
    acquiredExperiencePointNumTextShownPosition = totalAcquiredExperiencePointNumText.transform.position;

    // 非表示位置キャッシュ
    float hidePositionX = Screen.width;
    acquiredExperiencePointNumTextHidePosition = totalAcquiredExperiencePointNumText.transform.position;
    acquiredExperiencePointNumTextHidePosition.x += hidePositionX;

    // 初期位置に設定
    Vector3 initialHidePosition = totalAcquiredExperiencePointNumText.transform.position;
    initialHidePosition.x -= hidePositionX;
    totalAcquiredExperiencePointNumText.transform.position = initialHidePosition;
  }
  

  // バトル終了のアニメーション再生
  // 現在獲得した経験値を非表示
  public void HideCurrentAcquiredExperiencePoint (Transform currentAcquiredExperiencePoint)
  {
    currentAcquiredExperiencePoint.gameObject.SetActive (false);
  }

  void SetupExperienceBall (GameObject ballPrefab, Transform ballContainer, int ballNum, float ballScale)
  {
    for (var i = 0; i < ballNum; i++)
    {
      GameObject experienceBall = GameObject.Instantiate (ballPrefab);
      experienceBall.transform.SetParent (ballContainer);
      experienceBall.transform.localScale = new Vector3 (ballScale, ballScale, ballScale);
    }
  }

  //  総経験値の表示
  public void ShowTotalExperiencePoint (GameObject bigExperienceBallPrefab, GameObject smallExperienceBallPrefab, Transform totalAcquiredExperiencePoint, int acquiredExperienceNum)
  {
    // 1単位の経験値用意
    int smallExperienceBallNum = acquiredExperienceNum % 10;

    totalSmallExperienceBallContainer = totalAcquiredExperiencePoint.Find ("TotalSmallExperiencePointContainer");
    SetupExperienceBall (smallExperienceBallPrefab, totalSmallExperienceBallContainer, smallExperienceBallNum, baseSmallScale);

    // 10単位の経験値
    int bigExperienceBallNum = acquiredExperienceNum / 10;
    float baseBigScale = 0.4f;
    totalBigExperienceBallContainer = totalAcquiredExperiencePoint.Find ("TotalBigExperiencePointContainer");
    SetupExperienceBall (bigExperienceBallPrefab, totalBigExperienceBallContainer, bigExperienceBallNum, baseBigScale);

    totalAcquiredExperiencePoint.gameObject.SetActive (true);

    shownAcquiredTotalExperienceNum = acquiredExperienceNum;
  }

  public void ManageTotalAcquiredExperienceNum (GameObject smallExperienceBallPrefab)
  {
    if (shownAcquiredTotalExperienceNum != 0)
    {
      timeElapsed += Time.deltaTime;
      if (timeElapsed >= changeTime)
      {
        // UIに経験値を追加
        shownAcquiredTotalExperienceNum--;
        DecreaseUiTotalAcquiredExperience (smallExperienceBallPrefab);
        timeElapsed = 0.0f;
      }
    }
  }

  void DecreaseUiTotalAcquiredExperience (GameObject smallExperienceBallPrefab)
  {
    List<GameObject> smallExperienceBallList = menuCommonFunctions.GetChildList (totalSmallExperienceBallContainer.gameObject);
    List<GameObject> bigExperienceBallList = menuCommonFunctions.GetChildList (totalBigExperienceBallContainer.gameObject);

    int smallExperienceBallNum = shownAcquiredTotalExperienceNum % 10;
    if (smallExperienceBallNum == 9)
    {
      int lastNumBigBall = bigExperienceBallList.Count - 1;
      MonoBehaviour.Destroy (bigExperienceBallList[lastNumBigBall]);
      SetupExperienceBall (smallExperienceBallPrefab, totalSmallExperienceBallContainer, smallExperienceBallNum, baseSmallScale);
    }
    else
    {
      int lastNumSmallBall = smallExperienceBallList.Count - 1;
      MonoBehaviour.Destroy (smallExperienceBallList[lastNumSmallBall]);
    }
  }

  public async UniTask ShowAcquiredExperiencePointNumText (GameObject totalAcquiredExperiencePointNumText, int acquiredExperienceNum)
  {
    string acquiredExperienceNumText = acquiredExperienceNum.ToString ();
    totalAcquiredExperiencePointNumText.GetComponent<Text> ().text = acquiredExperienceNumText + " ポイント ゲット！";

    totalAcquiredExperiencePointNumText.SetActive (true);

    float moveDuration = 1.0f;
    await totalAcquiredExperiencePointNumText.transform.DOMoveX (acquiredExperiencePointNumTextShownPosition.x, moveDuration);
    totalSmallExperienceBallContainer.gameObject.SetActive (false);

    int showTextTime = 1000;
    await UniTask.Delay (showTextTime);

    await totalAcquiredExperiencePointNumText.transform.DOMoveX (acquiredExperiencePointNumTextHidePosition.x, moveDuration);
  }

  public bool EndDecreasingAllTotalExperienceBalls ()
  {
    List<GameObject> smallExperienceBallList = menuCommonFunctions.GetChildList (totalSmallExperienceBallContainer.gameObject);
    return smallExperienceBallList.Count == 0;
  }
}