using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

// バトルでの攻撃や防御に応じて表示を管理する必要があるUIを管理する
// 攻撃時にはプレイヤーのステータスは非表示にし、攻撃の合図と攻撃説明は表示
// アクション成功を示す文字(ナイス！)とダメージ量を表示する等
public class ActionUiMotionManager : MonoBehaviour
{
  [SerializeField] GameObject jumpActionSign;
  [SerializeField] GameObject hammerActionSign;
  [SerializeField] GameObject escapeActionSign;
  [SerializeField] GameObject dictionaryActionSign;
  [SerializeField] float singTweenDuration = 0.8f;
  [SerializeField] GameObject wholePlayerStatus;
  [SerializeField] float wholePlayerStatusTweenDuration = 1.0f;

  private GameObject normalJumpSign;
  private GameObject pressedJumpSign;
  private GameObject enemyDictionaryFrame;
  private GameObject enemyDictionaryCursor;

  private Image normalJumpSignImage;
  private Image pressedJumpSignImage;

  private Vector3 jumpSignShowingPosition;
  private Vector3 jumpSignOutOfScreenPosition;
  private Vector3 hammerSignShowingPosition;
  private Vector3 hammerSignOutOfScreenPosition;
  private Vector3 dictionaryFrameShowingPosition;
  private Vector3 dictionaryFrameOutOfScreenPosition;
  private Vector3 wholePlayerShowingPosition;
  private Vector3 wholePlayerOutOfScreenPosition;

  private Sequence spaceButtonPushLoop;

  private float hidePositionX;
  void Start ()
  {
    hidePositionX = Screen.width / 2.5f;
    SetupJumpSignPosition ();
    SetupHammerSignPosition ();
    SetupEscapeSign ();
    SetupDictionaryActionSign ();
    SetupWholePlayerStatusPosition ();
  }

  void SetupJumpSignPosition ()
  {
    normalJumpSign = jumpActionSign.transform.Find ("SpacekeyImage").gameObject;
    normalJumpSignImage = normalJumpSign.GetComponent<Image> ();
    pressedJumpSign = jumpActionSign.transform.Find ("PressedSpacekeyImage").gameObject;
    pressedJumpSignImage = pressedJumpSign.GetComponent<Image> ();

    jumpSignShowingPosition = jumpActionSign.transform.position;
    jumpSignOutOfScreenPosition = jumpActionSign.transform.position;
    jumpSignOutOfScreenPosition.x -= hidePositionX;
    jumpActionSign.transform.position = jumpSignOutOfScreenPosition;
    // ShowNormalJumpActionSign (); // デフォルトでノーマル状態を表示
  }

  void SetupHammerSignPosition ()
  {
    // 表示位置キャッシュ
    hammerSignShowingPosition = hammerActionSign.transform.position;
    // 非表示位置キャッシュ
    hammerSignOutOfScreenPosition = hammerActionSign.transform.position;
    hammerSignOutOfScreenPosition.x -= hidePositionX;
    // 非表示位置に移動
    hammerActionSign.transform.position = hammerSignOutOfScreenPosition;
  }

  void SetupEscapeSign ()
  {
    escapeActionSign.SetActive (false);
  }

  void SetupDictionaryActionSign ()
  {
    enemyDictionaryCursor = dictionaryActionSign.transform.Find ("ActionSign").gameObject;
    enemyDictionaryCursor.SetActive (false);

    enemyDictionaryFrame = dictionaryActionSign.transform.Find ("EnemyDictionaryFrame").gameObject;
    dictionaryFrameShowingPosition = enemyDictionaryFrame.transform.position;
    dictionaryFrameOutOfScreenPosition = enemyDictionaryFrame.transform.position;
    dictionaryFrameOutOfScreenPosition.x += Screen.width;
    enemyDictionaryFrame.transform.position = dictionaryFrameOutOfScreenPosition;
  }

  void SetupWholePlayerStatusPosition ()
  {
    wholePlayerShowingPosition = wholePlayerStatus.transform.position;
    wholePlayerOutOfScreenPosition = wholePlayerStatus.transform.position;
    wholePlayerOutOfScreenPosition.y = Screen.height;
    // 移動
    // wholePlayerStatus.transform.position = wholePlayerOutOfScreenPosition;
  }

  public void ShowNormalJumpActionSign ()
  {
    normalJumpSignImage.enabled = true;
    pressedJumpSignImage.enabled = false;
  }

  public void ShowPressedJumpActionSign ()
  {
    normalJumpSignImage.enabled = false;
    pressedJumpSignImage.enabled = true;
  }

  void ShowActionImage (GameObject actionImage, Vector3 showPosition)
  {
    actionImage.transform.DOMove (showPosition, singTweenDuration);
  }

  async UniTask HideActionSign (GameObject actionSign, Vector3 hidePosition)
  {
    await actionSign.transform.DOMove (hidePosition, singTweenDuration);
  }

  public void ShowJumpActionSign ()
  {
    ShowNormalJumpActionSign ();
    ShowActionImage (jumpActionSign, jumpSignShowingPosition);
  }

  public async UniTask HideJumpActionSign ()
  {
    await HideActionSign (jumpActionSign, jumpSignOutOfScreenPosition);
  }

  public void ShowHammerActionSign ()
  {
    ShowActionImage (hammerActionSign, hammerSignShowingPosition);
  }

  public async UniTask HideHammerActionSign ()
  {
    await HideActionSign (hammerActionSign, hammerSignOutOfScreenPosition);
  }

  public void ShowEscapeActionSign ()
  {
    escapeActionSign.SetActive (true);
  }

  // たしか失敗したときだけ非表示にしていたはず
  public void HideEscapeActionSign ()
  {
    escapeActionSign.SetActive (false);
  }

  public void ShowDictionaryCursor (GameObject target)
  {
    enemyDictionaryCursor.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, target.transform.position);
    enemyDictionaryCursor.SetActive (true);
  }
  public void HideDictionaryCursor ()
  {
    enemyDictionaryCursor.SetActive (false);
  }

  public void ShowDictionary ()
  {
    ShowActionImage (enemyDictionaryFrame, dictionaryFrameShowingPosition);
  }

  public async UniTask HideDictionary ()
  {
    await HideActionSign (enemyDictionaryFrame, dictionaryFrameOutOfScreenPosition);
  }

  public void ShowWholePlayerStatus ()
  {
    wholePlayerStatus.transform.DOMove (wholePlayerShowingPosition, wholePlayerStatusTweenDuration / 2);
  }

  public void HideWholePlayerStatus ()
  {
    wholePlayerStatus.transform.DOMove (wholePlayerOutOfScreenPosition, wholePlayerStatusTweenDuration);
  }
}