using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTargetsCursorAndNameContentsPreparer
{
  private CommonSelectedTargetNameContentsPreparer commonSelectedTargetNameContentsPreparer;
  private GameObject targetCursorPrefab;
  private GameObject selectedTargetNameFramePrefab;
  private GameObject battleCommonOptionMultipleTarget;

  private Transform targetCursors;
  private Transform targetNameFrames;
  public SelectedTargetsCursorAndNameContentsPreparer (
    GameObject battleCommonOptionMultipleTarget,
    GameObject targetCursorPrefab,
    GameObject selectedTargetNameFramePrefab
  )
  {
    commonSelectedTargetNameContentsPreparer = new CommonSelectedTargetNameContentsPreparer ();
    this.battleCommonOptionMultipleTarget = battleCommonOptionMultipleTarget;
    this.targetCursorPrefab = targetCursorPrefab;
    this.selectedTargetNameFramePrefab = selectedTargetNameFramePrefab;
  }

  public void SetupAllTargetsHandCursorAndTargetName (List<GameObject> allTargetList)
  {
    for (var i = 0; i < allTargetList.Count; i++)
    {
      GameObject target = allTargetList[i];

      // 親作成
      targetCursors = battleCommonOptionMultipleTarget.transform.Find ("TargetHandCursors");
      targetNameFrames = battleCommonOptionMultipleTarget.transform.Find ("SelectedTargetNameFrames");

      // サイズキャッシュ
      float baseTargetHandCursorScale = targetCursorPrefab.transform.localScale.x;

      // 作成と親設定
      GameObject targetCursor = GameObject.Instantiate (targetCursorPrefab);
      targetCursor.transform.SetParent (targetCursors);
      GameObject targetNameFrame = GameObject.Instantiate (selectedTargetNameFramePrefab);
      targetNameFrame.transform.SetParent (targetNameFrames);

      // スケール修正
      targetCursor.transform.localScale = new Vector3 (baseTargetHandCursorScale, baseTargetHandCursorScale, baseTargetHandCursorScale);
      float baseScale = 1.0f;
      targetNameFrame.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);

      // 位置変更
      Vector3 cursorTargetPosition = target.transform.Find ("CursorTarget").position;
      targetCursor.GetComponent<RectTransform> ().position = RectTransformUtility.WorldToScreenPoint (Camera.main, cursorTargetPosition);
      Vector3 targetNameFramePosition = new Vector3 (0, 0, 0);
      float frameHeight = targetNameFrame.transform.GetComponent<RectTransform> ().sizeDelta.y;
      targetNameFramePosition.y -= frameHeight * i;
      targetNameFrame.transform.localPosition = targetNameFramePosition;

      // フレーム編集
      GameObject selectedTargetNameText = targetNameFrame.transform.Find ("SelectedTargetNameText").gameObject;
      commonSelectedTargetNameContentsPreparer.ManageSelectedTargetNameText (target, selectedTargetNameText);
      commonSelectedTargetNameContentsPreparer.ManageSelectedTargetNameFrameSize (targetNameFrame, selectedTargetNameText);
    }
  }

  public void RemoveAllHandCursorsAndTargetNames ()
  {
    MenuCommonFunctions menuCommonFunctions = new MenuCommonFunctions ();
    List<GameObject> targetCursorList = menuCommonFunctions.GetChildList (targetCursors.gameObject);
    List<GameObject> targetNameFrameList = menuCommonFunctions.GetChildList (targetNameFrames.gameObject);
    void DestroyAll (List<GameObject> selectList)
    {
      foreach (var select in selectList)
      {
        MonoBehaviour.Destroy (select);
      }
    }
    DestroyAll (targetCursorList);
    DestroyAll (targetNameFrameList);
  }

}