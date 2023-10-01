using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

// 敵のActionColliderからfirstHammerGoalPositionまでの距離,
// firstHammerGoalPositionからDamageColliderまでの距離を統一していないと
// ハンマーアクションにずれが出る。上手くいかなければ、Frame数をカウントしたタイミングゲーにする
//      コライダーとTweenを用いて正確なアクションコマンド処理を作れる自信がないのでTime.deltaを用いる方法に変更
public class HammerRoute : MonoBehaviour
{
  private Vector3 spacingPosition;
  private Vector3 hammerTargetPosition;
  private Vector3 playerInitialPosition;
  private GameObject self;
  private GameObject wholeStageCameras;
  private GameObject zoomInEnemySideCamera;
  private GameObject zoomOutEnemySideCamera;
  private BattleVirtualCameras battleVirtualCameras;

  private float durationAdjuster;
  private bool startHammer;
  private bool cameToHammerPosition;
  private bool endHammer;
  public bool StartHammer
  {
    get { return startHammer; }
  }
  public bool CameToHammerPosition
  {
    get { return cameToHammerPosition; }
  }
  public bool EndHammer
  {
    get { return endHammer; }
  }
  void Start ()
  {
    self = this.gameObject;
    playerInitialPosition = self.transform.position;
    durationAdjuster = 1.0f;
    ResetBools ();
  }

  // void Update ()
  // {

  // }

  public void ResetBools ()
  {
    startHammer = false;
    cameToHammerPosition = false;
    endHammer = false;
  }

  public void SetupHammerRoute (GameObject target, BattleVirtualCameras battleVirtualCameras)
  {
    this.battleVirtualCameras = battleVirtualCameras;
    SetupTargetPosition (target);
    SetupVirtualCameras ();
  }

  // 攻撃場所への移動完了を検知して知らせることが出来るようにする
  // 攻撃終了後にBattleSakuraクラスでタイマー処理開始
  void SetupTargetPosition (GameObject target)
  {
    hammerTargetPosition = target.transform.position;
    spacingPosition = target.transform.Find ("PlayerStartAttackPosition").gameObject.transform.position;
    spacingPosition.y = self.transform.position.y;
  }

  void SetupVirtualCameras ()
  {
    wholeStageCameras = battleVirtualCameras.wholeStageCameras;
    GameObject jumpCameras = battleVirtualCameras.jumpCameras;
    GameObject hammerCameras = battleVirtualCameras.hammerCameras;
    GameObject playerSideCameras = battleVirtualCameras.playerSideCameras;
    zoomInEnemySideCamera = battleVirtualCameras.zoomInEnemySideHammerCamera;

    jumpCameras.SetActive (false);
    hammerCameras.SetActive (true);
    playerSideCameras.SetActive (false);
  }

  public async UniTask StartHammerAction ()
  {
    if (!startHammer)
    {
      startHammer = true;
      wholeStageCameras.SetActive (false);
      zoomInEnemySideCamera.SetActive (true);

      float approachToHammerPositionDuration = 1.5f * durationAdjuster;
      await self.transform.DOMove (spacingPosition, approachToHammerPositionDuration).SetEase (Ease.OutSine);
      cameToHammerPosition = true;
    }
  }

  public void AfterSwingDownCamera ()
  {
    zoomInEnemySideCamera.SetActive (false);
  }

  public async UniTask EndHammerAction ()
  {
    battleVirtualCameras.ActivateWholeStageCamera ();

    float backToInitialPositionDuration = 1.0f * durationAdjuster;
    int littleWaitMillSec = 500;
    await self.transform.DOMove (playerInitialPosition, backToInitialPositionDuration).SetEase (Ease.OutSine);
    await UniTask.Delay (littleWaitMillSec);
    endHammer = true;
  }
}