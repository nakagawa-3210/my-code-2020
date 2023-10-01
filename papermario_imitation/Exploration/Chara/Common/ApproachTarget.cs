using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachTarget : MonoBehaviour
{
  [SerializeField] Transform targetTran;
  [SerializeField] float approachSpeed;
  [SerializeField] float stopDistance;
  [SerializeField] float moveDistance;
  [SerializeField] bool isEnemy;
  private ApproachMovement approachMovement;
  private GameObject self;
  private Vector3 initialForward;
  private Vector3 targetDirection;

  void Start ()
  {
    // キャラ(root)の向きが追跡によって変更されないように初期値をキャッシュ
    initialForward = gameObject.transform.forward;
    self = gameObject;
    approachMovement = new ApproachMovement (self);
  }
  void Update ()
  {
    float distance = Vector3.Distance (gameObject.transform.position, targetTran.position);
    // 敵はプレイヤーを見つけた事を示してから追いかけるので、
    // プレイヤーを発見したサイン表示の条件も考える
    if (isEnemy)
    {
      // プレイヤーが一定距離範囲に入った時のみ追跡開始
      ChaseTarget (distance);
    }
    else
    {
      // 常にプレイヤーを追跡
      FollowTarget (distance);
    }
  }

  void FollowTarget (float distance)
  {
    if (distance > stopDistance)
    {
      approachMovement.Approach (targetTran, self, approachSpeed);
    }
  }

  void ChaseTarget (float distance)
  {
    if (distance < moveDistance && distance > stopDistance)
    {
      gameObject.transform.position =
        gameObject.transform.position + transform.forward * approachSpeed * Time.deltaTime;
    }
  }

}