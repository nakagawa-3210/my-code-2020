using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemConversationScript : MonoBehaviour
{
  [SerializeField] string talkTextFileName;
  public enum State
  {
    Wait,
    Talk
  }
  private State state;
  private GameObject talkingTarget = null;

  // VillagerScriptと共通の親としてBaseTargetConversationScriptクラスつくる
  // public void SetTalkingTarget (GameObject talkingTarget)
  // {
  //   this.talkingTarget = talkingTarget;
  // }

  public void ResetTalkingTarget ()
  {
    this.talkingTarget = null;
  }

  public State ShopItemState
  {
    get { return this.state; }
    set { this.state = value; }
  }

  public GameObject TalkingTarget
  {
    set { talkingTarget = value; }
  }

  public string TalkTextFileName
  {
    get { return talkTextFileName; }
  }
}