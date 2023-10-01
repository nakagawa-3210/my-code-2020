using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class DefeatedTurtleMotion : MonoBehaviour
{
  // [SerializeField] ManagersForPlayerParty managersForConversation = default;
  [SerializeField] string defeatedTurtleWordsFileName = default;
  private GameConversationManager gameConversationManager;
  private BattleVirtualCameras battleVirtualCamerasScript;
  private Animator selfAnimator;
  void Start ()
  {
    selfAnimator = this.gameObject.GetComponent<Animator> ();

    gameConversationManager = GameObject.Find ("GameConversationManager").GetComponent<GameConversationManager> ();

    GameObject battleVirtualCameras = GameObject.Find ("BattleVirtualCameras").gameObject;
    battleVirtualCamerasScript = battleVirtualCameras.GetComponent<BattleVirtualCameras> ();
  }

  private void Update ()
  {
    // Debug.Log ("AnimName : " + selfAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name);
    // Debug.Log ("AnimTime : " + selfAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime);
  }

  public async UniTask PlayDefeatedTurtle ()
  {
    // 顔が震えるモーション再生開始
    selfAnimator.SetTrigger ("PlayDefeated");
    // カメラが亀による
    battleVirtualCamerasScript.ActivateWholeStageCameraForDefeatedBoss ();
    // 会話が流せれるなら流したい
    gameConversationManager.OpenConversationCanvas (defeatedTurtleWordsFileName);
    await UniTask.WaitUntil (() => gameConversationManager.EndConversation);
    // 会話終了後にカメラをもとの場所に戻す
    battleVirtualCamerasScript.ActivateWholeStageCamera ();
    // 会話終了後に顔の震えモーションから顔がふっとぶモーションに切り替え
    selfAnimator.SetTrigger ("PlayFallDown");
    // モーション再生終了を検知して終了
    await UniTask.WaitUntil (() =>
      selfAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "BattleTurtleFallDownHead" &&
      selfAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f
    );
    selfAnimator.SetTrigger ("PlayLoopFallDown");
    // Debug.Log ("モーション再生終了");
  }

}