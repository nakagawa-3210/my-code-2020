using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Test_AttackRoute : MonoBehaviour
{
  [SerializeField] Transform playerPositionTransform;
  private Vector3 playerPositionVector3;
  private GameObject enemy;
  private GameObject player;
  private Transform enemyInitialPositionTransform;
  private Vector3 enemyInitialPositionVector3;
  void Start ()
  {
    player = GameObject.FindGameObjectWithTag ("Player");
    playerPositionVector3 = player.transform.position;
    enemy = this.gameObject;
    // 下記は使うとDOTween中に値が変わる。なぜ？
    enemyInitialPositionTransform = enemy.transform;
    // Vector3に代入すると値が変わらない
    enemyInitialPositionVector3 = enemyInitialPositionTransform.position;

    // 現在位置をキャッシュする : A
    // プレイヤーの位置をキャッシュする : B

    // タックル
    // 敵が少し後ろに移動する
    // Aの位置まで移動する
    Vector3 enemyPosition = enemyInitialPositionTransform.position;
    Vector3 enemyInitialPositionBack = enemyInitialPositionTransform.transform.position;
    enemyInitialPositionBack.x += 1.5f;
    // ダメージコライダーに入る、または、ガード成功の変数を受け取ることを検知する
    // 検知したらBの位置まで移動する
    enemy.transform.DOPath (new Vector3[] { enemyInitialPositionBack, playerPositionVector3 }, 5.0f).SetEase (Ease.InCubic);
  }

  void Update ()
  {
    // Debug.Log ("enemyInitialPositionVector3 : " + enemyInitialPositionVector3);
    if (Input.GetKeyDown (KeyCode.Space))
    {
      Vector3 playerPositionBack = player.transform.position;
      playerPositionBack.x += 1.0f;
      enemy.transform.DOPath (new Vector3[] { playerPositionBack, enemyInitialPositionVector3 }, 5.0f)
        .OnComplete (() =>
        {
          // Debug.Log ("元の位置にもどったよ");
        });
    }
  }
}