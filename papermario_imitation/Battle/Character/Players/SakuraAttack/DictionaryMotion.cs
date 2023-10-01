using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class DictionaryMotion : MonoBehaviour
{
  [SerializeField] Sprite dictionarySprite;
  private GameObject self;
  private GameObject itemImage;
  private GameObject itemFocus;
  // private ReactionMotion reactionMotion;

  void Start ()
  {
    self = this.gameObject;
    GameObject battleItemUsingUiCanvas = GameObject.Find ("BattleItemUsingUiCanvas");
    itemImage = battleItemUsingUiCanvas.transform.Find ("ItemImage").gameObject;
    itemFocus = battleItemUsingUiCanvas.transform.Find ("ItemFocusGroup").gameObject;
  }

  // アイテムのように本を頭上に表示
  public async UniTask ShowDictionary ()
  {
    await new CommonItemMotion ().ShowingUsingItemImage (self, itemImage, itemFocus, dictionarySprite);
  }

  // 頭上にはてなマークを出す
  public async UniTask FailedSearch (ReactionMotion reactionMotion)
  {
    // this.reactionMotion = reactionMotion;
    await reactionMotion.ShowingReactionQuestionMark ();
  }

}