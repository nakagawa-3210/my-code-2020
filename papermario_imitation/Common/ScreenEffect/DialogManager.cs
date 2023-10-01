using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
  [SerializeField] FadeManager fadeManager;
  [SerializeField] float showAndHideTweenSpeed = default;
  private GameObject self;

  private bool endTween;
  private bool endSetup;

  public bool EndTween
  {
    get { return endTween; }
  }
  public bool EndSetup
  {
    get { return endSetup; }
  }

  async UniTask Start ()
  {
    self = this.gameObject;
    endTween = true;
    await Setup ();
  }

  // void Update ()
  // {
  //   // if (Input.GetKeyDown (KeyCode.F))
  //   // {
  //   //   ShowDialog ();
  //   // }
  // }

  async UniTask Setup ()
  {
    self.SetActive (false);
    endSetup = false;
    float hideScale = 0.0f;
    Vector3 scaleVector3 = new Vector3 (hideScale, hideScale, hideScale);
    await self.transform.DOScale (scaleVector3, showAndHideTweenSpeed);
    self.SetActive (true);
    endSetup = true;
  }

  public async UniTask OpenDialog ()
  {
    await fadeManager.DialogFadeOut ();
    await ShowDialog ();
  }

  public async UniTask CloseDialog ()
  {
    await HideDialog ();
    await fadeManager.DialogFadeIn ();
  }

  public async UniTask ShowDialog ()
  {
    float showScale = 1.0f;
    await DialogTween (showScale);
  }

  public async UniTask HideDialog ()
  {
    float hideScale = 0.0f;
    Debug.Log ("ダイアログをTweenはじめ");
    await DialogTween (hideScale);
  }

  public async UniTask DialogTween (float scale)
  {
    endTween = false;
    Vector3 scaleVector3 = new Vector3 (scale, scale, scale);
    await self.transform.DOScale (scaleVector3, showAndHideTweenSpeed);
    Debug.Log ("ダイアログをTweenおわり！");
    endTween = true;
  }

  public void HideDialogAtFirst ()
  {
    float hideScale = 0.0f;
    self.transform.localScale = new Vector3 (hideScale, hideScale, hideScale);
  }

}