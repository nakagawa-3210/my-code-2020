using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsecutiveChangeSelfImage : MonoBehaviour
{
  [SerializeField] List<Sprite> spriteList;
  [SerializeField] float changeTime;
  [SerializeField] bool wantStartChangeSprite; // 作られた際に絶対に変化するように仕様を変更するかもしれない
  private GameObject self;
  private float timeElapsed;
  private int spriteNum;
  private bool startChangeSprite;

  // 他クラスから操作して変色のON,OFF
  public bool StartChangeSprite
  {
    set { startChangeSprite = value; }
  }
  void Start ()
  {
    self = this.gameObject;
    spriteNum = 0;
    
    // 作られた際に絶対に変化するように仕様を変更するかもしれない
    if (wantStartChangeSprite)
    {
      startChangeSprite = true;
    }
    else
    {
      startChangeSprite = false;
    }
  }

  private void OnValidate ()
  {

  }

  void Update ()
  {
    // test用
    if (Input.GetKeyDown (KeyCode.S))
    {
      startChangeSprite = true;
    }

    if (startChangeSprite)
    {
      timeElapsed += Time.deltaTime;
      if (timeElapsed >= changeTime)
      {
        ChangeSprite ();
        timeElapsed = 0.0f;
      }
    }
  }

  void ChangeSprite ()
  {
    int random = Random.Range (0, spriteList.Count);
    if (self.GetComponent<SpriteRenderer> () != null)
    {
      self.GetComponent<SpriteRenderer> ().sprite = spriteList[random];
    }
    else if (self.GetComponent<Image> () != null)
    {
      self.GetComponent<Image> ().sprite = spriteList[random];
    }
  }

}