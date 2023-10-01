using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerScript : MonoBehaviour
{
  [SerializeField] string talkTextFileName;

  private GameObject self;
  private GameObject selfMesh;
  private GameObject talkingTarget;
  private State state;

  public enum State
  {
    Wait,
    Walk,
    Talk
  }

  public State VillagerState
  {
    get { return this.state; }
    set { this.state = value; }
  }
  public GameObject TalkingTarget
  {
    set { talkingTarget = value; }
  }
  public string TalkTextFileName
  {
    get { return talkTextFileName; }
  }

  void Start ()
  {
    self = this.gameObject;
    selfMesh = self.transform.Find ("VillagerMesh").gameObject;

    talkingTarget = null;
  }

  void Update ()
  {
    // stateに合わせて状態の管理をする
    // 会話中の時のみ、プレイヤーと自身の位置関係に応じて、自身のスプライトメッシュの向きを管理する
    // Status.Talkの時にプレイヤーの方を向いていなければ、毎フレーム10度づつ回転し、プレイヤーの方を向く
    // Debug.Log ("state : " + state);
    if (state == State.Talk)
    {
      // 回転管理関数呼び出し
      ManageLookingAtPlayer ();
    }
  }

  void ManageLookingAtPlayer ()
  {
    // Debug.Log ("talkingTarget : " + talkingTarget);
    if (talkingTarget == null) return;
    // 位置がほしいだけ
    RotationManager rotationManager = new RotationManager ();
    float playerPositionX = talkingTarget.transform.position.x;
    float selfPositionX = self.transform.position.x;
    float rotationSpeed = 5.0f;
    float rotationY = self.transform.localEulerAngles.y;

    // プレイヤーよりも左側にいる
    if (selfPositionX > playerPositionX)
    {
      rotationManager.RotationLeft (selfMesh, rotationSpeed);
    }
    if (selfPositionX < playerPositionX)
    {
      rotationManager.RotationRight (selfMesh, rotationSpeed);
    }
  }

  public void ResetTalkingTarget ()
  {
    this.talkingTarget = null;
  }

}