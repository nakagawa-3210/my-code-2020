using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class BossDefeatedMotion : MonoBehaviour
{
  private GameObject self;
  void Start ()
  {
    self = this.gameObject;
  }

  void Update ()
  {

  }

  public async UniTask PlayDefeatedBossMotion ()
  {
    // ボスごとに分岐
    if (self.GetComponent<DefeatedTurtleMotion> () != null)
    {
      await self.GetComponent<DefeatedTurtleMotion> ().PlayDefeatedTurtle ();
    }
  }
}