using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class BattleOptionContentsManager : MonoBehaviour
{
  [SerializeField] bool isPlayer;
  [SerializeField] bool isPartner;
  [SerializeField] GameObject battleCommonOptionUi;
  [SerializeField] GameObject playerBattleOptionUi;
  [SerializeField] GameObject partnerBattleOptionUi;
  [SerializeField] GameObject battleManager;
  private MenuCommonFunctions menuCommonFunctions;
  private BattleCommonOptionUi battleCommonOptionUiScript;
  private PlayerBattleOptionUi playerBattleOptionUiScript;
  private PartnerBattleOptionUi partnerBattleOptionUiScript;
  private BattleManager battleManagerScript;
  private TextMesh playerCurrentSelectOptionText;
  private SpriteRenderer playerCurrentSelectOptionSpriteRenderer;
  private PlayerJumpSkillListContentsPreparer playerJumpSkillContentsPreparer;
  private PlayerHammerSkillListContentsPreparer playerHammerSkillListContentsPreparer;
  private PartnerSkillListContentsPreparer partnerSkillListContentsPreparer;
  private ItemOptionListContentsPreparer itemOptionListContentsPreparer;
  private StrategyOptionListContentsPreparer strategyOptionListContentsPreparer;
  private SelectedTargetNameContentsPreparer selectedEnemyNameContentsPreparer;
  private SelectedOptionButtonDescriptionContentsPreparer selectedOptionButtonDescriptionContentsPreparer;
  private SelectedAttackDescriptionContentsPreparer selectedAttackDescriptionContentsPreparer;
  private SelectedTargetsCursorAndNameContentsPreparer selectedTargetsCursorAndNameContentsPreparer;
  private List<GameObject> jumpSkillList;
  private List<GameObject> hammerSkillList;
  private List<GameObject> itemOptionList;
  private List<GameObject> strategyOptionList;
  private List<GameObject> partnerSkillList;
  private List<GameObject> battlePlayersCharacterList;
  private List<GameObject> battleEnemyCharacterList;
  private List<GameObject> battleTargetCharacterList;
  private int selectedTargetNum;

  public List<GameObject> BattlePlayersCharacterList
  {
    set { battlePlayersCharacterList = value; }
  }
  public List<GameObject> BattleEnemyCharacterList
  {
    set { battleEnemyCharacterList = value; }
  }
  public List<GameObject> BattleTargetCharacterList
  {
    set { battleTargetCharacterList = value; }
  }
  public int SelectedTargetNum
  {
    set { selectedTargetNum = value; }
  }
  void Start ()
  {
    SetupContents ();
  }
  void Update ()
  {
    // 敵の名前情報更新(バトルマネージャー側からリストを更新しても対応できるはずなので要確認)
    selectedEnemyNameContentsPreparer.ManageSelectedEnemyName (
      battleTargetCharacterList,
      selectedTargetNum
    );
    selectedOptionButtonDescriptionContentsPreparer.ManageSelectedOptionText ();
    selectedAttackDescriptionContentsPreparer.ManageSelectedAttackText (battleManagerScript.CurrentSelectedOptionButton);
  }

  void SetupContents ()
  {
    menuCommonFunctions = new MenuCommonFunctions ();
    battleCommonOptionUiScript = battleCommonOptionUi.GetComponent<BattleCommonOptionUi> ();
    if (isPlayer)
    {
      playerBattleOptionUiScript = playerBattleOptionUi.GetComponent<PlayerBattleOptionUi> ();
      playerCurrentSelectOptionText = playerBattleOptionUiScript.playerCurrentSelectOptionText.GetComponent<TextMesh> ();
      playerCurrentSelectOptionSpriteRenderer = playerBattleOptionUiScript.playerCurrentSelectOptionImg.GetComponent<SpriteRenderer> ();
      // ジャンプ
      playerJumpSkillContentsPreparer = new PlayerJumpSkillListContentsPreparer (
        playerBattleOptionUiScript.buttonForSkill,
        playerBattleOptionUiScript.playerJumpSkillListContainer.transform
      );
      // ハンマー
      playerHammerSkillListContentsPreparer = new PlayerHammerSkillListContentsPreparer (
        playerBattleOptionUiScript.buttonForSkill,
        playerBattleOptionUiScript.playerHammerSkillListContainer.transform
      );
      // アイテム(共通)
      // リストにアイテムを用意するのはあくまでもプレイヤー
      itemOptionListContentsPreparer = new ItemOptionListContentsPreparer (
        battleCommonOptionUiScript.itemButton,
        battleCommonOptionUiScript.itemOptionListContainer.transform
      );
      // さくせんリスト用意もアイテムと同様
      strategyOptionListContentsPreparer = new StrategyOptionListContentsPreparer (
        battleCommonOptionUiScript.strategyButton,
        battleCommonOptionUiScript.strategyOptionListContainer.transform
      );
      jumpSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerJumpSkillListContainer);
      hammerSkillList = menuCommonFunctions.GetChildList (playerBattleOptionUiScript.playerHammerSkillListContainer);
      menuCommonFunctions.InactivateButtonInteractable (jumpSkillList);
      menuCommonFunctions.InactivateButtonInteractable (hammerSkillList);
    }
    else if (isPartner)
    {
      partnerBattleOptionUiScript = partnerBattleOptionUi.GetComponent<PartnerBattleOptionUi> ();
      playerCurrentSelectOptionText = partnerBattleOptionUiScript.partnerCurrentSelectOptionText.GetComponent<TextMesh> ();
      playerCurrentSelectOptionSpriteRenderer = partnerBattleOptionUiScript.partnerCurrentSelectOptionImg.GetComponent<SpriteRenderer> ();
      // なかま固有スキル
      partnerSkillListContentsPreparer = new PartnerSkillListContentsPreparer (
        partnerBattleOptionUiScript.buttonForSkill,
        partnerBattleOptionUiScript.partnerSkillListContainer.transform
      );
      // アイテムリストの中身自体は作成しない
      itemOptionListContentsPreparer = new ItemOptionListContentsPreparer ();
      // さくせんリストも中身は作成しない
      strategyOptionListContentsPreparer = new StrategyOptionListContentsPreparer ();
      partnerSkillList = menuCommonFunctions.GetChildList (partnerBattleOptionUiScript.partnerSkillListContainer);
      menuCommonFunctions.InactivateButtonInteractable (partnerSkillList);
    }
    // 共通
    SetupItemList ();
    strategyOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.strategyOptionListContainer);

    battleManagerScript = battleManager.GetComponent<BattleManager> ();
    selectedEnemyNameContentsPreparer = new SelectedTargetNameContentsPreparer (
      battleCommonOptionUiScript.selectedEnemyNameFrame
    );
    selectedOptionButtonDescriptionContentsPreparer = new SelectedOptionButtonDescriptionContentsPreparer (
      battleCommonOptionUiScript.optionButtonDescriptionFrame,
      battleCommonOptionUiScript.eventSystem
    );
    selectedAttackDescriptionContentsPreparer = new SelectedAttackDescriptionContentsPreparer (
      battleCommonOptionUiScript.attackDescriptionFrame
    );
    selectedTargetsCursorAndNameContentsPreparer = new SelectedTargetsCursorAndNameContentsPreparer (
      battleCommonOptionUiScript.battleCommonOptionMultipleTarget,
      battleCommonOptionUiScript.forInstantiateTargetHandCursorPrefab,
      battleCommonOptionUiScript.forInstantiateTargetNameFramePrefab
    );
    selectedTargetNum = 0;
  }

  // ボタンの持つ情報を更新して新しい使用可能ボタンリストを用意
  public void StartPlayerTurn (int currentPlayerHp, int currentPlayerFp, int currentPartnerHp, int currentPartnerMaxHp)
  {
    // player
    if (isPlayer)
    {
      playerJumpSkillContentsPreparer.ResetAvailableJumpSkillButtonInformation (currentPlayerFp);
      playerHammerSkillListContentsPreparer.ResetAvailableJumpSkillButtonInformation (currentPlayerFp);
    }
    // partner
    else if (isPartner)
    {
      // FPを消費するスキルをリセットさせたい
    }
    itemOptionListContentsPreparer.ResetAvailableItemButtonInformation (itemOptionList, currentPlayerHp, currentPlayerFp, currentPartnerHp, currentPartnerMaxHp);
    // itemList
  }

  public void ResetIsSelectedInformation ()
  {
    // Debug.Log ("リセット");
    if (isPlayer)
    {
      menuCommonFunctions.ResetSkillButtonIsSelected (jumpSkillList);
      menuCommonFunctions.ResetSkillButtonIsSelected (hammerSkillList);
    }
    else if (isPartner)
    {
      partnerSkillList = menuCommonFunctions.GetChildList (partnerBattleOptionUiScript.partnerSkillListContainer);
      menuCommonFunctions.ResetSkillButtonIsSelected (partnerSkillList);
    }
    menuCommonFunctions.ResetItemButtonIsSelected (itemOptionList);
    strategyOptionListContentsPreparer.ResetStrategyButtonIsSelected (strategyOptionList);
  }

  // 他クラスでも使用するのため関数化
  public async UniTask SetupItemList ()
  {
    itemOptionList = null;
    itemOptionList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.itemOptionListContainer);
    await UniTask.WaitUntil (() => itemOptionList != null);
  }

  public List<string> GetHavingItemNameList ()
  {
    List<string> havingItemNameList = new List<string> ();
    // Debug.Log ("itemOptionList.Count : " + itemOptionList.Count);
    foreach (var item in itemOptionList)
    {
      // Debug.Log ("item : " + )
      // Debug.Log ("item.GetComponent<BelongingButtonInfoContainer> () == null : " + item.GetComponent<BelongingButtonInfoContainer> () == null);
      BelongingButtonInfoContainer itemInfoContainer = item.GetComponent<BelongingButtonInfoContainer> ();

      string itemName = itemInfoContainer.BelongingName;
      havingItemNameList.Add (itemName);
    }
    return havingItemNameList;
  }

  public async UniTask RemoveUsedItem ()
  {
    List<GameObject> itemButtonList = menuCommonFunctions.GetChildList (battleCommonOptionUiScript.itemOptionListContainer);
    foreach (var itemButton in itemButtonList)
    {
      BelongingButtonInfoContainer itemButtonInfoContainer = itemButton.GetComponent<BelongingButtonInfoContainer> ();
      // 使用可能だったアイテムかつ選ばれていたアイテムが使用されたアイテム
      if (itemButtonInfoContainer.IsSelected && itemButtonInfoContainer.IsSelectable)
      {
        Destroy (itemButton);
        await UniTask.WaitUntil (() => itemButton == null);
        // Debug.Log ("削除");
      }
    }
  }

  public void SetupAllTargetsInformation (bool isSlectingFromPlayerParty)
  {
    if (isSlectingFromPlayerParty)
    {
      // プレイヤーパーティーの全選択を用意
      selectedTargetsCursorAndNameContentsPreparer.SetupAllTargetsHandCursorAndTargetName (battlePlayersCharacterList);
    }
    else
    {
      // 敵パーティーの全選択を用意
      // Debug.Log ("敵のパーティ全員ねらう");
      selectedTargetsCursorAndNameContentsPreparer.SetupAllTargetsHandCursorAndTargetName (battleEnemyCharacterList);
    }
  }

  public void ResetpAllTargetsInformation ()
  {
    selectedTargetsCursorAndNameContentsPreparer.RemoveAllHandCursorsAndTargetNames ();
  }

  // プレイヤー
  public void ChangeCurrentOptionToJump ()
  {
    playerCurrentSelectOptionSpriteRenderer.sprite = playerBattleOptionUiScript.jumpImage;
    playerCurrentSelectOptionSpriteRenderer.flipX = true;
    playerCurrentSelectOptionText.text = "ジャンプ";
  }
  public void ChangeCurrentOptionToHammer ()
  {
    playerCurrentSelectOptionSpriteRenderer.sprite = playerBattleOptionUiScript.hammerImage;
    playerCurrentSelectOptionSpriteRenderer.flipX = false;
    playerCurrentSelectOptionText.text = "ハンマー";
  }

  // なかま
  public void ChangeCurrentOptionToSkill ()
  {
    playerCurrentSelectOptionSpriteRenderer.sprite = partnerBattleOptionUiScript.skillImage;
    playerCurrentSelectOptionSpriteRenderer.flipX = false;
    playerCurrentSelectOptionText.text = "ワ ザ";
  }

  // 共通
  public void ChangeCurrentOptionToItem ()
  {
    playerCurrentSelectOptionSpriteRenderer.sprite = battleCommonOptionUiScript.itemImage;
    playerCurrentSelectOptionSpriteRenderer.flipX = false;
    playerCurrentSelectOptionText.text = "アイテム";
  }
  public void ChangeCurrentOptionToStrategy ()
  {
    playerCurrentSelectOptionSpriteRenderer.sprite = battleCommonOptionUiScript.strategyImage;
    playerCurrentSelectOptionSpriteRenderer.flipX = false;
    playerCurrentSelectOptionText.text = "さくせん";
  }
}