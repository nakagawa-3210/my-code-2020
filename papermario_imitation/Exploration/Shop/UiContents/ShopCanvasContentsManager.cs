using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasContentsManager : MonoBehaviour
{
  [SerializeField] GameObject shopInformationGui;
  private ItemInformationContentsManager itemInformationContentsManager;
  private PlayerCoinInformationContentsManager playerCoinInformationContentsManager;
  private ShopInformationGUI shopInformationGuiScript;
  // private 
  void Start ()
  {
    shopInformationGuiScript = shopInformationGui.GetComponent<ShopInformationGUI> ();
    itemInformationContentsManager = new ItemInformationContentsManager (
      shopInformationGuiScript.shopItemGroup,
      shopInformationGuiScript.itemNameInfoText,
      shopInformationGuiScript.itemDescriptionInfoText
    );
    // テキストの初期設定とデータの変更等があった場合はこちらで行う
    playerCoinInformationContentsManager = new PlayerCoinInformationContentsManager (
      shopInformationGuiScript.playerHavingCoinText
    );
  }

  void Update ()
  {
    itemInformationContentsManager.SetItemInformation ();
  }

}