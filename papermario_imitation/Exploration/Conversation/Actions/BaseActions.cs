using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActions
{
  private ConversationContentsManager conversationContentsManager;
  private HanaPlayerTalkScript hanaPlayerTalkScript;
  private SceneTransitionManager sceneTransitionManager;

  public BaseActions (
    ConversationContentsManager conversationContentsManager,
    HanaPlayerTalkScript hanaPlayerTalkScript = null
  )
  {
    this.conversationContentsManager = conversationContentsManager;
    this.hanaPlayerTalkScript = hanaPlayerTalkScript;
    if (GameObject.Find ("SceneTransitionManager") == null) return;
    sceneTransitionManager = GameObject.Find ("SceneTransitionManager").GetComponent<SceneTransitionManager> ();
  }

  public void ChangeConversationStateToSelectingYesNo ()
  {
    conversationContentsManager.GameConversationState =
      ConversationContentsManager.ConversationState.selectingYesNo;
  }

  public void SaveGameData ()
  {
    // Debug.Log ("havingCoin : " + SaveSystem.Instance.userData.havingCoin);
    SaveSystem.Instance.Save ();
    sceneTransitionManager.GoToTitleScene ();
    Debug.Log ("現在のuserData.savedPlayerPosition : " + SaveSystem.Instance.userData.savedPlayerPosition);
  }

  public void EndTalking ()
  {
    // 会話終了のフラグを立てる
    // Debug.Log ("hanaPlayerTalkNULL : " + hanaPlayerTalkScript == null);
    conversationContentsManager.EndConversation = true;
    // if (hanaPlayerTalkScript != null)
    // {
    //   Debug.Log ("EndTalkingよんでるよー");
    //   hanaPlayerTalkScript.EndTalking ();
    // }
    // Debug.Log ("会話が終了したよ！");
  }

}