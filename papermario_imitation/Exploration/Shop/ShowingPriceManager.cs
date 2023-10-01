using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingPriceManager : MonoBehaviour
{
  [SerializeField] GameObject[] items;
  private bool isEnteringPlayer;
  void Start ()
  {
    isEnteringPlayer = false;
    HidePrice ();
  }

  public void ShowPrice ()
  {
    foreach (var item in items)
    {
      Transform itemPrice = item.transform.Find ("ItemPrice");
      itemPrice.gameObject.SetActive (true);
    }
  }
  public void HidePrice ()
  {
    foreach (var item in items)
    {
      Transform itemPrice = item.transform.Find ("ItemPrice");
      itemPrice.gameObject.SetActive (false);
    }
  }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      isEnteringPlayer = true;
      // Debug.Log ("プレイヤーが入った");
      HanaPlayerScript hanaPlayerScript = other.GetComponent<HanaPlayerScript> ();
      if (hanaPlayerScript.PlayerState != HanaPlayerScript.State.WalkingDoorway)
      {
        ShowPrice ();
      }
    }
  }
  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      isEnteringPlayer = false;
      // Debug.Log ("プレイヤーが出た");
      HanaPlayerScript hanaPlayerScript = other.GetComponent<HanaPlayerScript> ();
      if (hanaPlayerScript.PlayerState != HanaPlayerScript.State.WalkingDoorway)
      {
        HidePrice ();
      }
    }
  }

  public bool IsEnteringPlayer
  {
    get { return isEnteringPlayer; }
  }
}