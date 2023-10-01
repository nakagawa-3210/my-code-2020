using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
  [SerializeField] GameOverCanvasUi gameOverCanvasUi = default;
  [SerializeField] ScreenEffectUi screenEffectUi = default;
  [SerializeField] GameConversationManager gameConversationManager = default;

  async UniTask Start ()
  {
    // await screenEffectUi.fadeManager.FadeIn ();
    await UniTask.WaitUntil (() => screenEffectUi.fadeManager.EndFadeIn);
    Debug.Log ("フェードしゅうりょう");
    // // 会話開始
    string fileName = "gameClear";
    gameConversationManager.OpenConversationCanvas (fileName);
    await UniTask.WaitUntil (() => gameConversationManager.EndConversation);

    BGMManager bgmManager = BGMManager.Instance;
    string nextSceneName = "TitleScene";
    async UniTask GoToNextScene ()
    {
      await screenEffectUi.fadeManager.FadeOut ();
      string bgmPath = new BgmPathGetterWithTransitionSceneName ().GetBgmPathWithTransitionSceneName (nextSceneName);
      bgmManager.Play (bgmPath);
      SceneManager.LoadScene (nextSceneName);
    }
    bgmManager.FadeOut (1.0f, () =>
    {
      GoToNextScene ();
    });
  }
}