using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartnerBattleOptionUi : MonoBehaviour
{
  // モーション
  public GameObject forShowingRotation;
  public GameObject optionGroup;
  public List<GameObject> optionList;
  // コンテンツ
  public GameObject currentSelectOptionBalloon;
  public GameObject partnerCurrentSelectOptionImg;
  public GameObject partnerCurrentSelectOptionText;
  public GameObject buttonForSkill;
  public GameObject partnerSkillListContainer;
  public Sprite skillImage;

}