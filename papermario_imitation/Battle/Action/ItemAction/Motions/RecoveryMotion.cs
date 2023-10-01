using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using TMPro;
using UniRx.Async;
using UnityEngine;

// 探索シーン内でも使用できるようにしたい
public class RecoveryMotion : MonoBehaviour
{
  [SerializeField] GameObject recoveryParticle;
  [SerializeField] GameObject recoveryItemAmountSign;
  [SerializeField] GameObject recoveringItemGroup;
  [SerializeField] GameObject smallHeart;
  [SerializeField] GameObject smallFlower;
  [SerializeField] Sprite recoveryHeartFrameSprite;
  [SerializeField] Sprite recoveryFlowerFrameSprite;
  private CommonItemMotion commonItemMotion;
  private GameObject self;
  private GameObject targetRecoveryParticle;
  private GameObject targetRecoveringItemGroup;
  private GameObject itemImage;
  private GameObject itemFocus;
  private CircumferenceArrangement targetCircumferenceArrangement;
  private ParticleSystem recoveryParticleSystem;

  void Start ()
  {
    commonItemMotion = new CommonItemMotion ();
    self = this.gameObject;
    GameObject battleItemUsingUiCanvas = GameObject.Find ("BattleItemUsingUiCanvas");
    itemImage = battleItemUsingUiCanvas.transform.Find ("ItemImage").gameObject;
    itemFocus = battleItemUsingUiCanvas.transform.Find ("ItemFocusGroup").gameObject;
    itemImage.SetActive (false);
  }

  // 回復モーションの速度を変更できるように改修予定
  public async UniTask RecoveringCompletely (GameObject recoverTarget, int recoveringHpAmount, int recoveringFpAmount = 0, bool hasFp = true)
  {
    // HP回復モーション再生(はやめ)
    SetupRecoveringGroup (recoverTarget, smallHeart, recoveringHpAmount);
    string hpTypeName = "recoverHp";
    await StartRecoveringTweenMotion (recoverTarget, hpTypeName, recoveringHpAmount);
    // FP回復モーション再生(はやめ)
    if (hasFp)
    {
      SetupRecoveringGroup (recoverTarget, smallFlower, recoveringFpAmount);
      string fpTypeName = "recoverFp";
      await StartRecoveringTweenMotion (recoverTarget, fpTypeName, recoveringFpAmount);
    }
  }

