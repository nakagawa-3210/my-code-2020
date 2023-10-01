using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasMotionManager : MonoBehaviour
{
  [SerializeField] GameObject shopInformationGui;
  private HanaPlayerScript hanaPlayerScript;
  private HanaPlayerTalkScript hanaPlayerTalkScript;
  private ShowingPriceManager showingPriceManagerScript;
  private ItemInformationMotionManager itemInformationMotionManager;
  private PlayerCoinInformationMotionManager playerCoinInformationMotionManager;
  private HanaPlayerScript.State previousPlayerState;
  private ShopInformationGUI shopInformationGuiScript;
  private bool isItemInfoOpened;
  private bool endSetup;

  void Start ()
  {
    shopInformationGuiScript = shopInformationGui.GetComponent<ShopInformationGUI> ();
    hanaPlayerScript = shopInformationGuiScript.player.GetComponent<HanaPlayerScript> ();
    hanaPlayerTalkScript = shopInformationGuiScript.player.GetComponent<HanaPlayerTalkScript> ();

    showingPriceManagerScript = shopInformationGuiScript.showingPriceManager.GetComponent<ShowingPriceManager> ();
    itemInformationMotionManager = new ItemInformationMotionManager (
      shopInformationGuiScript.itemNamePanel,
      shopInformationGuiScript.itemDescriptionPanel
    );
    playerCoinInformationMotionManager = new PlayerCoinInformationMotionManager (
      shopInformationGuiScript.playerHavingCoinPanel
    );
    isItemInfoOpened = false;
    previousPlayerState = HanaPlayerScript.State.Normal;
  }

  void Update ()
  {
    if (hanaPlayerTalkScript.ConversationTarget == null)
    {
      if (endSetup)
      {
        endSetup = false;
      }
    }
    else
    {
      GameObject conversationPartner = hanaPlayerTalkScript.ConversationTarget;
      // 会話のターゲットが商品だったときのみ
      if (conversationPartner.GetComponent<ShopItemInfoContainer> () != null)
      {
        if (!endSetup)
        {
          endSetup = true;
          playerCoinInformationMotionManager.Setup ();
        }
        ManageItemInfoOpenCloseByColliderTrigger ();
        // 商品の購入確認の会話中のみ商品の情報は非表示にし、会話終了時に商品情報を再表示する
        ManageItemInfoOpenCloseByTalking ();
        // メニューを開いた際には情報を非表示にする
        ManageItemInfoOpenCloseByPlayerStatus ();
        // 現在のプレイヤーステータス更新
        CashCurrentPlayerState ();
        // やっぱりcontentsManagerで管理するように変更する予定
        // コイン減少(テキストの内容を変更しているけど、数字を減らす動きを担っているのでこのクラスで管理)
        playerCoinInformationMotionManager.DecreaseCoinNumTextMotion ();
      }
    }
  }

  void CashCurrentPlayerState ()
  {
    HanaPlayerScript.State currentPlayerState = hanaPlayerScript.PlayerState;
    previousPlayerState = currentPlayerState;
  }

  void ManageItemInfoOpenCloseByColliderTrigger ()
  {
    if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
    {
      bool isEnteringPlayer = showingPriceManagerScript.IsEnteringPlayer;
      // Debug.Log ("isEnteringPlayer : " + isEnteringPlayer);
      // Debug.Log ("isItemInfoOpened : " + isItemInfoOpened);
      if (isEnteringPlayer && !isItemInfoOpened)
      {
        isItemInfoOpened = true;
        itemInformationMotionManager.OpenItemInformation ();
      }
      else if (!isEnteringPlayer && isItemInfoOpened)
      {
        isItemInfoOpened = false;
        itemInformationMotionManager.CloseItemInformation ();
      }
    }
  }

  void ManageItemInfoOpenCloseByTalking ()
  {
    HanaPlayerScript.State currentPlayerState = hanaPlayerScript.PlayerState;
    if (previousPlayerState == currentPlayerState) return;
    if (previousPlayerState == HanaPlayerScript.State.Normal && currentPlayerState == HanaPlayerScript.State.Talk)
    {
      isItemInfoOpened = false;
      itemInformationMotionManager.CloseItemInformation ();
      playerCoinInformationMotionManager.ShowPlayerHavingCoinPlate ();
    }
    else if (previousPlayerState == HanaPlayerScript.State.Talk && currentPlayerState == HanaPlayerScript.State.Normal)
    {
      isItemInfoOpened = true;
      itemInformationMotionManager.OpenItemInformation ();
      playerCoinInformationMotionManager.HidePlayerHavingCoinPlate ();
    }
  }

  void ManageItemInfoOpenCloseByPlayerStatus ()
  {
    HanaPlayerScript.State currentPlayerState = hanaPlayerScript.PlayerState;
    if (previousPlayerState == currentPlayerState) return;
    // 商品情報が表示されているべき状態であるときに、メニューが開かれた時の処理
    if (isItemInfoOpened)
    {
      if (previousPlayerState == HanaPlayerScript.State.Normal && currentPlayerState == HanaPlayerScript.State.OpenMenu)
      {
        itemInformationMotionManager.CloseItemInformation ();
      }
      else if (previousPlayerState == HanaPlayerScript.State.OpenMenu && currentPlayerState == HanaPlayerScript.State.Normal)
      {
        itemInformationMotionManager.OpenItemInformation ();
      }
    }
  }
}