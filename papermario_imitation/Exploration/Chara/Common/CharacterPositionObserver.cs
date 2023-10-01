using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPositionObserver
{
  private GameObject charactorRootObject;
  // ゲームの画面(手前)に向かって進む事をisDirectionFront=trueとする
  private bool isDirectionFront;
  private bool isDirectionLeft;
  private bool isPreviousDirectionLeft;
  private Vector3 currentFrameRootObjectPosition;
  private Vector3 previousFrameRootObjectPosition;
  public CharacterPositionObserver (
    GameObject charactorRootObject
  )
  {
    isDirectionFront = true;
    isDirectionLeft = true;
    isPreviousDirectionLeft = true;
    this.charactorRootObject = charactorRootObject;
    SetCurrentRootPosition ();
    SetPreviousRootPosition ();
  }

  public void SetCurrentRootPosition ()
  {
    currentFrameRootObjectPosition = charactorRootObject.transform.position;
  }
  public void SetPreviousRootPosition ()
  {
    previousFrameRootObjectPosition = currentFrameRootObjectPosition;
  }

  public bool GetIsSameCurrentFrameRootObjPosiAndPreviousFrameRootObjPosi ()
  {
    // 小さすぎる値の違いに反応し、頻繁にtrueとfalseが入れ替わってしまう不具合がでないようにしたい
    // 小数点第三以下は切り捨てで判定する
    float currentPosiX = currentFrameRootObjectPosition.x;
    float currentPosiY = currentFrameRootObjectPosition.y;
    float currentPosiZ = currentFrameRootObjectPosition.z;
    currentPosiX = MathFloorForPosition (currentPosiX);
    currentPosiY = MathFloorForPosition (currentPosiY);
    currentPosiZ = MathFloorForPosition (currentPosiZ);

    float previousPosiX = previousFrameRootObjectPosition.x;
    float previousPosiY = previousFrameRootObjectPosition.y;
    float previousPosiZ = previousFrameRootObjectPosition.z;
    previousPosiX = MathFloorForPosition (previousPosiX);
    previousPosiY = MathFloorForPosition (previousPosiY);
    previousPosiZ = MathFloorForPosition (previousPosiZ);

    // ジャンプと落下は無視
    // return currentPosiX == previousPosiX && currentPosiY == previousPosiY && currentPosiZ == previousPosiZ;
    return currentPosiX == previousPosiX && currentPosiZ == previousPosiZ;

    // return currentFrameRootObjectPosition == previousFrameRootObjectPosition;
  }

  private float MathFloorForPosition (float originalFloatValue)
  {
    float forMathFloor = 1000;
    float afterMathFloor = Mathf.Floor (originalFloatValue * forMathFloor) / forMathFloor;
    return afterMathFloor;
  }
  public bool GetCurrentIsFront ()
  {
    // 小さすぎる値の違いに反応し、頻繁にtrueとfalseが入れ替わってしまう不具合がでないようにしたい
    // 小数点第三以下は切り捨てで判定する
    float currentRootObjectPositionZ = currentFrameRootObjectPosition.z;
    currentRootObjectPositionZ = MathFloorForPosition (currentRootObjectPositionZ);
    float previousRootObjectPositionZ = previousFrameRootObjectPosition.z;
    previousRootObjectPositionZ = MathFloorForPosition (previousRootObjectPositionZ);
    // 前フレームよりもz軸の値が大きければ画面奥に進んでいるので、isDirectionFront = false
    if (currentRootObjectPositionZ > previousRootObjectPositionZ)
    {
      isDirectionFront = false;
    }
    // 画面手前側に進んでいると前フレームよりもz軸の値小さいので、isDirectionFront = true
    if (currentRootObjectPositionZ < previousRootObjectPositionZ)
    {
      isDirectionFront = true;
    }
    return isDirectionFront;
  }

  public bool GetCurrentIsDirectionLeft ()
  {
    // MathFloorを用いない元の値だと、アローキーをチョン押しした移動の際にちゃんとした値がとれない？
    // 原因がわからないけど、右移動ではpositionXの値が前フレームより必ず大きくなるはずなのに小さくなる時がある(左移動同様)
    // 下記の値は誤差がでることを確認するときのためにコメントアウト中
    // float currentRootObjectPositionX = currentFrameRootObjectPosition.x;
    // float previousRootObjectPositionX = previousFrameRootObjectPosition.x;
    float currentRootObjectPositionX = MathFloorForPosition (currentFrameRootObjectPosition.x);
    float previousRootObjectPositionX = MathFloorForPosition (previousFrameRootObjectPosition.x);
    // 右に進んでいる時
    if (currentRootObjectPositionX > previousRootObjectPositionX)
    {
      isDirectionLeft = false;
      isPreviousDirectionLeft = isDirectionLeft;
    }
    // 左に進んでいる
    if (currentRootObjectPositionX < previousRootObjectPositionX)
    {
      isDirectionLeft = true;
      isPreviousDirectionLeft = isDirectionLeft;
    }
    if (currentRootObjectPositionX == previousRootObjectPositionX)
    {
      isDirectionLeft = isPreviousDirectionLeft;
    }
    // Debug.Log ("isDirectionLeft : " + isDirectionLeft);
    return isDirectionLeft;
  }

}