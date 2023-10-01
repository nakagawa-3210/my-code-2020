using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class HanaPlayerScript : MonoBehaviour
{
  // serializefield改修予定
  [SerializeField] GameObject directionOfTravelManager;
  [SerializeField] GameObject animationManager;
  [SerializeField] GameObject pickUpItemSign;
  [SerializeField] GameObject awarenessSign;
  private SwitchLoopOfActiveSelf switchLoopOfActiveSelf;

  public enum State
  {
    Normal,
    Talk,
    WalkingDoorway,
    WalkingNextScene,
    EncounteringEnemy,
    OpenMenu,
    PickUpItem
    // JumpAttack,
    // HammerAttack
  }

  private HanaPlayerTalkScript hanaPlayerTalkScript;
  private OperatingWithKeyBoard operatingWithKeyBoardScript;
  private PlayerAnimationManager playerAnimationManagerScript;
  private GameObject self;
  private GameObject selfFrontMeshWithBone;
  private GameObject selfBackMeshWithBone;
  private State state;
  private State prevFrameState;

  private bool escapedFromTheBattle;
  private bool wonTheBattle;

  // 外部からステータスの変更を受ける
  public State PlayerState
  {
    get { return this.state; }
    set { this.state = value; }
  }

  public bool EscapedFromTheBattle
  {
    set { escapedFromTheBattle = value; }
    get { return escapedFromTheBattle; }
  }

  public bool WonTheBattle
  {
    set { wonTheBattle = value; }
    get { return wonTheBattle; }
  }

  void Start ()
  {
    state = State.Normal;
    prevFrameState = state;
    self = gameObject;
    selfFrontMeshWithBone = self.transform.Find ("HanaFrontBodyMeshWithBone").gameObject;
    selfBackMeshWithBone = self.transform.Find ("HanaBackBodyMeshWithBone").gameObject;
    hanaPlayerTalkScript = GetComponent<HanaPlayerTalkScript> ();
    operatingWithKeyBoardScript = directionOfTravelManager.GetComponent<OperatingWithKeyBoard> ();
    playerAnimationManagerScript = animationManager.GetComponent<PlayerAnimationManager> ();

    switchLoopOfActiveSelf = new SwitchLoopOfActiveSelf ();

    pickUpItemSign.SetActive (false);
    awarenessSign.SetActive (false);
  }

  void Update ()
  {
    if (escapedFromTheBattle)
    {
      switchLoopOfActiveSelf.SwitchFrontAndBackMeshWithBoneActiveCondition (selfFrontMeshWithBone, selfBackMeshWithBone);
    }
    // Debug.Log ("state : " + state);
    if (prevFrameState == state)
    {
      if (state == State.Normal)
      {
        // 移動が可能
        // Debug.Log ("ボタン操作で移動可能");
        operatingWithKeyBoardScript.ManageMovement ();
      }
    }
    // else
    // {
    //   prevFrameState = state;
    // }
    // if (state == State.Normal)
    // {
    //   // 移動が可能
    //   operatingWithKeyBoardScript.ManageMovement ();
    // }

    // Debug.Log ("プレイヤーのステート確認中 : " + state);
  }

  public void ActivateSelfMeshWithBone ()
  {
    switchLoopOfActiveSelf.ActivateSelfMeshWithBone (selfFrontMeshWithBone, selfBackMeshWithBone);
  }

  async UniTask FixedUpdate ()
  {
    if (prevFrameState == state)
    {
      if (state == State.Normal)
      {
        // Debug.Log ("ボタン操作で移動に従っていどうする");
        operatingWithKeyBoardScript.MovePlayerBasedOnMoveValue ();
        // モーションの管理が可能
        playerAnimationManagerScript.ManagePlayerAnimation ();
      }
      else
      {
        // ベクトル削除で移動ストップ
        operatingWithKeyBoardScript.StopGetAxisMotionAndSpaceJump ();
        if (state == State.Talk)
        {
          // 歩きながら話しかけても、強制的に待機のモーションに切り替え
          playerAnimationManagerScript.ChangeIdleMotion ();
          // operatingWithKeyBoardScript.ResetVelocity ();
        }
        // 先制攻撃の有無を加えた条件式を今後増やす
        else if (state == State.EncounteringEnemy)
        {
          playerAnimationManagerScript.ChangeIdleMotion ();
        }
        else if (state == State.OpenMenu)
        {
          playerAnimationManagerScript.ChangeIdleMotion ();
        }
        else if (state == State.WalkingNextScene)
        {
          playerAnimationManagerScript.ManagePlayerAnimation ();
        }
        else if (state == State.PickUpItem)
        {
          playerAnimationManagerScript.ManagePlayerAnimation ();
        }
      }
    }
    else
    {
      await UniTask.DelayFrame (1);
      prevFrameState = state;
    }
  }

  // void LateUpdate ()
  // {
  //   if (prevFrameState != state)
  //   {
  //     prevFrameState = state;
  //   }
  // }

}