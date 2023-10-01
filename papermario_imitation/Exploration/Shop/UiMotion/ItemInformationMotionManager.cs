using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformationMotionManager
{
  private ScaleWithRotationFunctions scaleWithRotationFunctions;
  private GameObject itemNamePlate;
  private GameObject itemDescriptionPlate;
  private GameObject itemNameText;
  private GameObject itemDescriptionText;
  private Vector3 recoveringTargetScrollViewShowingScale;
  private Vector3 recoveringTargetScrollViewHidingScale;
  private Vector3 recoveringTargetScrollViewShowingDegree;
  private Vector3 recoveringTargetScrollViewHidingDegree;
  public ItemInformationMotionManager (
    GameObject itemNamePlate,
    GameObject itemDescriptionPlate
  )
  {
    this.itemNamePlate = itemNamePlate;
    this.itemDescriptionPlate = itemDescriptionPlate;
    itemNameText = this.itemNamePlate.transform.Find ("ItemNameInfoText").gameObject;
    itemDescriptionText = this.itemDescriptionPlate.transform.Find ("ItemDescriptionInfoText").gameObject;
    this.itemNamePlate.SetActive (false);
    this.itemDescriptionPlate.SetActive (false);
    itemNameText.SetActive (false);
    itemDescriptionText.SetActive (false);
    scaleWithRotationFunctions = new ScaleWithRotationFunctions ();
    HidingItemInfoTween ();
  }

  public async void OpenItemInformation ()
  {
    itemNamePlate.SetActive (true);
    itemDescriptionPlate.SetActive (true);
    await ShowingItemInfoTween ();
    // 表示の時はパネルがアニメーションで表示されたあとに文字が表示される
    itemNameText.SetActive (true);
    itemDescriptionText.SetActive (true);
  }

  public async void CloseItemInformation ()
  {
    // 非表示は文字が非表示にされたあとにパネルがアニメーションで非表示にされる
    itemNameText.SetActive (false);
    itemDescriptionText.SetActive (false);
    await HidingItemInfoTween ();
    itemNamePlate.SetActive (false);
    itemDescriptionPlate.SetActive (false);
  }

  async UniTask ShowingItemInfoTween ()
  {
    scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (itemNamePlate);
    await scaleWithRotationFunctions.ShowingTweenUsingScaleAndRotate (itemDescriptionPlate);
  }

  async UniTask HidingItemInfoTween ()
  {
    scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (itemNamePlate);
    await scaleWithRotationFunctions.HidingTweenUsingScaleAndRotate (itemDescriptionPlate);
  }
}