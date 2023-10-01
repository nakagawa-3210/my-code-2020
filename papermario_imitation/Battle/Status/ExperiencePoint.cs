using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePoint : MonoBehaviour
{
  private Vector3 forward;
  private Rigidbody rigidBody;
  void Start ()
  {
    rigidBody = this.GetComponent<Rigidbody> ();
    new DropMotion ().Drop (rigidBody);
  }

  void OnCollisionEnter (Collision other)
  {
    if (other.transform.tag == "Ground")
    {
      Destroy (this.gameObject);
    }
  }
}