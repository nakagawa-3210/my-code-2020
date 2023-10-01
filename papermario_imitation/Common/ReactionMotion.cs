using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class ReactionMotion
{
  private GameObject self;
  private GameObject reactionSign;
  private string commonPath;

  public ReactionMotion (GameObject self)
  {
    this.self = self;
    reactionSign = GameObject.Find ("ReactionSign");
    reactionSign.SetActive (false);
    commonPath = "Common/reaction/";
  }

  public async UniTask ShowingReactionExclamationMark ()
  {
    Sprite exclamation = Resources.Load<Sprite> (commonPath + "exclamation");
    await ShowReactionMark (exclamation);
  }

  public async UniTask ShowingReactionLightBulbMark ()
  {
    Sprite lightBulb = Resources.Load<Sprite> (commonPath + "lightBulb");
    await ShowReactionMark (lightBulb);
  }

  public async UniTask ShowingReactionQuestionMark ()
  {
    Sprite question = Resources.Load<Sprite> (commonPath + "question");
    await ShowReactionMark (question);
  }

  async UniTask ShowReactionMark (Sprite reactionSprite)
  {
    // 画像設定
    reactionSign.GetComponent<Image> ().sprite = reactionSprite;
    // 位置変更
    Vector3 reactionPosition = self.transform.position;
    float modifierX = 0.6f;
    float modifierY = 0.7f;
    reactionPosition.x += modifierX;
    reactionPosition.y += modifierY;
    reactionSign.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, reactionPosition);
    reactionSign.SetActive (true);

    int showDelay = 300;
    await UniTask.Delay (showDelay);
    reactionSign.SetActive (false);
  }

}