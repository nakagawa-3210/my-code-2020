using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
  [SerializeField] GameObject rootHanaPlayer;
  [SerializeField] GameObject frontMeshWithBone;
  [SerializeField] GameObject backMeshWithBone;
  [SerializeField] GameObject frontMesh;
  [SerializeField] GameObject backMesh;
  private HanaPlayerScript hanaPlayerScript;
  private CharacterPositionObserver playerPositionObserver;
  private Animator playerFrontAnimator;
  private Animator playerBackAnimator;
  private bool isFront;

  void Start ()
  {
    isFront = true;
    hanaPlayerScript = rootHanaPlayer.GetComponent<HanaPlayerScript> ();
    playerPositionObserver = new CharacterPositionObserver (rootHanaPlayer);
    playerFrontAnimator = frontMeshWithBone.GetComponent<Animator> ();
    playerBackAnimator = backMeshWithBone.GetComponent<Animator> ();
  }

  public void ManagePlayerAnimation ()
  {
    playerPositionObserver.SetCurrentRootPosition ();
    ManagePlayerIsFront ();
    ManageShowingMesh ();
    ManageWalkingAnimation ();
    playerPositionObserver.SetPreviousRootPosition ();
  }

  public void ManageShowingFrontMeshForShopTimeLine ()
  {
    isFront = true;
    frontMesh.SetActive (true);
    backMesh.SetActive (false);
  }

  void ManagePlayerIsFront ()
  {
    if (Input.GetKeyDown (KeyCode.UpArrow))
    {
      isFront = false;
    }
    if (Input.GetKeyDown (KeyCode.DownArrow))
    {
      isFront = true;
    }
  }

  public void ManageShowingMesh ()
  {
    // 左右のメッシュ回転と同様、表示するメッシュもキーボード操作で管理
    // isFront = playerPositionObserver.GetCurrentIsFront ();
    // // 上下の矢印キーで表示するメッシュを変更する
    if (isFront)
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

  public void ChangeIdleMotion ()
  {
    //待機になっていなかった場合のみ、外部の好みのタイミングで待機モーションに切り替え
    // 歩いていた時
    if (playerFrontAnimator.GetBool ("IsWalking") || playerBackAnimator.GetBool ("IsWalking"))
    {
      playerFrontAnimator.SetBool ("IsWalking", false);
      playerBackAnimator.SetBool ("IsWalking", false);
    }
  }

  public void ManageWalkingAnimation ()
  {
    bool isPlayerStopping = playerPositionObserver.GetIsSameCurrentFrameRootObjPosiAndPreviousFrameRootObjPosi ();
    // Debug.Log ("isPlayerStopping : " + isPlayerStopping);
    // 正面アニメ再生
    if (isFront)
    {
      if (isPlayerStopping)
      {
        playerFrontAnimator.SetBool ("IsWalking", false);
      }
      else
      {
        playerFrontAnimator.SetBool ("IsWalking", true);
      }
    }
    // 後ろ姿アニメ再生
    else
    {
      // プレイヤーの位置判定によるアニメ再生に変更したが、念のためにコメントアウトで残す
      // if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow))
      // {
      //   playerBackAnimator.SetBool ("IsWalking", true);
      // }
      // if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.UpArrow))
      // {
      //   playerBackAnimator.SetBool ("IsWalking", false);
      // }
      if (isPlayerStopping)
      {
        playerBackAnimator.SetBool ("IsWalking", false);
      }
      else
      {
        playerBackAnimator.SetBool ("IsWalking", true);
      }
    }
  }

  // タイムラインでのアニメに使用する関数

  public void SetPlayerStatusNormalFromTimeLine ()
  {
    // Debug.Log ("タイムラインからnormalになったよ");
    hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
  }
  public void SetPlayerStatusWalkingDoorwayFromTimeLine ()
  {
    hanaPlayerScript.PlayerState = HanaPlayerScript.State.WalkingDoorway;
  }

  public void SetTrueIsWalkingFromTimeLine ()
  {
    playerFrontAnimator.SetBool ("IsWalking", true);
    playerBackAnimator.SetBool ("IsWalking", true);
    // Debug.Log ("たいむらいんであるいてる");
  }

  public void SetFalseIsWalkingFromTimeLine ()
  {
    playerFrontAnimator.SetBool ("IsWalking", false);
    playerBackAnimator.SetBool ("IsWalking", false);
  }

  // public bool IsFront
  // {
  //   // set
  // }
}