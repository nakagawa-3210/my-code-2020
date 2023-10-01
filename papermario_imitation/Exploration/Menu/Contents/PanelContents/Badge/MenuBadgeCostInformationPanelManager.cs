using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBadgeCostInformationPanelManager
{
  private BaseBadgeListFunctions baseBadgeListFunctions;
  private GameObject badgeEmptyCostImage;
  private GameObject badgeFullCostImage;
  private GameObject playerTotalBadgePointText;
  private GameObject playerRestOfTheBadgePoint;
  private Transform allBadgeListContainerTra;
  private Transform emptyCostImgContainerTra;
  private Transform fullCostImgContainerTra;
  private Transform playerUsingBadgePointInformationContainerTra;
  private RectTransform pointInformationContainerRext;
  private string restOfTheBadgePointNumGameObjectName = "PlayerRestOfTheBadgePointNum";
  private float badgePointInformationContainerMinHeight;

  public MenuBadgeCostInformationPanelManager (
    GameObject badgeEmptyCostImage,
    GameObject badgeFullCostImage,
    GameObject playerTotalBadgePointText,
    GameObject playerRestOfTheBadgePoint,
    Transform playerUsingBadgePointInformationContainerTra,
    Transform allBadgeListContainerTra,
    Transform emptyCostImgContainerTra,
    Transform fullCostImgContainerTra
  )
  {
    baseBadgeListFunctions = new BaseBadgeListFunctions ();
    this.badgeEmptyCostImage = badgeEmptyCostImage;
    this.badgeFullCostImage = badgeFullCostImage;
    this.playerTotalBadgePointText = playerTotalBadgePointText;
    this.playerRestOfTheBadgePoint = playerRestOfTheBadgePoint;
    this.playerUsingBadgePointInformationContainerTra =
      playerUsingBadgePointInformationContainerTra;
    this.allBadgeListContainerTra = allBadgeListContainerTra;
    this.emptyCostImgContainerTra = emptyCostImgContainerTra;
    this.fullCostImgContainerTra = fullCostImgContainerTra;
    pointInformationContainerRext =
      playerUsingBadgePointInformationContainerTra.gameObject.GetComponent<RectTransform> ();
    badgePointInformationContainerMinHeight = 55.0f;
    SetupCostInformation ();
    // Debug.Log ("playerTotalBadgePointText : " + playerTotalBadgePointText);
  }

  void SetupCostInformation ()
  {
    // プレイヤーの最大バッジコストの取得
    int maxBadgePoint = SaveSystem.Instance.userData.playerMaxBp;
    // テスト
    // int maxBadgePoint = 99;
    // 現在の使用中コストの取得
    int usingBadgeCost =
      baseBadgeListFunctions.GetTotalEquippingBadgeCost (allBadgeListContainerTra.gameObject);
    float rowGap = 11.0f;
    ModifiySizeOfBadgePointInformationContainer (rowGap, maxBadgePoint);
    // 空のコストイメージ
    SetupCostImg (rowGap, maxBadgePoint, emptyCostImgContainerTra, badgeEmptyCostImage);
    // 満のコストイメージ
    SetupCostImg (rowGap, maxBadgePoint, fullCostImgContainerTra, badgeFullCostImage);
    baseBadgeListFunctions.SetupBadgeFullConstImgActivity (fullCostImgContainerTra, usingBadgeCost);
    // プレイヤーの持つ総バッジポイント表示
    SetupBadgePlayerBadgePointInfo ();
  }

  void ModifiySizeOfBadgePointInformationContainer (float rowGap, int totalCost)
  {
    float width = pointInformationContainerRext.sizeDelta.x;
    float minHeight = badgePointInformationContainerMinHeight;
    int oneLine = 10;
    float height = minHeight;
    if (totalCost > oneLine)
    {
      height = minHeight + (totalCost / oneLine) * rowGap;
    }
    pointInformationContainerRext.sizeDelta = new Vector2 (width, height);
  }

  void SetupCostImg (float rowGap, int totalCost, Transform costImgParentTra, GameObject costImg)
  {
    float column = -11.0f;
    float startPosiX = 0.0f;
    int oneLine = 10;
    for (var i = 0; i < totalCost; i++)
    {
      GameObject newCostImg = GameObject.Instantiate<GameObject> (costImg);
      newCostImg.transform.SetParent (costImgParentTra);
      float baseScale = 1.0f;
      newCostImg.transform.localScale = new Vector3 (baseScale, baseScale, baseScale);
      float posX = 0;
      // 10個目以降
      if (i > oneLine - 1)
      {
        // 例えばi=30のとき、i=0になるようにして、rowにiをかける
        posX = startPosiX + rowGap * (i - (oneLine * (i / oneLine)));
      }
      else
      {
        posX = startPosiX + rowGap * i;
      }
      float posY = 0;
      // for文だと10個目はi=9になるので、oneLine-1
      if (i / oneLine > 0 && i != oneLine - 1)
      {
        posY = column * (i / oneLine);
      }
      newCostImg.transform.localPosition = new Vector3 (posX, posY, 0);
    }
  }

  void SetupBadgePlayerBadgePointInfo ()
  {
    // 最大バッジポイント数表示
    int playerTotalBadgePoint = SaveSystem.Instance.userData.playerMaxBp;
    playerTotalBadgePointText.GetComponent<Text> ().text = playerTotalBadgePoint.ToString ();
    float pointInfoContainerHeight = pointInformationContainerRext.sizeDelta.y;
    // 最小サイズからどれだけ高くなったかに合わせて文字の位置調整
    float modifiedHeightSize = pointInfoContainerHeight - badgePointInformationContainerMinHeight;
    Vector3 modifyiedPosition = playerRestOfTheBadgePoint.transform.localPosition;
    modifyiedPosition.y -= modifiedHeightSize;
    playerRestOfTheBadgePoint.transform.localPosition = modifyiedPosition;
    // 残り使用できるバッジポイント数の表示
    ManageRestOfTheBadgePointNumText ();
  }

  // 別クラスでも用いるのでplayerTotalBadgePointは引数にしない
  // した方が楽そうなら変更するかも
  public void ManageRestOfTheBadgePointNumText ()
  {
    // 残り使用できるバッジポイント数の表示
    int playerTotalBadgePoint = SaveSystem.Instance.userData.playerMaxBp;
    int currentUsingBadgeCost = baseBadgeListFunctions.GetTotalEquippingBadgeCost (allBadgeListContainerTra.gameObject);
    int restOfTheBadgePoint = playerTotalBadgePoint - currentUsingBadgeCost;
    Text restOfTheBadgePointNum =
      playerRestOfTheBadgePoint.transform.Find (restOfTheBadgePointNumGameObjectName).GetComponent<Text> ();
    restOfTheBadgePointNum.text = restOfTheBadgePoint.ToString ();
  }
}