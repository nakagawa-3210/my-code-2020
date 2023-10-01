using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HanaPlayerActions
{
  ConversationContentsManager conversationContentsManager;
  ConversationAnimationManager cam;
  private PlayableDirector noddingDirector;
  private PlayableDirector headShakingDirector;

  public HanaPlayerActions (
    ConversationContentsManager conversationContentsManager,
    ConversationAnimationManager cam = null)
  {
    this.conversationContentsManager = conversationContentsManager;
    this.cam = cam;
    if (cam.hanaPlayerNoddingTimeline != null)
    {
      noddingDirector = cam.hanaPlayerNoddingTimeline.GetComponent<PlayableDirector> ();
      headShakingDirector = cam.hanaPlayerShakingHeadTimeline.GetComponent<PlayableDirector> ();
    }
  }

  // アニメ再生検知
  void Director_Played (PlayableDirector obj)
  {
    conversationContentsManager.IsPlayingCharaAnimation = true;
  }
  // アニメ終了検知
  void Director_Stopped (PlayableDirector obj)
  {
    conversationContentsManager.IsPlayingCharaAnimation = false;
  }

  public void PlayNoddingAnim ()
  {
    noddingDirector.played += Director_Played;
    noddingDirector.stopped += Director_Stopped;
    cam.hanaPlayerNoddingTimeline.GetComponent<PlayableDirector> ().Play ();
  }

  public void PlayShakingHeadAnim ()
  {
    headShakingDirector.played += Director_Played;
    headShakingDirector.stopped += Director_Stopped;
    cam.hanaPlayerShakingHeadTimeline.GetComponent<PlayableDirector> ().Play ();
  }

}