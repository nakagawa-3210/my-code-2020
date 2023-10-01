using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
  // 下のはparent使って親オブジェクト参照でいいかもしれない
  [SerializeField] GameObject sceneTransitionManager = default;
  [SerializeField] FadeManager fadeManager = default;
  [SerializeField] GameObject playerEnterArea = default;
  [SerializeField] GameObject playerExitArea = default;
  [SerializeField] GameObject player = default;
  [SerializeField] float approachSpeed = default;
  [SerializeField] string transitionSceneName = default;

  private Transform playerEnterAreaTran;
  private Transform playerExitAreaTran;
  private HanaPlayerScript hanaPlayerScript;
  private ApproachMovement approachMovement;
  // private FadeController_Test fadeController;
  private SceneTransitionManager sceneTransitionManagerScript;
  private EndSceneEnterDetector endSceneEnterDetector;
  // private BgmPathGetterWithTransitionSceneName bgmPathGetterWithTransitionSceneName;
  private bool endEnterMotion;
  private bool startEnterTransition;
  private bool startExitTransition;
  void Start ()
  {
    // 遷移先の指定がない場合はアクティブにしない
    if (transitionSceneName == string.Empty)
    {
      gameObject.SetActive (false);
    }
    else
    {
      endEnterMotion = false;
      startEnterTransition = false;
      startExitTransition = false;
      playerEnterAreaTran = playerEnterArea.transform;
      playerExitAreaTran = playerExitArea.transform;
      approachMovement = new ApproachMovement (player);
      hanaPlayerScript = player.GetComponent<HanaPlayerScript> ();
      // fadeController = screenEffectFade.GetComponent<FadeController_Test> ();
      sceneTransitionManagerScript = sceneTransitionManager.GetComponent<SceneTransitionManager> ();
      endSceneEnterDetector = playerEnterArea.GetComponent<EndSceneEnterDetector> ();
    }
  }

  void Update ()
  {
    endEnterMotion = endSceneEnterDetector.EnterArea;
    WalkingToEnterArea ();
    WalkingToExitArea ();
  }

  public string TransitionSceneName
  {
    get { return transitionSceneName; }
  }

  void WalkingToExitArea ()
  {
    if (!startExitTransition) return;
    approachMovement.Approach (playerExitAreaTran, player, approachSpeed);
  }

  void WalkingToEnterArea ()
  {
    if (!startEnterTransition || endEnterMotion) return;
    approachMovement.Approach (playerEnterAreaTran, player, approachSpeed);
  }

  async UniTask OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      if (endEnterMotion)
      {
        hanaPlayerScript.PlayerState = HanaPlayerScript.State.WalkingNextScene;
        startExitTransition = true;
        BGMManager bgmManager = BGMManager.Instance;
        bgmManager.FadeOut ();
        await fadeManager.FadeOut ();
        string pathName = new BgmPathGetterWithTransitionSceneName ().GetBgmPathWithTransitionSceneName (transitionSceneName);
        bgmManager.Play (pathName);
        sceneTransitionManagerScript.NextSceneName = transitionSceneName;
      }
      else
      {
        hanaPlayerScript.PlayerState = HanaPlayerScript.State.WalkingNextScene;
        startEnterTransition = true;
      }
    }
  }

}