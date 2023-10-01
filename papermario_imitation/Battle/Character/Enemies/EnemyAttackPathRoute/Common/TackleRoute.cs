using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TackleRoute : MonoBehaviour
{
  private Sequence tackleSequence;
  private Vector3 playerPositionVector3;
  private Vector3 tackleTargetPositionVector3;
  private GameObject player;
  private GameObject self;
  private Transform enemyInitialPositionTransform;
  private Vector3 enemyInitialPositionVector3;
  private bool startTackle;
  private bool endTackle;
  // private bool endBackToInitialPosition;

  public bool EndTackle
  {
    set { endTackle = value; }
    get { return endTackle; }
  }
  void Start ()
  {
    // 今だけ攻撃対象固定
    player = GameObject.FindGameObjectWithTag ("Player");
    // Debug.Log ("player : " + player.name);
    playerPositionVector3 = player.transform.position;
    self = this.gameObject;
    enemyInitialPositionVector3 = self.transform.position;
    SetupBools ();
    // startTackle = false;
    // tryGuard = false;
    // endBackToInitialPosition = false;
    // タックル
    // 敵が少し後ろに移動する
    // Aの位置まで移動する
    // Vector3 enemyPosition = enemyInitialPositionTransform.position;
    // Vector3 enemyInitialPositionBack = enemyInitialPositionVector3;
    // enemyInitialPositionBack.x += 0.8f;
    // ダメージコライダーに入る、または、ガード成功の変数を受け取ることを検知する
    // 検知したらBの位置まで移動する
    // enemy.transform.DOPath (new Vector3[] { enemyInitialPositionBack, playerPositionVector3 }, 5.0f).SetEase (Ease.InCubic);
  }

  // 確認テスト用
  // private void Update ()
  // {
  //   Debug.Log ("tackleTargetPositionVector3.x : " + tackleTargetPositionVector3.x);
  //   Debug.Log ("tackleTargetPositionVector3.x : " + tackleTargetPositionVector3.y);
  //   Debug.Log ("tackleTargetPositionVector3.x : " + tackleTargetPositionVector3.z);
  // }

  void SetupBools ()
  {
    startTackle = false;
    endTackle = false;
  }

  // プレイヤーと仲間が用意出来たら対応を始める
  public void SetupTargetPosition (GameObject target)
  {
    // 敵の位置を決めておく
    tackleTargetPositionVector3 = target.transform.position;
  }

  // 引数にターゲットを受け取って、攻撃対象の位置を取得する
  public void StartTackle ()
  {
    if (!startTackle)
    {
      // Debug.Log ("たっくる開始");
      startTackle = true;
      Vector3 enemyInitialPositionBack = enemyInitialPositionVector3;
      enemyInitialPositionBack.x += 0.8f;
      // テスト中のため、攻撃対象の位置は固定状態
      // Appendを用いて、攻撃の動きを丁寧にするようにした(Pathでは動き全体のEaseしか設定できなかったのでやめた)
      tackleSequence = DOTween.Sequence ().Append (self.transform.DOMove (enemyInitialPositionBack, 1.0f).SetEase (Ease.InCubic))
        .Append (self.transform.DOMove (tackleTargetPositionVector3, 3.0f).SetEase (Ease.InCubic));
      // .OnComplete (() =>
      // {
      //   // endTackle = true;
      // });
    }
  }

  public void BackToInitialPosition ()
  {
    if (startTackle)
    {
      startTackle = false;
      Vector3 playerPositionBack = tackleTargetPositionVector3;
      playerPositionBack.x += 1.0f;
      self.transform.DOPath (new Vector3[] { playerPositionBack, enemyInitialPositionVector3 }, 2.0f)
        .OnComplete (() =>
        {
          SetupBools ();
          // Debug.Log ("元の位置にもどったよ");
          endTackle = true;
        });
    }
  }
}