using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 複数間のマネージャーで共通する関数をまとめたクラス
public class MenuCommonFunctions
{
  private string[] previousItemArray;
  private int[] previousPuttingBadgeNums;
  private int previousPlayerCurrentHp;
  private int previousPlayerCurrentFp;
  private int previousSakuraCurrentHp;
  private string previousPartnerName;
  private int previousCoin;

  public MenuCommonFunctions ()
  {
    SetPreviousStatus ();
  }
  public bool IsSaveDataUpdated ()
  {
    bool updated = false;
    var userData = SaveSystem.Instance.userData;
    if (
      previousItemArray != userData.havingItemsName ||
      previousPuttingBadgeNums != userData.puttingBadgeNums ||
      previousPlayerCurrentHp != userData.playerCurrentHp ||
      previousPlayerCurrentFp != userData.playerCurrentFp ||
      previousSakuraCurrentHp != userData.sakuraCurrentHp ||
      previousPartnerName != userData.currentSelectedPartnerName ||
      previousCoin != userData.havingCoin
    )
    {
      updated = true;
      SetPreviousStatus ();
    }
    return updated;
  }

  public bool IsItemDataUpdated ()
  {
    bool updated = false;
    var userData = SaveSystem.Instance.userData;
    if (previousItemArray != userData.havingItemsName)
    {
      updated = true;
      previousItemArray = userData.havingItemsName;
    }
    return updated;
  }

  public bool IsBadgeDataUpdated ()
  {
    bool updated = false;
    var userData = SaveSystem.Instance.userData;
    if (previousPuttingBadgeNums != userData.puttingBadgeNums)
    {
      updated = true;
      previousPuttingBadgeNums = userData.puttingBadgeNums;
    }
    return updated;
  }

  void SetPreviousStatus ()
  {
    // 探検中に変わる値を想定
    // 道中でひろうコイン、ハート、ハナ
    // アイテム使用
    // 選択する仲間
    var userData = SaveSystem.Instance.userData;
    previousItemArray = userData.havingItemsName;
    previousPuttingBadgeNums = userData.puttingBadgeNums;
    previousPlayerCurrentHp = userData.playerCurrentHp;
    previousPlayerCurrentFp = userData.playerCurrentFp;
    previousSakuraCurrentHp = userData.sakuraCurrentHp;
    previousPartnerName = userData.currentSelectedPartnerName;
    previousCoin = userData.havingCoin;
  }
  public List<GameObject> GetChildList (GameObject listContainer)
  {
    List<GameObject> buttonList = new List<GameObject> ();
    foreach (Transform child in listContainer.transform)
    {
      buttonList.Add (child.gameObject);
    }
    return buttonList;
  }

  // 「メインスレッド以外でGetComponentしてはいけない」とエラーがでるのでUniTask.Run(()=>{});が出来ない
  public void ResetItemButtonIsSelected (List<GameObject> buttonsList)
  {
    // Debug.Log ("buttonsList.Count : " + buttonsList.Count);
    foreach (var button in buttonsList)
    {
      BelongingButtonInfoContainer itemButtonInfoContainter = button.GetComponent<BelongingButtonInfoContainer> ();
      itemButtonInfoContainter.IsSelected = false;
    }
  }
  // 「メインスレッド以外でGetComponentしてはいけない」とエラーがでるのでUniTask.Run(()=>{});が出来ない
  // MenuCommonFunctionに書かず、呼び出し元にて書き直すリファクタリング予定
  public void ResetTargetButtonIsSelected (List<GameObject> buttonsList)
  {
    foreach (var button in buttonsList)
    {
      TargetButtonInfoContainer itemButtonInfoContainter = button.GetComponent<TargetButtonInfoContainer> ();
      itemButtonInfoContainter.IsSelected = false;
    }
  }
  public void ResetSkillButtonIsSelected (List<GameObject> buttonsList)
  {
    foreach (var button in buttonsList)
    {
      SkillButtonInformationContainer skillButtonInformationContainer = button.GetComponent<SkillButtonInformationContainer> ();
      // Debug.Log ("skillButtonInformationContainer.IsSelected : " + skillButtonInformationContainer.IsSelected);
      skillButtonInformationContainer.IsSelected = false;
    }
  }

