using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

// 探索中に受けるダメージ、拾うアイテム、敵との遭遇を管理する
public class ExploreManager : MonoBehaviour
{
  [SerializeField] HanaPlayerScript hanaPlayerScript = default;
  [SerializeField] GameObject curtainGroup = default;
  [SerializeField] SceneTransitionManager sceneTransitionManager = default;
  // バトル勝利後に遷移する探索シーンの名前として基本的には今のシーンを渡す
  [SerializeField] string currentSceneName = default;
  // 探索場所がどこなのかをboolで受け取って遷移先を決めておく
  // bool値に合わせて遷移先を変更する処理の役割を持つクラスをつくってもいいかも
  [SerializeField] bool isSteppeField = default;
  [SerializeField] bool isBossField = default;
  private CurtainManager curtainManager;
  // private HanaPlayerScript[] hanaPlayerScripts;
  // private HanaPlayerScript hanaPlayerScript;
  // private HanaPlayerBattleScript[] hanaPlayerBattleScripts;
  private HanaPlayerBattleScript hanaPlayerBattleScript;
  private BattleBgmManagerFromExploration battleBgmManagerFromExploration;
  private PartnerScript partnerScript;
  private PartnerScript[] partnerScripts;

  private Vector3 playerPosition;
  private Vector3 partnerPosition;
  private List<string> defeatedEnemyNameList;
  private List<Vector3> enemyPositionList;
  private List<GameObject> battleEnemyList;
  private ExploreState currentExploreState;
  private string encounteredEnemyName;

  public Vector3 PlayerPosition
  {
    set { playerPosition = value; }
    get { return playerPosition; }
  }

  public Vector3 PartnerPosition
  {
    set { partnerPosition = value; }
    get { return partnerPosition; }
  }

  public List<string> DefeatedEnemyList
  {
    set { defeatedEnemyNameList = value; }
    get { return defeatedEnemyNameList; }
  }
  public List<Vector3> EnemyPositionList
  {
    set { enemyPositionList = value; }
    get { return enemyPositionList; }
  }

  public ExploreState CurrentExploreState
  {
    set { currentExploreState = value; }
    get { return currentExploreState; }
  }

  public string EncounteredEnemyName
  {
    set { encounteredEnemyName = value; }
  }

  public enum ExploreState
  {
    fromExploration,
    fromWonTheBattle,
    fromEscapeTheBattle
  }

  private bool startCurtainTween;
  private bool startLoadingScene;

  void Start ()
  {
    startCurtainTween = false;
    startLoadingScene = false;

    SceneManager.sceneLoaded += OnSceneLoaded;

    InitPlayerPartyScript ();
    SetupPlayerForComingFromBattle ();
    SetupEnemyForComingFromBattle ();
    curtainManager = curtainGroup.GetComponent<CurtainManager> ();

    battleBgmManagerFromExploration = new BattleBgmManagerFromExploration ();

    // JSONファイルテスト作成
    int test = SaveSystem.Instance.userData.playerCurrentHp;
  }

  async UniTask Update ()
  {
    // 一旦は先制攻撃なしの遭遇を作成する
    // Debug.Log ("hanaPlayerScript.PlayerState : " + hanaPlayerScript.PlayerState);
    if (hanaPlayerScript.PlayerState == HanaPlayerScript.State.EncounteringEnemy && !startCurtainTween)
    {
      battleBgmManagerFromExploration.PlayBattleBgm ();
      startCurtainTween = true;
      await curtainManager.ShowAllCurtains ();
    }

    // 画面遷移処理をUniTaskのUntil使って改修予定
    if (curtainManager.EndTween && !startLoadingScene)
    {
      startLoadingScene = true;
      // PlayerGameObjectがDestroyされる前に位置をキャッシュ
      playerPosition = hanaPlayerBattleScript.gameObject.transform.position;
      partnerPosition = partnerScript.gameObject.transform.position;
      // 敵の位置をリストにまとめて渡して保存
      enemyPositionList = new List<Vector3> ();
      foreach (Transform enemy in GameObject.Find ("SceneEnemy").transform)
      {
        enemyPositionList.Add (enemy.position);
      }
      // 闘う敵のリスト保存
      await UniTask.WaitUntil (() => hanaPlayerBattleScript.HanaPlayerBattleEnemyInformationHolder.BattleEnemyList != null);
      battleEnemyList = hanaPlayerBattleScript.HanaPlayerBattleEnemyInformationHolder.BattleEnemyList;
      // 敵の名前保存
      encounteredEnemyName = hanaPlayerBattleScript.EnemyName;
      hanaPlayerBattleScript.ResetBattleOpponent ();
      DOTween.Clear ();
      ManageSceneLoading ();
    }
  }

