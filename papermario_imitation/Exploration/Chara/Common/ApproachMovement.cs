using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachMovement
{
  private Vector3 initialSelfForward;
  public ApproachMovement (GameObject self)
  {
    initialSelfForward = self.transform.forward;
  }
  // あくまでも目的地に向かう処理を持つだけ
  public void Approach (Transform targetTransform, GameObject self, float approachSpeed)
  {
    Vector3 targetPos = targetTransform.position;
    targetPos.y = self.transform.position.y;
    // 目的地への方向取得
    self.transform.LookAt (targetPos);
    Vector3 targetDirection = self.transform.forward;
    // 方向を戻す
    self.transform.localEulerAngles = initialSelfForward;
    // 目的地の方向に移動する
    self.transform.position = self.transform.position + targetDirection * approachSpeed * Time.deltaTime;
  }

}