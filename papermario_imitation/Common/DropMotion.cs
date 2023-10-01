using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMotion
{
  public void Drop (Rigidbody rigidBody, float jumpPower = 5.0f)
  {
    float randomNumX = Random.Range (-1.5f, 1.0f);
    float randomNumZ = Random.Range (-1.5f, 1.0f);
    
    Vector3 forward = new Vector3 (randomNumX, jumpPower, randomNumZ);
    rigidBody.velocity = forward;
  }
}