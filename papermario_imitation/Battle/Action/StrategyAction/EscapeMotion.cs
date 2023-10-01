using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class EscapeMotion : MonoBehaviour
{
  private GameObject wholeStageCameras;
  private GameObject zoomInPlayerSideCamera;
  private GameObject zoomOutPlayerSideCamera;
  private GameObject runUpParticle;
  private GameObject playerPartyGroup;
  private List<GameObject> partyList;
  private List<Vector3> partyInitialPositionList;
  private BattleVirtualCameras battleVirtualCameras;

  private Sequence runUpDustSequence;
  private Sequence runUpSequence;

  private Vector3 partyInitialPosition;

  float playerPartyMoveDuration;
  void Start ()
  {
    partyInitialPositionList = new List<Vector3> ();
    runUpParticle = GameObject.Find ("RunUpDustParticleSystem").gameObject;
    playerPartyGroup = GameObject.Find ("PlayerParty").gameObject;
    partyInitialPosition = playerPartyGroup.transform.position;
    playerPartyMoveDuration = 0.6f;
  }

  // void Update ()
  // {

  // }

  // playerとpartnerが反対方向を向く
  // 前後にじたばたする 土埃が足元で舞う
  // 成功した場合は少しジャンプして画面外まで移動する
  // 失敗した場合は転んで、起き上がって、元向いていた方向を向く

  public void SetupEscapeRoute (List<GameObject> partyList, BattleVirtualCameras battleVirtualCameras)
  {
    this.partyList = partyList;

    foreach (var playerMember in partyList)
    {
      Vector3 memberPosition = playerMember.transform.position;
      partyInitialPositionList.Add (memberPosition);
    }

    SetupVirtualCameras (battleVirtualCameras);
  }

  // 後でどのカメラを有効にするかの処理部分を共通処理としてBattleVirtualCamerasの関数に作り直す
  void SetupVirtualCameras (BattleVirtualCameras battleVirtualCameras)
  {
    this.battleVirtualCameras = battleVirtualCameras;

    wholeStageCameras = battleVirtualCameras.wholeStageCameras;
    GameObject jumpCameras = battleVirtualCameras.jumpCameras;
    GameObject hammerCameras = battleVirtualCameras.hammerCameras;
    GameObject playerSideCameras = battleVirtualCameras.playerSideCameras;
    zoomInPlayerSideCamera = battleVirtualCameras.zoomInPlayerSideCamera;
    zoomOutPlayerSideCamera = battleVirtualCameras.zoomOutPlayerSideCamera;

    jumpCameras.SetActive (false);
    hammerCameras.SetActive (false);
    playerSideCameras.SetActive (true);
  }

  public async UniTask StartEscape ()
  {
    // カメラ切り替え
    wholeStageCameras.SetActive (false);
    zoomInPlayerSideCamera.SetActive (true);

    // プレイヤーキャラ達を動かすループ再生
    // Debug.Log ("partyList.Count : " + partyList.Count);
    foreach (var playerMember in partyList)
    {
      ChangeMemberDirectionForEscape (playerMember);
    }

    await StartRunUp ();

    // 足元の土埃再生
    // 土埃の位置等を要改修
    float runUpParticleDuration = 0.1f;
    float right = -1.47f;
    float left = -3.4f;
    runUpDustSequence = DOTween.Sequence ().Append (runUpParticle.transform.DOMoveX (right, runUpParticleDuration).SetEase (Ease.Linear))
      .Append (runUpParticle.transform.DOMoveX (left, runUpParticleDuration).SetEase (Ease.Linear))
      .SetLoops (-1);
  }

  void ChangeMemberDirectionForEscape (GameObject member)
  {
    GameObject selfBone = member.transform.Find ("DefeatedRotationRoot").Find ("FrontBodyMeshWithBone").gameObject;
    float escapeDirection = 180.0f;
    // 逃げる方向に向く
    selfBone.transform.DOLocalRotate (new Vector3 (0, escapeDirection, 0), playerPartyMoveDuration);
  }

  async UniTask StartRunUp ()
  {

    float selfInitialPositionX = playerPartyGroup.transform.position.x;
    float selfInitialPositionY = playerPartyGroup.transform.position.y;
    float jumpHeight = 0.5f;

    //  少しジャンプ
    await DOTween.Sequence ().Append (playerPartyGroup.transform.DOMoveY ((selfInitialPositionY + jumpHeight), playerPartyMoveDuration / 2))
      .Append (playerPartyGroup.transform.DOMoveY (selfInitialPositionY, playerPartyMoveDuration / 2));

    // 助走音再生
    SEManager.Instance.Play (SEPath.RUN_UP, 1, 0, 1, true, null);

    // 前後に移動ループ
    float runUp = 0.4f;
    float runUpDuration = 0.2f;
    runUpSequence = DOTween.Sequence ().Append (playerPartyGroup.transform.DOMoveX ((selfInitialPositionX + runUp), runUpDuration).SetEase (Ease.Linear))
      .Append (playerPartyGroup.transform.DOMoveX (selfInitialPositionX, runUpDuration).SetEase (Ease.Linear))
      .SetLoops (-1);
  }

  public void KillPartyMemberMoveTween ()
  {
    runUpDustSequence.Kill ();
    runUpSequence.Kill ();
  }

  public async UniTask SucceededEscape ()
  {
    // 助走ストップ
    SEManager.Instance.Stop (SEPath.RUN_UP);
    // 逃げる音再生
    SEManager.Instance.Play (SEPath.SUCCESS_ESCAPING);

    float outOfScreen = 10.0f;
    float escapeDuration = 1.0f;
    await playerPartyGroup.transform.DOMoveX (partyInitialPosition.x - outOfScreen, escapeDuration);
  }

  public async UniTask FailedEscape ()
  {
    // 助走ストップ
    SEManager.Instance.Stop (SEPath.RUN_UP);
    // 失敗音再生
    SEManager.Instance.Play (SEPath.FAIL_ESCAPING);

    float initialDegree = zoomInPlayerSideCamera.transform.localEulerAngles.z;
    float fallDownDegree = -12.0f;
    float rotationDuration = 0.1f;
    float standUpDuration = 0.5f;
    // 失敗風カメラワーク
    await DOTween.Sequence ().Append (zoomInPlayerSideCamera.transform.DOLocalRotate (new Vector3 (0, 0, fallDownDegree), rotationDuration))
      .Append (zoomInPlayerSideCamera.transform.DOLocalRotate (new Vector3 (0, 0, initialDegree), standUpDuration));
    // 元の位置に戻る
    await playerPartyGroup.transform.DOMove (partyInitialPosition, standUpDuration);

    // 敵の方を向く
    foreach (var member in partyList)
    {
      ChangeMemberDirectionForBattle (member);
    }

    // カメラを戻す
    battleVirtualCameras.ActivateWholeStageCamera ();
    zoomInPlayerSideCamera.SetActive (false);
  }

  void ChangeMemberDirectionForBattle (GameObject member)
  {
    GameObject selfBone = member.transform.Find ("DefeatedRotationRoot").Find ("FrontBodyMeshWithBone").gameObject;
    float escapeDirection = 0.0f;
    // 逃げる方向に向く
    selfBone.transform.DOLocalRotate (new Vector3 (0, escapeDirection, 0), playerPartyMoveDuration);
  }

}