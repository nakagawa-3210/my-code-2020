using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

public class TargetOptionCursorManager
{
  private GameObject targetHandCursor;
  private List<GameObject> playerPartyList;
  private List<GameObject> enemyPartyList;
  private List<Vector3> playerPartyCursorPositionList;
  private List<Vector3> enemyPartyCursorPositionList;
  private int selectedTargetNum;
  private int selectedTargetMaxNum;
  private int selectedPlayerPartyTargetMaxNum;
  private int selectedEnemyPartyTargetMaxNum;
  private bool isSelectingFromPlayerParty;

  public int SelectedTargetNum
  {
    get { return selectedTargetNum; }
  }

  public TargetOptionCursorManager (
    GameObject targetHandCursor
  )
  {
    this.targetHandCursor = targetHandCursor;
    playerPartyCursorPositionList = new List<Vector3> ();
    enemyPartyCursorPositionList = new List<Vector3> ();
    // デフォルトで先頭の選択肢を選ぶ
    selectedTargetNum = 0;
    selectedTargetMaxNum = 0;
    selectedPlayerPartyTargetMaxNum = 0;
    selectedEnemyPartyTargetMaxNum = 0;
  }

  void SetupTargetMaxNum ()
  {
    if (isSelectingFromPlayerParty)
    {
      selectedTargetMaxNum = selectedPlayerPartyTargetMaxNum;
      // Debug.Log ("selectedTargetMaxNum : " + selectedTargetMaxNum);
    }
    else
    {
      selectedTargetMaxNum = selectedEnemyPartyTargetMaxNum;
    }
  }

  public void ManageTargetHandCurSorPosition ()
  {
    ManageSelectedTargetNum ();
    SetTargetHandCursorPosition ();
  }

  public void ResetSelectedTargetNum ()
  {
    selectedTargetNum = 0;
  }

  // 何番目のターゲットを選択しているかの数字を管理
  public void ManageSelectedTargetNum ()
  {
    // ターゲットの選択肢が一つしかない場合は処理しない
    if (selectedTargetMaxNum == 0) return;
    if (Input.GetKeyDown (KeyCode.RightArrow))
    {
      selectedTargetNum++;
      SEManager.Instance.Play (SEPath.MENU_CURSOR);
      if (selectedTargetNum > selectedTargetMaxNum)
      {
        selectedTargetNum = 0;
      }
      // Debug.Log ("みぎへ移動");
    }
    else if (Input.GetKeyDown (KeyCode.LeftArrow))
    {
      selectedTargetNum--;
      SEManager.Instance.Play (SEPath.MENU_CURSOR);
      if (selectedTargetNum < 0)
      {
        selectedTargetNum = selectedTargetMaxNum;
      }
      // Debug.Log ("ひだりへ移動");
    }
  }

  // selectedTargetNumに応じてターゲットハンドカーソルの位置管理
  void SetTargetHandCursorPosition ()
  {
    if (isSelectingFromPlayerParty)
    {
      targetHandCursor.GetComponent<RectTransform> ().position =
        RectTransformUtility.WorldToScreenPoint (Camera.main, playerPartyCursorPositionList[selectedTargetNum]);
    }
    else
    {
      targetHandCursor.GetComponent<RectTransform> ().position =
        RectTransformUtility.WorldToScreenPoint (Camera.main, enemyPartyCursorPositionList[selectedTargetNum]);
    }
  }

  public void SetupCursorPositionLists (List<GameObject> playerPartyList, List<GameObject> enemyPartyList, bool isSelectingFromPlayerParty)
  {
    this.playerPartyList = playerPartyList;
    this.enemyPartyList = enemyPartyList;
    this.isSelectingFromPlayerParty = isSelectingFromPlayerParty;
    List<Vector3> newEnemyPartyCursorTargetPositionList = new List<Vector3> ();
    List<Vector3> newPlayerPartyCursorTargetPositionList = new List<Vector3> ();
    // プレイヤー分はまだ用意出来てないのでコメントアウト中
    foreach (var member in playerPartyList)
    {
      Vector3 memberCursorTargetPosition = member.transform.Find ("CursorTarget").position;
      newPlayerPartyCursorTargetPositionList.Add (memberCursorTargetPosition);
    }
    foreach (var enemyMember in enemyPartyList)
    {
      Vector3 memberCursorTargetPosition = enemyMember.transform.Find ("CursorTarget").position;
      newEnemyPartyCursorTargetPositionList.Add (memberCursorTargetPosition);
    }
    playerPartyCursorPositionList = newPlayerPartyCursorTargetPositionList;
    enemyPartyCursorPositionList = newEnemyPartyCursorTargetPositionList;
    // リストのカウントは0から始まるので-1
    selectedPlayerPartyTargetMaxNum = playerPartyCursorPositionList.Count - 1;
    selectedEnemyPartyTargetMaxNum = enemyPartyCursorPositionList.Count - 1;
    SetupTargetMaxNum ();
  }

}