using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class BattleExperienceManager : MonoBehaviour
{
  [SerializeField] ManagersForPlayerParty managersForPlayerParty;
  [SerializeField] ExperiencePoint experienceBallPrefab;
  [SerializeField] GameObject bigExperienceBallPrefab;
  [SerializeField] GameObject smallExperienceBallPrefab;

  [SerializeField] GameObject totalAcquiredExperiencePointNumText;
  [SerializeField] Transform totalAcquiredExperiencePoint;
  [SerializeField] Transform currentAcquiredExperiencePoint;

  private BattleExperienceMotionManager beforeClosingTheaterMotion;

  private Transform currentBigExperienceBallContainer;
  private Transform currentSmallExperienceBallContainer;

  private int acquiredExperienceNum;
  private int shownAcquiredExperienceNum;

  private float timeElapsed;
  private float changeTime;

  private bool startDecreaseTotalExperienceNum;

  public bool StartDecreaseTotalExperienceNum
  {
    set { startDecreaseTotalExperienceNum = value; }
  }

  public int AcquiredExperienceNum
  {
    set { acquiredExperienceNum = value; }
    get { return acquiredExperienceNum; }
  }

  void Start ()
  {
    beforeClosingTheaterMotion = new BattleExperienceMotionManager (
      managersForPlayerParty.battleVirtualCameras
      // bigExperienceBallPrefab,
      // smallExperienceBallPrefab,
      // totalAcquiredExperiencePointNumText,
      // totalAcquiredExperiencePoint,
      // currentAcquiredExperiencePoint
    );

    beforeClosingTheaterMotion.SetupAcquiredExperiencePointNumText (totalAcquiredExperiencePointNumText);

    acquiredExperienceNum = 0;
    shownAcquiredExperienceNum = acquiredExperienceNum;
    timeElapsed = 0.0f;
    changeTime = 0.01f;

    startDecreaseTotalExperienceNum = false;

    currentBigExperienceBallContainer = currentAcquiredExperiencePoint.Find ("CurrentBigExperiencePointContainer");
    currentSmallExperienceBallContainer = currentAcquiredExperiencePoint.Find ("CurrentSmallExperiencePointContainer");
    SetInactivate ();
  }

  void SetInactivate ()
  {
    totalAcquiredExperiencePointNumText.SetActive (false);
    totalAcquiredExperiencePointNumText.GetComponent<Text> ().text = "";

    totalAcquiredExperiencePoint.gameObject.SetActive (false);
  }

  void Update ()
  {
    ManageCurrentAcquiredExperienceNum ();
    if (startDecreaseTotalExperienceNum)
    {
      beforeClosingTheaterMotion.ManageTotalAcquiredExperienceNum (smallExperienceBallPrefab);
    }
  }

  void ManageCurrentAcquiredExperienceNum ()
  {
    if (acquiredExperienceNum != shownAcquiredExperienceNum)
    {
      timeElapsed += Time.deltaTime;
      if (timeElapsed >= changeTime)
      {
        // UIに経験値を追加
        shownAcquiredExperienceNum++;
        IncreaseUiCurrentAcquiredExperience ();
        timeElapsed = 0.0f;
      }
    }
  }

  void IncreaseUiCurrentAcquiredExperience ()
  {
    int smallExperienceBallNum = shownAcquiredExperienceNum % 10;
    // 1単位の経験値を表示管理
    //  下1ケタが0の場合
    if (smallExperienceBallNum == 0)
    {
      // 1単位のsmallExperienceBallを全削除して、10単位のbigExperienceBallを追加する
      foreach (Transform child in currentSmallExperienceBallContainer)
      {
        GameObject.Destroy (child.gameObject);
      }
      // 10単位の経験値を追加
      GameObject bigExperienceBall = GameObject.Instantiate (bigExperienceBallPrefab);
      bigExperienceBall.transform.SetParent (currentBigExperienceBallContainer);
      float baseScale = 0.3f;
      bigExperienceBall.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    }
    // 1単位の経験値を追加
    else
    {
      GameObject smallExperienceBall = GameObject.Instantiate (smallExperienceBallPrefab);
      smallExperienceBall.transform.SetParent (currentSmallExperienceBallContainer);
      float baseScale = 0.2f;
      smallExperienceBall.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
    }
  }

  public async UniTask ProduceExperiencePoint (GameObject defeatedEnemy)
  {
    BattleEnemyStatus defeatedEnemyStatus = defeatedEnemy.GetComponent<BattleEnemyStatus> ();
    int experienceNum = defeatedEnemyStatus.EnemyData.enemyExperience;
    // 経験値の数だけ玉を放出する
    for (var i = 0; i < experienceNum; i++)
    {
      Transform experiencePoint = GameObject.Instantiate (experienceBallPrefab).transform;
      Vector3 producePosition = new Vector3 ();
      float producePositionY = defeatedEnemy.transform.position.y + 1.0f;
      producePosition.y = producePositionY;
      experiencePoint.position = producePosition;
      experiencePoint.position = defeatedEnemy.transform.position;
      experiencePoint.transform.SetParent (defeatedEnemy.transform);
      float baseScale = 0.2f;
      experiencePoint.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
      // 玉の放出に間隔を作る
      int interval = 100;
      if (experienceNum >= 10)
      {
        interval /= 10;
      }
      await UniTask.Delay (interval);
    }
  }

  public void ShowTotalExperiencePoint ()
  {
    managersForPlayerParty.battleVirtualCameras.ZoomInPlayerParty ();
    beforeClosingTheaterMotion.HideCurrentAcquiredExperiencePoint (currentAcquiredExperiencePoint);
    beforeClosingTheaterMotion.ShowTotalExperiencePoint (bigExperienceBallPrefab, smallExperienceBallPrefab, totalAcquiredExperiencePoint, acquiredExperienceNum);
  }

  public async UniTask ManageExperienceResultMotion ()
  {
    await beforeClosingTheaterMotion.ShowAcquiredExperiencePointNumText (totalAcquiredExperiencePointNumText, acquiredExperienceNum);
  }

  public bool EndDecreasingAllTotalExperienceBalls ()
  {
    return beforeClosingTheaterMotion.EndDecreasingAllTotalExperienceBalls ();
  }

}