using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class PlayerMemberDefeatedMotion : MonoBehaviour
{
  private GameObject self;
  private GameObject wholeStageCameras;
  private GameObject playerSideCameras;
  private GameObject zoomInPlayerSideCamera;
  private GameObject zoomOutPlayerSideCamera;
  private BattleVirtualCameras battleVirtualCamerasScript;

  void Start ()
  {
    self = this.gameObject;
    GameObject battleVirtualCameras = GameObject.Find ("BattleVirtualCameras").gameObject;
    battleVirtualCamerasScript = battleVirtualCameras.GetComponent<BattleVirtualCameras> ();
    SetupVirtualCameras ();
  }

  void SetupVirtualCameras ()
  {
    wholeStageCameras = battleVirtualCamerasScript.wholeStageCameras;
    GameObject jumpCameras = battleVirtualCamerasScript.jumpCameras;
    GameObject hammerCameras = battleVirtualCamerasScript.hammerCameras;
    playerSideCameras = battleVirtualCamerasScript.playerSideCameras;
    zoomInPlayerSideCamera = battleVirtualCamerasScript.zoomInPlayerSideCamera;
    zoomOutPlayerSideCamera = battleVirtualCamerasScript.zoomOutPlayerSideCamera;

    jumpCameras.SetActive (false);
    hammerCameras.SetActive (false);
    playerSideCameras.SetActive (true);
  }

  public async UniTask DefeatedPlayerPartyMember (float rotationDuration = 1.2f)
  {
    wholeStageCameras.SetActive (false);
    playerSideCameras.SetActive (true);
    zoomInPlayerSideCamera.SetActive (true);
    GameObject defeatedRotationRoot = self.transform.Find ("DefeatedRotationRoot").gameObject;
    GameObject selfBone = defeatedRotationRoot.transform.Find ("FrontBodyMeshWithBone").gameObject;
    await new CommonDefeatedMotion ().DefeatedMotion (selfBone, defeatedRotationRoot, self);
  }

  public async UniTask ActivateWholeStageCamera ()
  {
    battleVirtualCamerasScript.ActivateWholeStageCamera ();
  }
}