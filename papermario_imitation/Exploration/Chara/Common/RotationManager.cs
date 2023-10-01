using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager
{
  // 左右の向きを変更する
  public void RotationRight (GameObject rotationTarget, float rotationSpeed)
  {
    float rightDirection = 0.0f;
    float rotationX = rotationTarget.transform.localEulerAngles.x;
    float rotationY = rotationTarget.transform.localEulerAngles.y;
    float rotationZ = rotationTarget.transform.localEulerAngles.z;
    if (rotationY == rightDirection) return;
    if (rotationY > rightDirection)
    {
      rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY - rotationSpeed, rotationZ);
      if (rotationY - rotationSpeed < rightDirection)
      {
        rotationY = rightDirection;
        rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY, rotationZ);
      }
    }
  }
  public void RotationLeft (GameObject rotationTarget, float rotationSpeed)
  {
    float leftDirection = 180.0f;
    float rotationX = rotationTarget.transform.localEulerAngles.x;
    float rotationY = rotationTarget.transform.localEulerAngles.y;
    float rotationZ = rotationTarget.transform.localEulerAngles.z;
    if (rotationY == leftDirection) return;
    if (rotationY < leftDirection)
    {
      rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY + rotationSpeed, rotationZ);
      // 回転しすぎた場合、回転角度が180以上の値かマイナスの値になる場合があるのでそれに備えた調整
      if (rotationY > leftDirection || rotationY < 0)
      {
        rotationY = leftDirection;
        rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY, rotationZ);
      }
    }
  }
  // 入店などでステージの正面の変更が起きる
  // その変更に合わせて特定のゲームオブジェクトの向きを変更する
  public void RotationForCameraChangeEast (GameObject rotationTarget, float rotationSpeed)
  {
    // マイナスにしていくと、360度からの引き算扱いとなる
    float rightDirection = 270.0f;
    float rotationX = rotationTarget.transform.localEulerAngles.x;
    float rotationY = rotationTarget.transform.localEulerAngles.y;
    float rotationZ = rotationTarget.transform.localEulerAngles.z;
    // すでに回転していた場合は返す(180度以上の値はマイナス値になる時がある)
    if (rotationY == -360 + rightDirection || rotationY == rightDirection) return;
    if (rightDirection > rotationY)
    {
      rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY + rotationSpeed, rotationZ);
      // Debug.Log ("rotationTarget.transform.localEulerAngles.y : " + rotationTarget.transform.localEulerAngles.y);
      // 回転しすぎた場合
      if (rotationY > rightDirection)
      {
        rotationY = rightDirection;
        rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY, rotationZ);
      }
    }
  }

  public void RotationForCameraChangeWest (GameObject rotationTarget, float rotationSpeed)
  {
    // マイナスにしていくと、360度からの引き算扱いとなる
    float leftDirection = 90.0f;
    float rotationX = rotationTarget.transform.localEulerAngles.x;
    float rotationY = rotationTarget.transform.localEulerAngles.y;
    float rotationZ = rotationTarget.transform.localEulerAngles.z;
    // すでに回転していた場合は返す
    if (rotationY == leftDirection) return;
    if (leftDirection > rotationY)
    {
      rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY + rotationSpeed, rotationZ);
      // Debug.Log ("rotationTarget.transform.localEulerAngles.y : " + rotationTarget.transform.localEulerAngles.y);
      // 回転しすぎた場合
      if (rotationY > leftDirection)
      {
        rotationY = leftDirection;
        rotationTarget.transform.localEulerAngles = new Vector3 (rotationX, rotationY, rotationZ);
      }
    }
  }
}