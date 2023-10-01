using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class StageItem : StageObject
{
  private GameObject self;
  private Rigidbody selfRigidBody;
  private HanaPlayerScript hanaPlayerScript;
  private FallingManager fallingManager;

  void Start ()
  {
    self = this.gameObject;
    selfRigidBody = self.GetComponent<Rigidbody> ();

    hanaPlayerScript = GameObject.FindWithTag ("Player").GetComponent<HanaPlayerScript> ();

    fallingManager = new FallingManager (selfRigidBody);

    base.Drop (self);
    base.InactivateSelfSphereColliderMilSec(self);
  }

  void Update ()
  {
    fallingManager.ManageSelfGravityMotion (hanaPlayerScript.PlayerState);
  }

  public void ChangeSelfSprite (Sprite sprite)
  {
    // selfがnullになるのでthis.gameObjectで呼ぶ
    this.gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
  }

  public override void OnCollideGround ()
  {
    SEManager.Instance.Play (SEPath.BOUNCE_ITEM);
  }

  public override void OnCollisionEnter (Collision other)
  {
    base.OnCollisionEnter (other);

    if (other.collider.tag == "Player")
    {
      // Debug.Log ("アイテムゲット");
      string itemSpriteName = self.GetComponent<SpriteRenderer> ().sprite.name;
      Item itemData = new CommonUiFunctions ().GetItemInformationByUsingSpriteName (itemSpriteName);

      HanaPlayerItemScript hanaPlayerItemScript = other.gameObject.transform.GetComponent<HanaPlayerItemScript> ();
      hanaPlayerItemScript.PickUpItem (itemData);

      Destroy (self);
    }

  }
}