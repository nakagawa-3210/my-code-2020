using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpItemManager : MonoBehaviour
{
  [SerializeField] CommonUiManager commonUiManager = default;
  [SerializeField] CommonUiGUIManager commonUiGUIManager = default;
  [SerializeField] PickUpItemGUIManager pickUpItemGUI = default;
  [SerializeField] SpawnManager itemSpawnManager = default;
  [SerializeField] HanaPlayerScript hanaPlayerScript = default;

  private CursorManager cursorManager;
  private PickUpItemMotionManager pickUpItemMotionManager;
  private PickUpItemContentsManager pickUpItemContentsManager;
  private Transform pickUpItemImage;
  private SpriteRenderer pickUpItemImageSpriteRenderer;

  void Start ()
  {
    pickUpItemImage = hanaPlayerScript.transform.Find ("Signs").Find ("PickUpItemSign");
    // Debug.Log ("pickUpItemImage : " + pickUpItemImage);

    cursorManager = new CursorManager (
      commonUiGUIManager.commonHandCursor,
      commonUiGUIManager.commonEventSystem
    );

    pickUpItemMotionManager = new PickUpItemMotionManager (
      pickUpItemGUI,
      commonUiManager,
      cursorManager,
      commonUiGUIManager.commonItemListContainer,
      pickUpItemImage
    );

    pickUpItemContentsManager = new PickUpItemContentsManager (
      cursorManager,
      commonUiGUIManager.commonItemListButtonPrefab.gameObject,
      commonUiGUIManager.commonNewItemListButtonPrefab.gameObject,
      commonUiGUIManager.commonItemDescriptionPanel,
      commonUiGUIManager.commonItemListContainer.transform
    );

    // pickUpItemImage.gameObject.SetActive (false);

  }

  void Update ()
  {
    cursorManager.MoveCursorTween ();
    pickUpItemContentsManager.SetupItemDescriptionText ();
  }

  public async UniTask ShowPickUpItem (Item itemData)
  {
    SEManager.Instance.Play (SEPath.GET_ITEM);

    MyGameData.MyData data = SaveSystem.Instance.userData;
    int currentHavingItemNum = data.havingItemsName.Length;
    int havingItemNumLimit = data.havingItemsNumRestriction;

    if (currentHavingItemNum < havingItemNumLimit)
    {
      ShowItem (itemData);
    }
    else
    {
      ShowItemButFull (itemData);
    }
  }

  async UniTask ShowItem (Item itemData)
  {
    pickUpItemMotionManager.ShowPickUpItemInformation (itemData);

    // セーブデータの書き込み
    string itemName = itemData.name;
    string[] newItemNamesArray = new MenuCommonFunctions ().GetNewStringItemNameArrayAddedNewItem (itemName);
    SaveNewHavingItemList (newItemNamesArray);

    await pickUpItemMotionManager.HideItemInformation ();
    hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
  }

  async UniTask ShowItemButFull (Item itemData)
  {
    pickUpItemMotionManager.ShowPickUpItemInformation (itemData);
    pickUpItemMotionManager.ShowPickUpItemCaution ();
    await pickUpItemMotionManager.HideItemInformation ();

    // リスト作成
    string itemName = itemData.name;
    await pickUpItemContentsManager.SetupItemList (itemName);
    // リスト表示
    await commonUiManager.ShowCommonUiForItemList ();
    pickUpItemMotionManager.SetupHandCursor ();

    // 捨てるアイテムを選んでセーブデータ更新
    await pickUpItemContentsManager.GetNewItemArrayRemovedRedundantItem ();
    string[] newItemNamesArray = pickUpItemContentsManager.NewItemArrayRemovedRedundantItem;
    SaveNewHavingItemList (newItemNamesArray);

    // リスト非表示
    await commonUiManager.HideCommonUiForItemList ();
    // リストの中身削除
    pickUpItemContentsManager.ResetItemListContents ();

    // 捨てたアイテム情報表示
    string discardedItemName = pickUpItemContentsManager.DiscardedItemName;
    await pickUpItemMotionManager.ShowDiscardedItemInformation (discardedItemName);

    // アイテム名確認まで待つ
    await UniTask.WaitUntil (() => Input.GetKeyDown (KeyCode.Space));
    Input.ResetInputAxes ();

    // 捨てたアイテム情報非表示
    await pickUpItemMotionManager.HideDiscardedItemInformation ();

    // 捨てたアイテムをマップに作成
    SpawnItem (discardedItemName);

    hanaPlayerScript.PlayerState = HanaPlayerScript.State.Normal;
  }

  void SpawnItem (string discardedItemName)
  {
    Vector3 spawnPosition = pickUpItemImage.transform.position;
    itemSpawnManager.SpawnItem (spawnPosition, discardedItemName);
  }

  void SaveNewHavingItemList (string[] newItemNamesArray)
  {
    SaveSystem.Instance.userData.havingItemsName = newItemNamesArray;
    // SaveSystem.Instance.Save ();
  }

}