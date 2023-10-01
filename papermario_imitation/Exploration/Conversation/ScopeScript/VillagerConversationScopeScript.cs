using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerConversationScopeScript : MonoBehaviour
{
  [SerializeField] GameObject selfSpeechBubble;
  private GameObject villager;
  private VillagerScript villagerScript;
  private CharacterPositionObserver playerPositionObserver;
  void Start ()
  {
    villager = transform.parent.gameObject;
    villagerScript = villager.GetComponent<VillagerScript> ();

    selfSpeechBubble.SetActive (false);
  }

  // 会話可能な状態を示すUIを表示管理する
  // hoge.SetActive(true)とか
  void OnTriggerStay (Collider other)
  {
    // プレイヤーかつ、プレイヤーが会話中でないとき
    if (other.tag == "Player")
    {
      GameObject player = other.gameObject;
      HanaPlayerScript hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      HanaPlayerTalkScript hanaPlayerTalkScript = player.GetComponent<HanaPlayerTalkScript> ();
      bool isPlayerLookingTarget = GetIsPlayerLookingVillager (player);

      if (isPlayerLookingTarget)
      {
        if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Talk)
        {
          selfSpeechBubble.SetActive (false);
        }
        else
        {
          // 村人の会話ステータス設定
          villagerScript.TalkingTarget = player;
          // プレイヤーの会話ステータス設定
          hanaPlayerTalkScript.ConversationTarget = villager;
          hanaPlayerTalkScript.ConversationFileName = villagerScript.TalkTextFileName;
          selfSpeechBubble.SetActive (true);
        }
      }
      else
      {
        villagerScript.ResetTalkingTarget ();
        hanaPlayerTalkScript.ResetConversationTarget (villager);
        selfSpeechBubble.SetActive (false);
      }
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player" && other.GetComponent<HanaPlayerScript> ().PlayerState != HanaPlayerScript.State.Talk)
    {
      GameObject player = other.gameObject;
      player.GetComponent<HanaPlayerTalkScript> ().ResetConversationTarget (villager);
      selfSpeechBubble.SetActive (false);
    }
  }

  bool GetIsPlayerLookingVillager (GameObject player)
  {
    // プレイヤーが会話相手よりも右側にいるときにはプレイヤーはisDirectionleftがtrue、
    // 左側にいるときはfalseでなければならない(会話相手のいる方向を向いていないとダメ)
    playerPositionObserver = GetPlayerPositionManager (player);
    // プレイヤーの位置情報更新
    playerPositionObserver.SetCurrentRootPosition ();
    bool isLookingLeft = playerPositionObserver.GetCurrentIsDirectionLeft ();
    bool isPlayerLookingTarget = false;
    float playerPositionX = player.transform.position.x;
    float villagerPositionX = villager.transform.position.x;
    // プレイヤーが村人(会話相手)の方を向いているかの確認 x軸の値を比較する
    // x軸の値が大きいほど画面の右側にいる
    // 条件に位置の前後関係も追加するかもしれない
    if ((playerPositionX > villagerPositionX && isLookingLeft) ||
      (playerPositionX < villagerPositionX && !isLookingLeft))
    {
      isPlayerLookingTarget = true;
    }
    // プレイヤーの位置キャッシュ更新
    playerPositionObserver.SetPreviousRootPosition ();
    return isPlayerLookingTarget;
  }

  CharacterPositionObserver GetPlayerPositionManager (GameObject player)
  {
    if (playerPositionObserver == null)
    {
      playerPositionObserver = new CharacterPositionObserver (player);
    }
    return playerPositionObserver;
  }
}