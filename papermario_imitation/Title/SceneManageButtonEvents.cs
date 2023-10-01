using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageButtonEvents : MonoBehaviour
{
  [SerializeField] TitleCanvasUi titleCanvasUi = default;
  [SerializeField] GameOverCanvasUi gameOverCanvasUi = default;
  [SerializeField] ScreenEffectUi screenEffectUi = default;

  private BGMManager bgmManager;
  private TitleMotionManager titleMotionManager;
  private GameOverMotionManager gameOverMotionManager;

  public TitleMotionManager TitleMotionManagerForButtonEvents
  {
    set { titleMotionManager = value; }
  }

  public GameOverMotionManager GameOverMotionManagerForButtonEvents
  {
    set { gameOverMotionManager = value; }
  }

  void Start ()
  {
    bgmManager = BGMManager.Instance;

    string currentSceneName = SceneManager.GetActiveScene ().name;
    string bgmPath = new BgmPathGetterWithTransitionSceneName ().GetBgmPathWithTransitionSceneName (currentSceneName);
    bgmManager.Play (bgmPath);

    // webglのためにファイルの有無確認テスト
    string userJsonPath = Application.persistentDataPath + "/paperHanadaGameData.json";
    // Debug.Log ("File.Exists (userJsonPath) : " + File.Exists (userJsonPath));
  }

  public void OnClickStartNewGameButton ()
  {
    if (screenEffectUi.fadeManager.StartFadeTween) return;
    AgreeToLoadingNewGame ();
  }

  async UniTask AgreeToLoadingNewGame ()
  {
    await titleCanvasUi.dialogManager.HideDialog ();
    LoadNewGame ();
  }

  public void OnClickStartButton ()
  {
    if (screenEffectUi.fadeManager.StartFadeTween) return;

    bool hasDataFile = new CommonUiFunctions ().HasGameDataFile ();
    if (hasDataFile)
    {
      OpenWarningDialog ();
    }
    else
    {
      LoadSavedScene ();
    }
  }

  public void OnClickCancelStartNewGameButton ()
  {
    if (screenEffectUi.fadeManager.StartFadeTween) return;
    CloseWarningDialog ();
  }

  public void OnClickContinueButton ()
  {
    if (screenEffectUi.fadeManager.StartFadeTween) return;
    if (titleCanvasUi != default)
    {
      titleMotionManager.EndSelectingOption ();
    }
    else if (gameOverCanvasUi != default)
    {
      gameOverMotionManager.EndSelectingOption ();
    }
    LoadSavedScene ();
  }

  public void OnClickTitleButton ()
  {
    if (screenEffectUi.fadeManager.StartFadeTween) return;
    gameOverMotionManager.EndSelectingOption ();
    LoadTitleScene ();
  }

  // ダイアログを表示非表示の際に、ダイアログのtweenが終了している事を条件に加えてボタン制御 
  async UniTask OpenWarningDialog ()
  {
    if (!titleCanvasUi.dialogManager.EndTween) return;
    titleMotionManager.CanNotSelectAnyButton ();
    await titleCanvasUi.dialogManager.OpenDialog ();
    titleMotionManager.SelectingFromNewGameDialogOptions ();
  }

  async UniTask CloseWarningDialog ()
  {
    if (!titleCanvasUi.dialogManager.EndTween) return;
    titleMotionManager.CanNotSelectAnyButton ();
    await titleCanvasUi.dialogManager.CloseDialog ();
    titleMotionManager.SelectingFromTitleOptions ();
  }

  async UniTask LoadTitleScene ()
  {
    await screenEffectUi.fadeManager.FadeOut ();
    string sceneName = "TitleScene";
    CommonLoadSceneTask (sceneName);
  }

  async UniTask LoadNewGame ()
  {
    string userJsonPath = Application.persistentDataPath + "/paperHanadaGameData.json";
    Debug.Log ("File.Exists (userJsonPath) : " + File.Exists (userJsonPath));
    File.Delete (userJsonPath);
    await UniTask.WaitUntil (() => !File.Exists (userJsonPath));
    // await UniTask.DelayFrame (1);
    // Debug.Log ("File.Exists (userJsonPath) : " + File.Exists (userJsonPath));
    SaveSystem.Instance.Load ();
    // Debug.Log ("SaveSystem.Instance.userData.experience : " + SaveSystem.Instance.userData.savedPlayerPosition);
    CommonLoadSavedSceneTask ();
  }

  void LoadSavedScene ()
  {
    CommonLoadSavedSceneTask ();
  }

  async UniTask CommonLoadSavedSceneTask ()
  {
    await screenEffectUi.fadeManager.FadeOut ();
    PlayerPrefs.DeleteAll ();

    string sceneName = SaveSystem.Instance.userData.savePointSceneName;

    CommonLoadSceneTask (sceneName);
  }

  void CommonLoadSceneTask (string sceneName)
  {
    string bgmPath = new BgmPathGetterWithTransitionSceneName ().GetBgmPathWithTransitionSceneName (sceneName);

    void PlayNextSceneBgmAndGoToNextScene ()
    {
      DOTween.Clear ();
      bgmManager.Play (bgmPath);
      SceneManager.LoadScene (sceneName);
    }

    bgmManager.FadeOut (1.0f, () =>
    {
      PlayNextSceneBgmAndGoToNextScene ();
    });
  }
}