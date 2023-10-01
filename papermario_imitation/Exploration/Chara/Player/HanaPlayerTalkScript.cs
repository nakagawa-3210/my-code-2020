using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HanaPlayerTalkScript : MonoBehaviour
{
  [SerializeField] GameConversationManager conversationManager;
  private HanaPlayerScript hanaPlayerScript;
  private GameObject conversationTarget;
  private string conversationFileName;
  private string shopItemName;
  private int shopItemPrice;

  public GameObject ConversationTarget
  {
    set { conversationTarget = value; }
    get { return conversationTarget; }
  }
  public string ConversationFileName
  {
    set { conversationFileName = value; }
  }
  public string ShopItemName
  {
    get { return shopItemName; }
  }
  public int ShopItemPrice
  {
    get { return shopItemPrice; }
  }

  void Start ()
  {
    conversationFileName = "";
    hanaPlayerScript = gameObject.GetComponent<HanaPlayerScript> ();
    // gameConversationManager = conversationCanvasController.GetComponent<GameConversationManager> ();
  }

  void Update ()
  {
    // プレイヤーのステータスの種類を増やす(TalkBuyingConfirmation)
    // アイテムショップ確認にてtargetのcomponentにShopItemInformationContainerが含まれているかを判断材料にする
    if (conversationTarget == null) return;
    if (Input.GetKeyDown (KeyCode.Space) && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
    {
      StartTalking ();
      conversationManager.OpenConversationCanvas (conversationFileName, this);
      // 会話ダイアログでのボタン押下のために会話開始に使用したボタン情報を初期化
      // Input.ResetInputAxes ();
    }
    // 会話キャンバスが終了しているかつStateがTalkの場合のみデフォルトで勝手に変更
    // Debug.Log ("conversationManager.EndConversation : " + conversationManager.EndConversation);
    if (conversationManager.EndConversation && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Talk)
    {
      // Debug.Log ("HanaPlayerScriptからnormalになったよ");
      hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
      EndTalking ();
    }

    // Debug.Log ("hanaPlayerScript.PlayerState : " + hanaPlayerScript.PlayerState);

  }

  // void LateUpdate ()
  // {
  //   // 会話相手がいる場合はTalkIconの位置を会話相手の頭上に表示
  //   if (conversationTarget != null)
  //   {
  //     // talkIcon.transform
  //     // Find関数を用いているので参考にしない
  //   }
  // }

  // 会話相手のリセット
  public void ResetConversationTarget (GameObject conversationTarget)
  {
    // Debug.Log ("会話相手のリセット");
    if (this.conversationTarget == null) return;
    // 現在の会話相手が、受け取った引数のインスタンスIDと同一のものを持っていれば会話相手を無しにする
    if (this.conversationTarget.GetInstanceID () == conversationTarget.GetInstanceID ())
    {
      // talkIcon.SetActive (false);
      // 会話相手のリセット
      this.conversationTarget = null;
    }
  }

  // 会話開始
  public void StartTalking ()
  {
    if (conversationTarget.GetComponent<VillagerScript> () != null)
    {
      var villagerScript = conversationTarget.GetComponent<VillagerScript> ();
      villagerScript.VillagerState = VillagerScript.State.Talk;
    }
    // 話し相手はあくまでもショップ店員なので、ショップのアイテムに話しかけるのも会話扱い
    if (conversationTarget.GetComponent<ShopItemConversationScript> () != null)
    {
      var shopItemConversationScript = conversationTarget.GetComponent<ShopItemConversationScript> ();
      shopItemConversationScript.ShopItemState = ShopItemConversationScript.State.Talk;

      // アイテムの価格と名前を受け取る
      ShopItemInfoContainer shopItemInfoContainer = conversationTarget.GetComponent<ShopItemInfoContainer> ();
      shopItemName = shopItemInfoContainer.ShopItemName;
      shopItemPrice = shopItemInfoContainer.ShopItemPrice;
    }
    gameObject.GetComponent<HanaPlayerScript> ().PlayerState = HanaPlayerScript.State.Talk;
  }

  public void EndTalking ()
  {
    // Debug.Log ("よんでるよー");
    if (conversationTarget.GetComponent<VillagerScript> () != null)
    {
      var villagerScript = conversationTarget.GetComponent<VillagerScript> ();
      villagerScript.VillagerState = VillagerScript.State.Wait;
    }
    if (conversationTarget.GetComponent<ShopItemConversationScript> () != null)
    {
      var shopItemConversationScript = conversationTarget.GetComponent<ShopItemConversationScript> ();
      // ショップアイテムインフォコンテナから名前、価格を受け取る
      shopItemConversationScript.ShopItemState = ShopItemConversationScript.State.Wait;
    }
    // gameObject.GetComponent<HanaPlayerScript> ().PlayerState = HanaPlayerScript.State.Normal;
    // hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
    ResetConversationTarget (conversationTarget);
    // 会話終了直後はどのボタンも押していなかったことにする
    // (じゃないと会話してない状態扱いでボタンが押されたと認識されて会話がまた始まる)
    // Input.ResetInputAxes ();
  }

}