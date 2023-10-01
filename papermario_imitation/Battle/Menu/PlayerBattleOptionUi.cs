using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBattleOptionUi : MonoBehaviour
{
  // モーション
  public GameObject forShowingRotation;
  public GameObject optionGroup;
  public List<GameObject> optionList;
  // コンテンツ
  public GameObject currentSelectOptionBalloon;
  public GameObject playerCurrentSelectOptionImg;
  public GameObject playerCurrentSelectOptionText;
  public GameObject buttonForSkill;
  public GameObject playerJumpSkillListContainer;
  public GameObject playerHammerSkillListContainer;
  public Sprite jumpImage;
  public Sprite hammerImage;

}