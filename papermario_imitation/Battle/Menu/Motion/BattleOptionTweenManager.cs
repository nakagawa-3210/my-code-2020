using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

// tweenを用いる動きを作る役割を担う
public class BattleOptionTweenManager
{
  private MenuCommonFunctions menuCommonFunctions;
  private List<GameObject> optionList;
  private GameObject forShowingRotation;
  private GameObject currentSelectOptionBalloon;
  // private GameObject jumpButtonListContainer;
  // private GameObject hammerButtonListContainer;
  // private GameObject partnerSkillButtonListContainer;
  private Transform optionBalloonMovePositionTransform;
  private Vector3 currentSelectOptionBalloonFirstPosition;
  private Vector3 forRotationV3FirstRotation;
  private Vector3 optionStandingScale;
  private Vector3 currentSelectOptionBalloonStandingScale;
  private float initialFirstOptionsScale;
  private float hidingScale;
  private float showingRotatingDuration;
  private float shrinkingDuration;
  private float balloonMovingDuration;
  private float listRotatingDuration;
  private bool endShowingFirstOptionTween;
  private bool endHideListTween;
  public BattleOptionTweenManager (
    List<GameObject> optionList,
    GameObject forShowingRotation,
    GameObject currentSelectOptionBalloon,
    Transform optionBalloonMovePositionTransform,
    float showingRotatingDuration,
    float shrinkingDuration,
    float listRotatingDuration
  )
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    this.optionList = optionList;
    this.forShowingRotation = forShowingRotation;
    this.currentSelectOptionBalloon = currentSelectOptionBalloon;
    this.optionBalloonMovePositionTransform = optionBalloonMovePositionTransform;
    this.showingRotatingDuration = showingRotatingDuration;
    this.shrinkingDuration = shrinkingDuration;
    this.listRotatingDuration = listRotatingDuration;
    balloonMovingDuration = shrinkingDuration * 2;
    // 最初の吹き出しの位置キャッシュ
    currentSelectOptionBalloonFirstPosition = currentSelectOptionBalloon.transform.position;
    currentSelectOptionBalloonStandingScale = currentSelectOptionBalloon.transform.localScale;
    // 選択肢の丸の初期の大きさをキャッシュ
    // Debug.Log ("optionList.Count : " + optionList.Count);
    initialFirstOptionsScale = optionList[0].transform.localScale.x;
    // 表示する際の最初の選択肢の角度キャッシュ 
    optionStandingScale = optionList[0].transform.localScale;
    // 最初の選択肢を隠す際の選択肢グループの回転角度キャッシュ
    Vector3 firstHidingRotation = forShowingRotation.transform.localEulerAngles;
    firstHidingRotation.y = 360 / optionList.Count;
    forShowingRotation.transform.localEulerAngles = firstHidingRotation;
    hidingScale = 0.0f;
    endShowingFirstOptionTween = false;
    endHideListTween = true;

