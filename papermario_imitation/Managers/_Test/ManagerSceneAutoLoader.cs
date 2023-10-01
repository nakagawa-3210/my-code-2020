using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneAutoLoader
{
  // ゲームの管理方法変更が終わるまでコメントアウト
  // [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
  // private static void LoadManagerScene ()
  // {
  //   string managerSceneName = "ManagerScene";
  //   Scene managerScene = SceneManager.GetSceneByName (managerSceneName);
  //   // ヒエラルキー上に有効なマネージャーシーンがない場合のみにマネージャーシーンをロード
  //   if (!managerScene.IsValid ())
  //   {
  //     SceneManager.LoadScene (managerSceneName, LoadSceneMode.Additive);
  //   }
  // }
}