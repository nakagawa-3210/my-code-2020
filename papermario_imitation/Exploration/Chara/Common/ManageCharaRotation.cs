using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 味方キャラ、街モブキャラ、敵キャラ等のキャラの向きの管理
public class ManageCharaRotation : MonoBehaviour
{
  // ゲームオブジェクトの元の座標を参考にする
  [SerializeField] GameObject charaRootObject;
  [SerializeField] GameObject rotationForCameraChange;
  // 回転対象の骨
  [SerializeField] GameObject frontBones;
  [SerializeField] GameObject backBones;
  // プレイヤーか、それ以外かで処理に分岐をさせるのに用いる
  [SerializeField] bool isPlayer;
  [SerializeField] bool isPartner;
  private PartnerScript partnerScript;
  private CharacterPositionObserver cpo;
  private RotationManager rotationManager;
  // x軸を基にキャラが左右のどちらに向いているかを把握し、向きに合わせてボーンを回転させる
  // 前フレームと現在のフレームにてキャラのx軸の差を測る
  private bool isDirectionLeft = true;
  private bool isDirectionFront = true;
  private Vector3 currentFrameCharaPosition;
  private Vector3 previousFrameCharaPosition;
  private bool isFrontSouth;
  private bool isFrontEast;
  private bool isFrontWest;

  void Start ()
  {
    isFrontSouth = true;
    isFrontEast = false;
    isFrontWest = false;
    if (charaRootObject.GetComponent<PartnerScript> () != null)
    {
      partnerScript = charaRootObject.GetComponent<PartnerScript> ();
    }
    cpo = new CharacterPositionObserver (charaRootObject);
    rotationManager = new RotationManager ();
  }

  void Update ()
  {
    // cpoを用いた方法はプレイヤーのみ適応しないかも
    cpo.SetCurrentRootPosition ();
    isDirectionFront = cpo.GetCurrentIsFront ();
    if (isPlayer)
    {
      // プレイヤーのステータスが入店でない場合、他シーンからの遷移中でない場合にキー操作で回転可能
      HanaPlayerScript hanaPlayerScript = charaRootObject.GetComponent<HanaPlayerScript> ();
      if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
      {
        ManageIsLeft ();
      }
      ManagePlayerBoneDirection ();
    }
    else if (isPartner)
    {
      isFrontSouth = partnerScript.IsFrontSouth;
      isFrontEast = partnerScript.IsFrontEast;
      isFrontWest = partnerScript.IsFrontWest;
      // 回転管理
      isDirectionLeft = cpo.GetCurrentIsDirectionLeft ();
      ManageRotationForPartner ();
    }
    else
    {
      isDirectionLeft = cpo.GetCurrentIsDirectionLeft ();
      ManageCharactorBoneDirection ();
    }
    cpo.SetPreviousRootPosition ();
  }

  void ManageIsLeft ()
  {
    // 左右のキーで回転を管理
    if (Input.GetKey (KeyCode.LeftArrow))
    {
      isDirectionLeft = true;
    }
    if (Input.GetKey (KeyCode.RightArrow))
    {
      isDirectionLeft = false;
    }
  }

  // 入店するときに必ず向いていてほしい向きがある時に使用
  // もしもやり方に困れば、移動座標に応じて向きを変更する仕様に戻す
  public void ManageLeftForStore ()
  {
    // パートナーも同じ関数を仕様するかも
    // if (isPlayer)
    // {
    isDirectionLeft = true;
    // }
  }
  public void ManageRightForStore ()
  {
    // if (isPlayer)
    // {
    isDirectionLeft = false;
    // }
  }

  void ManagePlayerBoneDirection ()
  {
    float rotationSpeed = 10.0f;
    // ショップでの移動のために変更
    // ショップ入店後はプレイヤーをy軸で回転させてしまうので、cpoに頼ると
    // 回転の挙動が操作と合わなくなってしまう
    if (isDirectionLeft)
    {
      rotationManager.RotationLeft (frontBones, rotationSpeed);
      rotationManager.RotationLeft (backBones, rotationSpeed);
    }
    if (!isDirectionLeft)
    {
      rotationManager.RotationRight (frontBones, rotationSpeed);
      rotationManager.RotationRight (backBones, rotationSpeed);
    }
  }

  void ManageCharactorBoneDirection ()
  {
    float rotationSpeed = 10.0f;
    // 現在の画面の正面の方向に応じて変更する
    // プレイヤーを移すカメラが東を向いている場合
    // 西を向いている場合
    // 南を向いている場合
    if (!isDirectionLeft)
    {
      rotationManager.RotationRight (frontBones, rotationSpeed);
      rotationManager.RotationRight (backBones, rotationSpeed);
    }
    if (isDirectionLeft)
    {
      rotationManager.RotationLeft (frontBones, rotationSpeed);
      rotationManager.RotationLeft (backBones, rotationSpeed);
    }
  }

  void ManagePartnerBoneDirectionForEastWest ()
  {
    float rotationSpeed = 10.0f;
    if (!isDirectionFront)
    {
      rotationManager.RotationRight (frontBones, rotationSpeed);
      rotationManager.RotationRight (backBones, rotationSpeed);
    }
    if (isDirectionFront)
    {
      rotationManager.RotationLeft (frontBones, rotationSpeed);
      rotationManager.RotationLeft (backBones, rotationSpeed);
    }
  }

  // 条件を分けるかもなのでisFrontEast,isFrontWest両方を用意しておく
  // 仲間の中身にある空オブジェクトを回転させて、さらに通常の回転を加える
  void ManageRotationForPartner ()
  {
    float rotationSpeed = 10.0f;
    if (isFrontSouth)
    {
      rotationManager.RotationRight (rotationForCameraChange, rotationSpeed);
      ManageCharactorBoneDirection ();
    }
    else if (isFrontEast)
    {
      rotationManager.RotationForCameraChangeEast (rotationForCameraChange, rotationSpeed);
      ManagePartnerBoneDirectionForEastWest ();
    }
    else if (isFrontWest)
    {
      rotationManager.RotationForCameraChangeEast (rotationForCameraChange, rotationSpeed);
      ManagePartnerBoneDirectionForEastWest ();
    }
  }
}