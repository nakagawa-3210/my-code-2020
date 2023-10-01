using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerScript : MonoBehaviour
{
  [SerializeField] GameObject animationManager = default;
  [SerializeField] float activateColliderDistance;
  private ApproacherAnimationManager approacherAnimation;
  private GameObject self;
  private bool isFrontSouth;
  private bool isFrontEast;
  private bool isFrontWest;

  // private State state;

  // public State PartnerState
  // {
  //   get { return this.state; }
  //   set { this.state = value; }
  // }

  public bool IsFrontSouth
  {
    get { return isFrontSouth; }
  }
  public bool IsFrontEast
  {
    get { return isFrontEast; }
  }
  public bool IsFrontWest
  {
    get { return isFrontWest; }
  }

  public enum State
  {
    Normal,
    Walk,
    Talk
  }

  void Start ()
  {
    approacherAnimation = animationManager.GetComponent<ApproacherAnimationManager> ();

    self = this.gameObject;

    isFrontSouth = true;
    isFrontEast = false;
    isFrontWest = false;
  }

  void Update ()
  {
    approacherAnimation.ManageApproacherAnimation ();
    // テスト
    // if (Input.GetKeyDown (KeyCode.Q))
    // {
    //   SetFrontEast ();
    // }
    // if (Input.GetKeyDown (KeyCode.W))
    // {
    //   SetFrontSouth ();
    // }
  }

  void OnCollisionEnter (Collision other)
  {
    if (other.collider.tag == "Player")
    {
      self.transform.GetComponent<Rigidbody> ().isKinematic = true;
      self.transform.GetComponent<BoxCollider> ().isTrigger = true;
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      float distance = Vector3.Distance (self.transform.position, other.transform.position);
      if (distance > activateColliderDistance)
      {
        self.transform.GetComponent<Rigidbody> ().isKinematic = false;
        self.transform.GetComponent<BoxCollider> ().isTrigger = false;
      }

      // self.transform.GetComponent<Rigidbody> ().isKinematic = false;
      // self.transform.GetComponent<BoxCollider> ().isTrigger = false;
    }
  }

  // 下記をタイムラインから呼ぶ
  public void SetFrontSouth ()
  {
    isFrontSouth = true;
    isFrontEast = false;
    isFrontWest = false;
  }
  public void SetFrontEast ()
  {
    isFrontSouth = false;
    isFrontEast = true;
    isFrontWest = false;
  }
  public void SetFrontWest ()
  {
    isFrontSouth = false;
    isFrontEast = false;
    isFrontWest = true;
  }

}