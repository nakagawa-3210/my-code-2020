using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadSceneEventsManager : MonoBehaviour
{
  [SerializeField] CurtainManager curtainManager;

  // void Update ()
  // {
  // }

  public void OnClickGoToTitleButton ()
  {
    GoToTitle ();
  }
  public void OnClickGoToContinueButton ()
  {
    GoToExploreScene ();
  }

  async UniTask GoToTitle ()
  {
    await curtainManager.ShowAllCurtains ();
    SceneManager.LoadScene ("Title");
  }

  async UniTask GoToExploreScene ()
  {
    await curtainManager.ShowAllCurtains ();
    // セーブデータに合わせて遷移する探索シーン変更
    SceneManager.LoadScene ("VillageEntrance");
  }

}