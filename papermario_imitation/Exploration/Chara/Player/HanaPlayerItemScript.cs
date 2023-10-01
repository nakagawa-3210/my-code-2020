using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanaPlayerItemScript : MonoBehaviour
{
  [SerializeField] PickUpItemManager pickUpItemManager;
  private HanaPlayerScript hanaPlayerScript;

  void Start ()
  {
    // Debug.Log ("プレイヤー");
    hanaPlayerScript = this.gameObject.transform.GetComponent<HanaPlayerScript> ();
  }

  public void PickUpItem (Item itemData)
  {
    if (hanaPlayerScript.PlayerState != HanaPlayerScript.State.PickUpItem)
    {
      hanaPlayerScript.PlayerState = HanaPlayerScript.State.PickUpItem;
      pickUpItemManager.ShowPickUpItem (itemData);
    }
  }

}