  // アイテムの購入する、拾うなどで使用
  public string[] GetNewStringItemNameArrayAddedNewItem (string newItemName)
  {
    string[] newerHavingItemsName = null;
    string[] currentHavingItemsName = SaveSystem.Instance.userData.havingItemsName;
    newerHavingItemsName = currentHavingItemsName.Concat (new string[] { newItemName }).ToArray ();
    return newerHavingItemsName;
  }

  // アイテム等を売ったりするときに使用する
  // 配列の何番目かを指定して要素を削除している
  public string[] GetNewStringArrayExcludingSelectedElement (string[] originalArray, int beExcludedElementNumber)
  {
    string[] newArray = originalArray.Where ((source, index) => index != beExcludedElementNumber).ToArray ();
    return newArray;
  }
  // 配列の削除方法は下記を参照
  // 配列削除テスト
  // string[] myArray = { "a", "b", "c", "d", "e" };
  // int indexToRemove = 1;
  // // Whereで引数を二つとる際、一つ目は要素全体(0~4)を指す(何を書いても良い、alphaの代わりにsourceでもいい)
  // // 二つ目の引数はeach文のように0,1,2,3,4が入る
  // // 下記はアロー関数の条件にあった要素をToArrayで配列としている
  // // 要素のうち、0~4の要素のなかで1ではない要素を配列にしてmyArrayに代入する
  // myArray = myArray.Where ((alpha, index) => index != indexToRemove).ToArray ();
  // for (var i = 0; i < myArray.Length; i++)
  // {
  //   Debug.Log ("myArray : [" + i + "]" + myArray[i]);
  //   // "a", "c", "d", "e" のみが表示される

  public GameObject GetSelectedSelectableItemButton (List<GameObject> buttonsList)
  {
    GameObject selectedButton = null;
    foreach (var itemButton in buttonsList)
    {
      if (itemButton == null) return selectedButton;
      BelongingButtonInfoContainer buttonInfoContainer = itemButton.GetComponent<BelongingButtonInfoContainer> ();
      bool isSelectable = buttonInfoContainer.IsSelectable;
      bool isSelected = buttonInfoContainer.IsSelected;
      if (isSelectable && isSelected)
      {
        selectedButton = itemButton;
      }
    }
    return selectedButton;
  }

  // nullじゃなければ何かしらの選択可能なボタンがあると判断できるboolとしても使うかも
  public GameObject GetSelectedSelectableSkillButton (List<GameObject> skillButtonList)
  {
    GameObject selectedButton = null;
    foreach (var skillButton in skillButtonList)
    {
      if (skillButton == null) return selectedButton;
      SkillButtonInformationContainer skillButtonInformationContainer = skillButton.GetComponent<SkillButtonInformationContainer> ();
      bool isSelectable = skillButtonInformationContainer.IsSelectable;
      bool isSelected = skillButtonInformationContainer.IsSelected;
      if (isSelectable && isSelected)
      {
        selectedButton = skillButton;
      }
    }
    return selectedButton;
  }

  public GameObject GetSelectedRecoveringTargetButton (List<GameObject> buttonsList)
  {
    GameObject selectedButton = null;
    foreach (var targetButton in buttonsList)
    {
      if (targetButton == null) return selectedButton;
      TargetButtonInfoContainer targetButtonInfoContainer = targetButton.GetComponent<TargetButtonInfoContainer> ();
      bool isSelectableHp = targetButtonInfoContainer.IsSelectableHp;
      bool isSelectableFp = targetButtonInfoContainer.IsSelectableFp;
      bool isSelected = targetButtonInfoContainer.IsSelected;
      if (isSelected && (isSelectableHp || isSelectableFp))
      {
        selectedButton = targetButton;
      }
    }
    return selectedButton;
  }

