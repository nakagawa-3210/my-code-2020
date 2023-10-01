using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SelectedCharacterNameContentsPreparerとかに変更する
// 敵の全選択、プレイヤーパーティーの全選択はまた別のクラスで行う
public class SelectedTargetNameContentsPreparer
{
  private CommonSelectedTargetNameContentsPreparer commonSelectedTargetNameContentsPreparer;
  private GameObject selectedTargetNameFrame;
  private GameObject selectedTargetNameText;
  public SelectedTargetNameContentsPreparer (
    GameObject selectedTargetNameFrame
  )
  {
    commonSelectedTargetNameContentsPreparer = new CommonSelectedTargetNameContentsPreparer ();
    this.selectedTargetNameFrame = selectedTargetNameFrame;
    selectedTargetNameText = selectedTargetNameFrame.transform.Find ("SelectedTargetNameText").gameObject;
  }

  public void ManageSelectedEnemyName (
    List<GameObject> battlePartyList,
    int selectedTargetNumber
  )
  {
    if (battlePartyList == null || battlePartyList.Count == 0) return;
    GameObject selectedTarget = battlePartyList[selectedTargetNumber];
    commonSelectedTargetNameContentsPreparer.ManageSelectedTargetInformation (selectedTarget, selectedTargetNameText, selectedTargetNameFrame);
    // 名前テキストの変更
    // commonSelectedTargetNameContentsPreparer.ManageSelectedTargetNameText (selectedTarget, selectedTargetNameText);
    // // 名前テキストの文字数に合わせてフレームサイズ変更
    // commonSelectedTargetNameContentsPreparer.ManageSelectedTargetNameFrameSize (selectedTargetNameFrame, selectedTargetNameText);
  }

}