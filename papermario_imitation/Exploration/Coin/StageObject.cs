using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class StageObject : MonoBehaviour
{
  public void Drop (GameObject self)
  {
    Rigidbody rigidbody = self.GetComponent<Rigidbody> ();
    float jumpPower = 10.0f;
    new DropMotion ().Drop (rigidbody, jumpPower);
  }

  public async UniTask InactivateSelfSphereColliderMilSec (GameObject self, float milSecDuration = 0.2f)
  {
    SphereCollider sphereCollider = self.GetComponent<SphereCollider> ();
    sphereCollider.enabled = false;
    int milSec = (int) (milSecDuration * 1000);
    await UniTask.Delay (milSec);
    sphereCollider.enabled = true;
  }

  public virtual void OnCollisionEnter (Collision other)
  {
    string tagName = other.collider.tag;
    if (tagName == "Ground")
    {
      OnCollideGround ();
    }
    // else if (tagName == "Player")
    // {
    //   OnCollidePlayer ();
    // }
  }

  public virtual void OnCollideGround ()
  {
    // 継承先で決める
  }

  // 継承先で親が受け取ったotherを用いる方法がわからないのでコメントアウト中
  // public virtual void OnCollidePlayer ()
  // {
  //   // 継承先で決める
  // }
}