  public bool GetIsSelectableTargetButtonSelected (List<GameObject> targetListViewButtonsList, List<GameObject> itemListViewButtonsList)
  {
    bool isSelected = false;
    for (var i = 0; i < targetListViewButtonsList.Count; i++)
    {
      GameObject targetButton = targetListViewButtonsList[i];
      // 選択されたアイテムのタイプを知りたい
      GameObject selectableSelectedItemButton = GetSelectedSelectableItemButton (itemListViewButtonsList);
      if (selectableSelectedItemButton == null)
      {
        // Debug.LogError ("選択されているボタンはないよ！");
        return false;
      }
      bool canItemRecoverTarget = CanItemRecoverTarget (selectableSelectedItemButton, targetButton);
      if (canItemRecoverTarget)
      {
        isSelected = true;
      }
    }
    // Debug.Log ("isSelected : " + isSelected);
    return isSelected;
  }

  public bool CanItemRecoverTarget (GameObject selectableSelectedItemButton, GameObject targetButton)
  {
    bool canItemRecoverTarget = false;
    bool isTargetSelectableHp = targetButton.GetComponent<TargetButtonInfoContainer> ().IsSelectableHp;
    bool isTargetSelectableFp = targetButton.GetComponent<TargetButtonInfoContainer> ().IsSelectableFp;
    string itemType = selectableSelectedItemButton.GetComponent<BelongingButtonInfoContainer> ().Type.ToString ();
    bool isTargetSelected = targetButton.GetComponent<TargetButtonInfoContainer> ().IsSelected;
    string typeHp = BelongingButtonInfoContainer.State.recoverHp.ToString ();
    string typeFp = BelongingButtonInfoContainer.State.recoverFp.ToString ();
    // そもそもボタンが選ばれていなかったら
    if (!isTargetSelected) return false;
    // 選んだアイテムのタイプがHPの時にプレイヤーのHPが回復可能な時、選んだアイテムのタイプがFPの時にプレイヤーのFPが回復可能な時
    if ((itemType == typeHp && isTargetSelectableHp) || (itemType == typeFp && isTargetSelectableFp))
    {
      canItemRecoverTarget = true;
    }
    return canItemRecoverTarget;
  }

  public string[] GetEquippingBadgeNameArray ()
  {
    // つけているバッジの名前リストを作成する
    string[] havingBadgeNameArr = SaveSystem.Instance.userData.havingBadgesName;
    int[] equippingBadgeNumArr = SaveSystem.Instance.userData.puttingBadgeNums;
    List<string> equippingBadgeNameList = new List<string> ();
    // 持っているバッジ、つけているバッジが何番目かをもとにつけているバッジのみを探す
    foreach (var equippingBadgeNum in equippingBadgeNumArr)
    {
      for (var i = 0; i < havingBadgeNameArr.Length; i++)
      {
        int badgeNum = i;
        string badgeName = havingBadgeNameArr[i];
        if (equippingBadgeNum == badgeNum)
        {
          equippingBadgeNameList.Add (badgeName);
        }
      }
    }
    // つけているバッジの名前リスト
    string[] equippingBadgeNameArr = equippingBadgeNameList.ToArray ();
    return equippingBadgeNameArr;
  }

  public int GetSelectedItemButtonNum (List<GameObject> itemListViewButtonsList)
  {
    int selectedNum = 0;
    for (var i = 0; i < itemListViewButtonsList.Count; i++)
    {
      GameObject itemButton = itemListViewButtonsList[i];
      bool isItemSelected = itemButton.GetComponent<BelongingButtonInfoContainer> ().IsSelected;
      bool isSelectable = itemButton.GetComponent<BelongingButtonInfoContainer> ().IsSelectable;
      int number = i;
      // 条件にselectableが必要
      if (isItemSelected & isSelectable)
      {
        selectedNum = i;
      }
    }
    return selectedNum;
  }

