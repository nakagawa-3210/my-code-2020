using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

// すべての敵キャラで使用する予定
public class EnemyDefeatedMotion : MonoBehaviour
{
  [SerializeField] GameObject defeatedRotationRoot;

  // プレイヤーが倒れた場合と共通の処理を改修予定
  public async UniTask HideDefeatedEnemySprite (float rotationDuration = 1.2f)
  {
    await new CommonDefeatedMotion ().DefeatedMotion (defeatedRotationRoot, defeatedRotationRoot, defeatedRotationRoot);
  }
}