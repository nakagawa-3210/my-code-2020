using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class HanaPlayerCoinScript : MonoBehaviour
{
  [SerializeField] PickUpCoinManager pickUpCoinManager;
  private GameObject self;
  void Start ()
  {
    self = this.gameObject;
  }

  public void PickUpCoin ()
  {
    SEManager.Instance.Play (SEPath.GET_COIN, 0.5f);
    pickUpCoinManager.SaveGetCoinNum ();
  }
}