using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBillBoard : MonoBehaviour
{
  private GameObject self;
  void Start ()
  {
    self = this.gameObject;
    }
  void Update ()
  {
    Vector3 mainCameraPosition = Camera.main.transform.position;
    mainCameraPosition.y = self.transform.position.y;
    self.transform.LookAt (mainCameraPosition);
  }

}