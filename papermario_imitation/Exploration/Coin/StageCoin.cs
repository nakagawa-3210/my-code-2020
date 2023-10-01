using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class StageCoin : StageObject
{
  private GameObject self;
  private Rigidbody selfRigidBody;
  private HanaPlayerScript hanaPlayerScript;
  private FallingManager fallingManager;

  void Start ()
  {
    self = this.gameObject;
    selfRigidBody = self.GetComponent<Rigidbody> ();

    hanaPlayerScript = GameObject.FindWithTag ("Player").GetComponent<HanaPlayerScript> ();

    fallingManager = new FallingManager (selfRigidBody);

    base.Drop (self);
    base.InactivateSelfSphereColliderMilSec(self);
  }

  void Update ()
  {
    fallingManager.ManageSelfGravityMotion (hanaPlayerScript.PlayerState);
  }

  public override void OnCollideGround ()
  {
    SEManager.Instance.Play (SEPath.BOUNCE_COIN);
  }

  public override void OnCollisionEnter (Collision other)
  {
    base.OnCollisionEnter (other);

    if (other.collider.tag == "Player")
    {
      HanaPlayerCoinScript hanaPlayerCoinScript = other.gameObject.transform.GetComponent<HanaPlayerCoinScript> ();
      hanaPlayerCoinScript.PickUpCoin ();

      Destroy (self);
    }
  }
}