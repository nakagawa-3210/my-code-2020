using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproacherAnimationManager : MonoBehaviour
{
  // プレイヤーのステータスが必要
  [SerializeField] GameObject hanaPlayer;
  // キャラの骨持ちメッシュの親元の位置取得
  [SerializeField] GameObject charaRootObject;
  [SerializeField] GameObject frontMeshWithBone;
  [SerializeField] GameObject backMeshWithBone;
  [SerializeField] GameObject frontMesh;
  [SerializeField] GameObject backMesh;
  [SerializeField] bool isPartner;
  private PartnerScript partnerScript;
  private CharacterPositionObserver approacherPositionObserver;
  private CharacterPositionObserver playerPositionObserver;
  private Animator approacherFrontAnimator;
  private Animator approacherBackAnimator;
  private bool isDirectionFront;
  private bool isDirectionLeft;
  private bool isFrontSouth;
  private bool isFrontEast;
  private bool isFrontWest;

  // プレイヤーを追うカメラはお店の入店に応じて変更される
  // カメラの向きがどちらなのかを監視しておく
  void Start ()
  {
    isDirectionFront = true;
    isDirectionLeft = true;
    isFrontSouth = true;
    isFrontEast = false;
    isFrontWest = false;
    if (charaRootObject.GetComponent<PartnerScript> () != null)
    {
      partnerScript = charaRootObject.GetComponent<PartnerScript> ();
    }
    approacherPositionObserver = new CharacterPositionObserver (charaRootObject);
    playerPositionObserver = new CharacterPositionObserver (hanaPlayer);
    approacherFrontAnimator = frontMeshWithBone.GetComponent<Animator> ();
    approacherBackAnimator = backMeshWithBone.GetComponent<Animator> ();
  }

  public void ManageApproacherAnimation ()
  {
    approacherPositionObserver.SetCurrentRootPosition ();
    if (isPartner)
    {
      isFrontSouth = partnerScript.IsFrontSouth;
      isFrontEast = partnerScript.IsFrontEast;
      isFrontWest = partnerScript.IsFrontWest;
      ManageShowingPartnerMeshWithBone ();
    }
    else
    {
      ManageShowingMeshWithBone ();
    }
    // ManageShowingMeshWithBone ();
    ManageWalkingAnimation ();
    approacherPositionObserver.SetPreviousRootPosition ();
  }

  // public void ManageShowingFrontMeshForShopTimeLine ()
  // {
  //   isDirectionFront = true;
  //   frontMesh.SetActive (true);
  //   backMesh.SetActive (false);
  //   Debug.Log ("メッシュは正面のはず！");
  // }
  // 通常は画面の正面は、ゲーム内ステージの南になっている
  // 店に入るとカメラが変更され、画面の正面は西か東に変更される
  // 変更された画面の正面に合わせて、isFrontの扱いを変更する
  public void ManageShowingMeshWithBone ()
  {
    isDirectionFront = approacherPositionObserver.GetCurrentIsFront ();
    if (isDirectionFront)
    {
      frontMesh.SetActive (true);
      backMesh.SetActive (false);
    }
    else
    {
      frontMesh.SetActive (false);
      backMesh.SetActive (true);
    }
  }

  public void ManageShowingPartnerMeshWithBone ()
  {
    isDirectionFront = approacherPositionObserver.GetCurrentIsFront ();
    isDirectionLeft = approacherPositionObserver.GetCurrentIsDirectionLeft ();
    if (isFrontSouth)
    {
      if (isDirectionFront)
      {
        frontMesh.SetActive (true);
        backMesh.SetActive (false);
      }
      else
      {
        frontMesh.SetActive (false);
        backMesh.SetActive (true);
      }
    }
    else if (isFrontEast)
    {
      if (isDirectionLeft)
      {
        frontMesh.SetActive (false);
        backMesh.SetActive (true);
      }
      else
      {
        frontMesh.SetActive (true);
        backMesh.SetActive (false);
      }
    }
    else if (isFrontWest)
    {
      if (!isDirectionLeft)
      {
        frontMesh.SetActive (true);
        backMesh.SetActive (false);
      }
      else
      {
        frontMesh.SetActive (false);
        backMesh.SetActive (true);
      }
    }
  }

  public void ManageWalkingAnimation ()
  {
    // このクラスが持つプレイヤーの位置情報更新
    playerPositionObserver.SetCurrentRootPosition ();
    isDirectionFront = approacherPositionObserver.GetCurrentIsFront ();
    isDirectionLeft = approacherPositionObserver.GetCurrentIsDirectionLeft ();
    bool isApproacherStoping = approacherPositionObserver.GetIsSameCurrentFrameRootObjPosiAndPreviousFrameRootObjPosi ();
    bool isPlayerStopping = playerPositionObserver.GetIsSameCurrentFrameRootObjPosiAndPreviousFrameRootObjPosi ();
    // 正面が南だった場合
    if (isFrontSouth)
    {
      if (isDirectionFront)
      {
        ActivateFrontWalking (isApproacherStoping, isPlayerStopping);
      }
      else
      {
        ActivateBackWalking (isApproacherStoping, isPlayerStopping);
      }
    }
    else if (isFrontEast)
    {
      if (!isDirectionLeft)
      {
        ActivateFrontWalking (isApproacherStoping, isPlayerStopping);
      }
      else
      {
        ActivateBackWalking (isApproacherStoping, isPlayerStopping);
      }
    }
    else if (isFrontWest)
    {
      if (isDirectionLeft)
      {
        ActivateFrontWalking (isApproacherStoping, isPlayerStopping);
      }
      else
      {
        ActivateBackWalking (isApproacherStoping, isPlayerStopping);
      }
    }
    playerPositionObserver.SetPreviousRootPosition ();
  }

  void ActivateFrontWalking (bool isApproacherStoping, bool isPlayerStopping)
  {
    // プレイヤーのと追いかける者が止まっていた時
    if (isApproacherStoping && isPlayerStopping)
    {
      approacherFrontAnimator.SetBool ("IsWalking", false);
    }
    else if (!isApproacherStoping && !isPlayerStopping)
    {
      approacherFrontAnimator.SetBool ("IsWalking", true);
    }
  }

  void ActivateBackWalking (bool isApproacherStoping, bool isPlayerStopping)
  {
    if (isApproacherStoping && isPlayerStopping)
    {
      approacherBackAnimator.SetBool ("IsWalking", false);
    }
    else if (!isApproacherStoping && !isPlayerStopping)
    {
      approacherBackAnimator.SetBool ("IsWalking", true);
    }
  }

}