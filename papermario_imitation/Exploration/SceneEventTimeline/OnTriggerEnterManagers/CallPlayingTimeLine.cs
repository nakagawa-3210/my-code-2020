using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CallPlayingTimeLine : MonoBehaviour
{
  [SerializeField] bool isDoorDirectionFrontSide;
  // 建物のドアが画面の奥側にある時
  [SerializeField] bool isDoorDirectionBackSide;
  // 建物のドアが画面の左側にある時
  [SerializeField] bool isDoorDirectionLeftSide;
  // 建物のドアが画面の右側にある時
  [SerializeField] bool isDoorDirectionRightSide;
  private CharacterPositionObserver playerPositionObserver;
  private HanaPlayerScript hanaPlayerScript;
  private PlayableDirector director;
  void Start ()
  {
    director = gameObject.GetComponent<PlayableDirector> ();
    playerPositionObserver = null;
  }

  void OnTriggerStay (Collider other)
  {
    // 条件式にプレイヤーの向いている方向を加える
    // プレイヤーオブジェクトにプレイヤーの向きを監視するcomponetが必要
    if (other.tag == "Player")
    {
      GameObject player = other.gameObject;
      playerPositionObserver = GetPlayerPositionManager (player);
      playerPositionObserver.SetCurrentRootPosition ();
      bool isFrontDirection = playerPositionObserver.GetCurrentIsFront ();
      bool isLeftDirection = playerPositionObserver.GetCurrentIsDirectionLeft ();
      hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      CallDirecorPlayBasedOnDoorCondition (isFrontDirection, isLeftDirection);
      playerPositionObserver.SetPreviousRootPosition ();
    }
  }

  CharacterPositionObserver GetPlayerPositionManager (GameObject player)
  {
    // 常に新しいものを作るとプレイヤーの位置が監視できないので
    // playerPositionObserverが未設定の場合のみオブザーバーをクラスより新規作成
    if (playerPositionObserver == null)
    {
      playerPositionObserver = new CharacterPositionObserver (player);
    }
    return playerPositionObserver;
  }

  void CallDirecorPlayBasedOnDoorCondition (bool isFrontDirection, bool isLeftDirection)
  {
    // isDoorDirectionFront等のUnity画面インスペクターで決めた値に応じて向いている向き条件を管理
    if (isDoorDirectionFrontSide)
    {
      if (isFrontDirection && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal && Input.GetKeyDown (KeyCode.Space))
      {
        director.Play ();
      }
    }
    // ドアが画面の奥側にある時
    else if (isDoorDirectionBackSide)
    {
      // isDoorDirectionFront等のUnity画面インスペクターで決めた値に応じて向いている向き条件を管理
      if (!isFrontDirection && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal && Input.GetKeyDown (KeyCode.Space))
      {
        director.Play ();
      }
    }
    // ドアが画面の左側にある時
    else if (isDoorDirectionLeftSide)
    {
      if (isLeftDirection && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal && Input.GetKeyDown (KeyCode.Space))
      {
        director.Play ();
      }
    }
    // ドアが画面の右側にある時
    else if (isDoorDirectionRightSide)
    {
      if (!isLeftDirection && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal && Input.GetKeyDown (KeyCode.Space))
      {
        director.Play ();
      }
    }
  }

}