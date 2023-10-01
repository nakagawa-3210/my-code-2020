using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemaVirtualCameraChanger : MonoBehaviour
{
  // [SerializeField] bool hasNorthRoad;
  // [SerializeField] bool hasSouthRoad;
  // [SerializeField] bool hasWestRoad;
  // [SerializeField] bool hasEastRoad;

  [SerializeField] GameObject virtualCameraChasingPlayer;
  [SerializeField] GameObject virtualWestCamera;
  [SerializeField] GameObject virtualEastCamera;
  private CinemachineVirtualCamera chasingPlayerCamera;
  private CinemachineOrbitalTransposer cot;
  private float virtualWestCameraPositionX;
  private float virtualEastCameraPositionX;
  private float initialOffsetY;
  void Start ()
  {
    // virtualWestCamera.SetActive (false);
    // virtualEastCamera.SetActive (false);

    // プレイヤーの位置に応じて有効にするカメラを動的に変更
    // 左と右のカメラの位置をキャッシュする
    // x: virtualWestCameraPositionX < virtualEastCameraPositionX
    virtualWestCameraPositionX = virtualWestCamera.transform.position.x;
    virtualEastCameraPositionX = virtualEastCamera.transform.position.x;

    // CinemachineOrbitalTransposerがうまく取れないよー
    // chasingPlayerCamera = virtualCameraChasingPlayer.GetComponent<CinemachineVirtualCamera> ();
    // cot = chasingPlayerCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    // initialOffsetY = cot.m_FollowOffset.y;
  }
  void Update ()
  {
    // ModifyPositionForCameraChange ();
  }
  // m_FollowOffsetを動的に変更したかったけどうまくいかなかったのでコメントアウト
  // void ModifyPositionForCameraChange ()
  // {
  //   float positionZ = virtualWestCamera.transform.position.z;
  //   if (virtualCameraChasingPlayer.transform.position.z < positionZ)
  //   {
  //     cot.m_FollowOffset.y = -10;
  //   }
  //   else
  //   {
  //     cot.m_FollowOffset.y = initialOffsetY;
  //   }
  // }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      // カメラはプレイヤーを追跡しているので、そのカメラのx座標に応じて有効にする端のカメラを変更する
      ChangeActiveCamera ();
    }
  }

  void ChangeActiveCamera ()
  {
    float chaseingCameraPositionX = virtualCameraChasingPlayer.transform.position.x;
    if (chaseingCameraPositionX > 0)
    {
      virtualEastCamera.SetActive (true);
    }
    else
    {
      virtualWestCamera.SetActive (true);
    }
    virtualCameraChasingPlayer.SetActive (false);
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      virtualCameraChasingPlayer.SetActive (true);
      virtualWestCamera.SetActive (false);
      virtualEastCamera.SetActive (false);
    }
  }

}