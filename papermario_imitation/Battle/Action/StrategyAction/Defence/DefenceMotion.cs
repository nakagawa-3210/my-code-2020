using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class DefenceMotion : MonoBehaviour
{
  [SerializeField] GameObject selfBone;
  [SerializeField] GameObject defenceShield;
  private GameObject self;

  private Vector3 initialSelfPosition;

  float jump;
  float jumpDuration;

  private bool isUsingDefence;

  public bool IsUsingDefence
  {
    set { isUsingDefence = value; }
    get { return isUsingDefence; }
  }

  void Start ()
  {
    self = this.gameObject;
    defenceShield.SetActive (false);
    initialSelfPosition = self.transform.position;

    jump = 0.4f;
    jumpDuration = 0.4f;

    isUsingDefence = false;
  }

  // 回転の関数処理を共通化して改修予定
  public async UniTask StartNormalDefenceMotion ()
  {
    // ガード作成音再生
    SEManager.Instance.Play (SEPath.START_GUARD);
    
    float rotationDuration = 0.1f;
    int rotationNum = 5;
    await selfBone.transform.DOLocalRotate (new Vector3 (0, 360f, 0), rotationDuration, RotateMode.FastBeyond360)
      .SetLoops (rotationNum, LoopType.Restart);

    // ほんの少し下に落ちて上に少しだけジャンプ
    await DOTween.Sequence ()
      .Append (self.transform.DOMoveY ((initialSelfPosition.y - jump / 5), jumpDuration))
      .Append (self.transform.DOMoveY ((initialSelfPosition.y + jump), jumpDuration))
      .Append (self.transform.DOMoveY (initialSelfPosition.y, jumpDuration / 2));

    // 半球の表示
    defenceShield.SetActive (true);
    isUsingDefence = true;
  }

  public async UniTask HideDefenceShield ()
  {
    // ガード解除音
    SEManager.Instance.Play (SEPath.END_GUARD);

    defenceShield.SetActive (false);
    await DOTween.Sequence ().Append (self.transform.DOMoveY ((initialSelfPosition.y + jump), jumpDuration))
      .Append (self.transform.DOMoveY (initialSelfPosition.y, jumpDuration / 2));
    isUsingDefence = false;
  }

}