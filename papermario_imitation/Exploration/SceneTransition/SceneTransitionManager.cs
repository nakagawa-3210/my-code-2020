using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
  [SerializeField] ExploreManager exploreManager;
  [SerializeField] HanaPlayerScript hanaPlayer;
  [SerializeField] GameObject partner;
  [SerializeField] FadeManager fadeManager;
  [SerializeField] GameObject northTriggerArea;
  [SerializeField] GameObject southTriggerArea;
  [SerializeField] GameObject eastTriggerArea;
  [SerializeField] GameObject westTriggerArea;
  // private FadeController_Test fadeController;
  // どこのシーンから来たのか
  // どこのシーンへ行くのか
  private GameObject player;
  private string previousSceneName;
  private string nextSceneName;
  private bool startLoadingScene;
  void Start ()
  {
    player = hanaPlayer.gameObject;
    nextSceneName = null;
    startLoadingScene = false;
    // fadeController = screenEffectFade.GetComponent<FadeController_Test> ();
    string currentSceneName = SceneManager.GetActiveScene ().name;
    SetPlayerInitialPosition ();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void Update ()
  {
    ManageSceneTransition ();
    // Debug.Log ("player.transform.position : " + player.transform.position);
  }

  public async UniTask GoToTitleScene ()
  {
    await fadeManager.FadeOut ();
    SceneManager.LoadScene ("TitleScene");
  }

  void OnSceneLoaded (Scene scene, LoadSceneMode mode)
  {
    DOTween.Clear();
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  public async UniTask SetPlayerInitialPositionFromBattle ()
  {
    // Debug.Log ("exploreManager : " + exploreManager);
    await UniTask.WaitUntil (() => player != null && partner != null);
    if (exploreManager.CurrentExploreState == ExploreManager.ExploreState.fromEscapeTheBattle)
    {
      // Debug.Log ("バトルから逃げてきた");
      // Debug.Log ("player : " + player);
      player.transform.position = exploreManager.PlayerPosition;
      partner.transform.position = exploreManager.PartnerPosition;
    }
    else if (hanaPlayer.WonTheBattle)
    {
      hanaPlayer.WonTheBattle = false;
      player.transform.position = exploreManager.PlayerPosition;
      partner.transform.position = exploreManager.PartnerPosition;
      // Debug.Log ("partner.transform.position : " + partner.transform.position);
    }
  }

  void SetPlayerInitialPosition ()
  {
    SceneTransitionTrigger northAreaTransitonTriggerScript = northTriggerArea.GetComponent<SceneTransitionTrigger> ();
    SceneTransitionTrigger southAreaTransitonTriggerScript = southTriggerArea.GetComponent<SceneTransitionTrigger> ();
    SceneTransitionTrigger eastAreaTransitonTriggerScript = eastTriggerArea.GetComponent<SceneTransitionTrigger> ();
    SceneTransitionTrigger westAreaTransitonTriggerScript = westTriggerArea.GetComponent<SceneTransitionTrigger> ();
    string northAreaTransitonSceneName = northAreaTransitonTriggerScript.TransitionSceneName;
    string southAreaTransitonSceneName = southAreaTransitonTriggerScript.TransitionSceneName;
    string eastAreaTransitonSceneName = eastAreaTransitonTriggerScript.TransitionSceneName;
    string westAreaTransitonSceneName = westAreaTransitonTriggerScript.TransitionSceneName;

    previousSceneName = GetPreviousSceneName ();

    // Debug.Log ("northAreaTransitonSceneName : " + northAreaTransitonSceneName);
    // Debug.Log ("southAreaTransitonSceneName : " + southAreaTransitonSceneName);
    // Debug.Log ("eastAreaTransitonSceneName : " + eastAreaTransitonSceneName);
    // Debug.Log ("westAreaTransitonSceneName : " + westAreaTransitonSceneName);

    // Debug.Log ("previousSceneName : " + previousSceneName);

    float partnerPositionModification = 1.5f;
    Vector3 partnerPosition = new Vector3 ();

    // if (exploreManager.CurrentExploreState == ExploreManager.ExploreState.fromEscapeTheBattle)
    // {
    //   player.transform.position = exploreManager.PlayerPosition;
    //   partner.transform.position = exploreManager.PartnerPosition;
    // }
    // else if (hanaPlayer.WonTheBattle)
    // {
    //   hanaPlayer.WonTheBattle = false;
    //   player.transform.position = exploreManager.PlayerPosition;
    //   partner.transform.position = exploreManager.PartnerPosition;
    //   // Debug.Log ("partner.transform.position : " + partner.transform.position);
    // }
    // else
    // {
    if (exploreManager.CurrentExploreState == ExploreManager.ExploreState.fromEscapeTheBattle || hanaPlayer.WonTheBattle) return;
    if (previousSceneName == null)
    {
      // Debug.Log ("ここでプレイヤーがセーブデータの位置に戻ってる");
      player.transform.position = SaveSystem.Instance.userData.savedPlayerPosition;
      partner.transform.position = SaveSystem.Instance.userData.savedPartnerPosition;
    }
    else if (previousSceneName == northAreaTransitonSceneName)
    {
      player.transform.position = northTriggerArea.transform.position;
      partnerPosition = northTriggerArea.transform.position;
      // partnerPosition.z
      partner.transform.position = northTriggerArea.transform.position;
    }
    else if (previousSceneName == southAreaTransitonSceneName)
    {
      player.transform.position = southTriggerArea.transform.position;

      partnerPosition = southTriggerArea.transform.position;
      partnerPosition.z -= partnerPositionModification;
      partner.transform.position = partnerPosition;
    }
    else if (previousSceneName == eastAreaTransitonSceneName)
    {
      player.transform.position = eastTriggerArea.transform.position;

      partner.transform.position = eastTriggerArea.transform.position;
    }
    else if (previousSceneName == westAreaTransitonSceneName)
    {
      player.transform.position = westTriggerArea.transform.position;
      partner.transform.position = westTriggerArea.transform.position;
    }
    // }
  }

  string GetPreviousSceneName ()
  {
    string sceneName = null;
    sceneName = PlayerPrefs.GetString ("previousSceneName");
    // Debug.Log ("previousSceneName : " + sceneName);
    if (!sceneName.Contains ("Scene"))
    {
      sceneName = null;
    }
    // bool hasNoScene = sceneName.Contains ("Scene");
    // Debug.Log ("contains Scene : " + test);
    return sceneName;
  }

  void SetPreviousSceneName (string previousSceneName)
  {
    PlayerPrefs.SetString ("previousSceneName", previousSceneName);
  }

  void ManageSceneTransition ()
  {
    if (nextSceneName == null) return;
    if (startLoadingScene == false && fadeManager.EndFadeOut)
    {
      DOTween.Clear ();
      startLoadingScene = true;
      previousSceneName = SceneManager.GetActiveScene ().name;
      // 次のシーンにて、どこのシーンから来たかを知るのに使う
      SetPreviousSceneName (previousSceneName);
      // Debug.Log ("nextSceneName : " + nextSceneName);
      SceneManager.LoadScene (nextSceneName);
    }
  }

  public string NextSceneName
  {
    set { nextSceneName = value; }
    get { return nextSceneName; }
  }

}