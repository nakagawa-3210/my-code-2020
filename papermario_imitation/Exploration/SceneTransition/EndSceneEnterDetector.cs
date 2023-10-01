using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneEnterDetector : MonoBehaviour
{
  private HanaPlayerScript hanaPlayerScript;
  private bool enterArea;
  void Start ()
  {
    enterArea = false;
  }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      enterArea = true;
      GameObject player = other.gameObject;
      hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
    }
  }

  public bool EnterArea
  {
    get { return enterArea; }
  }
}