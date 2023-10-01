using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
public class TargetListOpenCloseManager
{
  private MenuCommonFunctions menuCommonFunctions;
  private ScaleWithRotationFunctions scaleWithRotationFunctions;
  private GameObject whoPlate;
  private GameObject recoveringTargetScrollView;
  private Vector3 whoPlateInitialSize;
  // private GameObject itemListContainer;
  private List<GameObject> itemListViewButtonsList;
  private List<GameObject> recoveringTargetListViewButtonsList;
  private bool isTweening;
  public TargetListOpenCloseManager (
    GameObject whoPlate,
    GameObject recoveringTargetScrollView
  )
  {
    this.whoPlate = whoPlate;
    // this.itemListContainer = itemListContainer;
    this.whoPlate.gameObject.SetActive (false);
    itemListViewButtonsList = new List<GameObject> ();
    recoveringTargetListViewButtonsList = new List<GameObject> ();
    this.recoveringTargetScrollView = recoveringTargetScrollView;
    this.recoveringTargetScrollView.SetActive (false);
    // InitRecoveringTargeListView ();
    menuCommonFunctions = new MenuCommonFunctions ();
    scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    isTweening = false;
    InitRecoveringTargeListView ();
  }

  // 回転と大きさ変更の処理はアイテムショップインフォマネージャの関数と共通化するかも
  void InitRecoveringTargeListView ()
  {
    HidingRecoveringTargetScrollViewTween ();
  }
  // MenuCanvasEnableButtonsManagerクラスと同じ関数を使用している
  // CanvasManagerをひとまとめにしてメニューの中身作成後の処理として書かないと改修できない
  // 機能自体が完成次第、contentsManagerとmotionManagerを管理するクラスを作成する

  // 最新のitemListContainerを取得する
  // public bool IsAnyItemButtonSelected (GameObject itemListContainer)
  // {
  //   bool isSelected = false;
  //   // itemListContainerから子要素を全取得
  //   itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
  //   if (itemListViewButtonsList.Count == 0)
  //   {
  //     // Debug.Log ("ボタンが用意出来てない、またはアイテムがないよ");
  //     return isSelected;
  //   }
  //   foreach (var itemListViewButton in itemListViewButtonsList)
  //   {
  //     if (itemListViewButton == null) return isSelected;
  //     BelongingButtonInfoContainer buttonInfoContainer = itemListViewButton.GetComponent<BelongingButtonInfoContainer> ();
  //     // 選ばれたかつ、選べるボタンだった場合
  //     if (buttonInfoContainer.IsSelected && buttonInfoContainer.IsSelectable)
  //     {
  //       // Debug.Log ("アイテムは選ばれたよ");
  //       isSelected = true;
  //       return isSelected;
  //     }
  //     else
  //     {
  //       // Debug.Log ("アイテムはえらばれてないよ");
  //       isSelected = false;
  //     }
  //   }
  //   // 子要素であるボタンのどれかが選ばれていたら、isSelected = true;
  //   return isSelected;
  // }

  public void ManageTargetCanSelectColor (GameObject itemListContainer, GameObject targetListContainer)
  {
    itemListViewButtonsList = menuCommonFunctions.GetChildList (itemListContainer);
    recoveringTargetListViewButtonsList = menuCommonFunctions.GetChildList (targetListContainer);
    if (itemListViewButtonsList.Count == 0) return;
    int selectedItemButtonNum = menuCommonFunctions.GetSelectedItemButtonNum (itemListViewButtonsList);
    GameObject selectedItemButton = itemListViewButtonsList[selectedItemButtonNum];
    BelongingButtonInfoContainer buttonInfoContainer = selectedItemButton.GetComponent<BelongingButtonInfoContainer> ();
    string itemType = buttonInfoContainer.Type;
    foreach (var target in recoveringTargetListViewButtonsList)
    {
      BaseMenuListPreparer baseMenuListPreparer = new BaseMenuListPreparer ();
      TargetButtonInfoContainer targetButtonInfoContainer = target.GetComponent<TargetButtonInfoContainer> ();
      Text targetName = target.transform.Find ("NameText").GetComponent<Text> ();
      Image targetImg = target.transform.Find ("CharaImg").GetComponent<Image> ();
      bool isSelectableHp = targetButtonInfoContainer.IsSelectableHp;
      bool isSelectableFp = targetButtonInfoContainer.IsSelectableFp;
      // Debug.Log ("isSelectableFp : " + isSelectableFp);
      // 選んだアイテムを使って回復出来ない場合
      if ((itemType == BelongingButtonInfoContainer.State.recoverHp.ToString () && !isSelectableHp) ||
        (itemType == BelongingButtonInfoContainer.State.recoverFp.ToString () && !isSelectableFp))
      {
        baseMenuListPreparer.SetCanNotSelectColor (targetName, targetImg);
      }
      // 出来る場合
      else if ((itemType == BelongingButtonInfoContainer.State.recoverHp.ToString () && isSelectableHp) ||
        (itemType == BelongingButtonInfoContainer.State.recoverFp.ToString () && isSelectableFp))
      {
        // Debug.Log ("つかえるいろだよ");
        baseMenuListPreparer.SetCanSelectColor (targetName, targetImg);
      }
    }
  }

  public async UniTask OpenRecoveringTargetList ()
  {
    isTweening = true;
    whoPlate.gameObject.SetActive (true);
    recoveringTargetScrollView.SetActive (true);
    await ShowingRecoveringTargetScrollViewTween ();
    isTweening = false;
  }
  public async UniTask CloseRecoveringTargetList ()
  {
    // 先にリストの中身を隠してからtweenさせるほうがいいかも？
    isTweening = true;
    await HidingRecoveringTargetScrollViewTween ();
    whoPlate.gameObject.SetActive (false);
    recoveringTargetScrollView.SetActive (false);
    isTweening = false;
  }

  // 時計回りに回転しながら拡大される動きで出現
  // ダレに使う？のメッセージはtween無し。ぱっと出てぱっと消える
  async UniTask ShowingRecoveringTargetScrollViewTween ()
  {
    await scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (recoveringTargetScrollView);
  }
  async UniTask HidingRecoveringTargetScrollViewTween ()
  {
    await scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (recoveringTargetScrollView);
  }

  public bool IsTweening ()
  {
    return isTweening;
  }

  public bool IsTargetListShown ()
  {
    return whoPlate.activeSelf || recoveringTargetScrollView.activeSelf;
  }

}