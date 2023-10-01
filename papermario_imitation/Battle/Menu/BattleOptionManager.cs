using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// モーションとコンテンツの処理の順序や関係を考える必要があるとき、メニューのInputを管理するときにこのクラスで管理
// プレイヤーとパートナーで処理を分岐させるだけなので、親クラスは作らない(仕組み自体が完成したら改修するかもしれない)
public class BattleOptionManager : MonoBehaviour
{
  [SerializeField] bool isPlayer;
  [SerializeField] bool isPartner;
  [SerializeField] BattleOptionMotionManager battleOptionMotionManager;
  [SerializeField] BattleOptionContentsManager battleOptionContentsManager;
  private MenuCommonFunctions menuCommonFunctions;

  private int selectedOptionNum;
  private bool isShowingAllTargets;

  void Start ()
  {
    selectedOptionNum = 0;
    isShowingAllTargets = false;
    menuCommonFunctions = new MenuCommonFunctions ();
  }

  // 選択された
  void Update ()
  {
    // selectedTargetNumをこのクラスで保持して、ChangeCurrentOptionContentsのようにするかもしれない
    SetContentsManagerTargetInformation ();
    ChangeCurrentOptionContents ();
    ManageOptionFlows ();
    // 選択肢が選ばれたら
    //    選択肢を縮小で非表示
    //    選択肢ふきだしの移動(移動中のみx軸回転を0度に変更し、移動終了後戻す)
    //    新たな選択肢リストの表示
  }

  // モーションで選択中の敵番号引き渡し
  void SetContentsManagerTargetInformation ()
  {
    battleOptionContentsManager.SelectedTargetNum =
      battleOptionMotionManager.SelectedTargetNum;
    battleOptionContentsManager.BattleTargetCharacterList =
      battleOptionMotionManager.SelectabelBattleTargetCharacterList;
  }

  void ManageOptionFlows ()
  {
    BattleOptionMotionManager.BattleMenuState currentPlayerState = battleOptionMotionManager.CurrentBattleMenuState;
    if (Input.GetKeyDown (KeyCode.Space))
    {
      battleOptionMotionManager.ManageFirstOption ();
      battleOptionMotionManager.ManageTargetOption ();
      battleOptionMotionManager.ManageAllTargetsOption();
    }
    else if (Input.GetKeyDown (KeyCode.Backspace))
    {
      // 選択していたリストボタンリセット
      battleOptionContentsManager.ResetIsSelectedInformation ();
      if (currentPlayerState == BattleOptionMotionManager.BattleMenuState.targetOption)
      {
        battleOptionMotionManager.ManageBackToListOption ();
      }
      // たぶんBattleMenuState.targetOptionのときと同じ
      else if (currentPlayerState == BattleOptionMotionManager.BattleMenuState.allTargetOption)
      {
        battleOptionContentsManager.ResetpAllTargetsInformation ();
        battleOptionMotionManager.HideAllTargetsSelectingInformation ();
        battleOptionMotionManager.ManageBackToListOption ();
      }
      else if (currentPlayerState != BattleOptionMotionManager.BattleMenuState.firstOption ||
        currentPlayerState != BattleOptionMotionManager.BattleMenuState.targetOption)
      {
        battleOptionMotionManager.ManageBackToFirstPlayerOption ();
      }
    }
    ManageAllTargetsOption (currentPlayerState);
    // ボタンのリセットを確認してからリストの管理
    battleOptionMotionManager.ManageButtonListOption ();
  }

  // 全選択する対象のターゲットカーソルと名前を生成する
  void ManageAllTargetsOption (BattleOptionMotionManager.BattleMenuState currentPlayerState)
  {
    bool isSlectingFromPlayerParty = battleOptionMotionManager.IsSelectingFromPlayerParty;
    if (currentPlayerState == BattleOptionMotionManager.BattleMenuState.allTargetOption && !isShowingAllTargets)
    {
      // Debug.Log ("全体表示呼ばれた");
      isShowingAllTargets = true;
      battleOptionContentsManager.SetupAllTargetsInformation (isSlectingFromPlayerParty);
    }
    else if (currentPlayerState != BattleOptionMotionManager.BattleMenuState.allTargetOption && isShowingAllTargets)
    {
      isShowingAllTargets = false;
    }
  }

  void ChangeCurrentOptionContents ()
  {
    if (isPlayer)
    {
      ChangePlayerCurrentOptionContents ();
    }
    else if (isPartner)
    {
      ChangePartnerCurrentOptionContents ();
    }
  }

  void ChangePlayerCurrentOptionContents ()
  {
    // モーションから数字取得
    // プレイヤーと仲間で異なる
    selectedOptionNum = battleOptionMotionManager.SelectedOptionNum;
    int jump = 0;
    int hammer = 1;
    int item = 2;
    int strategy = 3;
    // 数字に応じてコンテンツ変更
    if (selectedOptionNum == jump)
    {
      battleOptionContentsManager.ChangeCurrentOptionToJump ();
    }
    if (selectedOptionNum == hammer)
    {
      battleOptionContentsManager.ChangeCurrentOptionToHammer ();
    }
    if (selectedOptionNum == item)
    {
      battleOptionContentsManager.ChangeCurrentOptionToItem ();
    }
    if (selectedOptionNum == strategy)
    {
      battleOptionContentsManager.ChangeCurrentOptionToStrategy ();
    }
  }

  void ChangePartnerCurrentOptionContents ()
  {
    selectedOptionNum = battleOptionMotionManager.SelectedOptionNum;
    int skill = 0;
    int item = 1;
    int strategy = 2;
    if (selectedOptionNum == skill)
    {
      battleOptionContentsManager.ChangeCurrentOptionToSkill ();
    }
    if (selectedOptionNum == item)
    {
      battleOptionContentsManager.ChangeCurrentOptionToItem ();
    }
    if (selectedOptionNum == strategy)
    {
      battleOptionContentsManager.ChangeCurrentOptionToStrategy ();
    }
  }
}