using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class EnemyAttackMotion : MonoBehaviour
{

  private GameObject self;
  [SerializeField] GameObject selfAwarenessSign;
  [SerializeField] GameObject selfMissSign;

  private EnemyScript enemyScript;
  private HanaPlayerScript hanaPlayerScript;

  private Sequence patrolRoute;
  private Sequence chasingPlayer;

  private SEManager seManager;

  private bool isChasing;
  private bool endSetUp;

  void Start ()
  {

    Setup ();
  }

  async UniTask Setup ()
  {
    endSetUp = false;
    self = this.gameObject;
    enemyScript = self.GetComponent<EnemyScript> ();

    seManager = SEManager.Instance;

    isChasing = false;

    ResetSigns ();
    await UniTask.WaitUntil (() => enemyScript != null && seManager != null);
    endSetUp = true;
  }

  void Update ()
  {
    ManagePlayingTween ();
  }

  void ManagePlayingTween ()
  {
    if (patrolRoute != null && patrolRoute.IsActive () && hanaPlayerScript != null)
    {
      if (patrolRoute.IsPlaying () && hanaPlayerScript.PlayerState != HanaPlayerScript.State.Normal)
      {
        patrolRoute.Pause ();
      }
      else if (!patrolRoute.IsPlaying () && hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal)
      {
        patrolRoute.Play ();
      }
    }
  }

  void ResetSigns ()
  {
    selfAwarenessSign.SetActive (false);
    selfMissSign.SetActive (false);
  }

  async UniTask ManageEnemySensesSign (GameObject senseSign)
  {
    // 表示中のサインリセット
    ResetSigns ();
    // サイン一定時間表示

    senseSign.SetActive (true);
    int showTimeMilSec = 800;
    await UniTask.Delay (showTimeMilSec);
    senseSign.SetActive (false);
  }

  async UniTask ChaseTarget (GameObject target)
  {
    isChasing = true;

    Vector3 targetPosition = target.transform.position;
    float selfPositionY = self.transform.position.y;
    targetPosition.y = selfPositionY;

    float chasingDuration = 1.2f;
    patrolRoute = DOTween.Sequence ().Append (self.transform.DOMove (targetPosition, chasingDuration));
    await patrolRoute;

    // 敵は追いかけた後に隙ができる
    int distractedDelayMilSec = 500;
    await UniTask.Delay (distractedDelayMilSec);
    isChasing = false;
  }

  void OnTriggerStay (Collider other)
  {
    if (other.tag == "Player" && !isChasing && endSetUp)
    {
      GameObject player = other.gameObject;
      hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();

      if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.Normal && enemyScript.EnemyState != EnemyScript.State.defeated)
      {
        seManager.Play (SEPath.AWARENESS, 0.7f, 0, 1, false);
        ManageEnemySensesSign (selfAwarenessSign);

        ChaseTarget (player);
      }
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player" && endSetUp)
    {
      seManager.Play (SEPath.MISS);
      ManageEnemySensesSign (selfMissSign);
    }
  }
}