  public GameObject GetSelectedItemButton (List<GameObject> itemListViewButtonsList)
  {
    GameObject selectedItem = null;
    foreach (var itemButton in itemListViewButtonsList)
    {
      bool isItemSelected = itemButton.GetComponent<BelongingButtonInfoContainer> ().IsSelected;
      bool isSelectable = itemButton.GetComponent<BelongingButtonInfoContainer> ().IsSelectable;
      if (isItemSelected & isSelectable)
      {
        selectedItem = itemButton;
      }
    }
    return selectedItem;
  }

  public GameObject GetSelectedStrategyButton (List<GameObject> strategyListViewButtonList)
  {
    GameObject selectedStrategy = null;
    foreach (var strategyButton in strategyListViewButtonList)
    {
      bool isStrategySelected = strategyButton.GetComponent<StrategyButtonInformationContainer> ().IsSelected;
      bool isStrategySelectable = strategyButton.GetComponent<StrategyButtonInformationContainer> ().IsSelectable;
      if (isStrategySelected && isStrategySelectable)
      {
        selectedStrategy = strategyButton;
      }
    }
    return selectedStrategy;
  }

  public List<int> GetRandomIntNumList (int randomNumRange)
  {
    List<int> intList = new List<int> ();
    // 装備するバッジに割り振るIDは0~199
    // プレイヤーのバッジコストは最大で99
    // バッジの中でもコスト0のものは原作では10種ほど
    // 最大でも110ほどのバッジしかつけられないので割り振れるIDは200種で固定
    // 引数で決めるようにする
    // int range = 200;
    for (var intNum = 0; intNum < randomNumRange; intNum++)
    {
      intList.Add (intNum);
    }
    return intList;
  }

  // メニューの回復対象ボタンのTargetNameが引数として使われる前提の関数
  // targetNameにはアルファベットを用いる
  public int GetPartnerBaseHp (string targetName, PartnerDataArray partnerDataArray)
  {
    int baseHp = 0;
    // Debug.Log ("partnerDataArray.gamePartners : " + partnerDataArray.gamePartners);
    foreach (var partner in partnerDataArray.gamePartners)
    {
      // targetName
      if (targetName == partner.imgFileName)
      {
        baseHp = partner.baseHp;
      }
    }
    return baseHp;
  }

  //  userDataからの情報を用いる
  // partnerNameにはアルファベットを用いない
  // partnerDataArrayは関数内にて作るように改修するかも
  public int GetPartnerMaxHp (string partnerName, int partnerCurrentLevel, PartnerDataArray partnerDataArray)
  {
    // 仲間のステータスはレベルに比例する
    // maxHp = baseHp * level
    // at += level
    int maxHp = 0;
    foreach (var partner in partnerDataArray.gamePartners)
    {
      if (partnerName == partner.name)
      {
        maxHp = partner.baseHp * partnerCurrentLevel;
      }
    }
    return maxHp;
  }

  //  userDataからの情報を用いる
  // partnerNameにはアルファベットを用いない
  public Sprite GetPartnerButtonSprite (string partnerName, PartnerDataArray partnerDataArr, Sprite[] partnerSpriteArr)
  {
    Sprite buttonImg = null;
    foreach (var partner in partnerDataArr.gamePartners)
    {
      if (partnerName == partner.name)
      {
        // Debug.Log ("partnerName : " + partnerName);
        foreach (var partnerSprite in partnerSpriteArr)
        {
          if (partner.imgFileName == partnerSprite.name)
          {
            buttonImg = partnerSprite;
          }
        }
      }
    }
    return buttonImg;
  }

  public void DeleteButtonFromListView (Transform buttonListContainerTra)
  {
    List<GameObject> badgeListViewButtonsList = GetChildList (buttonListContainerTra.gameObject);
    if (badgeListViewButtonsList.Count == 0) return;
    for (var i = 0; i < badgeListViewButtonsList.Count; i++)
    {
      GameObject badgeButton = buttonListContainerTra.GetChild (i).gameObject;
      MonoBehaviour.Destroy (badgeButton);
    }
    // Debug.Log ("全部をけしたよ");
  }

