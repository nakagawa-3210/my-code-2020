using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
  [SerializeField] GameObject waveCollider;
  [SerializeField] float waveMotionDuration;
  private Animator selfAnimator;
  private AttackedDetector targetAttackedDetector;
  private Vector3 targetPositionVector3;
  private Vector3 waveColliderInitialPositionVector3;
  private GameObject shockWave;

  void Start ()
  {
    targetPositionVector3 = new Vector3 ();
    waveColliderInitialPositionVector3 = waveCollider.transform.position;
    shockWave = waveCollider.transform.Find ("ShockWave").gameObject;
    shockWave.SetActive (false);
    selfAnimator = this.gameObject.GetComponent<Animator> ();
  }

  // void Update ()
  // {
  //   if (Input.GetKeyDown (KeyCode.Q))
  //   {
  //     shockWave.SetActive (false);
  //   }
  // }

  public async UniTask SetUpTarget (GameObject target)
  {
    targetAttackedDetector = target.GetComponent<AttackedDetector> ();
    targetAttackedDetector.ResetDetector ();
    targetPositionVector3 = target.transform.position;
    targetPositionVector3.y = waveCollider.transform.position.y;
    await UniTask.WaitUntil (() => targetPositionVector3 != new Vector3 ());
  }

  public async UniTask ShockWaveAttack ()
  {
    // Debug.Log ("tackleTargetPositionVector3 : " + targetPositionVector3);
    await SwingDown ();
    await MakeShockWave ();
    // 同じDurationで再生する
    // 衝撃波の再生
    // コライダーの移動
  }

  async UniTask SwingDown ()
  {
    selfAnimator.SetTrigger ("PlayHitting");

    // はたきアニメ終了待ち
    await UniTask.WaitUntil (() =>
      selfAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "BattleTurtleHitting" &&
      selfAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f
    );
    // Debug.Log ("はたくアニメ終了");
    // アニメを待機に戻す
    selfAnimator.SetTrigger ("PlayIdle");

    // await MakeShockWave ();
  }

  async UniTask MakeShockWave ()
  {
    shockWave.SetActive (true);
    float backDuration = 1.0f;
    await DOTween.Sequence ().Append (waveCollider.transform.DOMove (targetPositionVector3, waveMotionDuration)).SetEase (Ease.InQuad)
      .AppendCallback (() => shockWave.SetActive (false))
      .Append (waveCollider.transform.DOMove (waveColliderInitialPositionVector3, backDuration));
  }

}