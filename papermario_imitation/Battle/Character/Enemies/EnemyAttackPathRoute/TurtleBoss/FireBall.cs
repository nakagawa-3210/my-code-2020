using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class FireBall : MonoBehaviour
{
  [SerializeField] GameObject fireBallCollider;
  [SerializeField] float fireBallDuration;
  private Animator selfAnimator;
  private AttackedDetector targetAttackedDetector;
  private GameObject fireBall;
  private Vector3 targetPositionVector3;
  private Vector3 fireBallColliderInitialPositionVector3;

  private Vector3 fireBallInitialPositionVector3;

  void Start ()
  {
    targetPositionVector3 = new Vector3 ();
    fireBallColliderInitialPositionVector3 = fireBallCollider.transform.position;
    fireBall = fireBallCollider.transform.Find ("FireBall").gameObject;
    fireBallInitialPositionVector3 = fireBall.transform.position;
    fireBall.SetActive (false);
    selfAnimator = this.gameObject.GetComponent<Animator> ();
  }

  // ほかの攻撃との共通処理改修予定
  public async UniTask SetUpTarget (GameObject target)
  {
    targetAttackedDetector = target.GetComponent<AttackedDetector> ();
    targetAttackedDetector.ResetDetector ();
    targetPositionVector3 = target.transform.position;
    await UniTask.WaitUntil (() => targetPositionVector3 != new Vector3 ());
  }

  public async UniTask FireBallAttack ()
  {
    await EmitFireBall ();
    await MoveFireBall ();
  }

  async UniTask EmitFireBall ()
  {
    selfAnimator.SetTrigger ("PlayFireBall");

    await UniTask.WaitUntil (() =>
      selfAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "BattleTurtleFireBall" &&
      selfAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.65f
    );

    // selfAnimator.SetTrigger ("PlayIdle");
  }

  async UniTask MoveFireBall ()
  {
    fireBall.SetActive (true);
    float backDuration = 1.0f;
    await DOTween.Sequence ().Append (fireBallCollider.transform.DOMove (targetPositionVector3, fireBallDuration)).SetEase (Ease.InQuad)
      .Join (fireBall.transform.DOMoveY (targetPositionVector3.y, fireBallDuration))
      .AppendCallback (() => fireBall.SetActive (false))
      .AppendCallback (() => selfAnimator.SetTrigger ("PlayIdle"))
      .Append (fireBallCollider.transform.DOMove (fireBallColliderInitialPositionVector3, backDuration));
    fireBall.transform.position = fireBallInitialPositionVector3;
  }

}