using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyStatusManager : MonoBehaviour
{
  [SerializeField] GameObject enemyGaugeCanvas;
  [SerializeField] GameObject battleEnemyHpGauge;
  private List<GameObject> battleEnemyCharacterList;
  private List<GameObject> enemyHpGaugeList;
  private List<Vector3> enemyGaugeGameObjectPositionList;
  private List<int> enemyHpList;

  void Start ()
  {
    battleEnemyCharacterList = new List<GameObject> ();
    enemyHpGaugeList = new List<GameObject> ();
    // Debug.Log ("Screen.width : " + Screen.width);
    // Debug.Log ("Screen.height : " + Screen.height);
  }

  void Update ()
  {
    SetupInitialEnemyHpText ();
    ManageEnemyGaugePosition ();
  }

  public void SetupEnemyHpGauge (List<GameObject> battleEnemyCharacterList)
  {
    this.battleEnemyCharacterList = battleEnemyCharacterList;
    enemyHpGaugeList = new List<GameObject> ();
    enemyGaugeGameObjectPositionList = new List<Vector3> ();
    foreach (var enemy in battleEnemyCharacterList)
    {
      GameObject enemyHpGauge = GameObject.Instantiate<GameObject> (battleEnemyHpGauge);
      enemyHpGaugeList.Add (enemyHpGauge);
      enemyHpGauge.transform.SetParent (enemyGaugeCanvas.transform);
      Vector3 enemyGaugePosition = enemy.transform.Find ("HpGaugePosition").position;
      enemyGaugeGameObjectPositionList.Add (enemyGaugePosition);
      enemyHpGauge.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, enemyGaugePosition);
      SetupInitialGaugeSize (enemyHpGauge);
    }
    HideAllEnemyGauges ();
  }

  void ManageEnemyGaugePosition ()
  {
    if (enemyGaugeGameObjectPositionList == null) return;
    for (var i = 0; i < enemyGaugeGameObjectPositionList.Count; i++)
    {
      // ゲージが表示されるべき場所
      Vector3 enemyGaugeGameObjectPosition = enemyGaugeGameObjectPositionList[i];
      Vector2 correctGaugePosition = RectTransformUtility.WorldToScreenPoint (Camera.main, enemyGaugeGameObjectPosition);
      // 現在のゲージ位置
      Vector2 currentEnemyGaugePosition = enemyHpGaugeList[i].GetComponent<RectTransform> ().position;
      // 位置修正
      if (correctGaugePosition != currentEnemyGaugePosition)
      {
        enemyHpGaugeList[i].GetComponent<RectTransform> ().position = correctGaugePosition;
      }
    }
  }

  void SetupInitialGaugeSize (GameObject enemyHpGauge)
  {
    // 動的に作成するとscale値が変わるので元に戻す
    float baseScale = 1.0f;
    enemyHpGauge.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
  }

  // BattleManagerで呼び出しても、BattleEnemyStatusのEnemyデータが用意される前になってしまい、
  // Startの呼び出し順をエディタ側での設定に依存する方法もとりたくないので無理やり最初の設定のみこのやり方
  void SetupInitialEnemyHpText ()
  {
    if (enemyHpGaugeList.Count == 0 || battleEnemyCharacterList.Count == 0) return;
    bool isEnemyHpTextActive = enemyHpGaugeList[0].transform.Find ("EnemyHpText").gameObject.activeSelf;
    if (isEnemyHpTextActive) return;
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      GameObject enemy = battleEnemyCharacterList[i];
      if (enemy.GetComponent<BattleEnemyStatus> ().EnemyData != null)
      {
        SetEnemyHpText ();
      }
    }
  }

  public void SetEnemyHpText ()
  {
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      GameObject enemy = battleEnemyCharacterList[i];
      Enemy enemyData = enemy.GetComponent<BattleEnemyStatus> ().EnemyData;
      // Debug.Log ("敵情報の確認 : " + enemyData);
      // 関数にするかも
      enemyHpGaugeList[i].transform.Find ("EnemyHpText").GetComponent<TextMeshProUGUI> ().text = enemyData.enemyHp.ToString ();
      enemyHpGaugeList[i].transform.Find ("EnemyHpText").gameObject.SetActive (true);
    }
  }

  void SetupGaugeInitialValueForReset ()
  {
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      Enemy enemyData = battleEnemyCharacterList[i].GetComponent<BattleEnemyStatus> ().EnemyData;
      GameObject gauge = enemyHpGaugeList[i];
      Image yellowGauge = gauge.transform.Find ("YellowGauge").GetComponent<Image> ();
      float fillAmountValue = (float) enemyData.enemyHp / enemyData.enemyMaxHp;
      yellowGauge.fillAmount = fillAmountValue;
    }
  }

  public void EnemyHpGaugeValueReduction (GameObject attackedEnemy, float damageValue, float duration = 0.8f)
  {
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      GameObject enemy = battleEnemyCharacterList[i];
      if (attackedEnemy == enemy)
      {
        Enemy enemyData = enemy.GetComponent<BattleEnemyStatus> ().EnemyData;
        float valueFrom = (float) enemyData.enemyHp / enemyData.enemyMaxHp;
        float valueTo = (float) (enemyData.enemyHp - damageValue) / enemyData.enemyMaxHp;

        // ゲージ減少tween再生
        GameObject enemyHpGauge = enemyHpGaugeList[i];
        Image yellowGauge = enemyHpGauge.transform.Find ("YellowGauge").GetComponent<Image> ();
        new GaugeValueTween ().ChangeGaugeValueTween (
          yellowGauge,
          valueFrom,
          valueTo,
          duration
        );
      }
    }
  }

  public void ShowTargetEnemyHpGauge (GameObject target)
  {
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      GameObject enemy = battleEnemyCharacterList[i];
      if (target == enemy)
      {
        GameObject enemyHpGauge = enemyHpGaugeList[i];
        float showingValue = 1.0f;
        enemyHpGauge.GetComponent<CanvasGroup> ().alpha = showingValue;
      }
    }
  }

  // 最後に受けた攻撃の後に呼びたいけど、タイミングがこのクラスからとる方法が思いつかないので攻撃処理から呼んでもらっている
  public void HideTargetEnemyHpGauge (GameObject target)
  {
    for (var i = 0; i < battleEnemyCharacterList.Count; i++)
    {
      GameObject enemy = battleEnemyCharacterList[i];
      if (target == enemy)
      {
        GameObject enemyHpGauge = enemyHpGaugeList[i];
        float hidingValue = 0.0f;
        float hideDuration = 0.8f;
        enemyHpGauge.GetComponent<CanvasGroup> ().DOFade (hidingValue, hideDuration);
      }
    }
  }

  public void ResetEnemyHpGauge (List<GameObject> newBattleEnemyCharacterList)
  {
    RemoveEnemyHpGauge ();
    SetupEnemyHpGauge (newBattleEnemyCharacterList);
    SetupGaugeInitialValueForReset ();
  }

  void RemoveEnemyHpGauge ()
  {
    foreach (var enemyHpGauge in enemyHpGaugeList)
    {
      Destroy (enemyHpGauge);
    }
  }

  public void ShowAllEnemyGauges ()
  {
    foreach (var enemyHpGauge in enemyHpGaugeList)
    {
      enemyHpGauge.GetComponent<CanvasGroup> ().alpha = 1.0f;
    }
  }

  public void HideAllEnemyGauges ()
  {
    foreach (var enemyHpGauge in enemyHpGaugeList)
    {
      enemyHpGauge.GetComponent<CanvasGroup> ().alpha = 0.0f;
    }
  }

}