using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class BattleLevelUpManager : MonoBehaviour
{
  [SerializeField] Light gameDirectionalLight;
  [SerializeField] Light gameSpotLightForPlayer;
  [SerializeField] ManagersForPlayerParty managersForPlayerParty;
  [SerializeField] LevelUpCurtainResize levelUpCurtain;
  [SerializeField] Text informingLevelUpText;
  [SerializeField] ParticleSystem confettiParticle;
  [SerializeField] GameObject levelUpOptionDescription;
  [SerializeField] GameObject badgeLevelOption;
  [SerializeField] GameObject heartLevelOption;
  [SerializeField] GameObject flowerLevelOption;
  [SerializeField] GameObject levelUpSpotLight;
  [SerializeField] Transform hanaPlayerStartAppearingPosition;
  [SerializeField] Transform hanaPlayerJumpGoalPosition;

  private BGMManager bgmManager;
  private SEManager seManager;

  private GameObject hanaPlayer;
  private LevelUpTextManager levelUpTextManager;
  private LevelUpOptionSelectMotionManager levelUpOptionSelectMotionManager;
  private LevelUpOptionContentsManager levelUpOptionContentsManager;
  private Vector3 initialLevelUpCurtainPosition;

  private bool startSelectingLevelUpOption;

  void Start ()
  {
    levelUpTextManager = new LevelUpTextManager (informingLevelUpText);
    levelUpOptionSelectMotionManager = new LevelUpOptionSelectMotionManager (
      badgeLevelOption,
      heartLevelOption,
      flowerLevelOption,
      levelUpSpotLight,
      levelUpOptionDescription
    );
    levelUpOptionContentsManager = new LevelUpOptionContentsManager (
      badgeLevelOption,
      heartLevelOption,
      flowerLevelOption,
      levelUpOptionDescription
    );

    // hanaPlayer = GameObject.FindWithTag ("Player");

    initialLevelUpCurtainPosition = levelUpCurtain.gameObject.transform.position;

    gameSpotLightForPlayer.gameObject.SetActive (false);

    startSelectingLevelUpOption = false;

    // bgmManager = BGMManager.Instance;
    seManager = SEManager.Instance;
  }

  void Update ()
  {
    // if (Input.GetKeyDown (KeyCode.L))
    // {
    //   // テスト中
    //   ShowLevelUpPerformance ();
    // }

    bool endSelectingOption = levelUpOptionSelectMotionManager.EndSelecting;
    if (startSelectingLevelUpOption && !endSelectingOption)
    {
      // 選択肢が選べる
      levelUpOptionSelectMotionManager.ManageSelectingOption ();

      // 選択肢の説明更新
      GameObject selectingOption = levelUpOptionSelectMotionManager.GetSelectedLevelUpOption ();
      levelUpOptionContentsManager.ManageLevelUpOptionDescription (selectingOption);

      // 決定
      if (Input.GetKeyDown (KeyCode.Space))
      {
        SEManager.Instance.Play (SEPath.MENU_DECISION);
        levelUpOptionSelectMotionManager.EndSelecting = true;
      }

    }
  }

  public async UniTask ShowLevelUpPerformance (BGMManager bgmManager)
  {
    this.bgmManager = bgmManager;
    // プレイヤーセット
    hanaPlayer = GameObject.FindWithTag ("Player");
    // レベルアップBGM再生
    bgmManager.Play (BGMPath.LEVEL_UP);
    // 画面を暗く 
    float beforeDarkerIntensity = gameDirectionalLight.intensity;
    float darkerLightIntensity = 0.65f;
    await ManageDirectionalLightTween (beforeDarkerIntensity, darkerLightIntensity);

    // カメラを引きに変更
    managersForPlayerParty.battleVirtualCameras.ActivateWholeStageCamera ();

    // カーテンを舞台の真ん中まで移動
    float centerOfStageX = 0.0f;
    float curtainDuration = 1.0f;
    Vector3 curtainPosition = initialLevelUpCurtainPosition;
    curtainPosition.x = centerOfStageX;
    seManager.Play (SEPath.CURTAIN);
    await levelUpCurtain.gameObject.transform.DOMove (curtainPosition, curtainDuration);
    // レベルアップの文字表示
    await levelUpTextManager.ShowInformingLevelUpText ();

    // 花吹雪パーティクル再生
    confettiParticle.Play ();

    // プレイヤーキャラを舞台の真ん中まで移動
    await AppearingPlayer ();

    // テキスト非表示
    levelUpTextManager.HideInformingLevelUpText ();

    // スポットライト解除
    gameSpotLightForPlayer.gameObject.SetActive (false);

    // 少し間を作ってから明かりを戻してカーテンを閉じる
    int hideCuratainDelay = 1000;
    await UniTask.Delay (hideCuratainDelay);

    float beforeBrighterIntensity = gameDirectionalLight.intensity;
    float brighterLightIntensity = 1.0f;
    ManageDirectionalLightTween (beforeBrighterIntensity, brighterLightIntensity);

    // レベルアップ選択肢を表示
    levelUpOptionSelectMotionManager.ShowLevelUpOptions ();

    // カーテンを舞台から隠れる位置に移動(初期位置)
    seManager.Play (SEPath.CURTAIN);
    await levelUpCurtain.gameObject.transform.DOMove (initialLevelUpCurtainPosition, curtainDuration);

    // レベルアップの選択肢開始(キャンバスではなく、3dで作成) 
    startSelectingLevelUpOption = true;



    // 選択肢を選んだことを検知するまで待つ
    await UniTask.WaitUntil (() => levelUpOptionSelectMotionManager.EndSelecting);

    // スポットライト非表示
    levelUpOptionSelectMotionManager.InactivateSpotLight ();
    // 説明文非表示
    levelUpOptionSelectMotionManager.InactivateLevelUpOptionDescription ();
    // 選ばれなかった選択肢を隠す
    levelUpOptionSelectMotionManager.FadeOutNotSelectedOptions ();
    // 選択された選択肢の動き
    await levelUpOptionSelectMotionManager.ApproachingSelectedOption ();
    // 選択された選択肢をプレイヤーがジャンプして叩く
    GameObject selectedOption = levelUpOptionSelectMotionManager.GetSelectedLevelUpOption ();
    await HitSelectedOption (selectedOption);

    // // 選択したオプションに応じて体力等のデータを変更する
    string selectedOptionName = levelUpOptionSelectMotionManager.GetSelectedLevelUpOptionName ();
    LevelUpSaveDataManager levelUpSaveDataManager = new LevelUpSaveDataManager ();
    await levelUpSaveDataManager.LevelUpPlayerStatus (selectedOptionName);

    // // HPの回復とFPの回復のモーションを再生する
    await RecoveringCompletelyMotion ();
    //終了
    Debug.Log ("レベルアップ終了");
  }

  async UniTask ManageDirectionalLightTween (float fromIntensity, float toIntensity)
  {
    float intensityDuration = 1.0f;
    await DOTween.To (
      () => fromIntensity,
      x =>
      {
        gameDirectionalLight.intensity = x;
      },
      toIntensity,
      intensityDuration
    );
  }

  async UniTask AppearingPlayer ()
  {
    hanaPlayer.transform.position = hanaPlayerStartAppearingPosition.position;

    // ライトセット
    Vector3 spotLightPosition = gameSpotLightForPlayer.transform.position;
    float spotLightPositionZ = spotLightPosition.z;
    spotLightPosition = hanaPlayerStartAppearingPosition.position;
    spotLightPosition.z = spotLightPositionZ;
    gameSpotLightForPlayer.transform.position = spotLightPosition;
    gameSpotLightForPlayer.gameObject.SetActive (true);

    // 移動
    // スポットライトもプレイヤーキャラに当たるように移動しながら表示
    float centerOfTheStageX = 0.0f;
    float appearingDuration = 2.3f;
    await DOTween.Sequence ().Append (hanaPlayer.transform.DOMoveX (centerOfTheStageX, appearingDuration))
      .Join (gameSpotLightForPlayer.transform.DOMoveX (centerOfTheStageX, appearingDuration));
  }

  async UniTask HitSelectedOption (GameObject selectedOption)
  {
    Vector3 playerPositionBeforeJump = hanaPlayer.transform.position;
    Vector3 selectedOptionPositionBeforeHit = selectedOption.transform.position;

    // ジャンプする
    float jumpDuration = 0.4f;
    await hanaPlayer.transform.DOMove (hanaPlayerJumpGoalPosition.position, jumpDuration);

    //  叩かれたタイミングでカメラが上下に揺れる

    // 叩かれた選択肢が動く
    seManager.Play (SEPath.HIT_LEVEL_UP_OPTION);
    Vector3 selectedOptionUpPosition = selectedOption.transform.position;
    float recoilDuration = 0.2f;
    selectedOptionUpPosition.y += 0.15f;
    DOTween.Sequence ().Append (selectedOption.transform.DOMove (selectedOptionUpPosition, recoilDuration))
      .Append (selectedOption.transform.DOMove (selectedOptionPositionBeforeHit, recoilDuration));

    // 着地する
    float landingDuration = 0.3f;
    await hanaPlayer.transform.DOMove (playerPositionBeforeJump, landingDuration);
  }

  async UniTask RecoveringCompletelyMotion ()
  {
    RecoveryMotion recoveryMotion = hanaPlayer.transform.GetComponent<RecoveryMotion> ();
    MyGameData.MyData data = SaveSystem.Instance.userData;
    int maxHp = data.playerMaxHp;
    int maxFp = data.playerMaxFp;
    await recoveryMotion.RecoveringCompletely (hanaPlayer, maxHp, maxFp);
  }
}