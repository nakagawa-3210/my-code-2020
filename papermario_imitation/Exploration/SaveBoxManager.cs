using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBoxManager : MonoBehaviour
{
  [SerializeField] GameConversationManager gameConversationManager;
  private float initialPlayerPositionY;
  void Start ()
  {
    initialPlayerPositionY = GameObject.FindWithTag ("Player").transform.position.y;
    // Debug.Log ("initialPlayerPositionY : " + initialPlayerPositionY);
  }

  // 確認不要なのでコメントアウト中
  // void Update ()
  // {
  //   // セーブデータ上のプレイヤーの位置確認テスト
  //   if (Input.GetKeyDown (KeyCode.Q))
  //   {
  //     // float test = 0.0476214885f;
  //     // int mul = 1000;
  //     // float beforeMathf = test * mul;
  //     // Debug.Log ("beforeMathf : " + beforeMathf);
  //     // float mathRound = Mathf.Round (beforeMathf);
  //     // Debug.Log ("mathRound : " + mathRound);
  //     // float modosu = mathRound / mul;
  //     // Debug.Log ("modosu : " + modosu);
  //     MyGameData.MyData userData = SaveSystem.Instance.userData;
  //     Debug.Log ("現在のuserData.savedPlayerPosition : " + userData.savedPlayerPosition);
  //     // Debug.Log ("セーブしたあとのuserData.savedPartnerPosition : " + userData.savedPartnerPosition);
  //   }
  // }

  void OnCollisionEnter (Collision other)
  {
    if (other.collider.tag == "Player")
    {
      HanaPlayerScript hanaPlayerScript = other.gameObject.GetComponent<HanaPlayerScript> ();
      hanaPlayerScript.PlayerState = HanaPlayerScript.State.Talk;

      HanaPlayerTalkScript hanaPlayerTalkScript = other.gameObject.GetComponent<HanaPlayerTalkScript> ();
      hanaPlayerTalkScript.ConversationTarget = this.gameObject;

      MyGameData.MyData userData = SaveSystem.Instance.userData;
      Vector3 playerPosition = GetRoundedPosition (other.gameObject.transform.position);
      userData.savedPlayerPosition = playerPosition;
      // Debug.Log ("playerPosition : " + playerPosition);

      GameObject partner = GameObject.FindWithTag ("Partner");
      Vector3 partnerPosition = GetRoundedPosition (partner.transform.position);
      userData.savedPartnerPosition = partnerPosition;
      // Debug.Log ("partnerPosition : " + partnerPosition);

      userData.savePointSceneName = SceneManager.GetActiveScene ().name;
      // Debug.Log ("userData.savePointSceneName : " + userData.savePointSceneName);

      string conversationFileName = "gameSave";
      gameConversationManager.OpenConversationCanvas (conversationFileName, hanaPlayerTalkScript);
    }
  }

  Vector3 GetRoundedPosition (Vector3 vector3BeforeRounded)
  {
    float GetRoundedFloat (float positionFloat)
    {
      float roundedFloat = 0.0f;
      int mul = 1000;
      float beforeRounded = positionFloat * mul;
      roundedFloat = Mathf.Round (beforeRounded) / mul;
      return roundedFloat;
    }

    float beforeMathfX = GetRoundedFloat (vector3BeforeRounded.x);
    float beforeMathfY = GetRoundedFloat (initialPlayerPositionY);
    float beforeMathfZ = GetRoundedFloat (vector3BeforeRounded.z);

    Vector3 roundedVector3 = new Vector3 (beforeMathfX, beforeMathfY, beforeMathfZ);

    return roundedVector3;
  }

}