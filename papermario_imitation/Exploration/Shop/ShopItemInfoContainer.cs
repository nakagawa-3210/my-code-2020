using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemInfoContainer : MonoBehaviour
{
  private bool isPlayerEnterering;
  private string shopItemName;
  private int shopItemPrice;
  private string shopItemDescription;
  void Start ()
  {
    isPlayerEnterering = false;
    Item itemInformation = GetItemInformationByUsingSpriteName ();
    shopItemName = itemInformation.name;
    shopItemPrice = itemInformation.playersBuyingPrice;
    shopItemDescription = itemInformation.description;
  }

  Item GetItemInformationByUsingSpriteName ()
  {
    Item itemInformation = null;
    GameObject itemImg = gameObject.transform.Find ("ItemImg").gameObject;
    string itemImgFileName = itemImg.GetComponent<SpriteRenderer> ().sprite.name;
    itemInformation = new CommonUiFunctions ().GetItemInformationByUsingSpriteName (itemImgFileName);
    return itemInformation;
  }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      isPlayerEnterering = true;
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      isPlayerEnterering = false;
    }
  }

  public bool IsPlayerEnterering
  {
    get { return isPlayerEnterering; }
  }
  public string ShopItemName
  {
    set { shopItemName = value; }
    get { return shopItemName; }
  }
  public int ShopItemPrice
  {
    set { shopItemPrice = value; }
    get { return shopItemPrice; }
  }
  public string ShopItemDescription
  {
    set { shopItemDescription = value; }
    get { return shopItemDescription; }
  }

}