  // SceneManager.sceneLoadedに加えるためには引数を下記の通りに指定する必要がある
  void OnSceneLoaded (Scene scene, LoadSceneMode mode)
  {
    // 別クラスに処理移動予定
    if (SceneManager.GetActiveScene ().name == "SteppeBattleScene" || SceneManager.GetActiveScene ().name == "TurtleBossBattleScene")
    {
      // 画面遷移時にバトルシーンに敵リストを渡す
      BattleManager battleManager = GameObject.FindWithTag ("BattleManager").GetComponent<BattleManager> ();

      battleManager.BattleEnemyCharacterListForInstantiating = battleEnemyList;
      // バトルシーンにプレイヤーと仲間の今いる位置の座標と今のシーンの名前も渡す
      battleManager.NextSceneName = currentSceneName;

      battleManager.ExplorationScenePlayerPosition = playerPosition;
      battleManager.ExplorationScenePartnerPosition = partnerPosition;

      battleManager.ExplorationEnemyPositionList = enemyPositionList;

      battleManager.ExplorationDefeatedEnemyList = defeatedEnemyNameList;
      battleManager.EncounteredEnemyName = encounteredEnemyName;
    }
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  void ManageSceneLoading ()
  {
    // 別クラスに処理移動予定
    if (isSteppeField)
    {
      if (isBossField)
      {
        SceneManager.LoadScene ("TurtleBossBattleScene");
      }
      else
      {
        SceneManager.LoadScene ("SteppeBattleScene");
      }
    }
  }

  void InitPlayerPartyScript ()
  {
    hanaPlayerBattleScript = hanaPlayerScript.gameObject.GetComponent<HanaPlayerBattleScript> ();
    InitPartnerScript ();
  }

  void InitPartnerScript ()
  {
    // 仲間は動的に変えるからこの方法にしておく
    partnerScripts = FindObjectsOfType<PartnerScript> ();
    if (partnerScripts.Length > 0)
    {
      if (!partnerScript) partnerScript = partnerScripts[0];
    }
  }

  async UniTask SetupPlayerForComingFromBattle ()
  {
    GameObject player = GameObject.Find ("Player");
    // バトルから逃げてきた
    if (currentExploreState == ExploreState.fromEscapeTheBattle)
    {
      HanaPlayerScript hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      hanaPlayerScript.EscapedFromTheBattle = true;
      ManageEscapeTime ();
      // 下記のプレイヤー達の位置変更処理の書き方変更予定
      // await UniTask.WaitUntil (() => sceneTransitionManager != null);
      await sceneTransitionManager.SetPlayerInitialPositionFromBattle ();

    }
    // バトルに勝って戻ってきた
    else if (currentExploreState == ExploreState.fromWonTheBattle)
    {
      hanaPlayerScript.WonTheBattle = true;
      // 下記のプレイヤー達の位置変更処理の書き方変更予定
      // await UniTask.WaitUntil (() => sceneTransitionManager != null);
      await sceneTransitionManager.SetPlayerInitialPositionFromBattle ();
    }
    // 下記のプレイヤー達の位置変更処理の書き方変更予定
    // sceneTransitionManager.SetPlayerInitialPositionFromBattle ();

  }

  async UniTask SetupEnemyForComingFromBattle ()
  {
    if (enemyPositionList == null) return;

    int enemyNum = 0;
    // ここでdefeatedEnemyNameListが初期化されていない場合初期化
    try
    {
      // Debug.Log ("defeatedEnemyNameList.Count : " + defeatedEnemyNameList.Count);
    }
    catch (System.Exception ex)
    {
      defeatedEnemyNameList = new List<string> ();
      // Debug.Log ("defeatedEnemyNameListが初期化されてないぞ");
    }

    foreach (Transform enemy in GameObject.Find ("SceneEnemy").transform)
    {
      // 敵の位置情報と倒された情報をバトルに入る前の状態変更
      // foreach内で呼ばれる敵と同じ順番のenemyPositionList要素を用いる
      enemy.position = enemyPositionList[enemyNum];
      // すでに倒された敵は非アクティブ化する
      foreach (var defeatedEnemyName in defeatedEnemyNameList)
      {
        // Debug.Log ("defeatedEnemyName : " + defeatedEnemyName);
        // Debug.Log ("enemy.name : " + enemy.gameObject.name);
        if (defeatedEnemyName == enemy.gameObject.name)
        {
          enemy.gameObject.SetActive (false);
        }
      }
      enemyNum++;

      // 今回戻ってきたバトルで倒した敵を管理
      string enemyName = enemy.gameObject.name;
      if (encounteredEnemyName == enemyName && currentExploreState == ExploreState.fromWonTheBattle)
      {
        // 倒された敵リストに追加
        defeatedEnemyNameList.Add (enemyName);
        // 状態変更
        enemy.GetComponent<EnemyScript> ().EnemyState = EnemyScript.State.defeated;

        // 敵に倒れる様子を再生させる
        DefeatedEnemyMotion defeatedEnemyMotion = enemy.GetComponent<DefeatedEnemyMotion> ();
        defeatedEnemyMotion.PlayDefeatedEnemy ();
      }
    }

  }

  async UniTask ManageEscapeTime ()
  {
    int escapeTime = 3000;
    await UniTask.Delay (escapeTime);
    hanaPlayerScript.ActivateSelfMeshWithBone ();
    hanaPlayerScript.EscapedFromTheBattle = false;
  }

}