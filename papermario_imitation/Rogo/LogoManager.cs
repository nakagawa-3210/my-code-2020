using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
  async UniTask Start ()
  {
    await UniTask.DelayFrame (1);
    DOTween.Clear ();
    SceneManager.LoadScene ("TitleScene");
  }

  // void Update ()
  // {
  //   if (Input.GetKeyDown (KeyCode.Space))
  //   {
  //     SceneManager.LoadScene ("TitleScene");
  //   }
  // }
}