  public void ActivateButtonInteractable (List<GameObject> buttonList)
  {
    foreach (var button in buttonList)
    {
      button.GetComponent<Button> ().interactable = true;
    }
  }
  public void InactivateButtonInteractable (List<GameObject> buttonList)
  {
    foreach (var button in buttonList)
    {
      button.GetComponent<Button> ().interactable = false;
    }
  }
  public void ChangeButtonColorsToGray (GameObject skillButton)
  {
    float gray = 193.0f;
    float max = 255.0f;
    ColorBlock cb = skillButton.GetComponent<Button> ().colors;
    cb.selectedColor = new Color (gray / max, gray / max, gray / max, max / max);
    cb.pressedColor = new Color (gray / max, gray / max, gray / max, max / max);
    skillButton.GetComponent<Button> ().colors = cb;
    // 下記みたいに直で変更できない
    // skillButton.GetComponent<Button> ().colors.disabledColor = new Color (gray / max, gray / max, gray / max, max / max);
  }

  public void ChangeButtonColorsToPink (GameObject skillButton)
  {
    float max = 255.0f;
    float red = max;
    float green = 124.0f;
    float blue = 228.0f;
    Color pink = new Color (red / max, green / max, blue / max, max / max);
    ColorBlock cb = skillButton.GetComponent<Button> ().colors;
    cb.selectedColor = pink;
    cb.pressedColor = pink;
    skillButton.GetComponent<Button> ().colors = cb;
  }

  // すべて同じ数字になる色限定
  // 下記の方法で分母を含めた色指定をしないと、色が白か黒の2色しか指定できない
  public void ChangeImageColor (Image image, float color)
  {
    float max = 255.0f;
    image.color = new Color (color / max, color / max, color / max, max / max);
  }

  public void ChangeTextColor (Text text, float color)
  {
    float max = 255.0f;
    text.color = new Color (color / max, color / max, color / max, max / max);
  }

  // プレイヤーとなかまの「技」用
  public void SetBattleSkillButtonGrayColor (
    GameObject skillButton,
    Image skillImg,
    Text skillNameText,
    Text skillFpCostText,
    Text skillFp
  )
  {
    float imageGrayColor = 126.0f;
    float textGrayColor = 89.0f;
    ChangeButtonColorsToGray (skillButton);
    ChangeImageColor (skillImg, imageGrayColor);
    ChangeTextColor (skillNameText, textGrayColor);
    ChangeTextColor (skillFpCostText, textGrayColor);
    ChangeTextColor (skillFp, textGrayColor);
  }

  public void SetBattleSkillButtonDefaultColor (
    GameObject skillButton,
    Image skillImg,
    Text skillNameText,
    Text skillFpCostText,
    Text skillFp
  )
  {
    float imageWhiteColor = 255.0f;
    float textBlackColor = 0.0f;
    ChangeButtonColorsToPink (skillButton);
    ChangeImageColor (skillImg, imageWhiteColor);
    ChangeTextColor (skillNameText, textBlackColor);
    ChangeTextColor (skillFpCostText, textBlackColor);
    ChangeTextColor (skillFp, textBlackColor);
  }

  public void SetBattleOptionButtonDefaultColor (
    GameObject optionButton,
    Image optionImg,
    Text optionNameText
  )
  {
    float imageWhiteColor = 255.0f;
    float textBlackColor = 0.0f;
    ChangeButtonColorsToPink (optionButton);
    ChangeImageColor (optionImg, imageWhiteColor);
    ChangeTextColor (optionNameText, textBlackColor);
  }

  // プレイヤーとなかまの「アイテム」と「さくせん」用
  public void SetBattleOptionButtonGrayColor (
    GameObject optionButton,
    Image optionImg,
    Text optionNameText
  )
  {
    float imageGrayColor = 126.0f;
    float textGrayColor = 89.0f;
    ChangeButtonColorsToGray (optionButton);
    ChangeImageColor (optionImg, imageGrayColor);
    ChangeTextColor (optionNameText, textGrayColor);
  }
}