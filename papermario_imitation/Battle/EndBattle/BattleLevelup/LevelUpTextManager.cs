using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpTextManager
{
  private Text informingLevelUpText;
  private Vector3 initialHidePosition;
  private Vector3 showingInformingLevelUpTextPosition;
  public LevelUpTextManager (Text informingLevelUpText)
  {
    this.informingLevelUpText = informingLevelUpText;
    SetupInformingLevelUpText ();
  }

  void SetupInformingLevelUpText ()
  {
    showingInformingLevelUpTextPosition = informingLevelUpText.gameObject.transform.position;
    initialHidePosition = informingLevelUpText.gameObject.transform.position;
    initialHidePosition.y += Screen.height / 2;
    informingLevelUpText.gameObject.transform.position = initialHidePosition;
  }

  public async UniTask ShowInformingLevelUpText ()
  {
    float showingDuration = 0.8f;
    await informingLevelUpText.gameObject.transform.DOMove (showingInformingLevelUpTextPosition, showingDuration);
  }

  public void HideInformingLevelUpText ()
  {
    float showingDuration = 0.8f;
    // Vector3 hidePosition = informingLevelUpText.gameObject.transform.position;
    // hidePosition.x
    informingLevelUpText.gameObject.transform.DOMove (initialHidePosition, showingDuration);
  }

}