    // playerの時のみに使う関数を作成し下記削除
    // jumpButtonListContainer.SetActive (false);
    // HideListButtons (jumpButtonListContainer);
    ResetFirstOption ();
  }

  public bool EndShowingFirstOptionTween
  {
    get { return endShowingFirstOptionTween; }
  }

  public bool EndHideListTween
  {
    get { return endHideListTween; }
  }

  public void SetupPlayerList (GameObject jumpButtonListContainer, GameObject hammerButtonListContainer)
  {
    jumpButtonListContainer.SetActive (false);
    HideListButtons (jumpButtonListContainer);
    hammerButtonListContainer.SetActive (false);
    HideListButtons (hammerButtonListContainer);
  }

  public void SetupPartnerList (GameObject partnerSkillButtonListContainer)
  {
    // this.partnerSkillButtonListContainer = partnerSkillButtonListContainer;
    partnerSkillButtonListContainer.SetActive (false);
    HideListButtons (partnerSkillButtonListContainer);
  }

  // アイテム、さくせんリスト用
  public void SetupCommonList (GameObject itemOptionButtonListContainer)
  {
    itemOptionButtonListContainer.SetActive (false);
    HideListButtons (itemOptionButtonListContainer);
  }

  public async UniTask ShowFirstOptionsWithRotating ()
  {
    endShowingFirstOptionTween = false;
    // 選択肢グループ回転の処理
    Vector3 forRotationV3 = forShowingRotation.transform.localEulerAngles;
    // 最初の度数キャッシュ
    forRotationV3FirstRotation = forRotationV3;
    // 最初の選択肢が(360 * optionList.Count - 1 / optionList.Count)度の回転をする
    float rotationDegree = 360 * (optionList.Count - 1) / optionList.Count;
    forRotationV3.y = rotationDegree;
    forShowingRotation.transform.DORotate (forRotationV3, showingRotatingDuration, RotateMode.LocalAxisAdd);

    // 予定変更=>選択肢グループのうち、手前と奥の選択肢の角度がどうしてもずれてしまうので回転で立ち上げる動作の再現はできない。
    // => 代わりにy軸スケールを0から1に変更する処理で立ち上げるような動きを作る
    // 選択肢は倒れている(-90度)が、グループが180度回転したところで元の状態(15度)に立ち上がって入る
    foreach (var option in optionList)
    {
      option.transform.DOScaleY (optionStandingScale.y, showingRotatingDuration / 2);
      // 次の表示するタイミングは好みなので直打ち
      await UniTask.WaitUntil (() => option.transform.localScale.y > optionStandingScale.y / 3);
      // 最後の選択肢は代わりに吹き出しが表示されるので、最後の選択肢表示Unitask終了時に吹き出しの表示を開始する
      if (option == optionList[optionList.Count - 1])
      {
        await currentSelectOptionBalloon.transform.DOScaleY (currentSelectOptionBalloonStandingScale.y, showingRotatingDuration / 2);
      }
    }
    endShowingFirstOptionTween = true;
  }

  // 登場前の状態に戻す(すべての選択肢が画面奥側に倒れ、最初の度数に戻っている)
  public void ResetFirstOption ()
  {
    float hideScaleY = 0.0f;
    Vector3 hidingScale = optionStandingScale;
    // 選択肢スケールの処理
    foreach (var option in optionList)
    {
      hidingScale.y = hideScaleY;
      option.transform.localScale = hidingScale;
    }
    // 吹き出しのスケール処理
    Vector3 hidingBalloonScale = currentSelectOptionBalloonStandingScale;
    hidingBalloonScale.y = hideScaleY;
    currentSelectOptionBalloon.transform.localScale = hidingBalloonScale;
    // 選択肢グループの回転
    forShowingRotation.transform.DORotate (forRotationV3FirstRotation, 0, RotateMode.LocalAxisAdd);
  }

  public async UniTask ShrinkFirstOptions ()
  {
    for (var i = 0; i < optionList.Count; i++)
    {
      GameObject option = optionList[i];
      int last = optionList.Count - 1;
      if (i == last)
      {
        await option.transform.DOScale (hidingScale, shrinkingDuration);
      }
      else
      {
        option.transform.DOScale (hidingScale, shrinkingDuration);
      }
    }
  }

  public async UniTask ExpandFirstOptions ()
  {
    for (var i = 0; i < optionList.Count; i++)
    {
      GameObject option = optionList[i];
      int last = optionList.Count - 1;
      // 最後のいっこだけ待たせる(tweenが終わるタイミングはすべて同じ)
      if (i == last)
      {
        await option.transform.DOScale (initialFirstOptionsScale, shrinkingDuration);
      }
      else
      {
        option.transform.DOScale (initialFirstOptionsScale, shrinkingDuration);
      }
    }
  }

  public async UniTask MoveOptionBalloonToListPosition ()
  {
    currentSelectOptionBalloon.GetComponent<SpriteRenderer> ().flipX = true;
    await currentSelectOptionBalloon.transform.DOMove (optionBalloonMovePositionTransform.position, balloonMovingDuration);
    // int rotationNum = 2;
    // AnimatorのSetBoolに頼らない方法で吹き出しの回転を試したけど上手くいかなかった。
    // 改修できるかもなのでコメントアウト中
    // await DOTween.Sequence ()
    //   .Append (currentSelectOptionBalloon.transform.DOMove (optionBalloonMovePositionTransform.position, balloonMovingDuration))
    // .Append (currentSelectOptionBalloon.transform.DOLocalRotate (new Vector3 (0, 360f, 0), balloonMovingDuration, RotateMode.FastBeyond360)
    //   .SetEase (Ease.Linear)
    //   .SetLoops (rotationNum, LoopType.Restart));
  }

  public async UniTask MoveOptionBalloonToFirstPosition ()
  {
    currentSelectOptionBalloon.GetComponent<SpriteRenderer> ().flipX = false;
    await DOTween.Sequence ()
      .Append (currentSelectOptionBalloon.transform.DOMove (currentSelectOptionBalloonFirstPosition, balloonMovingDuration));
  }

  // ジャンプ、ハンマー、アイテム、さくせんの4つのリストすべては共通のtween
  public async UniTask ShowListButtons (GameObject ListContainer, bool isItemList = false)
  {
    float listDuration = GetListRotationDuration (isItemList);
    List<GameObject> buttonList = menuCommonFunctions.GetChildList (ListContainer);
    ListContainer.SetActive (true);
    foreach (var button in buttonList)
    {
      Vector3 buttonRotation = button.transform.localEulerAngles;
      buttonRotation.y = 0.0f;
      await button.transform.DORotate (buttonRotation, listDuration);
    }
  }

  public async UniTask HideListButtons (GameObject ListContainer, bool isItemList = false)
  {
    endHideListTween = false;
    float listDuration = GetListRotationDuration (isItemList);
    List<GameObject> buttonList = menuCommonFunctions.GetChildList (ListContainer);
    foreach (var button in buttonList)
    {
      Vector3 buttonRotation = button.transform.localEulerAngles;
      buttonRotation.y = 90.0f;
      await button.transform.DORotate (buttonRotation, listDuration);
    }
    endHideListTween = true;
    ListContainer.SetActive (false);
  }

  // アイテムリストは最大で20個のボタンを持つので
  // 表示の速度をスキルリストよりも速くして調整する
  float GetListRotationDuration (bool isItemList)
  {
    float listDuration = listRotatingDuration;
    if (isItemList)
    {
      listDuration /= 10;
    }
    return listDuration;
  }

}