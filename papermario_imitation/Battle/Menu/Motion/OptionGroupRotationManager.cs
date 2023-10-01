using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;


// 選択肢を選んでいる時の動きを作る役割を担う(tween無し)
public class OptionGroupRotationManager
{
  private GameObject optionGroup;
  private List<GameObject> optionList;
  private Vector3 initialRotation;
  private int selectedOptionNum;
  private int selectedOptionMaxNum;
  float degree;
  private bool isRotating;
  private bool isLeft;

  public int SelectedOptionNum
  {
    set { selectedOptionNum = value; }
    get { return selectedOptionNum; }
  }
  public bool IsRotating
  {
    get { return isRotating; }
  }
  public OptionGroupRotationManager (
    GameObject optionGroup,
    List<GameObject> optionList
  )
  {
    this.optionGroup = optionGroup;
    this.optionList = optionList;
    initialRotation = optionGroup.transform.localEulerAngles;
    selectedOptionNum = 0;
    selectedOptionMaxNum = optionList.Count - 1;
    degree = 360.0f / optionList.Count;
    isRotating = false;
    isLeft = false;
  }

  public void ManageSelectedOptionNum ()
  {
    // Debug.Log ("isRotating : " + isRotating);
    if (!isRotating)
    {
      if (Input.GetKeyDown (KeyCode.RightArrow))
      {
        isLeft = false;
        selectedOptionNum++;
        SEManager.Instance.Play(SEPath.MENU_CURSOR);
        if (selectedOptionNum > selectedOptionMaxNum)
        {
          selectedOptionNum = 0;
        }
      }
      else if (Input.GetKeyDown (KeyCode.LeftArrow))
      {
        isLeft = true;
        selectedOptionNum--;
        SEManager.Instance.Play(SEPath.MENU_CURSOR);
        if (selectedOptionNum < 0)
        {
          selectedOptionNum = selectedOptionMaxNum;
        }
      }
    }
  }

  public void ManageOptionRotationBasedOnNum (float rotationSpeed)
  {
    rotationSpeed *= Time.deltaTime;
    RotationRight (rotationSpeed);
    RotationLeft (rotationSpeed);
  }

  public void ResetOptionRotation ()
  {
    isLeft = false;
    selectedOptionNum = 0;
    optionGroup.transform.localRotation = Quaternion.Euler (initialRotation);
  }

  // 探索シーン用のキャラに用いた回転とは異なるので別で作ってしまう
  void RotationRight (float rotationSpeed)
  {
    if (!isLeft)
    {
      float rotationDegree = degree * selectedOptionNum;
      if (optionGroup.transform.localEulerAngles.y > 180.0f && rotationDegree == 0)
      {
        rotationDegree = 360.0f;
      }
      float groupDegreeX = optionGroup.transform.localEulerAngles.x;
      float groupDegreeY = optionGroup.transform.localEulerAngles.y;
      float groupDegreeZ = optionGroup.transform.localEulerAngles.z;
      // 回転が終了しているか確認とリターン
      bool iSameDegree = IsSameDegree (groupDegreeY, rotationDegree);
      if (iSameDegree)
      {
        isRotating = false;
        return;
      }
      if (groupDegreeY < rotationDegree)
      {
        isRotating = true;
        float rotatedY = groupDegreeY + rotationSpeed;
        optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotatedY, groupDegreeZ);
        // 回転しすぎた時の修正
        if (optionGroup.transform.localEulerAngles.y > rotationDegree)
        {
          optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotationDegree, groupDegreeZ);
        }
      }
      // 360度ないし0度になるべき時に回転しすぎてしまった時の修正
      if (rotationDegree == 360.0f && optionGroup.transform.localEulerAngles.y < degree)
      {
        isRotating = true;
        rotationDegree = 0.0f;
        optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotationDegree, groupDegreeZ);
      }
    }
  }

  void RotationLeft (float rotationSpeed)
  {
    if (isLeft)
    {
      float rotationDegree = degree * selectedOptionNum;
      float groupDegreeX = optionGroup.transform.localEulerAngles.x;
      float groupDegreeY = optionGroup.transform.localEulerAngles.y;
      float groupDegreeZ = optionGroup.transform.localEulerAngles.z;
      // 0度からマイナスしても360度からマイナスと計算される
      // -90度の角度にする場合でも270度の角度にする場合と考える
      bool iSameDegree = IsSameDegree (groupDegreeY, rotationDegree);
      if (iSameDegree)
      {
        isRotating = false;
        return;
      }
      if (groupDegreeY > -rotationDegree)
      {
        isRotating = true;
        float rotatedY = groupDegreeY - rotationSpeed;
        optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotatedY, groupDegreeZ);
        if (optionGroup.transform.localEulerAngles.y < rotationDegree)
        {
          optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotationDegree, groupDegreeZ);
        }
      }
      if (rotationDegree == 0.0f && optionGroup.transform.localEulerAngles.y > degree)
      {
        isRotating = true;
        optionGroup.transform.localRotation = Quaternion.Euler (groupDegreeX, rotationDegree, groupDegreeZ);
      }
    }
  }

  // DebugLogでfloat値がまったく一致することを確認できていても、
  // if文で一致しないことが回転する角度によって起こるので値を丸めて計算することにした(キャラの位置判定と同様)
  bool IsSameDegree (float groupDegreeY, float rotationDegree)
  {
    bool isSame = false;
    float forMathFloor = 1000;
    float afterMathFloorGroupDegreeY = Mathf.Floor (groupDegreeY * forMathFloor) / forMathFloor;
    float afterMathFloorRotationDegree = Mathf.Floor (rotationDegree * forMathFloor) / forMathFloor;
    if (afterMathFloorGroupDegreeY == afterMathFloorRotationDegree)
    {
      isSame = true;
    }
    return isSame;
  }

}