using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class FireFlowerMotion : MonoBehaviour
{
  [SerializeField] GameObject fireFlowerPrefab;
  private GameObject itemImage;
  private GameObject itemFocus;
  private GameObject fireFlower;
  private GameObject fireFlowerImage;
  private Vector3 fireFlowerPosition;
  private ParticleSystem fireParticleSystem;
  private Animator fireFlowerAnimator;
  private Image fireFlowerImg;

  private GameObject self;
  // ファイヤーフラワーのイメージプレファブをインスタンス化してアニメ再生？
  // 
  void Start ()
  {
    self = this.gameObject;
    GameObject battleItemUsingUiCanvas = GameObject.Find ("BattleItemUsingUiCanvas");
    itemImage = battleItemUsingUiCanvas.transform.Find ("ItemImage").gameObject;
    itemFocus = battleItemUsingUiCanvas.transform.Find ("ItemFocusGroup").gameObject;
    // ファイヤーフラワーの位置取得
    fireFlowerPosition = GameObject.Find ("FireFlowerPosition").transform.position;
    itemImage.SetActive (false);
    itemFocus.SetActive (false);
  }

  void Update ()
  {
    // if (Input.GetKeyDown (KeyCode.A))
    // {
    //   fireFlowerAnimator.SetBool ("boolStartFire", false);
    //   fireFlowerAnimator.SetTrigger ("triggerStartFire");
    // }
  }

  public async UniTask ShowUsingItem (BelongingButtonInfoContainer itemInformation)
  {
    string itemName = itemInformation.BelongingName;
    Sprite usingItemSprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
    await new CommonItemMotion ().ShowingUsingItemImage (self, itemImage, itemFocus, usingItemSprite);
    // ファイヤーフラワーが徐々に大きくなりながら出現
    fireFlower = GameObject.Instantiate (fireFlowerPrefab);
    fireFlower.transform.position = fireFlowerPosition;
    await ShowFlower (fireFlower);
    fireFlowerImage = fireFlower.transform.Find ("FlowerTulipFace").gameObject;
    fireFlowerAnimator = fireFlowerImage.GetComponent<Animator> ();
    fireFlowerAnimator.SetBool ("boolStartFire", true);
    // アニメーション再生状態を検知する方法が見つからないのでUniTask.Delay()で調整
    int milSec = 200;
    await UniTask.Delay (milSec);
    StartFire ();
  }

  async UniTask ShowFlower (GameObject fireFlower)
  {
    // 最初は表示しない
    fireFlower.transform.localScale = new Vector3 (0, 0, 0);
    float showScale = 1.0f;
    float showDuration = 0.5f;
    await fireFlower.transform.DOScale (new Vector3 (showScale, showScale, showScale), showDuration);
    // 拡大後少し待つ
    int littleWait = 250;
    await UniTask.Delay (littleWait);
  }

  void StartFire ()
  {
    fireParticleSystem = fireFlower.transform.Find ("FireParticle").gameObject.GetComponent<ParticleSystem> ();
    fireParticleSystem.Play ();
    // 効果音再生
    SEManager.Instance.Play (SEPath.FIRE_FLOWER, 1, 0, 1, true, null);
  }

  public async UniTask StopFire ()
  {
    // 効果音停止
    fireFlowerAnimator.SetBool ("boolStartFire", false);
    fireFlowerAnimator.SetTrigger ("triggerEndFire");

    int stopFireParticleDelay = 600;
    await UniTask.Delay (stopFireParticleDelay);
    SEManager.Instance.Stop (SEPath.FIRE_FLOWER);
    fireParticleSystem.Stop ();

    int endFireDelay = 300;
    await UniTask.Delay (endFireDelay);
    SEManager.Instance.Play (SEPath.END_FIRE_FLOWER);

    int stopForRemove = 1200;
    await UniTask.Delay (stopForRemove);
    await RemoveFireFlower ();
  }

  async UniTask RemoveFireFlower ()
  {
    await RotationFadeOut ();
    MonoBehaviour.Destroy (fireFlower);
  }

  async UniTask RotationFadeOut ()
  {
    SpriteRenderer fireFlowerSpriteRenderer = fireFlowerImage.GetComponent<SpriteRenderer> ();
    float hideDuration = 0.8f;
    await DOTween.Sequence ().Append (fireFlower.transform.DOLocalRotate (new Vector3 (0, 360f, 0), hideDuration, RotateMode.FastBeyond360)
        .SetLoops (1, LoopType.Restart))
      .Join (fireFlowerSpriteRenderer.DOFade (0, hideDuration));
  }

  // まだ未実装の処理
  // ファイヤーフラワーの出現に合わせて星を散らす
  // ダメージを受けた敵キャラのImageSpriteは黒色に変化する
  // ダメージを受けた敵は体力がゼロでなければ、ImageSpriteが通常の色に戻る
}