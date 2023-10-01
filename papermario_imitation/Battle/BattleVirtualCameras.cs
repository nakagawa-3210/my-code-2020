using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVirtualCameras : MonoBehaviour
{
  public GameObject wholeStageCameras;
  public GameObject jumpCameras;
  public GameObject hammerCameras;
  public GameObject playerSideCameras;
  public GameObject wholeStageCamera;
  public GameObject wholeStageCameraWithNoise;
  public GameObject wholeStageCameraForDefeatedBoss;
  public GameObject zoomInEnemySideJumpCamera;
  public GameObject zoomOutEnemySideJumpCamera;
  public GameObject zoomInEnemySideHammerCamera;
  public GameObject zoomOutEnemySideHammerCamera;
  public GameObject zoomInPlayerSideCamera;
  public GameObject zoomOutPlayerSideCamera;

  void Start ()
  {
    ActivateWholeStageCamera ();
  }

  public void ActivateWholeStageCamera ()
  {
    CommonTaskOfActivatingWholeCamera ();
    wholeStageCamera.SetActive (true);
    wholeStageCameraWithNoise.SetActive (false);
    wholeStageCameraForDefeatedBoss.SetActive (false);
  }

  public void ActivateWholeStageCameraWithNoise ()
  {
    CommonTaskOfActivatingWholeCamera ();
    wholeStageCamera.SetActive (false);
    wholeStageCameraWithNoise.SetActive (true);
    wholeStageCameraForDefeatedBoss.SetActive (false);
  }

  public void ActivateWholeStageCameraForDefeatedBoss ()
  {
    CommonTaskOfActivatingWholeCamera ();
    wholeStageCamera.SetActive (false);
    wholeStageCameraWithNoise.SetActive (false);
    wholeStageCameraForDefeatedBoss.SetActive (true);
  }

  void CommonTaskOfActivatingWholeCamera ()
  {
    wholeStageCameras.SetActive (true);
    jumpCameras.SetActive (false);
    hammerCameras.SetActive (false);
    playerSideCameras.SetActive (false);
  }

  //  カメラがプレイヤーたちに寄る
  public void ZoomInPlayerParty ()
  {
    wholeStageCameras.SetActive (false);
    jumpCameras.SetActive (false);
    hammerCameras.SetActive (false);
    playerSideCameras.SetActive (true);
  }
}