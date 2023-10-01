using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGroundManager : MonoBehaviour
{
  private bool isOnGround;
  void Start ()
  {
    isOnGround = false;
  }

  void OnTriggerStay (Collider other)
  {
    if (other.tag == "Ground")
    {
      isOnGround = true;
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Ground")
    {
      isOnGround = false;
    }
  }

  public bool IsOnGround
  {
    get { return isOnGround; }
  }

}