  public async UniTask ShowUsingItem (GameObject recoverTarget, BelongingButtonInfoContainer itemInformation)
  {
    // 使用回復アイテム画像表示
    string itemName = itemInformation.BelongingName;
    Sprite usingItemSprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
    await commonItemMotion.ShowingUsingItemImage (self, itemImage, itemFocus, usingItemSprite);
    // 回復キラキラ用意
    SetupRecoveryParticle (recoverTarget);
    // HeartGroup用意
    int recoveringAmount = itemInformation.Amount;
    if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverHp.ToString ())
    {
      SetupRecoveringGroup (recoverTarget, smallHeart, recoveringAmount);
    }
    else if (itemInformation.Type == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      SetupRecoveringGroup (recoverTarget, smallFlower, recoveringAmount);
    }
    // 回復音再生
    SEManager.Instance.Play (SEPath.RECOVERING);
    // キラキラ表示
    recoveryParticleSystem.Play ();
    await RotatingParticle ();
    // ハート、フラワー表示による回復の様子表現
    string itemType = itemInformation.Type;
    await StartRecoveringTweenMotion (recoverTarget, itemType, recoveringAmount);
    int littleDelay = 500;
    await UniTask.Delay (littleDelay);
  }

  void SetupRecoveryParticle (GameObject recoverTarget)
  {
    targetRecoveryParticle = GameObject.Instantiate<GameObject> (recoveryParticle);
    float modifier = -0.4f;
    CommonSetup (recoverTarget, targetRecoveryParticle, modifier);
    recoveryParticleSystem = targetRecoveryParticle.GetComponent<ParticleSystem> ();
  }

  void SetupRecoveringGroup (GameObject recoverTarget, GameObject recoveringItem, int recoveringAmount)
  {
    targetRecoveringItemGroup = GameObject.Instantiate<GameObject> (recoveringItemGroup);
    float modifier = 0.3f;
    CommonSetup (recoverTarget, targetRecoveringItemGroup, modifier);

    for (var i = 0; i < recoveringAmount; i++)
    {
      GameObject recoveringSmallItem = GameObject.Instantiate<GameObject> (recoveringItem);
      float smallItemModifier = 0.0f;
      float baseScale = 0.3f;
      CommonSetup (targetRecoveringItemGroup, recoveringSmallItem, smallItemModifier, baseScale);
      recoveringSmallItem.transform.SetParent (targetRecoveringItemGroup.transform);
      // SetActiveでGameObjectを変えると上手くいかないのでSpriteRendererを触る
      recoveringSmallItem.GetComponent<SpriteRenderer> ().enabled = false;
    }
    targetCircumferenceArrangement = targetRecoveringItemGroup.GetComponent<CircumferenceArrangement> ();
  }

  void CommonSetup (GameObject parent, GameObject setupObject, float modifier = 0.0f, float baseScale = 1.0f)
  {
    setupObject.transform.SetParent (parent.transform);
    Vector3 setupObjectPosition = parent.transform.position;
    setupObjectPosition.y += modifier;
    setupObject.transform.position = setupObjectPosition;
    setupObject.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
  }

  // Easeで調整が必要
  async UniTask StartRecoveringTweenMotion (GameObject recoverTarget, string itemType, int recoveringAmount)
  {
    // 回復表示のハートをインスタンス化する
    Transform targetRecoveryItem = GameObject.Instantiate<GameObject> (recoveryItemAmountSign).transform;
    targetRecoveryItem.SetParent (recoverTarget.transform);

    // 移動
    Transform recoveryItem = targetRecoveryItem.transform.Find ("RecoveryItem");

    // 左右の揺れ
    Transform itemImgFrameRoot = recoveryItem.Find ("ItemImgFrameRoot");

    // ピボットが中心の伸縮
    Transform itemImgFrame = itemImgFrameRoot.Find ("ItemImgFrame");

    // 回復量テキスト更新
    SetRecoveringHpAmountText (itemImgFrame, itemType, recoveringAmount);

    // 表示位置のキャッシュ
    float recoveryItemInitialPositionY = recoveryItem.position.y;

    // RecoveryHeartTween要素セットアップ
    SetupRecoveringHeart (recoverTarget.transform, targetRecoveryItem, recoveryItem, itemImgFrameRoot, itemImgFrame);
    float baseScale = 1.0f;

    // 登場
    float showingDuration = 0.25f;
    float showingScale = 1.0f;
    await DOTween.Sequence ()
      .Append (recoveryItem.DOMoveY (recoveryItemInitialPositionY, showingDuration))
      .Join (recoveryItem.DOScale (new Vector3 (showingScale, showingScale, showingScale), showingDuration));

    // 回復対象を囲むように回復量と同じ数だけの小さいハートを表示
    ActivateAllSmallItems ();

    // 小さいアイテムの円拡大
    ExpandSmallItems ();

    // ピボット中心の着地による伸び縮み
    float centerShrinkScale = 0.6f;
    float centerShrinkDuration = 0.1f;
    await DOTween.Sequence ().Append (itemImgFrame.DOScaleY (centerShrinkScale, centerShrinkDuration))
      .Append (itemImgFrame.DOScaleY (baseScale, centerShrinkDuration));

    // 小さいハートの回転開始
    RotatingTargetRecoveringSmallItemGroup ();

    // 左右の揺れ, ピボットが足元の縮み
    float swing = 18.0f;
    float swingDuration = 0.3f;
    Vector3 noSwing = itemImgFrameRoot.localEulerAngles;
    Vector3 swingRight = itemImgFrameRoot.localEulerAngles;
    swingRight.z += swing;
    Vector3 swingLeft = itemImgFrameRoot.localEulerAngles;
    swingLeft.z -= swing;
    float bottomShrinkScale = 0.5f;
    float bottomDuration = 0.3f;
    await DOTween.Sequence ().Append (itemImgFrameRoot.DOLocalRotate (swingRight, swingDuration))
      .Append (itemImgFrameRoot.DOLocalRotate (swingLeft, swingDuration))
      .Append (itemImgFrameRoot.DOLocalRotate (noSwing, swingDuration))
      .Append (itemImgFrameRoot.DOScaleY (bottomShrinkScale, bottomDuration));

    // 回復回収音
    SEManager.Instance.Play (SEPath.END_RECOVERING);

    // ハートの非表示
    float hideDuration = 0.4f;
    float bottomVerticalExpandScale = 1.2f;
    float bottomHorizontalShrinkScale = 0.0f;
    float hidePositionY = recoveryItemInitialPositionY + 2.0f;
    await DOTween.Sequence ().Append (itemImgFrameRoot.DOScaleY (bottomVerticalExpandScale, hideDuration)) //縦に伸ばす
      .Join (itemImgFrameRoot.DOScaleX (bottomHorizontalShrinkScale, hideDuration)) //横に縮める
      .Join (recoveryItem.DOMoveY (hidePositionY, hideDuration)); //ハートを上に移動
    await InactivateAllSmallItems (recoverTarget.transform);

    // 使用した回復エフェクト削除
    Destroy (targetRecoveryItem.gameObject);
    Destroy (targetRecoveryParticle);
    Destroy (targetRecoveringItemGroup);
  }

  // 名前改修予定
  void SetRecoveringHpAmountText (Transform itemImgFrame, string itemType, int recoveringAmount)
  {
    if (itemType == BelongingButtonInfoContainer.State.recoverHp.ToString ())
    {
      itemImgFrame.GetComponent<SpriteRenderer> ().sprite = recoveryHeartFrameSprite;
    }
    else if (itemType == BelongingButtonInfoContainer.State.recoverFp.ToString ())
    {
      itemImgFrame.GetComponent<SpriteRenderer> ().sprite = recoveryFlowerFrameSprite;
    }
    GameObject heartAmountText = itemImgFrame.Find ("ItemAmountText").gameObject;
    heartAmountText.GetComponent<TextMeshPro> ().text = recoveringAmount.ToString ();
  }

  void SetupRecoveringHeart (Transform recoverTarget, Transform targetRecoveryItem, Transform recoveryItem, Transform itemImgFrameRoot, Transform itemImgFrame)
  {
    // インスタンス化したtargetRecoveryItemの初期設定
    float baseScale = 1.5f;
    targetRecoveryItem.localScale = new Vector3 (baseScale, baseScale, baseScale);
    targetRecoveryItem.position = recoverTarget.transform.position;
    float hideScale = 0.0f;
    float heartHigherPositionY = 5.0f;
    recoveryItem.transform.localScale = new Vector3 (hideScale, hideScale, hideScale);
    Vector3 heartInitialPosition = recoveryItem.localPosition;
    heartInitialPosition.y += heartHigherPositionY;
    recoveryItem.transform.localPosition = heartInitialPosition;
    targetRecoveryItem.gameObject.SetActive (true);
  }

  void ActivateAllSmallItems ()
  {
    List<GameObject> smallItemList = new MenuCommonFunctions ().GetChildList (targetRecoveringItemGroup);
    foreach (var smallItem in smallItemList)
    {
      smallItem.GetComponent<SpriteRenderer> ().enabled = true;
      smallItem.GetComponent<ConsecutiveChangeSelfImage> ().StartChangeSprite = true;
    }
  }

  void ExpandSmallItems ()
  {
    float expandDuration = 0.4f;
    List<GameObject> smallItemList = new MenuCommonFunctions ().GetChildList (targetRecoveringItemGroup);
    for (var i = 0; i < smallItemList.Count; i++)
    {
      GameObject smallItem = smallItemList[i];
      int childeNum = i;
      Vector3 movePosition = targetCircumferenceArrangement.GetExpandedCircumferencePositionXZ (smallItem, childeNum, smallItemList.Count);
      smallItem.transform.DOMove (movePosition, expandDuration);
    }
  }

  async UniTask InactivateAllSmallItems (Transform recoverTarget)
  {
    List<GameObject> smallItemList = new MenuCommonFunctions ().GetChildList (targetRecoveringItemGroup);
    foreach (var smallItem in smallItemList)
    {
      Vector3 targetPosition = recoverTarget.position;
      // 回復量の多さに比例してハートが吸収される動きを早くする
      float moveDuration = 0.2f / (smallItemList.Count / 5);
      float hideScall = smallItem.transform.localScale.x / 3.0f;
      smallItem.transform.DOScale (hideScall, moveDuration);
      await smallItem.transform.DOMove (targetPosition, moveDuration);
      smallItem.GetComponent<SpriteRenderer> ().enabled = false;
      smallItem.GetComponent<ConsecutiveChangeSelfImage> ().StartChangeSprite = false;
    }
  }

  void RotatingTargetRecoveringSmallItemGroup ()
  {
    float rotationDuration = 1.0f;
    targetRecoveringItemGroup.transform.DOLocalRotate (new Vector3 (0, 360f, 0), rotationDuration, RotateMode.FastBeyond360)
      .SetLoops (2, LoopType.Restart);
  }

  async UniTask RotatingParticle ()
  {
    // パーティクルシステムがx軸にデフォルトで-90度になっているのを270度に修正してからでないと回転が上手くいかない
    // -90のままここで回転を-90度にしていしても上手くいかない
    float rotationDuration = 0.8f;
    await targetRecoveryParticle.transform.DOLocalRotate (new Vector3 (270f, 360f, 0), rotationDuration, RotateMode.FastBeyond360)
      .SetLoops (1, LoopType.Restart);
  }
}