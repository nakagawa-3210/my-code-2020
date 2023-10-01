using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class PlayerJumpRoute : MonoBehaviour
{
  private GameObject firstJumpGoal;
  private GameObject playerFirstJumpPosition;
  private GameObject self;
  private GameObject wholeStageCameras;
  // private GameObject jumpCameras;
  // private GameObject hammerCameras;
  // private GameCharactorOrderInLayerManager
  private GameObject zoomInEnemySideCamera;
  private GameObject zoomOutEnemySideCamera;

  private BattleVirtualCameras battleVirtualCameras;
  private ActionUiMotionManager actionUiMotionManagerScript;

  private Vector3 jumpTargetPosition;
  // マリオはジャンプする際に、敵の位置に関わらず、ステージの中央まで移動してジャンプを行う
  // =>そうじゃなかった。
  private Vector3 playerJumpStartPosition;
  private Vector3 firstJumpGoalPosition;
  private Vector3 playerInitialPosition;
  private float durationAdjuster;

  // 不要なbool値改修予定
  private bool startJump;
  private bool startLastJump;
  private bool endJump;
  private bool canChooseAction;

  public bool EndJump
  {
    get { return endJump; }
  }
  public bool CanChooseAction
  {
    get { return canChooseAction; }
  }
  void Start ()
  {
    self = this.gameObject;
    playerInitialPosition = self.transform.position;
    GameObject actionUiMotionManager = GameObject.Find ("BattleActionCommandUiMotionManager");
    actionUiMotionManagerScript = actionUiMotionManager.GetComponent<ActionUiMotionManager> ();
    // ジャンプの動き全体の速さ変更用変数。値が小さいほど速い。コマンドフレームには変化無し
    durationAdjuster = 1.0f;
    ResetBools ();
  }

  void Update ()
  {
    // 動きテスト用
    // if (Input.GetKeyDown (KeyCode.A))
    // {
    //   JumpAgain ();
    // }
  }
  public void ResetBools ()
  {
    startJump = false;
    endJump = false;
    startLastJump = false;
    canChooseAction = false;
  }

  public void SetupJumpRoute (GameObject target, BattleVirtualCameras battleVirtualCameras)
  {
    this.battleVirtualCameras = battleVirtualCameras;
    SetupTargetPosition (target);
    SetupVirtualCameras ();
  }

  void SetupTargetPosition (GameObject target)
  {
    // 敵の位置を決めておく
    jumpTargetPosition = target.transform.position;
    firstJumpGoal = target.transform.Find ("FirstJumpGoal").gameObject;
    playerFirstJumpPosition = target.transform.Find ("PlayerStartAttackPosition").gameObject;
    playerJumpStartPosition = playerFirstJumpPosition.transform.position;
    playerJumpStartPosition.y = self.transform.position.y;
    firstJumpGoalPosition = firstJumpGoal.transform.position;
  }

  void SetupVirtualCameras ()
  {
    wholeStageCameras = battleVirtualCameras.wholeStageCameras;
    GameObject jumpCameras = battleVirtualCameras.jumpCameras;
    GameObject hammerCameras = battleVirtualCameras.hammerCameras;
    GameObject playerSideCameras = battleVirtualCameras.playerSideCameras;
    zoomInEnemySideCamera = battleVirtualCameras.zoomInEnemySideJumpCamera;
    zoomOutEnemySideCamera = battleVirtualCameras.zoomOutEnemySideJumpCamera;

    // ハンマー用カメラは使えないようにしておく
    jumpCameras.SetActive (true);
    hammerCameras.SetActive (false);
    playerSideCameras.SetActive (false);
  }

  // 敵の近くに移動した際にカメラがマリオと敵に寄る動作は探索シーンと同様の方法にする
  // ステージにコライダーを設置し、コライダーに主人公が入ったらカメラを切り替えるようにする
  public async UniTask StartJump ()
  {
    if (!startJump)
    {
      startJump = true;

      Vector3 jumpHighestPosition = jumpTargetPosition;
      jumpHighestPosition.x -= 0.3f;
      jumpHighestPosition.y += 1.0f;

      // 敵寄りカメラへ切り替え
      wholeStageCameras.SetActive (false);
      zoomInEnemySideCamera.SetActive (true);

      // 敵のすぐそばまで移動
      float approachToJumpStartPositionDuration = 1.5f * durationAdjuster;
      // Debug.Log ("playerJumpStartPosition : " + playerJumpStartPosition);
      await self.transform.DOMove (playerJumpStartPosition, approachToJumpStartPositionDuration).SetEase (Ease.OutSine);

      // DOPathをキャンセルする方法、AppendでつなげたtweenをDOPathのようになめらかな動きでつなげる方法がわからなかったので
      float jumpDuration = 0.8f * durationAdjuster;
      await self.transform.DOPath (new Vector3[] { jumpHighestPosition, firstJumpGoalPosition }, jumpDuration, PathType.CatmullRom);
    }
  }

  // ジャンプコマンド成功時に再度敵の頭上にジャンプする
  public async UniTask JumpAgain ()
  {
    Vector3 jumpHighestPosition = jumpTargetPosition;
    float littleHigher = 1.2f;
    jumpHighestPosition.y += littleHigher;
    // Debug.Log ("コマンド成功でのジャンプ");
    float jumpAgainDuration = 0.4f * durationAdjuster;

    // 敵寄りのカメラを少し前に戻す動き
    ActivateZoomInEnemySideCamera (jumpAgainDuration);

    // Sequenceでのawaitのやり方が見つからなかったのでひとつづつ対応
    // =>できた…。ので後で改修
    await DOTween.Sequence ().Append (self.transform.DOMove (jumpHighestPosition, jumpAgainDuration).SetEase (Ease.OutCubic))
      .Append (self.transform.DOMove (firstJumpGoalPosition, jumpAgainDuration));
    // await self.transform.DOMove (jumpHighestPosition, jumpAgainDuration).SetEase (Ease.OutCubic);
    // await self.transform.DOMove (firstJumpGoalPosition, jumpAgainDuration);
  }

  async UniTask ActivateZoomInEnemySideCamera (float jumpAgainDuration)
  {
    int waitForJumpAgain = (int) (jumpAgainDuration * 1000);
    await UniTask.Delay (waitForJumpAgain);
    zoomInEnemySideCamera.SetActive (true);
  }

  public void ApproachToTarget ()
  {
    float approachToDamageColliderDuration = 0.8f * durationAdjuster;
    self.transform.DOMove (jumpTargetPosition, approachToDamageColliderDuration);
  }

  public async UniTask WaitingJump ()
  {
    // Debug.Log ("ジャンプアクションチャンス！");
    actionUiMotionManagerScript.ShowPressedJumpActionSign ();
    // 待ち時間をバッジの有無で変更すれば原作に近くなる(セーブデータを用いた条件式でactionCommandFrameを変更すればいい)
    int actionCommandFrame = 10; // バッジの有無で変更
    await UniTask.DelayFrame (actionCommandFrame);

    // 敵寄りのカメラを少し後ろに引く動き
    zoomInEnemySideCamera.SetActive (false);
    canChooseAction = true;
  }

  // ジャンプコマンド攻撃終了時の動き
  public async UniTask LastJump ()
  {
    Vector3 jumpHighestPosition = jumpTargetPosition;
    float higher = 1.2f;
    jumpHighestPosition.y += higher;
    float littleBack = -0.5f;
    jumpHighestPosition.x += littleBack;
    // 余計なbool値の確認のためにコメントアウト中
    if (!startLastJump)
    {
      startLastJump = true;
      // かいしゅうとちゅう
      battleVirtualCameras.ActivateWholeStageCamera();
      float lastJumpDuration = 1.0f * durationAdjuster;
      // Debug.Log ("playerJumpStartPosition : " + playerJumpStartPosition);
      await self.transform.DOPath (new Vector3[] { jumpHighestPosition, playerJumpStartPosition }, lastJumpDuration, PathType.CatmullRom);
    }
  }

  public async UniTask BackToPlayerInitialPosition ()
  {
    float backToInitialPositionDuration = 1.5f * durationAdjuster;
    await self.transform.DOMove (playerInitialPosition, backToInitialPositionDuration).SetEase (Ease.OutSine);
    // Debug.Log ("ジャンプ攻撃終了");
    endJump = true;
  }

}