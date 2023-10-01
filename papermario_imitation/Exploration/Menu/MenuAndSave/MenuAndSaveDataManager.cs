using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAndSaveDataManager : MonoBehaviour
{
  [SerializeField] GameObject itemListContainer;
  [SerializeField] GameObject recoveringTargetListContainer;
  [SerializeField] GameObject allHavingBadgeListContainer;
  [SerializeField] GameObject equippingBadgeListContainer;
  [SerializeField] EventSystem eventSystem;
  private MenuCommonFunctions menuCommonFunctions;
  private MenuAndItemSaveData menuAndItemSaveData;
  private MenuAndBadgeSaveData menuAndBadgeSaveData;
  private List<bool> prevEquippingBadgeBoolCheckList;
  private List<GameObject> currentEquippingBadgeList;

  void Start ()
  {
    menuAndItemSaveData = new MenuAndItemSaveData (
      itemListContainer,
      recoveringTargetListContainer
    );
    menuAndBadgeSaveData = new MenuAndBadgeSaveData ();
    menuCommonFunctions = new MenuCommonFunctions ();
    currentEquippingBadgeList = new List<GameObject> ();
    prevEquippingBadgeBoolCheckList = new List<bool> ();
  }

  void Update ()
  {
    menuAndItemSaveData.ManageRecoveryByUsingItem ();
    menuAndBadgeSaveData.ManageEquippingBadgeFromAll (allHavingBadgeListContainer);
    menuAndBadgeSaveData.ManageEquippingBadgeFromEquipping (equippingBadgeListContainer, allHavingBadgeListContainer);
  }

}