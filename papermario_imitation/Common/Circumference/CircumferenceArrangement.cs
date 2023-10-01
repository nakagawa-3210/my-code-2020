using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircumferenceArrangement : MonoBehaviour
{
  [SerializeField] float startRadius;
  [SerializeField] float endRadius;
  [SerializeField] float degree; // 扇状に配列できるようになる

  // void OnValidate ()
  // {
  //   Deploy ();
  // }

  void Update ()
  {
    // test用
    // if (Input.GetKeyDown (KeyCode.S))
    // {
    //   ResetDeployXY ();
    // }
  }

  // 参考サイト
  // https://kan-kikuchi.hatenablog.com/entry/CircleDeployer

  public void ResetDeployXY ()
  {
    GameObject self = this.gameObject;
    //子を取得
    List<GameObject> childList = new MenuCommonFunctions ().GetChildList (self);
    //オブジェクト間の角度差
    float angleDiff = GetAngleDiff (childList.Count);
    //各オブジェクトを円状に配置
    for (int i = 0; i < childList.Count; i++)
    {
      Vector3 childPostion = transform.position;
      float angle = (90.0f - angleDiff * i) * Mathf.Deg2Rad;
      childPostion.x += startRadius * Mathf.Cos (angle);
      childPostion.y += startRadius * Mathf.Sin (angle);
      childList[i].transform.position = childPostion;
    }
  }

  public Vector3 GetExpandedCircumferencePositionXY (GameObject child, int arrangementNum, int arrangementLength, float endRadiusXY = 0)
  {
    // Debug.Log ("画面横サイズ : " + Screen.width);
    // Debug.Log ("endRadius : " + endRadius);
    if (endRadiusXY != 0)
    {
      endRadius = endRadiusXY;
    }
    Vector3 postion = child.transform.position;
    float angleDiff = GetAngleDiff (arrangementLength);
    float angle = (90.0f - angleDiff * arrangementNum) * Mathf.Deg2Rad;
    postion.x += endRadius * Mathf.Cos (angle);
    postion.y += endRadius * Mathf.Sin (angle);
    return postion;
  }

  public Vector3 GetExpandedCircumferencePositionXZ (GameObject child, int arrangementNum, int arrangementLength)
  {
    Vector3 postion = child.transform.position;
    float angleDiff = GetAngleDiff (arrangementLength);
    float angle = (90.0f - angleDiff * arrangementNum) * Mathf.Deg2Rad;
    postion.x += endRadius * Mathf.Cos (angle);
    postion.z += endRadius * Mathf.Sin (angle);
    return postion;
  }

  float GetAngleDiff (int childNum)
  {
    float angleDiff = 0.0f;
    if (degree == 0)
    {
      angleDiff = 360.0f / (float) childNum;
    }
    else
    {
      angleDiff = degree / (float) childNum;
    }
    return angleDiff;
  }

}