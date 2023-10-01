using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingManager
{
  private Rigidbody selfRigidBody;
  private Vector3 selfVelocity;
  private bool playGravityMotion;

  public FallingManager (Rigidbody selfRigidBody)
  {
    this.selfRigidBody = selfRigidBody;

    playGravityMotion = true;
  }

  public void ManageSelfGravityMotion (HanaPlayerScript.State playerState)
  {
    if (playerState == HanaPlayerScript.State.Normal && !playGravityMotion)
    {
      playGravityMotion = true;
      PlayGravityMotion ();
    }
    else if (playerState != HanaPlayerScript.State.Normal && playGravityMotion)
    {
      playGravityMotion = false;
      StopGravityMotion ();
    }
  }

  void StopGravityMotion ()
  {
    selfVelocity = selfRigidBody.velocity;
    selfRigidBody.constraints = RigidbodyConstraints.FreezePosition;

    selfRigidBody.useGravity = false;
  }

  void PlayGravityMotion ()
  {
    selfRigidBody.constraints = RigidbodyConstraints.None;
    selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;

    selfRigidBody.useGravity = true;

    selfRigidBody.velocity = selfVelocity;
  }
}